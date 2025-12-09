using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RawGames.Model
{
    public class MovieResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }

        [JsonPropertyName("results")]
        public List<Game> Results { get; set; } = new();
    }


    public class Game
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("overview")]
        public string Overview { get; set; } = string.Empty;

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; } = string.Empty;

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        // ✔ ESTA ES LA QUE TU XAML NECESITA
        public string BackgroundImage => $"https://image.tmdb.org/t/p/w500{PosterPath}";

        // (Opcional) Puedes dejar FullPoster si quieres
        public string FullPoster => $"https://image.tmdb.org/t/p/w500{PosterPath}";

        public string VoteAverageFormatted =>
            VoteAverage > 0 ? $"Puntuación: {VoteAverage}" : "Sin puntuación";
    }


    public class GameDetail : Game
    {
        [JsonPropertyName("runtime")]
        public int Runtime { get; set; }

        [JsonPropertyName("tagline")]
        public string Tagline { get; set; } = string.Empty;
    }

}
