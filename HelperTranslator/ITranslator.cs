using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTranslator {
    public interface ITranslator {
        ITranslator? Init(string parm1, string parm2);

        Task<string?> TranslateAsync(string sourceText, string srcLang= "jp", string desLang="zh");

        string? GetLastError();
    }
}
