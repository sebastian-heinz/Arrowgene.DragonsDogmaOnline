using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer
{
    public class WeatherManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WeatherManager));

        public static readonly long OriginalRealTimeSec = 0x55DDD470; // Taken from the pcaps. A few days before DDOn release
        public static readonly long OriginalGameTimeSec = 0x22C2ED000; // Taken from the pcaps.
        public static readonly uint GameTimeDayHour = 24;
        public static readonly uint GameTimeMoonAges = 30;
        public static readonly uint MoonAgeLoopSec = 1209600; // Taken from the pcaps; 14 real life days per lunar cycle.

        /// <summary>
        /// Number of game hours between forecast times. 
        /// The first forecast is always at the top of the next hour, then the next three are separated by IntervalGameHour.
        /// </summary>
        public static readonly uint ForecastIntervalGameHour = 3;

        private readonly uint GameClockTimescale;

        private readonly DdonGameServer _Server;

        public List<CDataWeatherLoop> WeatherLoopList { get; private set; }
        public ulong WeatherLoopTotalLength { get; private set; }

        public WeatherManager(DdonGameServer server)
        {
            _Server = server;
            GameClockTimescale = server.GameSettings.GameServerSettings.GameClockTimescale;
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

        public uint GetMoonPhase()
        {
            return GetMoonPhase(DateTimeOffset.UtcNow);
        }

        public uint GetMoonPhase(DateTimeOffset time)
        {
            ulong secondsPerLestanianDay = _Server.GameSettings.GameServerSettings.GameClockTimescale * 60;
            ulong secondsPerMoonAge = MoonAgeLoopSec / GameTimeMoonAges;
            ulong secondsElapsed = (ulong)(time.ToUnixTimeSeconds() - OriginalRealTimeSec);

            // This is how the android app calculates this. Is this exploiting some integer math trickery?
            ulong offsetMoonTime = (secondsElapsed + secondsPerLestanianDay / 2) / secondsPerLestanianDay * secondsPerLestanianDay; 

            return (uint)(offsetMoonTime / secondsPerMoonAge) % GameTimeMoonAges;
        }

        public List<CDataWeatherForecast> GetForecast()
        {
            List<CDataWeatherForecast> forecast = new List<CDataWeatherForecast>();
            
            // All weather weights are 0, so the weather is always Fine.
            if (WeatherLoopTotalLength == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    forecast.Add(new CDataWeatherForecast()
                    {
                        Weather = Weather.Fine
                    });
                }
                return forecast;
            }

            // The first entry on the forecast is always the next Lestanian hour.
            // Weather changes are set up on the hour, so we go one second over to see what's coming up.
            DateTimeOffset forecastTime = GetTimeForNextGameHour().AddSeconds(1);
            forecast.Add(new CDataWeatherForecast()
            {
                Weather = GetWeather(forecastTime)
            });

            uint forecastInterval = ForecastIntervalGameHour;
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
            uint offsetSecondsToNextHour = (uint)((1 - (currentTimeHour % 1)) * 3600);

            return now.AddSeconds(offsetSecondsToNextHour);
        }

        // Adapted from the client's code
        public long RealTimeToGameTimeMS(DateTimeOffset realTime)
        {
            return 1440 * (realTime.Millisecond + 1000 * (realTime.ToUnixTimeSeconds() - OriginalRealTimeSec)) / GameClockTimescale
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

            uint seqLength = _Server.GameSettings.GameServerSettings.WeatherSequenceLength;
            List<(uint MeanLength, uint Weight)> seqStats = _Server.GameSettings.GameServerSettings.WeatherStatistics;

            if (!seqStats.Where(x => x.Weight > 0).Any())
            {
                Logger.Error($"All weather weights zero; defaulting to always Fine weather.");
                WeatherLoopList = weatherLoop;
                WeatherLoopTotalLength = 0;
                return;
            }

            // Some setup for the randomization.
            List<(Weather Weather, uint MeanLength, uint AccumulatedWeight)> cumStats = new List<(Weather weather, uint meanLength, uint accumulatedWeight)>();
            uint accumulatedWeight = 0;
            for (int i = 0; i < seqStats.Count; i++)
            {
                accumulatedWeight += seqStats[i].Weight;
                cumStats.Add((
                    (Weather)(i + 1),
                    seqStats[i].MeanLength,
                    accumulatedWeight
                ));
            }

            ulong totalLength = 0;
            for (int i = 0; i < seqLength; i++)
            {
                // Find the next weather type.
                double rngWeight = Random.Shared.NextDouble() * accumulatedWeight;
                (Weather weather, uint meanLength, uint _) = cumStats.Find(x => x.AccumulatedWeight >= rngWeight);

                // Calculate how long it should last, using an exponential distribution.
                double rngUniformLength = Random.Shared.NextDouble();
                double rngExpLength = -Math.Log(rngUniformLength) * meanLength;

                // Round to the nearest Lestanian hour.
                uint gameHourSeconds = GameClockTimescale * 60 / 24;
                rngExpLength = Math.Round(rngExpLength / gameHourSeconds) * gameHourSeconds;
                if (rngExpLength == 0) rngExpLength = gameHourSeconds;

                // Add it to the list
                weatherLoop.Add(new CDataWeatherLoop()
                {
                    WeatherId = weather,
                    TimeSec = (uint)rngExpLength
                });

                totalLength += (uint)rngExpLength;

                Logger.Debug($"Weather Step {i + 1}/{seqLength}: {weather}, {rngExpLength} seconds");
            }

            Logger.Debug($"Total Weather Cycle: {totalLength / 3600.0:f} (real) hours.");

            WeatherLoopList = weatherLoop;
            WeatherLoopTotalLength = totalLength;
        }
    }
}
