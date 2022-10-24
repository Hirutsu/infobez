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
            int q = Alphabet.Length;

            while (key.Length < text.Length)
            {
                key += key;
            }

            string gamma = key[..text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                var letterIndex = Alphabet.IndexOf(text[i]);
                var codeIndex = Alphabet.IndexOf(gamma[i]);
                if (letterIndex < 0)
                {
                    result.Append(text[i]);
                }
                else
                {
                    result.Append(Alphabet[(q + letterIndex + ((doEncrypt ? 1 : -1) * codeIndex)) % q]);
                }
            }

            return result.ToString();
        }
    }
}
