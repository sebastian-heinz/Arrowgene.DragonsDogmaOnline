using Arrowgene.Ddon.GameServer.Handler;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class WeatherManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WeatherManager));

        private static readonly long OriginalRealTimeSec=0x55DDD470; // Taken from the pcaps. A few days before DDOn release
        private static readonly long OriginalGameTimeSec=0x22C2ED000; // Taken from the pcaps.
        private static readonly uint GameTimeDayHour = 24;

        public static readonly uint ForecastIntervalGameHour = 6; //Used in 

        private readonly uint GameClockTimescale;

        private readonly DdonGameServer _Server;

        public List<CDataWeatherLoop> WeatherLoopList { get; private set; }
        public ulong WeatherLoopTotalLength { get; private set; }

        public WeatherManager(DdonGameServer server)
        {
            _Server = server;
            GameClockTimescale = server.Setting.GameLogicSetting.GameClockTimescale;
            GenerateWeatherSequence();
        }

        public Weather GetWeather()
        {
            return GetWeather(DateTimeOffset.UtcNow);
        }

        public Weather GetWeather(DateTimeOffset time)
        {
            ulong secondsElapsed = (ulong)(time.ToUnixTimeSeconds() - OriginalRealTimeSec);
            ulong remainingSeconds = secondsElapsed % WeatherLoopTotalLength;
            foreach (var weatherLoop in WeatherLoopList)
            {
                if (remainingSeconds <= weatherLoop.TimeSec)
                {
                    return weatherLoop.WeatherId;
                }
                remainingSeconds -= weatherLoop.TimeSec;
            }
            return Weather.Fine;
        }

        public List<CDataWeatherForecast> GetForecast()
        {
            List<CDataWeatherForecast> forecast = new List<CDataWeatherForecast>();

            // The first entry on the forecast is always the next Lestanian hour.
            // Weather changes are set up on the hour, so we go one second over to see what's coming up.
            DateTimeOffset nextHour = GetTimeForNextGameHour().AddSeconds(1);
            forecast.Add(new CDataWeatherForecast()
            {
                Weather = GetWeather(nextHour)
            });

            uint forecastInterval = ServerWeatherForecastGetHandler.IntervalGameHour;
            DateTimeOffset forecastTime = nextHour;
            for (int i = 0; i < 3; i++)
            {
                forecastTime = forecastTime.AddMinutes(GameClockTimescale * forecastInterval / GameTimeDayHour);
                forecast.Add(new CDataWeatherForecast()
                {
                    Weather = GetWeather(forecastTime)
                });
            }

            return forecast;
        }

        private DateTimeOffset GetTimeForNextGameHour()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            long currentTimeMS = RealTimeToGameTimeMS(now);
            double currentTimeHour = currentTimeMS / 3600000.0;
            uint offsetSecondsToNextHour = (uint)((1 - currentTimeHour % 1) * 3600);

            return now.AddSeconds(offsetSecondsToNextHour);
        }

        // Adapted from the client's code
        public long RealTimeToGameTimeMS(DateTimeOffset realTime)
        {
            return (1440 * (realTime.Millisecond + 1000 * (realTime.ToUnixTimeSeconds() - OriginalRealTimeSec)) / GameClockTimescale)
            % (3600000 * GameTimeDayHour);
        }

        public TimeSpan RealTimeToGameTime(DateTimeOffset realTime)
        {
            long gameTimeMs = RealTimeToGameTimeMS(realTime);
            return TimeSpan.FromMilliseconds(gameTimeMs);
        }

        private void GenerateWeatherSequence()
        {
            List<CDataWeatherLoop> weatherLoop = new List<CDataWeatherLoop>();

            uint seqLength = _Server.Setting.GameLogicSetting.WeatherSequenceLength;
            List<(uint meanLength, uint weight)> seqStats = _Server.Setting.GameLogicSetting.WeatherStatistics;

            if (!seqStats.Where(x => x.weight > 0).Any())
            {
                Logger.Error($"All weather weights zero; defaulting to always Fine weather.");
                WeatherLoopList = weatherLoop;
            }

            // Some setup for the randomization.
            Random rnd = new Random();
            List<(Weather weather, uint meanLength, uint accumulatedWeight)> cumStats = new List<(Weather weather, uint meanLength, uint accumulatedWeight)>();
            uint accumulatedWeight = 0;
            for (int i = 0; i < seqStats.Count; i++)
            {
                accumulatedWeight += seqStats[i].weight;
                cumStats.Add((
                    (Weather)(i+1), 
                    seqStats[i].meanLength,
                    accumulatedWeight
                ));
            }

            ulong totalLength = 0;
            for (int i = 0; i < seqLength; i++)
            {
                //Find the next weather type.
                double rngWeight = rnd.NextDouble() * accumulatedWeight;
                (Weather weather, uint meanLength, uint _) = cumStats.Find(x => x.accumulatedWeight >= rngWeight);

                //Calculate how long it should last, using an exponential distribution.
                double rngUniformLength = rnd.NextDouble();
                uint rngExpLength = (uint)(-Math.Log(rngUniformLength) * meanLength);

                //Round to the next Lestanian hour.
                uint gameHourSeconds = GameClockTimescale * 60 / 24;
                rngExpLength = (uint)(Math.Round((double)rngExpLength / gameHourSeconds) * gameHourSeconds);
                if (rngExpLength == 0) rngExpLength = gameHourSeconds;

                //Add it to the list
                weatherLoop.Add(new CDataWeatherLoop()
                {
                    WeatherId = weather,
                    TimeSec = rngExpLength
                });

                totalLength += rngExpLength;

                Logger.Debug($"Weather Step {i + 1}/{seqLength}: {weather}, {rngExpLength} seconds");
            }

            Logger.Debug($"Total Weather Cycle: {totalLength / 3600.0:f} (real) hours.");

            WeatherLoopList = weatherLoop;
            WeatherLoopTotalLength = totalLength;
        }
    }
}
