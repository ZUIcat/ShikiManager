using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTextractor {
    public class TextDataRegxHandler {
        private TextDataRegxHandler() { }

        public static string Remove2SameChar(string text) {
            var newCharArray = new char[text.Length / 2];
            for (int i = 0, j = 0; i < text.Length && j < newCharArray.Length; i += 2, j += 1) {
                newCharArray[j] = text[i];
            }
            return new string(newCharArray);
        }

        public static string Remove3SameChar(string text) {
            var newCharArray = new char[text.Length / 3];
            for (int i = 0, j = 0; i < text.Length && j < newCharArray.Length; i += 3, j += 1) {
                newCharArray[j] = text[i];
            }
            return new string(newCharArray);
        }
    }
}
