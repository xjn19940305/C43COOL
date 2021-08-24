using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Enum.Base
{
    public enum ModuleTypeEnum
    {
        [Description("菜单")]
        Menu = 0,
        [Description("按钮")]
        Button = 1,
        [Description("未知")]
        UnKnown = 2
    }
}
