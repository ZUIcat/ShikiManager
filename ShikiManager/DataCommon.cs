using HelperConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShikiManager {
    internal class DataCommon {
        public static ConfigHelper ConfigHelperIns { get; private set; }

        static DataCommon() { 
            ConfigHelperIns = ConfigHelper.Instance;
        }

        public DataCommon() {
            ConfigHelperIns = ConfigHelper.Instance;
        }

        public bool Init() {
            return true;
        }

        public bool Destroy() {
            return true;
        }
    }
}
