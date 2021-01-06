﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Offers.API.Services;

namespace Offers.API.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddBlobStorage(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var useAzureBlobStorage = bool.TryParse(
                configuration.GetSection("UseAzureBlobStorage").Value, out var valueResult) && valueResult;
            if (useAzureBlobStorage)
            {
                services.Configure<AzureStorageConfig>(configuration.GetSection("AzureStorage"));
                services.AddSingleton<IBlobStorage, AzureBlobStorage>();
            }
            else
            {
                services.AddSingleton<IBlobStorage, LocalBlobStorage>();
            }

            return services;
        }
    }
}
