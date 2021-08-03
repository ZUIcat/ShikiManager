using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperConfig {
    public class TextractorConfig {
        public string DirPath { get; set; }

        public TextractorConfig() {
            DirPath = @".\Textractor";
        }
    }
}
