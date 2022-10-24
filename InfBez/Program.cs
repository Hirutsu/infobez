using Infbez;
using Infbez.Task1;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

//константы
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
const string directory = @"L:\GIT\InfBez\InfBez\Task1";
const string infoFile = @"L:\GIT\InfBez\InfBez\Task1\File1.txt";
const string fileShirf = @"L:\GIT\InfBez\InfBez\data_shifr.txt";
const string key = "мелкая";
const string data = @"L:\GIT\InfBez\InfBez\data.txt";
const string directoryOutPut = @"C:\Users\ROKO000000005\Desktop";
const string inPut = @"L:\GIT\InfBez\InfBez\input.txt";
const string outPut = @"L:\GIT\InfBez\InfBez\output.txt";
var code = Encoding.GetEncoding(1251);

//task1
void Task1()
{
    List<KeyValuePair<string, int>> files = new();
    Hash.GetFiles(directory, ref files);
    foreach (var file in files)
    {
        Console.WriteLine($"{file.Key} -> Hash {file.Value}");
    }
    Hash.Save(infoFile, files);
}

//task2
Dictionary<byte, byte> GetEncoder(Encoding code)
{
    Dictionary<byte, byte> res = new()
    {
        { code.GetBytes(new char[] { 'а' })[0], code.GetBytes(new char[] { 'a' })[0] },
        { code.GetBytes(new char[] { 'А' })[0], code.GetBytes(new char[] { 'A' })[0] },
        { code.GetBytes(new char[] { 'с' })[0], code.GetBytes(new char[] { 'c' })[0] },
        { code.GetBytes(new char[] { 'С' })[0], code.GetBytes(new char[] { 'C' })[0] },
        { code.GetBytes(new char[] { 'е' })[0], code.GetBytes(new char[] { 'e' })[0] },
        { code.GetBytes(new char[] { 'Е' })[0], code.GetBytes(new char[] { 'E' })[0] },
        { code.GetBytes(new char[] { 'у' })[0], code.GetBytes(new char[] { 'y' })[0] },
        { code.GetBytes(new char[] { 'У' })[0], code.GetBytes(new char[] { 'Y' })[0] },
        { code.GetBytes(new char[] { 'Т' })[0], code.GetBytes(new char[] { 'T' })[0] },
        { code.GetBytes(new char[] { 'о' })[0], code.GetBytes(new char[] { 'o' })[0] },
        { code.GetBytes(new char[] { 'О' })[0], code.GetBytes(new char[] { 'O' })[0] },
        { code.GetBytes(new char[] { 'р' })[0], code.GetBytes(new char[] { 'p' })[0] },
        { code.GetBytes(new char[] { 'Р' })[0], code.GetBytes(new char[] { 'P' })[0] },
        { code.GetBytes(new char[] { 'х' })[0], code.GetBytes(new char[] { 'x' })[0] },
        { code.GetBytes(new char[] { 'Х' })[0], code.GetBytes(new char[] { 'X' })[0] }
    };

    return res;
}

