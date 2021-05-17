using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnigmaMachine
{
    public class PatternSplitter
    {
        private char Splitter { get; } = '|';

        public List<List<char>> SplitPattern(List<char> pattern)
        {
            HandleSplitPatternErrors(pattern);

            List<List<char>> splitPattern = new List<List<char>>();
            int startPosition = 0;

            foreach (char splitter in pattern.Where(x => x == Splitter))
            {
                int countToNextSplitter = pattern.IndexOf(splitter, startPosition) - startPosition;
                List<char> permutation = pattern.GetRange(startPosition, countToNextSplitter);
                splitPattern.Add(permutation);

                startPosition = pattern.IndexOf(splitter, startPosition) + 1;
            }

            return splitPattern;
        }

        private void HandleSplitPatternErrors(List<char> pattern)
        {
            if (!pattern.Contains(Splitter))
            {
                throw new ArgumentException("Pattern must contain \'|\' to be splittable");
            }

            if (BeginsWithSplitter(pattern))
            {
                throw new ArgumentException("Splitters cannot be at the beginning of a pattern");
            }

            if (ContainsConsecutiveSplitters(pattern))
            {
                throw new ArgumentException("Pattern cannot contain consecutive splitters");
            }
        }

        private bool BeginsWithSplitter(List<char> pattern)
        {
            return pattern[0] == Splitter;
        }

        private bool ContainsConsecutiveSplitters(List<char> pattern)
        {
            bool containsConsecutiveSplitters = false;

            for (int i = 0; i < pattern.Count - 1; i++)
            {
                if (pattern[i] == Splitter &&
                    (pattern[i] == pattern[i - 1] || pattern[i] == pattern[i + 1]))
                {
                    containsConsecutiveSplitters = true;
                    break;
                }
            }

            return containsConsecutiveSplitters;
        }
    }
}
