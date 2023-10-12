using CinemaCriticApp.Shared;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaCriticAppRepositories
{
    public class CinemaCriticAppRepository : ICinemaCriticAppRepository
    {
        private readonly IDbConnection _dbConnection;

        public CinemaCriticAppRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task DeleteComment(int id)
        {
            var sql = @" DELETE FROM Comments WHERE Id = @id";

            var result = await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            var sql = @" SELECT Id
                                ,Title
                                ,Rating
                                ,Critic
                        FROM Comments";

            return await _dbConnection.QueryAsync<Comment>(sql, new {});
        }

        public async Task<bool> InsertComment(Comment comment)
        {
            try
            {
                var sql = @"INSERT INTO Comments(Title, Rating, Critic)
                            VALUES(@Title, @Rating, @Critic)";

                var result = await _dbConnection.ExecuteAsync(
                    sql, new
                    {
                        comment.Title,
                        comment.Rating,
                        comment.Critic
                    });

                return result > 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
