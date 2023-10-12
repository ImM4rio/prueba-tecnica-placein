using CinemaCriticApp.Client.Models;
using Newtonsoft.Json;
using System.Net;

namespace CinemaCriticApp.Client.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly HttpClient _httpClient;
        public MoviesService(HttpClient httpClient) 
        {
            _httpClient = httpClient;

        }
        public async Task<IEnumerable<Movie>> GetPopularMovies()
        {
            var jsonResponse = await _httpClient.GetStringAsync("movie/popular?api_key=8f781d70654b5a6f2fa69770d1d115a3");
            var movieResponse = JsonConvert.DeserializeObject<MovieResponse>(jsonResponse);
            return movieResponse.Results;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByName(string name)
        {
            var queryName = WebUtility.UrlEncode(name);
            var jsonResponse = await _httpClient.GetStringAsync($"search/movie?query={queryName}&api_key=8f781d70654b5a6f2fa69770d1d115a3");
            var movieResponse = JsonConvert.DeserializeObject<MovieResponse> (jsonResponse);
            return movieResponse.Results;
        }
    }
}
