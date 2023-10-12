using Newtonsoft.Json;

namespace CinemaCriticApp.Client.Models
{
    public class MovieResponse
    {
        [JsonProperty("results")]
        public List<Movie> Results { get; set;}
    }
    public class Movie
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("poster_path")]
        public string? Posterpath { get; set; }

        [JsonProperty("adult")]
        public Boolean Adult { get; set; }

        [JsonProperty("overview")]
        public string? Overview { get; set; }

        [JsonProperty("release_Date")]
        public string? ReleaseDate { get; set; }

        [JsonProperty("original_Title")]
        public string? OriginalTitle { get; set; }

        [JsonProperty("original_language")]
        public string? OriginalLanguage { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("backdrop_path")]
        public string? Backdrop_Path { get; set; }

        [JsonProperty("popularity")]
        public float Popularity { get; set; }

        [JsonProperty("vote_average")]
        public float VoteAverage { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }
    
    }
}
