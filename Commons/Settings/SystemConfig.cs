using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Relation.Commons.Settings
{
    public class SystemConfig
    {
        public SystemConfig() 
        {
            ConnStr = new Dictionary<string, string>();

            ConfigStr = new Dictionary<string, string>();
        }

        public Dictionary<string, string> ConnStr { get; set; }
        public Dictionary<string, string> ConfigStr { get; set; }
    }
}
