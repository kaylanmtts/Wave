using Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wave.Api.Services;
using Wave.Domain.Entities;

namespace Wave.Api.Controllers
{
    [Route("v1/artists")]
    public class ArtistController : Controller
    {
        // GET: v1/artists
        /// <summary>
        /// Gets the list of all artists.
        /// </summary>
        /// <param name="context">The data context to retrieve artist data.</param>
        /// <returns>A list of artists.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Artist>>> Get([FromServices] DataContext context)
        {
            return Ok(await new ArtistService(context).Get());
        }

        // GET: v1/artists/{id}
        /// <summary>
        /// Gets a specific artist by its ID.
        /// </summary>
        /// <param name="context">The data context to retrieve artist data.</param>
        /// <param name="id">The ID of the artist to retrieve.</param>
        /// <returns>The artist with the specified ID.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Artist>> GetById([FromServices] DataContext context, int id)
        {
            return Ok(await new ArtistService(context).GetById(id));
        }

        // POST: v1/artists
        /// <summary>
        /// Creates a new artist record.
        /// </summary>
        /// <param name="model">The artist model to create.</param>
        /// <param name="context">The data context to save the artist data.</param>
        /// <returns>The created artist record.</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Artist>> Post([FromBody] Artist model, [FromServices] DataContext context)
        {
            await new ArtistService(context).Post(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: v1/artists
        /// <summary>
        /// Updates an existing artist record.
        /// </summary>
        /// <param name="model">The artist model with updated data.</param>
        /// <param name="context">The data context to update the artist data.</param>
        /// <returns>The updated artist record.</returns>
        [HttpPut]
        [Authorize]
        public async Task<ActionResult<Artist>> Put([FromBody] Artist model, [FromServices] DataContext context)
        {
            return Ok(await new ArtistService(context).Put(model));
        }

        // DELETE: v1/artists/{id}
        /// <summary>
        /// Deletes a specific artist record by its ID.
        /// </summary>
        /// <param name="id">The ID of the artist to delete.</param>
        /// <param name="context">The data context to remove the artist record.</param>
        /// <returns>A confirmation message indicating the deletion was successful.</returns>
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Artist>> Delete(int id, [FromServices] DataContext context)
        {
            await new ArtistService(context).Delete(id);
            return Ok($"Artist with ID {id} deleted successfully.");
        }
    }
}
