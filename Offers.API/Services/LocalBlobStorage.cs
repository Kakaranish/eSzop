﻿using Offers.API.Services.Dto;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Offers.API.Services
{
    public class LocalBlobStorage : IBlobStorage
    {
        private bool _ensured = false;

        public string ContainerName => "wwwroot";

        public string UploadDir => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, ContainerName);

        public async Task<UploadedFileDto> UploadAsync(Stream content, string blobName)
        {
            EnsureUploadDirExists();

            var path = Path.Combine(UploadDir, blobName);
            await using var stream = new FileStream(path, FileMode.Create);
            await content.CopyToAsync(stream);

            return new UploadedFileDto
            {
                ContainerName = ContainerName,
                Filename = blobName
            };
        }

        public Task<Stream> DownloadAsync(string blobName)
        {
            EnsureUploadDirExists();

            var path = Path.Combine(UploadDir, blobName);
            if (!File.Exists(path)) return null;

            var file = new FileStream(path, FileMode.Open);
            return Task.FromResult((Stream)file);
        }

        private void EnsureUploadDirExists()
        {
            if (_ensured) return;

            _ensured = true;
            if (Directory.Exists(UploadDir)) return;

            Directory.CreateDirectory(UploadDir);
        }
    }
}
