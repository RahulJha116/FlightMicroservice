using FlightMicroservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMicroservice.Repository
{
    public interface IDiscountRepository
    {
        void AddDiscount(Discount discount);

        Discount GetDiscountByID(int discountId);

        IEnumerable<Model.Discount> GetDiscounts();

        void DeleteDiscount(int discountId);
        void UpdateDiscount(Discount discount);
        void Save();

    }
}
