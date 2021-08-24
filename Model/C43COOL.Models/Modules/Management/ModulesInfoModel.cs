using C43COOL.Enum.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Modules.Management
{
    public class ModulesInfoModel
    {
        public string Id { get; set; } 

        public string Path { get; set; }

        public string ComponentUrl { get; set; }

        public string Name { get; set; }

        public string ModuleName { get; set; }

        public string Icon { get; set; }

        public string ParentId { get; set; }
        /// <summary>
        /// 模块类型 0菜单 1按钮
        /// </summary>
        public ModuleTypeEnum ModuleType { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModify { get; set; }
    }
}
