using C43COOL.Domain.Base;
using C43COOL.Models.Modules.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Permission.Strategy
{
    public interface IAuthStrategy
    {
        List<ModuleViewModel> Modules { get; }

        List<Role> Role { get; }
    }
}
