
using RawGames.Model;
using RawGames.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RawGames.ViewModel
{
    public class GameListViewModel : BaseViewModel
    {
        private readonly MoviesApiService _apiService;
        private bool _isLoadingMore = false;
        private string _searchText = string.Empty;
        private int _currentPage = 1;

        public ObservableCollection<Game> Games { get; } = new();

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    //Funcion asincrona de busqueda
                }
            }
        }

        public bool IsLoadingMore
        {
            get => _isLoadingMore;
            set => SetProperty(ref _isLoadingMore, value);
        }

        public ICommand LoadGamesCommand { get; }
        public ICommand LoadMoreGamesCommand { get; }
        public ICommand RefreshCommand { get; }

        public ICommand GameSelectedCommand { get; }

        public GameListViewModel(MoviesApiService apiService)
        {
            _apiService = apiService;
            Title = "Raw Games";

            LoadGamesCommand = new Command(async () => await LoadGamesAsync());
            LoadMoreGamesCommand = new Command(async () => await LoadMoreGamesAsync());
            RefreshCommand = new Command(async () => await RefreshGameAsync());
            GameSelectedCommand = new Command<Game>(OnGameSelected);

            // Cargar juegos al inicializar
            Task.Run(async () => await LoadGamesAsync());
        }

        private void OnGameSelected(Game game)
        {
            if (game == null)
                return;

            // Aquí puedes navegar a la página de detalles del juego
            // Por ahora solo mostramos un mensaje
            Application.Current?.MainPage?.DisplayAlert("Juego Seleccionado", game.Name, "OK");
        }

        private async Task LoadGamesAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                _currentPage = 1;
                Games.Clear();

                MovieResponse? response;

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    response = await _apiService.GetMoviesAsync(_currentPage);
                }
                else
                {
                    response = await _apiService.SearchMoviesAsync(SearchText, _currentPage);
                }

                if (response?.Results != null)
                {
                    foreach (var game in response.Results)
                    {
                        Games.Add(game);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar las peliculas: {ex.Message}");
                await Application.Current!.MainPage!.DisplayAlert("Error", "No se pudieron cargar las peliculas. Verifica la conexion a Internet.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task LoadMoreGamesAsync()
        {
            if (IsLoadingMore || IsBusy)
                return;
            try
            {
                IsLoadingMore = true;
                _currentPage++;
                MovieResponse? response;
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    response = await _apiService.GetMoviesAsync(_currentPage);
                }
                else
                {
                    response = await _apiService.SearchMoviesAsync(SearchText, _currentPage);
                }
                if (response?.Results != null)
                {
                    foreach (var game in response.Results)
                    {
                        Games.Add(game);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar mas peliculas: {ex.Message}");
            }
            finally
            {
                IsLoadingMore = false;

            }
        }
        private async Task RefreshGameAsync()
        {
            await LoadGamesAsync();
        }

        private async Task SearchGamesAsync()
        {
            await LoadGamesAsync();
        }
    }
}