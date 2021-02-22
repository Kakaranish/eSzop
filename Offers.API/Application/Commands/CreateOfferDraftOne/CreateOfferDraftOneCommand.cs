﻿using System;
using System.Collections.Generic;
using Common.Extensions;
using Common.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.Domain.Validators;

namespace Offers.API.Application.Commands.CreateOfferDraftOne
{
    public class CreateOfferDraftOneCommand : IRequest<Guid>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int TotalStock { get; init; }
        public string CategoryId { get; init; }
        public IList<IFormFile> Images { get; init; }
        public string ImagesMetadata { get; init; }
        public string KeyValueInfos { get; init; }
    }

    public class CreateOfferDraftOneCommandValidator : AbstractValidator<CreateOfferDraftOneCommand>
    {
        public CreateOfferDraftOneCommandValidator()
        {
            RuleFor(x => x.Name)
                .SetValidator(new OfferNameValidator())
                .OverridePropertyName("Name");

            RuleFor(x => x.Price)
                .SetValidator(new OfferPriceValidator());

            RuleFor(x => x.Description)
                .SetValidator(new OfferDescriptionValidator());

            RuleFor(x => x.TotalStock)
                .SetValidator(new TotalStockValidator());

            RuleFor(x => x.CategoryId)
                .IsNotEmptyGuid();

            RuleFor(x => x.Images)
                .Must(x => x.Count > 0)
                .WithMessage("Min number of images is 1")
                .Must(x => x.Count <= 5)
                .WithName("Max number of images is 5");

            RuleFor(x => x.ImagesMetadata)
                .NotEmpty();

            RuleFor(x => x.KeyValueInfos)
                .Must(x => x == null || !string.IsNullOrWhiteSpace(x))
                .WithMessage("Must be null or not empty string");
        }
    }
}