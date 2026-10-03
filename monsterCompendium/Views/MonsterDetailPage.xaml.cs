using monsterCompendium.Models;
using monsterCompendium.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace monsterCompendium.Views
{
    public partial class MonsterDetailPage : ContentPage
    {
        public MonsterDetailPage(MonsterDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}
