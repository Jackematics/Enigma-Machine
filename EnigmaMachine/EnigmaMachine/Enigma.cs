using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine
{
	class Enigma
	{
		private List<char> Alphabet { get; } = new List<char>()
		{
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
			'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
		};

		public string Transform(
				int[] wheelOrientations,				
				string cipherText)
		{
			HandleTransformErrors(wheelOrientations);

			var currentlyRotatingWheel = 0;

			Reflector reflector = new Reflector();
			StringBuilder codeBuilder = new StringBuilder();			
				
			foreach (char cipherLetter in cipherText)
			{
				if (cipherLetter == ' ')
				{
					codeBuilder.Append(cipherLetter);
					continue;
				}

				char wheelZeroEncrypt = CryptCharacter(0, wheelOrientations[0], cipherLetter, CryptType.Encrypt);
				char wheelOneEncrypt = CryptCharacter(1, wheelOrientations[1], wheelZeroEncrypt, CryptType.Encrypt);
				char wheelTwoEncrypt = CryptCharacter(2, wheelOrientations[2], wheelOneEncrypt, CryptType.Encrypt);

				char reflectedResult = reflector.Reflect(wheelTwoEncrypt);

				char wheelTwoDecrypt = CryptCharacter(2, wheelOrientations[2], reflectedResult, CryptType.Decrypt);
				char wheelOneDecrypt = CryptCharacter(1, wheelOrientations[1], wheelTwoDecrypt, CryptType.Decrypt);
				char wheelZeroDecrypt = CryptCharacter(0, wheelOrientations[0], wheelOneDecrypt, CryptType.Decrypt);

				codeBuilder.Append(wheelZeroDecrypt);

				wheelOrientations = RotateCurrentWheel(wheelOrientations, currentlyRotatingWheel);
				currentlyRotatingWheel = SetCurrentlyRotatingWheel(wheelOrientations, currentlyRotatingWheel);
			}

			return codeBuilder.ToString();
		}

		private char CryptCharacter(
				int wheelNumber,
				int orientation,
				char cipherLetter,
				CryptType cryptType)
		{
			HandleCryptCharacterErrors(orientation, cipherLetter);

			List<char> pattern;

			switch (wheelNumber)
			{
				case 0:
					pattern = new List<char>()
					{
						'A', 'Z', '|', 'B', 'Y', '|', 'C', 'X', '|', 'D', 'W', '|',
						'E', 'V', '|', 'F', 'U', '|', 'G', 'T', '|', 'H', 'S', '|',
						'I', 'R', '|', 'J', 'Q', '|', 'K', 'P', '|', 'L', 'O', '|', 
						'M', 'N', '|'
					};
					break;

				case 1:
					pattern = new List<char>()
					{
						'A', 'D', 'T', 'Q', 'R', 'G', 'U', 'P', 'X', 'E', 'V', 'O', 'W',
						'Y', 'Z', 'N', 'I', 'C', 'L', 'M', 'B', 'K', 'J', 'H', 'S', 'F', '|'
					};
					break;

				case 2:
					pattern = new List<char>()
					{
						'A', 'B', 'D', 'H', 'P', 'E', 'J', 'T', 'M', 'Z', 'Y', 'W', 'S', 'K',
						'V', 'Q', 'G', 'N', '|', 'C', 'F', 'L', 'X', 'U', 'O', '|', 'I', 'R', '|'
					};
					break;

				default:
					throw new ArgumentException("wheelNumber must be an integer between 0 and 2");
			}

			List<char> adjustedPattern = new List<char>(OrientatePattern(pattern, orientation));
			adjustedPattern = GetSubPattern(adjustedPattern, cipherLetter);
			int preMappedLetterPosition = adjustedPattern.IndexOf(cipherLetter);

			return GetCryptedCharacter(preMappedLetterPosition, cryptType, adjustedPattern);
		}		

		private List<char> OrientatePattern(List<char> pattern, int orientation)
		{
			List<char> orientatedPattern = new List<char>();
			
			foreach (char cipherText in pattern)
			{
				if (cipherText == '|')
				{
					orientatedPattern.Add(cipherText);
				}
				else
				{
					char orientatedLetter = NumberToChar((Alphabet.IndexOf(cipherText) - orientation + Alphabet.Count) % Alphabet.Count);
					orientatedPattern.Add(orientatedLetter);
				}				
			}

			return orientatedPattern;
		}

		private List<char> GetSubPattern(List<char> pattern, char cipherLetter)
		{
			PatternSplitter patternSplitter = new PatternSplitter();
			List<List<char>> splitPattern = patternSplitter.SplitPattern(pattern);

			foreach (List<char> subPattern in splitPattern)
			{
				if (subPattern.Contains(cipherLetter))
				{
					pattern = subPattern;
					break;
				}
			}

			return pattern;
		}

		private char GetCryptedCharacter(
				int preMappedLetterPosition, 
				CryptType cryptType, 
				List<char> pattern)
		{
			char mappedLetter;

			switch (cryptType)
			{
				case CryptType.Encrypt:
					mappedLetter = pattern[(preMappedLetterPosition + 1) % pattern.Count];
					break;

				case CryptType.Decrypt:
					mappedLetter = pattern[(preMappedLetterPosition - 1 + pattern.Count) % pattern.Count];
					break;

				default:
					throw new ArgumentException("cryptType must be Encrypt or Decrypt");
			}

			return mappedLetter;
		}			
		
		private int[] RotateCurrentWheel(int[] wheelOrientations, int currentlyRotatingWheel)
		{
			if (wheelOrientations[currentlyRotatingWheel] == 25)
			{
				wheelOrientations[currentlyRotatingWheel] = 0;
			}
			else
			{
				wheelOrientations[currentlyRotatingWheel]++;
			}

			return wheelOrientations;
		}

		private int SetCurrentlyRotatingWheel(int[] wheelOrientations, int currentlyRotatingWheel)
		{
			if (wheelOrientations[currentlyRotatingWheel] == 0)
			{
				currentlyRotatingWheel = currentlyRotatingWheel == 2 ?
						0 :
						currentlyRotatingWheel + 1;
			}

			return currentlyRotatingWheel;
		}

		private char NumberToChar(int number)
		{
			return Convert.ToChar(number + 65);
		}

		public enum CryptType
		{
			Encrypt,
			Decrypt
		}

		private void HandleTransformErrors(int[] wheelOrientations)
		{
			if (wheelOrientations.Length != 3)
			{
				throw new ArgumentException("The must be exactly three wheelOrientations");
			}
		}

		private void HandleCryptCharacterErrors(
				int orientation, 
				char cipherText)
		{
			if (orientation < 0 || orientation > 25)
			{
				throw new ArgumentException("Orientation must be between 0 and 25");
			}

			if (!Alphabet.Contains(cipherText))
			{
				throw new ArgumentException("You must provide an uppercase letter of the alphabet");
			}
		}
	}
}
