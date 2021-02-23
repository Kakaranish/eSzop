﻿using Common.Extensions;
using Identity.API.DataAccess.Repositories;
using Identity.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.RemoveDeliveryAddress
{
    public class RemoveDeliveryAddressCommandHandler : IRequestHandler<RemoveDeliveryAddressCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly IUserRepository _userRepository;

        public RemoveDeliveryAddressCommandHandler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Unit> Handle(RemoveDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var deliveryAddressId = Guid.Parse(request.DeliveryAddressId);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new IdentityDomainException("There is no such user");

            user.RemoveDeliveryAddress(deliveryAddressId);

            _userRepository.Update(user);
            await _userRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
