using Microsoft.Maui.Controls;
using monsterCompendium.ViewModels;

namespace monsterCompendium.Views
{
    public partial class FavoritePage : ContentPage
    {
        private readonly FavoritesViewModel _viewModel;

        public FavoritePage(FavoritesViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.LoadFavoritesCommand.CanExecute(null))
            {
                _viewModel.LoadFavoritesCommand.Execute(null);
            }
        }
    }
}
