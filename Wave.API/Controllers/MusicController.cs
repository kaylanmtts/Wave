using Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Wave.Api.Services;
using Wave.Domain.Entities;

namespace Wave.API.Controllers
{
    [Route("v1/musics")]
    public class MusicController : Controller
    {
        // GET: v1/musics
        /// <summary>
        /// Gets the list of all musics.
        /// </summary>
        /// <param name="context">The data context to retrieve music data.</param>
        /// <returns>The list of musics.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Music>>> Get([FromServices] DataContext context)
        {
            return Ok(await new MusicService(context).Get());
        }

        // GET: v1/musics/{id}
        /// <summary>
        /// Gets a specific music by its ID.
        /// </summary>
        /// <param name="context">The data context to retrieve music data.</param>
        /// <param name="id">The ID of the music to retrieve.</param>
        /// <returns>The music with the specified ID.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Music>> GetById([FromServices] DataContext context, int id)
        {
            return Ok(await new MusicService(context).GetById(id));
        }

        // GET: v1/musics/GetByArtist/{id}
        /// <summary>
        /// Gets a list of musics by the artist's ID.
        /// </summary>
        /// <param name="context">The data context to retrieve music data.</param>
        /// <param name="id">The ID of the artist to filter the music by.</param>
        /// <returns>A list of musics by the artist with the specified ID.</returns>
        [HttpGet]
        [Route("GetByArtist/{id:int}")]
        public async Task<ActionResult<List<Music>>> GetByArtistId([FromServices] DataContext context, int id)
        {
            return Ok(await new MusicService(context).GetByArtistId(id));
        }

        // GET: v1/musics/GetByArtist/{name}
        /// <summary>
        /// Gets a list of musics by the artist's name.
        /// </summary>
        /// <param name="context">The data context to retrieve music data.</param>
        /// <param name="name">The name of the artist to filter the music by.</param>
        /// <returns>A list of musics by the artist with the specified name.</returns>
        [HttpGet]
        [Route("GetByArtist/{name}")]
        public async Task<ActionResult<List<Music>>> GetByArtistName([FromServices] DataContext context, string name)
        {
            return Ok(await new MusicService(context).GetByArtistName(name));
        }

        // POST: v1/musics
        /// <summary>
        /// Creates a new music record.
        /// </summary>
        /// <param name="model">The music model to create.</param>
        /// <param name="context">The data context to save the music data.</param>
        /// <returns>The created music record.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Music>> Post([FromBody] Music model, [FromServices] DataContext context)
        {
            await new MusicService(context).Post(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: v1/musics
        /// <summary>
        /// Updates an existing music record.
        /// </summary>
        /// <param name="model">The music model with updated data.</param>
        /// <param name="context">The data context to update the music data.</param>
        /// <returns>The updated music record.</returns>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Music>> Put([FromBody] Music model, [FromServices] DataContext context)
        {
            return Ok(await new MusicService(context).Put(model));
        }

        // DELETE: v1/musics/{id}
        /// <summary>
        /// Deletes a specific music record by its ID.
        /// </summary>
        /// <param name="id">The ID of the music to delete.</param>
        /// <param name="context">The data context to remove the music record.</param>
        /// <returns>A confirmation message indicating the deletion was successful.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id, [FromServices] DataContext context)
        {
            await new MusicService(context).Delete(id);
            return Ok($"Music with ID {id} deleted successfully.");
        }
    }
}