void Task2()
{
    var encoder = GetEncoder(code);

    Console.WriteLine("Выберите опцию:");
    Console.WriteLine("1. Шифрование");
    Console.WriteLine("2. Дешифрование");

    _ = int.TryParse(Console.ReadLine(), out int menu);

    switch (menu)
    {
        case 1:
            {
                string inputText = "";
                string dataText = "";

                using (StreamReader sr = new(inPut, code))
                {
                    inputText = sr.ReadToEnd();
                }

                using (StreamReader sr = new(data, code))
                {
                    dataText = sr.ReadToEnd();
                }

                using StreamWriter sw = new(data, false, code);
                int index = 0;

                foreach (char ch in inputText)
                {
                    byte[] bytes = code.GetBytes(new char[] { ch }).Take(1).ToArray();

                    BitArray bits = new(bytes);

                    foreach (bool item in bits)
                    {
                        bool charFound = false;

                        while ((!charFound) && (index <= dataText.Length - 1))
                        {
                            byte checker = code.GetBytes(new char[] { dataText[index] })[0];

                            if (encoder.ContainsKey(checker))
                            {
                                charFound = true;

                                if (item)
                                {
                                    dataText = dataText.Remove(index, 1).Insert(index, code.GetString(new byte[] { encoder[checker] }));
                                }
                            }
                            index++;
                        }
                    }
                }
                sw.Write(dataText);
                break;
            }
        case 2:
            {
                using StreamReader sr = new(data, code);
                using StreamWriter sw = new(outPut, false, code);
                bool shouldContinue = true;
                string dataText = sr.ReadToEnd();
                int iter = 0;

                do
                {
                    BitArray bits = new(8, false);

                    for (int i = 0; i < bits.Length; i++)
                    {
                        bool charFound = false;

                        while ((!charFound) && (iter <= dataText.Length - 1))
                        {
                            byte checker = code.GetBytes(new char[] { dataText[iter] })[0];

                            if (encoder.ContainsKey(checker) || encoder.ContainsValue(checker))
                            {
                                charFound = true;

                                if (encoder.ContainsValue(checker))
                                {
                                    bits[i] = true;
                                }
                            }
                            iter++;
                        }
                    }

                    byte[] bytes = new byte[1];
                    bits.CopyTo(bytes, 0);

                    if (bytes[0] == byte.MinValue)
                    {
                        shouldContinue = false;
                    }
                    else
                    {
                        sw.Write(code.GetString(bytes));
                    }
                }
                while (shouldContinue);
                break;
            }
        default:
            {
                Console.WriteLine("Вы ввели неверно");
                break;
            }
    }
}

//task3
string GetAlphabet()
{
    StringBuilder res = new();

    for (byte i = byte.MinValue; i < byte.MaxValue; i++)
    {
        res.Append(code?.GetString(new byte[] { i }));
    }

    return res.ToString();
}

void Task3()
{
    string alphabet = GetAlphabet();

    Console.WriteLine("Выберите действие:");
    Console.WriteLine("1. Шифрование каталога");
    Console.WriteLine("2. Дешифрование каталога");

    _ = int.TryParse(Console.ReadLine(), out int menu);

    switch (menu)
    {
        case 1:
            {
                Folder data = new(directory);

                string dataToCypher;
                using (MemoryStream stream = new())
                {
                    new BinaryFormatter().Serialize(stream, data);
                    dataToCypher = code.GetString(stream.ToArray());
                }

                using (StreamWriter sw = new(fileShirf))
                {
                    Vigenere vigenere = new(alphabet);
                    sw.Write(vigenere.Encrypt(dataToCypher, key));
                }

                var dir = new DirectoryInfo(directory);
                dir.Delete(true);
                break;
            }
        case 2:
            {
                string dataToConvert;

                using (StreamReader sr = new(fileShirf))
                {
                    Vigenere vigenere = new(alphabet);
                    dataToConvert = vigenere.Decrypt(sr.ReadToEnd(), key);
                }

                Folder data;

                using (MemoryStream stream = new(code.GetBytes(dataToConvert)))
                {
                    data = (Folder)new BinaryFormatter().Deserialize(stream);
                }

                data.DeployFolder(directoryOutPut);
                File.Delete(directory);
                break;
            }
        default:
            {
                Console.WriteLine("Введите от 1 до 2");
                break;
            }
    }
}

Console.WriteLine("Задание 1 - 1");
Console.WriteLine("Задание 4 - 3");
Console.WriteLine("Задание 6 - 3");
Console.Write("Меню - ");
int menu = Convert.ToInt32(Console.ReadLine());

switch (menu)
{
    case 1:
        {
            Task1();
            break;
        }
    case 2:
        {
            Task2();
            break;
        }
    case 3:
        {
            Task3();
            break;
        }
    default:
        {
            Console.WriteLine("Введите от 1 до 3!");
            break;
        }
}
