using C43COOL.Models.Article.Management;
using C43COOL.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Interface.Management
{
    public interface IArticleMangementService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Create(ArticleCreateModel dto);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Update(ArticleModifyModel dto);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task Delete(string[] ids);
        /// <summary>
        /// 获取单个资讯信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ArticleDTO> Get(string Id);
        /// <summary>
        /// 分页获取资讯
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedViewModel> Query(ArticleRequest request);
    }
}
