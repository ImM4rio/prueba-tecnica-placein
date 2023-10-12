using CinemaCriticApp.Shared;
using System.Net.Http.Json;

namespace CinemaCriticApp.Client.Services
{
    public class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;
        public CommentService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7061/") };
        }
        public async Task DeleteComment(int id)
        {
            await _httpClient.DeleteAsync($"api/comments/{id}");
        }

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<Comment>>($"api/comments");
        }

        public async Task SaveComment(Comment comment)
        {
            if(comment.Id == 0)
            {
                //insert
                await _httpClient.PostAsJsonAsync<Comment>($"api/comments", comment);
            }
        }
    }
}
