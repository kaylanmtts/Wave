using Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wave.Api.Services;
using Wave.Domain.Entities;

namespace Wave.Api.Controllers
{
    [Route("v1/albums")]
    public class AlbumController : Controller
    {
        // GET: v1/albums
        /// <summary>
        /// Gets the list of all albums.
        /// </summary>
        /// <param name="context">The data context to retrieve album data.</param>
        /// <returns>A list of albums.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Album>>> Get([FromServices] DataContext context)
        {
            return Ok(await new AlbumService(context).Get());
        }

        // GET: v1/albums/{id}
        /// <summary>
        /// Gets a specific album by its ID.
        /// </summary>
        /// <param name="context">The data context to retrieve album data.</param>
        /// <param name="id">The ID of the album to retrieve.</param>
        /// <returns>The album with the specified ID.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Album>> GetById([FromServices] DataContext context, int id)
        {
            return Ok(await new AlbumService(context).GetById(id));
        }

        // POST: v1/albums
        /// <summary>
        /// Creates a new album record.
        /// </summary>
        /// <param name="model">The album model to create.</param>
        /// <param name="context">The data context to save the album data.</param>
        /// <returns>The created album record.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Album>> Post([FromBody] Album model, [FromServices] DataContext context)
        {
            await new AlbumService(context).Post(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: v1/albums
        /// <summary>
        /// Updates an existing album record.
        /// </summary>
        /// <param name="model">The album model with updated data.</param>
        /// <param name="context">The data context to update the album data.</param>
        /// <returns>The updated album record.</returns>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Album>> Put([FromBody] Album model, [FromServices] DataContext context)
        {
            return Ok(await new AlbumService(context).Put(model));
        }

        // DELETE: v1/albums/{id}
        /// <summary>
        /// Deletes a specific album record by its ID.
        /// </summary>
        /// <param name="id">The ID of the album to delete.</param>
        /// <param name="context">The data context to remove the album record.</param>
        /// <returns>A confirmation message indicating the deletion was successful.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Album>> Delete(int id, [FromServices] DataContext context)
        {
            await new AlbumService(context).Delete(id);
            return Ok($"Album with ID {id} deleted successfully.");
        }
    }
}
