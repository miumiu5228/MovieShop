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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<UserFavoriteMoviesModel> UserFavorite(UserFavoriteMoviesModel requestModel)
        {
    
     
            var userResponse = new UserFavoriteMoviesModel
            {
              Id = requestModel.Id,
              UserId = requestModel.UserId,
              MovieId = requestModel.MovieId,
              MoiveName = requestModel.MoiveName
            };

            return userResponse;
        }




        //public async Task<PurchaseMovieResponseModel> Purchase(PurchaseMovieModel purchaseMovie)
        //{
        //    var purchase = new PurchaseMovieResponseModel()
        //    {
        //        MovieId = purchaseMovie.MovieId,
        //        UserId = purchaseMovie.MovieId,


        //    };

        //    return purchase;
        //}
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
                   TotalPrice = u.TotalPrice,
                   PurchaseDateTime = u.PurchaseDateTime,
                   MovieId = u.MovieId,
                   MovieName = u.Movie.Title

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
    }
}