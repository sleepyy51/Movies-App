using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using RawGames.Model;
using System.Net.WebSockets;

namespace RawGames.Services
{
    public class MoviesApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.themoviedb.org/3";
        private const string ApiKey = "6e2bb10d03b2eacf2edda685914d5486";

        private readonly JsonSerializerOptions _jsonOptions;

        public MoviesApiService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<MovieResponse?> GetMoviesAsync(int currentPage)
        {
            try
            {
                string url = $"{BaseUrl}/movie/popular?api_key={ApiKey}&language=es-MX&page={currentPage}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<MovieResponse>(json, _jsonOptions);
                }
                else
                {
                    Console.WriteLine($"Error en GetMoviesAsync: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching movies: {ex.Message}");
                return null;
            }
        }

        public async Task<MovieResponse?> SearchMoviesAsync(string query, int page = 1, int pageSize = 20)
        {
            try
            {
                // Si no hay búsqueda, regresa películas populares
                if (string.IsNullOrWhiteSpace(query))
                    return await GetMoviesAsync(page);

                string url =
                    $"{BaseUrl}/search/movie?api_key={ApiKey}&language=es-MX&query={Uri.EscapeDataString(query)}&page={page}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<MovieResponse>(json, _jsonOptions);
                }
                else
                {
                    Console.WriteLine($"Error en SearchMoviesAsync: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching movies: {ex.Message}");
                return null;
            }
        }
    }


}