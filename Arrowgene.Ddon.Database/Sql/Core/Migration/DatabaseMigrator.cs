using System.Linq;
using System.Collections.Generic;
using System;
using Arrowgene.Ddon.Shared.Entity;

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
                List<IMigrationStrategy> strategies = FindStrategies(fromVersion, toVersion, out bool completesMigration);
                if(!completesMigration)
                {
                    throw new Exception("No migration possible from "+fromVersion+" to "+toVersion);
                }

                foreach (var strategy in strategies)
                {
                    try
                    {
                        if (strategy.DisableTransaction)
                        {
                            strategy.Migrate(db, db.OpenNewConnection());
                        }
                        else
                        {
                            db.ExecuteInTransaction(conn => strategy.Migrate(db, conn));
                        }
                    }
                    catch (Exception)
                    {
                        db.SetMeta(new DatabaseMeta { DatabaseVersion = strategy.From });
                        throw;
                    }
                }
                return true;
        }

        // Recursively find a list of strategies to ugprade the database from fromVersion to toVersion
        private List<IMigrationStrategy> FindStrategies(uint fromVersion, uint toVersion, out bool completesMigration)
        {
            if(fromVersion == toVersion)
            {
                completesMigration = true;
                return new List<IMigrationStrategy>();
            }

            // Get all strategies that upgrade from fromVersion to toVersion
            IEnumerable<IMigrationStrategy> strategiesFromVersion = MigrationStrategies
                .Where(strategy => strategy.From == fromVersion);

            List<IMigrationStrategy>? candidateStrategies = null;
            foreach (var strategy in strategiesFromVersion)
            {
                List<IMigrationStrategy> nextStrategies = FindStrategies(strategy.To, toVersion, out bool nextStrategiesCompleteMigration);
                if(nextStrategiesCompleteMigration && (candidateStrategies == null || (nextStrategies.Count+1) < candidateStrategies.Count))
                {
                    candidateStrategies = new List<IMigrationStrategy>() { strategy }.Concat(nextStrategies).ToList();;
                }
            }

            if (candidateStrategies != null)
            {
                completesMigration = true;
                return candidateStrategies;
            }
            else
            {
                completesMigration = false;
                return new List<IMigrationStrategy>();
            }
        }
    }
}
