using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Models;

namespace monsterCompendium.Services
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly DatabaseService _databaseService;

        public FavoriteRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<FavoriteMonster>> GetAllAsync()
        {
            var database = await _databaseService.GetConnectionAsync();

            return await database
                .Table<FavoriteMonster>()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(string index)
        {
            var database = await _databaseService.GetConnectionAsync();

            var favorite = await database
                .Table<FavoriteMonster>()
                .FirstOrDefaultAsync(f => f.Index == index);

            return favorite is not null;
        }

        public async Task AddAsync(string index)
        {
            var database = await _databaseService.GetConnectionAsync();

            var exists = await ExistsAsync(index);

            if (exists)
                return;

            var favorite = new FavoriteMonster
            {
                Index = index
            };

            await database.InsertAsync(favorite);
        }

        public async Task RemoveAsync(string index)
        {
            var database = await _databaseService.GetConnectionAsync();

            var favorite = await database
                .Table<FavoriteMonster>()
                .FirstOrDefaultAsync(f => f.Index == index);

            if (favorite is null)
                return;

            await database.DeleteAsync(favorite);
        }
    }
}
