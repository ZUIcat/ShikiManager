using System;
using System.Collections.Generic;

namespace HelperConfig {
    public class ShikiGameConfig {
        public IList<string> Title { get; set; }
        public IList<string> Version { get; set; }
        public IList<string> Company { get; set; }
        public IList<string> Date { get; set; }
        public IList<string> Description { get; set; }
        public IList<string> Modify { get; set; }
        public IList<ExeConfig> ShikiConfig { get; set; }
        //public IList<string> Tag { get; set; }
        //public int ConfigVersion { get; set; }

        public ShikiGameConfig() {
        }
    }

    public class ExeConfig {
        public string Name { get; set; }
        public string ExePath { get; set; }
        public string SpecialCode { get; set; }

        public ExeConfig() {
        }
    }
}
