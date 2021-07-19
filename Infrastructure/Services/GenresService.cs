using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;

namespace Infrastructure.Services
{
    public class GenresService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenresService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<GenreModel>> GetAllGenres()
        {
            var genres = await _genreRepository.ListAllAsync();

            var genresModel = new List<GenreModel>();

            foreach (var genre in genres)
            {
                genresModel.Add(new GenreModel { Id = genre.Id, Name = genre.Name });
            }
            return genresModel;
        }

       


    }
}