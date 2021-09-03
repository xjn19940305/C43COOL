using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Models.File;
using C43COOL.Service.Global;
using C43COOL.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Impl
{
    public class FileService : IFileService
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUserContext userContext;

        public FileService(
            C43DbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IUserContext userContext
            )
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = configuration;
            this.userContext = userContext;
        }

        public async Task<FileModel> CreateFileAsync(CreateFileRequest request)
        {
            var relatedDir = $"{DateTime.Now.ToString("yyyy/MM/dd")}/";
            var relatedPath = $"{relatedDir}{Guid.NewGuid().ToString()}{System.IO.Path.GetExtension(request.Name)}";
            var e_file = new Files()
            {
                Id = Guid.NewGuid().ToString(),
                FileType = request.FileType,
                Name = request.Name,
                UserId = userContext?.Id,
                Path = relatedPath,
                DateCreated = DateTime.Now
            };
            var path = GetPath(relatedPath);
            var dirpaht = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirpaht))
            {
                Directory.CreateDirectory(dirpaht);
            }
            using (var fs = await Task.FromResult<Stream>(System.IO.File.Open(path, FileMode.OpenOrCreate)))
            {
                await request.FileStream.CopyToAsync(fs);
            }
            dbContext.Add(e_file);
            await dbContext.SaveChangesAsync();
            return mapper.Map<FileModel>(e_file);
        }

        public async Task DeleteFiles(string[] fileIds)
        {
            if (fileIds.Any())
            {
                var files = await dbContext.Files.Where(p => fileIds.Any(f => p.Id == f)).ToArrayAsync();
                dbContext.RemoveRange(files);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<FileModel> GetFile(string id)
        {
            var e_file = await dbContext.Files.FirstOrDefaultAsync(p => p.Id == id);
            return mapper.Map<FileModel>(e_file);
        }

        public Task<Stream> OpenAsRead(string path)
        {
            path = GetPath(path);
            if (File.Exists(path))
            {
                return Task.FromResult<Stream>(System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read));
            }
            return Task.FromResult<Stream>(null);
        }

        private string GetPath(string path)
        {
            var Root = configuration.GetSection("FileRoot").Value;
            return Path.Combine(Root, path);
        }

    }
}
