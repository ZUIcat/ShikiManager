using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperConfig {
    public class LocaleEmulatorConfig {
        public string DirPath { get; set; }

        public LocaleEmulatorConfig() {
            DirPath = @".\LocaleEmulator";
        }
    }
}
