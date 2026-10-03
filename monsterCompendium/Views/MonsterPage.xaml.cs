using Microsoft.Maui.Controls;
using monsterCompendium.ViewModels;

namespace monsterCompendium.Views
{
    public partial class MonsterPage : ContentPage
    {
        public MonsterPage()
        {
            InitializeComponent();
        }

        public MonsterPage(MonstersViewModel viewModel) : this()
        {
            BindingContext = viewModel;
        }
    }
}
