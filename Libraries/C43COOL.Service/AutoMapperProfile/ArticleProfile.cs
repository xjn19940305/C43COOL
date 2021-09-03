using AutoMapper;
using C43COOL.Domain.Base;
using C43COOL.Models.Article.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.AutoMapperProfile
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleCreateModel, Article>().ReverseMap();
            CreateMap<ArticleModifyModel, Article>().ReverseMap();
            CreateMap<ArticleDTO, Article>().ReverseMap();
        }
    }
}
