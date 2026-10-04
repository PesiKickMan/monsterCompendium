using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Models;

namespace monsterCompendium.Services
{
    public interface IFavoriteRepository
    {
        Task<List<FavoriteMonster>> GetAllAsync();

        Task<bool> ExistsAsync(string index);

        Task AddAsync(string index);

        Task RemoveAsync(string index);
    }
}
