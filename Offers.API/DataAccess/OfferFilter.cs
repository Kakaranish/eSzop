﻿using System;

namespace Offers.API.DataAccess
{
    public class OfferFilter
    {
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public Guid? Category { get; set; }

        public OfferFilter(decimal? priceFrom = default, decimal? priceTo = default, Guid? category = default)
        {
            if (priceFrom < 0)
                throw new ArgumentException($"'{nameof(priceFrom)}' must be >= 0");
            if (priceTo < 0)
                throw new ArgumentException($"'{nameof(priceTo)}' must be >= 0");
            if (priceFrom > priceTo)
                throw new ArgumentException($"'{nameof(priceFrom)}' must be <= '{nameof(priceTo)}'");

            PriceFrom = priceFrom;
            PriceTo = priceTo;
            Category = category;
        }
    }
}
