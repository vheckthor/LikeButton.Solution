using LikeButton.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LikeButton.Tests.Infrastructure.Helpers
{
    public class DbHelpers
    {
        public static AppDbContext InitContext(string DBName)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();

            builder.UseInMemoryDatabase(DBName) // doesn't suport transactions and sql statement , use InitContextWithTransactionAndSQLSupport
                                                // Don't raise the error warning us that the in memory db doesn't support transactions
                   .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            return new AppDbContext(builder.Options);
        }

        public static AppDbContext InitContextWithTransactionAndSQLSupport()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }


        public static void DetachAllEntities(AppDbContext context)
        {
            var changedEntriesCopy = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public static int NumberOfTrackedEntities(AppDbContext context)
        {
            return context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted ||
                            e.State == EntityState.Unchanged)
                .Count();

        }


        public static int NumberOfEntitiesWithAddedEntityState(AppDbContext context)
        {
            return context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Count();
        }

        public static IList<T> AddedEntities<T>(AppDbContext context) where T : class
        {
            return context.ChangeTracker.Entries<T>()
                .Where(e => e.State == EntityState.Added)
                .Select(x => x.Entity)
                .ToList();
        }

        public static IList<T> ModifiedEntities<T>(AppDbContext context) where T : class
        {
            return context.ChangeTracker.Entries<T>()
                .Where(e => e.State == EntityState.Modified)
                .Select(x => x.Entity)
                .ToList();
        }

        public static IList<T> RemovedEntities<T>(AppDbContext context) where T : class
        {
            return context.ChangeTracker.Entries<T>()
                .Where(e => e.State == EntityState.Deleted)
                .Select(x => x.Entity)
                .ToList();
        }
    }
}
