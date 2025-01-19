using Infra;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using Wave.Domain.Entities;

namespace Wave.Api.Services
{
    public class MusicService
    {
        private readonly DataContext _context;

        public MusicService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Music>> Get()
        {
            var musics = await _context.Musics.AsNoTracking().ToListAsync();
            return musics;
        }

        public async Task<Music> GetById(int id)
        {
            var music = await _context.Musics.FindAsync(id);

            if (music == null)
            {
                throw new KeyNotFoundException($"Music with ID {id} not found.");
            }

            return music;
        }

        public async Task<List<Music>> GetByArtistId(int artistId)
        {
            var musics = await _context.Musics.Where(m => m.ArtistId == artistId).AsNoTracking().ToListAsync();
            return musics;
        }

        public async Task<List<Music>> GetByArtistName(string artistName)
        {
            try
            {
                var artist = await _context.Artists.AsNoTracking().FirstOrDefaultAsync(a => a.Name.ToLower() == artistName.ToLower());
                var musics = await _context.Musics.Where(m => m.ArtistId == artist.Id).AsNoTracking().ToListAsync();
                return musics;

            }
            catch (Exception)
            {
                throw new Exception($"Artist \"{artistName}\" does not exist.");
            }
        }

        public async Task Post(Music model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Music model is null");
            }

            // Verificar se o álbum existe antes de adicionar a música
            var album = await _context.Albums.FindAsync(model.AlbumId);

            if (album == null)
            {
                throw new Exception($"Album with ID {model.AlbumId} not found.");
            }

            // Criador do album = mesmo criador da musica
            model.ArtistId = album.ArtistId;

            // Adicionar a música no banco de dados
            _context.Musics.Add(model);

            // Salvar as alterações no banco de dados
            await _context.SaveChangesAsync();

            // Atualizar a duração total do álbum
            await UpdateAlbum(model.AlbumId);
        }

        public async Task<Music> Put(Music model)
        {
            var album = await _context.Albums.FindAsync(model.AlbumId);
            model.ArtistId = album.ArtistId;
            _context.Entry<Music>(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            await UpdateAlbum(model.AlbumId);
            return model;
        }

        public async Task Delete(int id)
        {
            var music = await _context.Musics.FindAsync(id);
            _context.Musics.Remove(music);
            await _context.SaveChangesAsync();
            await UpdateAlbum(music.AlbumId);
        }

        public async Task UpdateAlbum(int albumId)
        {
            // Buscar as músicas associadas ao álbum pelo albumId
            var musics = await _context.Musics
                .Where(m => m.AlbumId == albumId)
                .ToListAsync();

            // Buscar o álbum
            var album = await _context.Albums.FindAsync(albumId);

            if (album != null)
            {
                // Calcular o total de duração somando as durações das músicas
                album.TotalDuration = musics.Sum(m => m.Duration);
                album.TotalMusics = musics.Count;

                // Atualizar o álbum
                _context.Albums.Update(album);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the album doesn't exist
                // For example, throw an exception or log the error
                throw new Exception($"Album with ID {albumId} not found.");
            }
        }
    }
}
