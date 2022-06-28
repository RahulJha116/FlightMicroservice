using FlightMicroservice.DbContextFlight;
using FlightMicroservice.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMicroservice.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly FlightContext _dbContext;

        public DiscountRepository(FlightContext flightContext)
        {
            _dbContext = flightContext;
        }

        public IEnumerable<Model.Discount> GetDiscounts()
        {
            return _dbContext.Discounts.ToList();
        }
        public void AddDiscount(Discount discount)
        {
             _dbContext.Add(discount);
            Save();
        }

        public void DeleteDiscount(int discountId)
        {
            var f = _dbContext.Discounts.Find(discountId);
            _dbContext.Discounts.Remove(f);
            Save();
        }

        public Discount GetDiscountByID(int discountId)
        {
            return _dbContext.Discounts.Find(discountId);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateDiscount(Discount discount)
        {
            _dbContext.Entry(discount).State = EntityState.Modified;
            Save();
        }
    }
}
