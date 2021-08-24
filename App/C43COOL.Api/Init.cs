using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Infrastructure;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace C43COOL.Api
{
    public class Init
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private string Salt = string.Empty;
        public Init(
            C43DbContext dbContext,
            IMapper mapper,
            IConfiguration configuration
            )
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = configuration;
            Salt = configuration.GetValue<string>("Salt");
        }

        public async Task Seeds()
        {
            dbContext.Add(new User
            {
                Name = "管理员江文",
                Password = "123456".GetMd5WithSalt(Salt),
                NickName = "江文",
                Enabled = true,
                PhoneNumber = "13961111005"
            });
            using (var sr = new StreamReader("../../ImportData/Menu.json"))
            {
                var content = await sr.ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<List<Modules>>(content);
                foreach (var item in data)
                {
                    dbContext.Add(item);
                }
            }
            await dbContext.SaveChangesAsync();
        }
    }
}