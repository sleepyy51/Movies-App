using RawGames.ViewModel;

namespace RawGames.Views;

public partial class GameListPage : ContentPage
{
    public GameListPage(GameListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}