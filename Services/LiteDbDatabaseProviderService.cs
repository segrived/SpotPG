using System;
using System.IO;
using LiteDB;
using SpotPG.Services.Abstractions;

namespace SpotPG.Services
{
    public class LiteDbDatabaseProviderService : IDatabaseProviderService
    {
        public ILiteDatabase Database { get; }

        public LiteDbDatabaseProviderService()
        {
            string dbPath = Environment.GetEnvironmentVariable("SPOTPG_DB_PATH") ?? "config.db";

            string dbDir = Path.GetDirectoryName(dbPath);
            if (dbDir != null && !Directory.Exists(dbDir))
                Directory.CreateDirectory(dbDir);

            this.Database = new LiteDatabase(dbPath);
        }
    }
}