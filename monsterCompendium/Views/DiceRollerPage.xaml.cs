using System;
using System.Collections.Generic;
using System.Text;
using monsterCompendium.ViewModels;

namespace monsterCompendium.Views
{
    public partial class DiceRollerPage : ContentPage
    {
        private readonly DiceRollerViewModel _viewModel;

        public DiceRollerPage(DiceRollerViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.StartListening();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            _viewModel.StopListening();
        }
    }
}
