using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.Models;
using monsterCompendium.Services;

namespace monsterCompendium.Tests.Fakes
{
    public class FakeFavoriteRepository : IFavoriteRepository
    {
        private readonly List<FavoriteMonster> _favorites = new();

        public Task<List<FavoriteMonster>> GetAllAsync()
        {
            return Task.FromResult(_favorites.ToList());
        }

        public Task<bool> ExistsAsync(string index)
        {
            bool exists = _favorites.Any(favorite => favorite.Index == index);

            return Task.FromResult(exists);
        }

        public Task AddAsync(string index)
        {
            bool alreadyExists = _favorites.Any(
                favorite => favorite.Index == index);

            if (!alreadyExists)
            {
                _favorites.Add(new FavoriteMonster
                {
                    Index = index
                });
            }

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string index)
        {
            var favorite = _favorites.FirstOrDefault(
                favorite => favorite.Index == index);

            if (favorite != null)
            {
                _favorites.Remove(favorite);
            }

            return Task.CompletedTask;
        }
    }
}
