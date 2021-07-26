using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMovieRepository _movieRespository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IReviewRepository _reviewRepository;

        public UserService(IUserRepository userRepository, ICurrentUser currentUser, IPurchaseRepository purchaseRepository, IMovieRepository movieRepository, IFavoriteRepository favoriteRepository, IReviewRepository reviewRepository)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _purchaseRepository = purchaseRepository;
            _movieRespository = movieRepository;
            _favoriteRepository = favoriteRepository;
            _reviewRepository = reviewRepository;
        }


        public async Task<PurchaseMovieResponseModel> PurchaseMovie(PurchaseMovieModel purchaseMovie)
        {
            var dbPurchase = await _purchaseRepository.GetExistsAsync(p => p.MovieId == purchaseMovie.MovieId && p.UserId == purchaseMovie.UserId);

            if (dbPurchase == true)
            {
                throw new ConflictException("You already bought this movie");
            }


            await _purchaseRepository.AddAsync(new Purchase
            {
               UserId = purchaseMovie.UserId,
               MovieId = purchaseMovie.MovieId

            });



            var purchase = new PurchaseMovieResponseModel()
            {
                MovieId = purchaseMovie.MovieId,
                UserId = purchaseMovie.MovieId,
                


            };

            return purchase;
        }
        public async Task<List<UserPurchaseModel>> GetPurchaseById(int id)
        {
            var user = await _userRepository.GetPurchase(id);

            var UserPurchaseMovies = new List<UserPurchaseModel>();

            foreach (var u in user.Purchases)
            {
                UserPurchaseMovies.Add(new UserPurchaseModel
                {
                   PurchaseId = u.Id,
                   UserId = u.UserId,
                
                
                   MovieId = u.MovieId,
                 

                });
            }

            return UserPurchaseMovies;

        }


        public async Task<List<UserFavoriteMoviesModel>> GetFavoriteById(int id)
        {
            var user = await _userRepository.GetFavorites(id);

            var UserFavoriteMovies = new List<UserFavoriteMoviesModel>();

            foreach(var m in user.Favorites)
            {
                UserFavoriteMovies.Add(new UserFavoriteMoviesModel
                {
                    Id = m.Id,
                    UserId = m.UserId,
                    MoiveName = m.Movie.Title,
                    MovieId = m.MovieId
                });

            }

            return UserFavoriteMovies;
        }

        public async Task<List<MovieReviewsModel>> GetReviews(int id)
        {
            var user = await _userRepository.GetReviewsById(id);


            var movieReviw = new List<MovieReviewsModel>();

            foreach(var m in user.Reviews)
            {
                movieReviw.Add(new MovieReviewsModel
                {
                   
                    Rating = m.Rating,
                    ReviewText = m.ReviewText,
                    MovieId = m.MovieId,
                    UserId =m.UserId

                });
            }
          
            return movieReviw;


        }

        public async Task<UserResponseModel> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            var userResponseModel = new UserResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth
            };
            return userResponseModel;
        }

        public async Task<UserLoginResponseModel> Login(string email, string password)
        {
            var dbUser = await _userRepository.GetUserByEmail(email);
            if (dbUser == null)
            {
                throw new NotFoundException("Email does not exists, please register first");
            }

            var hashedPssword = HashPassword(password, dbUser.Salt);

            if (hashedPssword == dbUser.HashedPassword)
            {
                // good, correct password

                var userLoginRespone = new UserLoginResponseModel
                {

                    Id = dbUser.Id,
                    Email = dbUser.Email,
                    FirstName = dbUser.FirstName,
                    DateOfBirth = dbUser.DateOfBirth,
                    LastName = dbUser.LastName
                };

                return userLoginRespone;
            }

            return null;
        }

        public async Task<UserRegisterResponseModel> RegisterUser(UserRegisterRequestModel requestModel)
        {
            // Make sure email does not exists in database User table

            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);

            if (dbUser != null)
            {
                // we already have user with same email
                throw new ConflictException("Email arleady exists");
            }

            // create a unique salt

            var salt = CreateSalt();

            var hashedPassword = HashPassword(requestModel.Password, salt);

            // save to database

            var user = new User
            {
                Email = requestModel.Email,
                Salt = salt,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                HashedPassword = hashedPassword
            };

            // save to database by calling UserRepository Add method
            var createdUser = await _userRepository.AddAsync(user);

            var userResponse = new UserRegisterResponseModel
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName
            };

            return userResponse;
        }

       

        private string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return Convert.ToBase64String(randomBytes);
        }

  

        private string HashPassword(string password, string salt)
        {
            // Aarogon
            // Pbkdf2
            // BCrypt
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Convert.FromBase64String(salt),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            return hashed;
        }

        public async Task<FavoriteResponseModel> AddToFavorite(FavoriteRequestModel model)
        {
            var dbFavorite = await _favoriteRepository.GetExistsAsync(f => f.MovieId == model.MovieId && f.UserId == model.UserId);
            if (dbFavorite == true)
            {
                throw new ConflictException("The movie arleady added!");
            }

            await _favoriteRepository.AddAsync(new Favorite
            {
                UserId = model.UserId,
                MovieId = model.MovieId
            });



            var addFavoriteResonse = new FavoriteResponseModel
            {

                UserId = model.UserId,
                MovieId = model.MovieId
            };

            return addFavoriteResonse;
        }

        public async Task<UnFavoriteResponseModel> removefromFavorite(UnFavoriteRequestModel model)
        {
            var dbFavorite = await _favoriteRepository.GetExistsAsync(f => f.Id == model.Id);

            if (dbFavorite != true)
            {
                throw new ConflictException("The movie dose not exists!");
            }

            await _favoriteRepository.DeleteAsync(new Favorite
            {
                Id = model.Id, 
                UserId = model.UserId,
                MovieId = model.MovieId
            });



            var deleteFavoriteResonse = new UnFavoriteResponseModel
            {
                Id = model.Id,
                UserId = model.UserId,
                MovieId = model.MovieId
            };

            return deleteFavoriteResonse;
        }

        public async Task<PostReviewsResponseModel> PostReviews(PostReviewsRequestModel model)
        {
            var dbReviews = await _reviewRepository.GetExistsAsync(f => f.MovieId == model.MovieId && f.UserId == model.UserId);

            if (dbReviews != false)
            {
                throw new ConflictException("You already posted a review for this movie");
            }
            await _reviewRepository.AddAsync(new Review
            {
                MovieId = model.MovieId,
                UserId = model.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText
            });

            var postReviewsResponse = new PostReviewsResponseModel
            {
                MovieId = model.MovieId,
                UserId = model.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText

            };

            return postReviewsResponse;
        }

        public async Task<PostReviewsResponseModel> PutReviews(PostReviewsRequestModel model)
        {
            var dbReviews = await _reviewRepository.GetExistsAsync(f => f.MovieId == model.MovieId && f.UserId == model.UserId);

            if (dbReviews != true)
            {
                throw new ConflictException("Conflict");
            }
            await _reviewRepository.UpdateAsync(new Review
            {
                MovieId = model.MovieId,
                UserId = model.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText
            });

            var postReviewsResponse = new PostReviewsResponseModel
            {
                MovieId = model.MovieId,
                UserId = model.UserId,
                Rating = model.Rating,
                ReviewText = model.ReviewText

            };

            return postReviewsResponse;
        }

        public async Task<MovieCardResponseModel> GetFavoriteMovieDetail(int id, int movieId)
        {
            var dbFavorite = await _favoriteRepository.GetFavorite(id, movieId);




            var movieCardResponseModel = new MovieCardResponseModel
            {
                Id = dbFavorite.Id,
                Title = dbFavorite.Movie.Title,
                PosterUrl = dbFavorite.Movie.PosterUrl,
                Budget = (decimal)dbFavorite.Movie.Budget

            };

            return movieCardResponseModel;

        }

        public async Task<string> DeleteReviews(int id, int movieId)
        {
            var dbreview = await _reviewRepository.GetExistsAsync(r => r.UserId == id && r.MovieId == movieId);

            if (dbreview != true)
            {
                throw new ConflictException("Conflict");
            }

            //var review = await _reviewRepository.GetReviews(id, movieId);

          
            await _reviewRepository.DeleteAsync(new Review
            {
                MovieId = movieId,
                UserId = id
             

            });
            

          

            return "The reviews is Deleted";

        }
    }
}