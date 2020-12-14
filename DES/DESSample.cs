// разрешения использования типов в пространстве имен, чтобы не нужно было квалифицировать использование типа в этом пространстве имен. WriteLine() например
using System;
// Пространство имен System.Security.Cryptography предоставляет криптографические службы, включающие безопасное кодирование и декодирование данных, хэширование, генерация случайных чисел и проверка подлинности сообщений.
using System.Security.Cryptography;
// Пространство имен System.IO содержит типы, позволяющие осуществлять чтение и запись в файлы и потоки данных,  типы для базовой поддержки файлов и папок.
using System.IO;

class DESSample
{

    static void Main()
    {
        try 
        {
            // Создаем новый объект DES для генерации ключа
            // и вектор инициализации (IV) случайное число
            DES DESalg = DES.Create();

            Console.WriteLine("Введите текст, который хотите зашифровать. ");

            // Создаем строку для шифрования.
            string sData = Console.ReadLine();
     
            //"Гордей Васильев курсач анализ криптостойкости блочных криптосистем DES."
            string FileName = @"D:\CText.txt";

            // Шифрует текст в файл, используя имя файла, ключ и IV
            EncryptTextToFile(sData, FileName, DESalg.Key, DESalg.IV);

            // Расшифрует текст из файла с помощью имени файла, ключа и IV
            string Final = DecryptTextFromFile(FileName, DESalg.Key, DESalg.IV);

            // Отображение расшифрованной строки в файле
            // Ключевое слово Using упрощает работу с объектами которые реализуют интерфейс IDisposable
            // Интерфейс IDisposable содержит один метод .Dispose(), который используется для освобождения ресурсов, которые захватил объект. При использовании Using не обязательно явно вызывать .Dispose() для объекта
            // var - Локальная переменная с неявным типом имеет строгую типизацию
            using (var sw = File.AppendText(@"D:\Mtext.txt"))
            {
                sw.WriteLine(Final);
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    // Модификатор static используется для объявления статического члена, принадлежащего собственно типу, а не конкретному объекту
    public static void EncryptTextToFile(String Data, String FileName, byte[] Key, byte[] IV)
    {
        try
        {
            // Создаем или открываем указанный файл
            FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);

            // Создаем новый объект DES
            DES DESalg = DES.Create();

            // Создаем CryptoStream с помощью FileStream
            // и переданный ключ и вектор инициализации (IV)
            CryptoStream cStream = new CryptoStream(fStream, DESalg.CreateEncryptor(Key, IV), CryptoStreamMode.Write);

            // Создаем StreamWriter с помощью CryptoStream
            StreamWriter sWriter = new StreamWriter(cStream);

            // Запись данных в поток
            // для шифрования
            sWriter.WriteLine(Data);

            // Закройте потоки и
            // Закройте файл
            sWriter.Close();
            cStream.Close();
            fStream.Close();
            
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("Произошла криптографическая ошибка: {0}", e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("Произошла ошибка файла: {0}", e.Message);
        }
    }

    public static string DecryptTextFromFile(String FileName, byte[] Key, byte[] IV)
    {
        try
        {
            // Создаем или открываем указанный файл
            FileStream fStream = File.Open(FileName, FileMode.OpenOrCreate);

            // Создаем новый объект DES
            DES DESalg = DES.Create();

            // Создаем CryptoStream с помощью FileStream
            // и переданный ключ и вектор инициализации (IV)
            CryptoStream cStream = new CryptoStream(fStream, DESalg.CreateDecryptor(Key, IV), CryptoStreamMode.Read);

            // Создаем StreamReader с помощью CryptoStream
            StreamReader sReader = new StreamReader(cStream);

            // Чтение данных из потока
            // расшифровать его.
            string val = sReader.ReadLine();

            // Закройте потоки и
            // Закройте файл
            sReader.Close();
            cStream.Close();
            fStream.Close();

            // Вернем строку.
            return val;
        }
        catch (CryptographicException e)
        {
            Console.WriteLine("Произошла криптографическая ошибка: {0}", e.Message);
            return null;
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine("Произошла ошибка файла: {0}", e.Message);
            return null;
        }
    }
}