using CinemaCriticApp.Shared;

namespace CinemaCriticApp.Client.Services
{
    public interface ICommentService
    {
        public Task DeleteComment(int id);
        public Task<IEnumerable<Comment>> GetAllComments();
        public Task SaveComment(Comment comment);

    }
}
