using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Models.File;
using C43COOL.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Impl
{
    public class MinioFileService : IFileService
    {
        private readonly IConfiguration configuration;
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;
        private MinioClient minio;
        private string BucketName = string.Empty;
        public MinioFileService(
            IConfiguration configuration,
            C43DbContext dbContext,
            IMapper mapper
            )
        {
            BucketName = configuration.GetSection("Minio:BucketName").Value;
            minio = new MinioClient(configuration.GetSection("Minio:Url").Value, configuration.GetSection("Minio:AccessKey").Value, configuration.GetSection("Minio:Secrect").Value);
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<FileModel> CreateFileAsync(CreateFileRequest request)
        {
            var relatedDir = $"{DateTime.Now.ToString("yyyy/MM/dd")}/";
            var Id = Guid.NewGuid().ToString();
            var e_file = new Files()
            {
                Id = Id,
                FileType = request.FileType,
                Name = request.Name,
                //UserIdCreated = userContext?.Id,
                Path = $"{relatedDir}{Id}{System.IO.Path.GetExtension(request.Name)}",
                DateCreated = DateTime.UtcNow
            };
            if (!await minio.BucketExistsAsync(BucketName))
            {
                await minio.MakeBucketAsync(BucketName);
            }
            request.FileStream.Position = 0;
            await minio.PutObjectAsync(BucketName, $"{e_file.Path}", request.FileStream, request.FileStream.Length, contentType: request.FileType);
            dbContext.Add(e_file);
            await dbContext.SaveChangesAsync();
            return mapper.Map<FileModel>(e_file);
        }

        public Task DeleteFiles(string[] fileIds)
        {
            throw new NotImplementedException();
        }

        public async Task<FileModel> GetFile(string id)
        {
            var e_file = await dbContext.Files.FirstOrDefaultAsync(p => p.Id == id);
            return mapper.Map<FileModel>(e_file);
        }

        public async Task<Stream> OpenAsRead(string path)
        {
            Stream Stream = new MemoryStream();
            var r = await minio.StatObjectAsync(BucketName, path);
            await minio.GetObjectAsync(BucketName, path, (stream) =>
            {
                stream.CopyToAsync(Stream).GetAwaiter().GetResult();
            });
            Stream.Position = 0;
            return Stream;
        }
    }
}
