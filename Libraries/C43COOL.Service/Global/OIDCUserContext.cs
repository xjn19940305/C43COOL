using C43COOL.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Global
{
    public class OIDCUserContext : IUserContext
    {
        private readonly JwtBearerOptions bearerOptions;
        private readonly IOptionsMonitor<JwtBearerOptions> options;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly RedisCache cache;
        private readonly List<Claim> claims = new List<Claim>();

        public OIDCUserContext(
            IOptionsMonitor<JwtBearerOptions> options,
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            RedisCache cache
            )
        {

            bearerOptions = options.Get(JwtBearerDefaults.AuthenticationScheme);
            this.options = options;
            this.httpContextAccessor = httpContextAccessor;
            this.httpClientFactory = httpClientFactory;
            this.cache = cache;
            if (httpContextAccessor.HttpContext?.User.Claims?.Count() > 0)
            {
                claims.AddRange(httpContextAccessor.HttpContext.User.Claims);
            }

        }
        string getClaimString(string type) => claims.FirstOrDefault(p => p.Type == type)?.Value;
        public string Id => getClaimString(IdentityModel.JwtClaimTypes.Subject);
        public string Name => getClaimString(IdentityModel.JwtClaimTypes.Name);
        public string Nickname => getNickName(IdentityModel.JwtClaimTypes.NickName);
        public string Email => getClaimString(IdentityModel.JwtClaimTypes.Email);
        public string PhoneNumber => getClaimString(IdentityModel.JwtClaimTypes.PhoneNumber);
        public string Gender => getClaimString(IdentityModel.JwtClaimTypes.Gender);
        public string ZoneInfo => getClaimString(IdentityModel.JwtClaimTypes.ZoneInfo);
        public string Locale => getClaimString(IdentityModel.JwtClaimTypes.Locale);
        public DateTime? BirthDate => getClaimDateTime(IdentityModel.JwtClaimTypes.BirthDate);
        public DateTime? UpdatedAt => getClaimUNIXDateTime(IdentityModel.JwtClaimTypes.BirthDate);
        public IPAddress UserIpAddress => httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

        public string Avatar => getClaimString("avatar");

        public string OpenId => getClaimString("OpenId");

        //string IUserContext.OpenId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerable<Claim> GetClaims() => claims;
        bool getClaimBool(string type)
        {
            var str = getClaimString(type);
            if (string.IsNullOrWhiteSpace(str) == false)
            {
                if (bool.TryParse(str, out var result))
                {
                    return result;
                }
            }
            return false;
        }
        DateTime? getClaimDateTime(string type)
        {
            var str = getClaimString(type);
            if (string.IsNullOrWhiteSpace(str) == false)
            {
                if (DateTime.TryParse(str, out var result))
                {
                    return result;
                }
            }
            return null;
        }
        DateTime? getClaimUNIXDateTime(string type)
        {
            var str = getClaimString(type);
            if (string.IsNullOrWhiteSpace(str) == false)
            {
                if (long.TryParse(str, out var result))
                {
                    return DateTimeOffset.FromUnixTimeSeconds(result).UtcDateTime;
                }
            }
            return null;
        }
        string getNickName(string NickName)
        {
            var _nickName = getClaimString(NickName);
            return _nickName;
        }
    }
    public class UserContextRequiredAttribute : Attribute
    {

    }
}
