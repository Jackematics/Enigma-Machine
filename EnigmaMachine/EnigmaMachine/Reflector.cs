using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine
{
    class Reflector
    {
        public char Reflect(char text)
        {
            List<char> reflectorPattern = new List<char>()
            {
                'A', 'M', '|', 'S', 'G', '|', 'F', 'T', '|', 'N', 'Z', '|',
                'U', 'L', '|', 'R', 'D', '|', 'C', 'E', '|', 'W', 'K', '|',
                'O', 'X', '|', 'Y', 'B', '|', 'V', 'P', '|', 'H', 'J', '|',
                'I', 'Q', '|'
            };

            int preMappedLetterPosition = reflectorPattern.IndexOf(text);
            int mappedLetterPosition = reflectorPattern[preMappedLetterPosition + 1] == '|' ? 
                        preMappedLetterPosition - 1 : 
                        preMappedLetterPosition + 1;

            return reflectorPattern[mappedLetterPosition];
        }
    }
}
