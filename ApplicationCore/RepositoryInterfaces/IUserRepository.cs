using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.RepositoryInterfaces
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetReviewsById(int id);
        Task<User> GetFavorites(int id);
        Task<User> GetPurchase(int userId);
        //Task<User> PurchaseMovie(int movieId, int userId);



    }
}
