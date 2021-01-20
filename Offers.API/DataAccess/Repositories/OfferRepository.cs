﻿using Common.DataAccess;
using Common.Extensions;
using Common.Types;
using Microsoft.EntityFrameworkCore;
using Offers.API.Application.Dto;
using Offers.API.Domain;
using Offers.API.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Offers.API.DataAccess.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly AppDbContext _appDbContext;

        public IUnitOfWork UnitOfWork => _appDbContext;

        public OfferRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task<IList<Offer>> GetAllAsync()
        {
            return await _appDbContext.Offers.Include(x => x.Category).ToListAsync();
        }

        public async Task<IList<Offer>> GetAllPublishedAsync()
        {
            return await _appDbContext.Offers.Include(x => x.Category)
                .Where(x => x.PublishedAt != null).ToListAsync();
        }

        public async Task<IList<Offer>> GetAllByUserIdAsync(Guid userId)
        {
            return await _appDbContext.Offers.Where(x => x.OwnerId == userId).ToListAsync();
        }

        public async Task<Offer> GetByIdAsync(Guid offerId)
        {
            return await _appDbContext.Offers.Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == offerId);
        }

        public async Task<Pagination<OfferDto>> GetFiltered(OfferFilter filter, PageDetails pageDetails)
        {
            var offers = _appDbContext.Offers.AsQueryable();

            offers = offers.OrderByDescending(x => x.CreatedAt);
            if (filter.PriceFrom != null) offers = offers.Where(x => x.Price >= filter.PriceFrom);
            if (filter.PriceTo != null) offers = offers.Where(x => x.Price <= filter.PriceTo);
            if (filter.Category != null) offers = offers.Where(x => x.Category.Id == filter.Category);

            var offerPagination = await offers.PaginateAsync(pageDetails);
            var offerDtoPagination = new Pagination<OfferDto>(offerPagination.PageDetails,
                offerPagination.Items.Select(offer => offer.ToDto()).ToList(), offerPagination.TotalPages);

            return offerDtoPagination;
        }

        public async Task AddAsync(Offer offer)
        {
            await _appDbContext.Offers.AddAsync(offer);
        }

        public void Update(Offer offer)
        {
            _appDbContext.Update(offer);
        }
    }
}
