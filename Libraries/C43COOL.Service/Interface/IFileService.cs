using C43COOL.Models.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Interface
{
    public interface IFileService
    {
        Task<FileModel> GetFile(string id);
        Task<System.IO.Stream> OpenAsRead(string path);
        Task DeleteFiles(string[] fileIds);
        Task<FileModel> CreateFileAsync(CreateFileRequest request);
    }
}
