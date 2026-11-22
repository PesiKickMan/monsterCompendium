using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace monsterCompendium.Models
{
    public partial class FavoriteItem : ObservableObject
    {
        public Monster Monster { get; }

        [ObservableProperty]
        private string? photoPath;

        public FavoriteItem(Monster monster, string? photoPath)
        {
            Monster = monster;
            PhotoPath = photoPath;
        }
    }
}
