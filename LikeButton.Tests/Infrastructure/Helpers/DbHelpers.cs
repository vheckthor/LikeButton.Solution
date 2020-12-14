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

            builder.UseInMemoryDatabase(DBName) 
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


    }
}
