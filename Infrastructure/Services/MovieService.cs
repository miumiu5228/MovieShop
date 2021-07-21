using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<List<MovieReviewsModel>> GetTopRatingMovies()
        {
            var movies = await _movieRepository.GetMoviesByTopRating();

            foreach (var m in movies)
            {
                var movie = await _movieRepository.GetByIdAsync(m.Id);
                m.Rating = movie.Rating;
            }

            var movieReview = new List<MovieReviewsModel>();

            foreach (var movie in movies)
            {
                movieReview.Add(new MovieReviewsModel { MovieId = movie.Id, Rating = (decimal)movie.Rating, MovieName = movie.Title });
            }

            return movieReview;
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
    }





}
