using Infra;
using Microsoft.EntityFrameworkCore;
using Wave.Domain.Entities;

namespace Wave.Api.Services
{
    public class ArtistService
    {
        private readonly DataContext _context; 

        public ArtistService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Artist>> Get()
        {
            var artists = await _context.Artists.AsNoTracking().ToListAsync();
            return artists;
        }

        public async Task<Artist> GetById(int id)
        {
            var artist = await _context.Artists.FindAsync(id);

            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {id} not found.");
            }

            return artist;
        }

        public async Task Post(Artist model)
        {
            // Verifica se o modelo do artista é nulo e lança uma exceção se for o caso.
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Artist model is null");
            }

            // Verifica se já existe um artista com o mesmo nome.
            var existingArtist = await _context.Artists.FirstOrDefaultAsync(
                x => x.Name.ToLower().Equals(model.Name.ToLower()));

            // Se um artista com o mesmo nome já existe, lança uma exceção.
            if (existingArtist != null)
            {
                throw new Exception($"Artist with name \"{model.Name}\" already exists.");
            }
            _context.Artists.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Artist> Put(Artist model)
        {
            _context.Entry<Artist>(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return model;
        }
        public async Task Delete(int id)
        {
            var artist = await _context.Artists.FindAsync(id);

            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with ID \"{id}\" does not exist.");
            }
            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
        }
    }
}
