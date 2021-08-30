using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Permission.Strategy
{
    public class AuthContextFactory
    {
        private readonly AdminAuthStrategy adminAuthStrategy;
        private readonly NormalAuthStrategy normalAuthStrategy;
        private readonly IConfiguration configuration;

        public AuthContextFactory(
            AdminAuthStrategy adminAuthStrategy,
            NormalAuthStrategy normalAuthStrategy,
            IConfiguration configuration
            )
        {
            this.adminAuthStrategy = adminAuthStrategy;
            this.normalAuthStrategy = normalAuthStrategy;
            this.configuration = configuration;
        }
        public IAuthStrategy GetAuthStrategyContext(string username)
        {
            var WhiteList = configuration.GetSection("WhiteList").Get<string[]>();
            IAuthStrategy service = normalAuthStrategy;
            if (WhiteList.Any(x => x.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                service = adminAuthStrategy;
            }
            return service;
        }
    }
}
