using Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wave.Api.Services;
using Wave.Domain.Entities;

namespace Wave.API.Controllers
{
    [Route("v1/users")]
    public class UserController : Controller
    {
        // GET: v1/users
        /// <summary>
        /// Gets the list of all users. Requires the "manager" role.
        /// </summary>
        /// <param name="context">The data context to retrieve user data.</param>
        /// <param name="model">The user model (not used in this endpoint).</param>
        /// <returns>A list of all users.</returns>
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context, [FromBody] User model)
        {
            var users = await context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        // POST: v1/users/create
        /// <summary>
        /// Creates a new user with a default role of "user".
        /// </summary>
        /// <param name="context">The data context to save the new user data.</param>
        /// <param name="model">The user model to be created.</param>
        /// <returns>The created user, with the password hidden.</returns>
        [HttpPost]
        [Route("create")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // Força o usuário a ser sempre "user"
                model.Role = "user";

                context.Users.Add(model);
                await context.SaveChangesAsync();

                // Esconde a senha
                model.Password = "";

                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Nao foi possivel criar o usuario" });
            }
        }

        // POST: v1/users/login
        /// <summary>
        /// Authenticates a user by username and password.
        /// </summary>
        /// <param name="context">The data context to retrieve the user data.</param>
        /// <param name="model">The user model containing the username and password for authentication.</param>
        /// <returns>A token if authentication is successful, otherwise an error message.</returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null) return NotFound(new { message = "Usuario ou senha invalidos" });
            var token = TokenService.GenerateToken(user);

            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        // PUT: v1/users/{id}
        /// <summary>
        /// Updates an existing user record by ID. Requires the "manager" role.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="model">The updated user model.</param>
        /// <param name="context">The data context to update the user data.</param>
        /// <returns>The updated user record.</returns>
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(int id, [FromBody] User model, [FromServices] DataContext context)
        {
            if (id != model.Id) return NotFound(new { message = "Usuario nao encontrado" });
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                context.Entry<User>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro ja foi atualizado" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Nao foi possivel atualizar o usuario" });
            }
        }
    }
}
