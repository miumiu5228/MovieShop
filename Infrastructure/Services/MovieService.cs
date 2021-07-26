using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;

namespace Infrastructure.Services
{

    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<MovieCardResponseModel>> GetTopRevenueMovies()
        {
            var movies = await _movieRepository.GetHighest30GrossingMovies();

            var movieCards = new List<MovieCardResponseModel>();

            foreach (var movie in movies)
            {
                movieCards.Add(new MovieCardResponseModel { Id = movie.Id, Budget = movie.Budget.GetValueOrDefault(), PosterUrl = movie.PosterUrl, Title = movie.Title });
            }

            return movieCards;
        }

        public  async Task<List<MovieCardResponseModel>> GetfilterGenres(int id)
        {
            var movies = await _movieRepository.GetMoviesByGenre(id);

            var movieCards = new List<MovieCardResponseModel>();

            foreach (var movie in movies)
            {
                foreach(var genre in movie.Genres)
                {
                   if(genre.Id == id)
                    {
                        movieCards.Add(new MovieCardResponseModel { Id = movie.Id, Budget = movie.Budget.GetValueOrDefault(), PosterUrl = movie.PosterUrl, Title = movie.Title });
                    }
                }
            } 

            return movieCards;

        }

        public async Task<MovieDetailsResponseModel> GetMovieDetails(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            var movieDetails = new MovieDetailsResponseModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Budget = movie.Budget.GetValueOrDefault(),
                PosterUrl = movie.PosterUrl,
                Overview = movie.Overview,
                Rating = movie.Rating,
                RunTime = movie.RunTime,
                ReleaseDate = movie.ReleaseDate,
                BackdropUrl = movie.BackdropUrl,
                Tagline = movie.Tagline,
                ImdbUrl = movie.ImdbUrl,
                Price = movie.Price,
                Revenue = movie.Revenue
                

            };

            movieDetails.MovieCardResponseModel = new MovieCardResponseModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Budget = movie.Budget.GetValueOrDefault(),
                PosterUrl = movie.PosterUrl
            };



            movieDetails.Casts = new List<CastResponseModel>();

            foreach (var cast in movie.MovieCasts)
            {
                movieDetails.Casts.Add(new CastResponseModel
                {
                    Id = cast.CastId,
                    Name = cast.Cast.Name,
                    Character = cast.Character,
                    ProfilePath = cast.Cast.ProfilePath,
                    
                    
                    
                });
            }

            movieDetails.Genres = new List<GenreModel>();
            foreach (var genre in movie.Genres)
            {
                movieDetails.Genres.Add(
                    new GenreModel
                    {
                        Id = genre.Id,
                        Name = genre.Name
                    }
                    );
            }

            

            return movieDetails;
        }

        public async Task<List<MovieCardResponseModel>> GetMovieByCast(int id)
        {
            var movieCasts = await _movieRepository.GetMoviesCast();
            var movieCards = new List<MovieCardResponseModel>();

            foreach (var movieCast in movieCasts)
            {
                
                foreach(var cast in movieCast.MovieCasts)
                {
                    if(cast.CastId == id)
                    {
                        
                        movieCards.Add(new MovieCardResponseModel { Id = movieCast.Id, Budget = movieCast.Budget.GetValueOrDefault(), PosterUrl = movieCast.PosterUrl, Title = movieCast.Title }) ;
                    }
                }
            }

            return movieCards;

            
        }

        public async Task<List<MovieCardResponseModel>> GetTopRatingMovies()
        {
            var dbMovies = await _movieRepository.GetTopRatedMovies();

            var movies = new List<MovieCardResponseModel>();
            foreach (var movie in dbMovies)
            {
                movies.Add(new MovieCardResponseModel
                {
                    Id = movie.Id,
                    Budget = movie.Budget.GetValueOrDefault(),
                    PosterUrl = movie.PosterUrl,
                    Title = movie.Title,
                    Rating = movie.Rating,
                });
            }
            return movies;
        }

        public async Task<List<MovieReviewsModel>> GetMovieReviews(int id)
        {
            var movie = await _movieRepository.GetMovieReviews(id);

            var movieReviews = new List<MovieReviewsModel>();
            foreach (var review in movie.Reviews)
            {
                movieReviews.Add(new MovieReviewsModel { UserId = review.UserId, ReviewText = review.ReviewText, Rating = review.Rating});
            }

            return movieReviews;
        }

        public async Task<MovieCardResponseModel> CreateMovie(MovieCreateResquestModel movie)
        {
            var newMovie = await _movieRepository.AddAsync(new Movie
            {
                Title = movie.Title,
                PosterUrl = movie.PosterUrl,
                BackdropUrl = movie.BackdropUrl,
                Overview = movie.Overview,
                Tagline = movie.Tagline,
                Budget = movie.Budget,
                Revenue = movie.Revenue,
                ImdbUrl = movie.ImdbUrl,
                TmdbUrl = movie.TmdbUrl,
                ReleaseDate = movie.ReleaseDate,
                RunTime = movie.RunTime,
                Price = movie.Price,
            });

            return new MovieCardResponseModel
            {
                Id = newMovie.Id,
                Budget = newMovie.Budget.GetValueOrDefault(),
                PosterUrl = newMovie.PosterUrl,
                Title = newMovie.Title,
               
            };
        }

        public async Task<MovieDetailsResponseModel> UpdateMovie(MovieUpdateRequestModel movie)
        {
            var dbMovie = await _movieRepository.GetByIdAsync(movie.Id);
            if (dbMovie == null)
            {
                throw new ConflictException("No movie exists");
            }

            dbMovie.Title = movie.Title;
            dbMovie.PosterUrl = movie.PosterUrl;
            dbMovie.BackdropUrl = movie.BackdropUrl;
            dbMovie.Overview = movie.Overview;
            dbMovie.Tagline = movie.Tagline;
            dbMovie.Budget = movie.Budget;
            dbMovie.Revenue = movie.Revenue;
            dbMovie.ImdbUrl = movie.ImdbUrl;
            dbMovie.TmdbUrl = movie.TmdbUrl;
            dbMovie.ReleaseDate = movie.ReleaseDate;
            dbMovie.RunTime = movie.RunTime;
            dbMovie.Price = movie.Price;

            var updatedDbMovie = await _movieRepository.UpdateAsync(dbMovie);

            return await GetMovieDetails(updatedDbMovie.Id);
        }
    }





}
