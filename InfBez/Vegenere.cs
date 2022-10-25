using System.Text;

namespace Infbez
{
    public class Vigenere
    {
        public string Alphabet { get; set; }

        public Vigenere(string alphabet)
        {
            Alphabet = alphabet;
        }

        public string Encrypt(string text, string key)
        {
            return Execute(text, key, true);
        }

        public string Decrypt(string text, string key)
        {
            return Execute(text, key, false);
        }

        private string Execute(string text, string key, bool doEncrypt)
        {
            StringBuilder result = new(text.Length);

            while (key.Length < text.Length)
            {
                key += key;
            }

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = Alphabet.IndexOf(text[i]);
                var codeIndex = Alphabet.IndexOf(key[..text.Length][i]);
                if (letterIndex < 0)
                {
                    result.Append(text[i]);
                }
                else
                {
                    result.Append(Alphabet[(Alphabet.Length + letterIndex + ((doEncrypt ? 1 : -1) * codeIndex)) % Alphabet.Length]);
                }
            }

            return result.ToString();
        }
    }
}
