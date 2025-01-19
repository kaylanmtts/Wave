using Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wave.Domain.Entities;

namespace Wave.Api.Services
{
    public class AlbumService : Controller
    {
        private readonly DataContext _context;

        public AlbumService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Album>> Get()
        {
            var albums = await _context.Albums.AsNoTracking().ToListAsync();
            return albums;
        }

        public async Task<Album> GetById(int id)
        {
            var album = await _context.Albums.FindAsync(id);

            if (album == null)
            {
                throw new KeyNotFoundException($"Album with ID {id} not found.");
            }

            return album;
        }

        public async Task Post(Album model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Album model is null");
            }
            _context.Albums.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Album> Put(Album model)
        {
            var existingAlbum = await _context.Albums.FindAsync(model.Id);

            if (existingAlbum == null)
            {
                throw new KeyNotFoundException($"Album with ID {model.Id} not found.");
            }

            if (!string.IsNullOrEmpty(model.Name)) existingAlbum.Name = model.Name;
            if (await _context.Artists.FindAsync(model.ArtistId) != null) existingAlbum.ArtistId = model.ArtistId;

            _context.Entry(existingAlbum).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingAlbum;
        }


        public async Task Delete(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
        }
    }
}
