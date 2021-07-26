using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Models;

namespace ApplicationCore.ServiceInterfaces
{
    public interface IUserService
    {
        Task<UserRegisterResponseModel> RegisterUser(UserRegisterRequestModel userRegisterRequestModel);

        Task<UserLoginResponseModel> Login(string email, string password);
        Task<UserResponseModel> GetUserById(int id);
        Task<List<MovieReviewsModel>> GetReviews(int id);
        Task<List<UserFavoriteMoviesModel>> GetFavoriteById(int id);

        Task<List<UserPurchaseModel>> GetPurchaseById(int id);
        Task<PurchaseMovieResponseModel> PurchaseMovie(PurchaseMovieModel purchaseMovie);
        Task<FavoriteResponseModel> AddToFavorite(FavoriteRequestModel model);
        Task<UnFavoriteResponseModel> removefromFavorite(UnFavoriteRequestModel model);
        Task<PostReviewsResponseModel> PostReviews(PostReviewsRequestModel model);
        Task<PostReviewsResponseModel> PutReviews(PostReviewsRequestModel model);
        Task<MovieCardResponseModel> GetFavoriteMovieDetail(int id, int movieId);

        Task<string> DeleteReviews(int id, int movieId);
        
    }
}
