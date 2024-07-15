using System.Linq;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class DatabaseMigrator
    {
        private readonly List<IMigrationStrategy> MigrationStrategies;

        public DatabaseMigrator(List<IMigrationStrategy> strategies)
        {
            MigrationStrategies = strategies;
        }

        public bool MigrateDatabase(IDatabase db, uint fromVersion, uint toVersion)
        {
            return db.ExecuteInTransaction((conn) => {
                foreach (var strategy in FindStrategies(fromVersion, toVersion))
                {
                    strategy.Migrate(db, conn);
                }
            });
        }

        private List<IMigrationStrategy> FindStrategies(uint fromVersion, uint toVersion)
        {
            return FindStrategies(fromVersion, toVersion, out bool _);
        }

        // Recursively find a list of strategies to ugprade the database from fromVersion to toVersion
        private List<IMigrationStrategy> FindStrategies(uint fromVersion, uint toVersion, out bool done)
        {
            if(fromVersion == toVersion)
            {
                done = true;
                return new List<IMigrationStrategy>();
            }

            // Get all strategies that upgrade from fromVersion to toVersion
            IEnumerable<IMigrationStrategy> strategiesFromVersion = MigrationStrategies
                .Where(strategy => strategy.From == fromVersion);

            List<IMigrationStrategy>? candidateStrategies = null;
            foreach (var strategy in strategiesFromVersion)
            {
                List<IMigrationStrategy> nextStrategies = FindStrategies(strategy.To, toVersion, out bool nextDone);
                if(nextDone && (candidateStrategies == null || (nextStrategies.Count+1) < candidateStrategies.Count))
                {
                    candidateStrategies = new List<IMigrationStrategy>() { strategy }.Concat(nextStrategies).ToList();;
                }
            }

            if (candidateStrategies != null)
            {
                done = true;
                return candidateStrategies;
            }
            else
            {
                done = false;
                return new List<IMigrationStrategy>();
            }
        }
    }
}
