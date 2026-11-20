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

        // Si es null → usamos la ruta productiva
        // Si trae valor → lo usan los tests
        private readonly string? _customDatabasePath;

        // Constructor de PRODUCCIÓN
        public DatabaseService()
        {
        }

        // Constructor para TESTS
        public DatabaseService(string databasePath)
        {
            _customDatabasePath = databasePath;
        }

        public async Task InitializeAsync()
        {
            if (_database is not null)
                return;

            var databasePath = _customDatabasePath
                ?? Path.Combine(FileSystem.AppDataDirectory, "dndcompendium.db3");

            _database = new SQLiteAsyncConnection(databasePath);

            await _database.CreateTableAsync<FavoriteMonster>();
        }

        public async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            await InitializeAsync();

            return _database!;
        }

        // Cierra la conexión y, en :memory:, destruye la base.
        public async Task CloseAsync()
        {
            if (_database is null)
                return;

            await _database.CloseAsync();

            _database = null;
        }
    }
}
