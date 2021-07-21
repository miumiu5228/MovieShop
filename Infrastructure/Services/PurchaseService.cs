using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;

namespace Infrastructure.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseService(IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<List<UserPurchaseModel>> GetPurchaseById(int id)
        {
            var purchase = await _purchaseRepository.GetPurchase(id);

            var userPurchase = new List<UserPurchaseModel>();

            foreach(var p in purchase)
            {
                userPurchase.Add(
                    new UserPurchaseModel
                    {
                        UserId = p.UserId,
                        PurchaseId = p.Id,
                        TotalPrice = p.TotalPrice,
                        PurchaseDateTime = p.PurchaseDateTime,
                        MovieId = p.MovieId,
                        MovieName = p.Movie.Title,

                    }
                 );
            }

            return userPurchase;
        }
    }
}
