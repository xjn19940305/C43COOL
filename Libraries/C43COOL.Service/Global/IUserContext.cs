using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Global
{
    public interface IUserContext
    {
        string Id { get; }
        string Name { get; }
        string Nickname { get; }
        string Email { get; }
        string PhoneNumber { get; }
        string Gender { get; }
        string Avatar { get; }
        string ZoneInfo { get; }
        string Locale { get; }
        string OpenId { get; }
        string CascadeId { get; }
        DateTime? BirthDate { get; }
        DateTime? UpdatedAt { get; }
        IPAddress UserIpAddress { get; }
        IEnumerable<Claim> GetClaims();
    }
}
