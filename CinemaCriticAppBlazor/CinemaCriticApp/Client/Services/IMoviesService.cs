using CinemaCriticApp.Client.Models;

namespace CinemaCriticApp.Client.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetPopularMovies();
        Task<IEnumerable<Movie>> GetMoviesByName(string name);
    }
}
