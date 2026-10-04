using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using monsterCompendium.Models;

namespace monsterCompendium.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        public async Task InitializeAsync()
        {
            if (_database is not null)
                return;

            var databasePath = Path.Combine(
                FileSystem.AppDataDirectory,
                "dndcompendium.db3");

            _database = new SQLiteAsyncConnection(databasePath);

            await _database.CreateTableAsync<FavoriteMonster>();
        }

        public async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            await InitializeAsync();

            return _database!;
        }
    }
}
