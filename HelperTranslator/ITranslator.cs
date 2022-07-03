using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTranslator {
    public interface ITranslator {
        ITranslator Init();

        Task<string?> TranslateAsync(string sourceText, string srcLang= "jp", string desLang="zh");

        string? GetLastError();
    }
}
