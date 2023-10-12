using CinemaCriticApp.Shared;
using CinemaCriticAppRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaCriticApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICinemaCriticAppRepository _cinemaCriticAppRepository; 
        
        public CommentsController(ICinemaCriticAppRepository cinemaCriticAppRepository)
        {
            _cinemaCriticAppRepository = cinemaCriticAppRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Comment comment)
        {
            if(comment == null)
            {
                return BadRequest();
            }
            if(string.IsNullOrEmpty(comment.Title))
            {
                ModelState.AddModelError("Title", "El título no puede estar vacío");
            }
            if (string.IsNullOrEmpty(comment.Critic))
            {
                ModelState.AddModelError("Critic", "La crítica no puede estar vacía");
            }
            if(string.IsNullOrEmpty(comment.Rating.ToString())) 
            {
                ModelState.AddModelError("Rating", "La crítica tiene que llevar asociada una puntuación");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _cinemaCriticAppRepository.InsertComment(comment);

            return NoContent();
        }

        [HttpGet]
        public async Task<IEnumerable<Comment>> Get()
        {
            return await _cinemaCriticAppRepository.GetAllComments();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _cinemaCriticAppRepository.DeleteComment(id);
        }
    
    }

  

}
