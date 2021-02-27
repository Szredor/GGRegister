using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CommandLine;


namespace GGRegister
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = CommandLine.Parser.Default.ParseArguments<Options>(args)
                .MapResult((opts) => TryNumbers(opts), (errs) => HandleParseError(errs));
        }

        static int HandleParseError(IEnumerable<Error> errs)
        {
            var result = -2;
            Console.WriteLine("errors {0}", errs.Count());
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
                result = -1;
            Console.WriteLine("Exit code {0}", result);
            return result;
        }

        static private int TryNumbers(Options o)
        {
            //Wczytanie danych potrzebnych do testu.
            if (o.Verbose) Console.WriteLine("Loading personal data from {0}.", o.PersonalDataPath);
            RegisterData data = new RegisterData();
            LoadPersonalData(data, o.PersonalDataPath);
            if (o.Verbose) Console.WriteLine("Loading numbers from {0}.", o.PhoneNumbersPath);
            string[] numbers = LoadNumbers(o.PhoneNumbersPath);

            if (o.Verbose)
                Console.WriteLine("Loaded {0} numbers!", numbers.Length);

            //Sprawdzanie numerow kolejno.
            Registration reg = new Registration();
            for (int i = 0; i < numbers.Length; ++i)
            {
                if (o.Verbose) Console.WriteLine("Trying {0}.{1}", i + 1, numbers[i]);

                data.PhoneNumber = numbers[i];
                try
                {
                    reg.RegisterAccount(data);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Selenium error. Check if the page structure hasn't changed.");
                    Console.Write(e.Message);
                }
            }
            reg.Dispose();

            //Jezeli uda sie zarejestrowac, to stworz nowy plik z niesprawdzonymi numerami.

            return 0;
        }

        static private void LoadPersonalData(RegisterData data, string path)
        {
            try
            {
                using (StreamReader personals = new StreamReader(path))
                {
                    data.Email = personals.ReadLine();
                    data.Password = personals.ReadLine();
                    data.FirstLastName = personals.ReadLine();
                    data.BirthDate = personals.ReadLine();
                    data.Town = personals.ReadLine();
                    data.PhoneNumber = personals.ReadLine();
                }
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("Insufficient memory to read new line. (Maybe giant lines somewhere?)");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            
            catch (ArgumentException e)
            {
                Console.WriteLine("Path to Personal Data is empty.");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File {0} cannot be found. Please try again.", path);
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Directory, in which the file is, cannot be found. Please try again.");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (IOException e)
            {
                Console.WriteLine("Occured IO error.");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        static private string[] LoadNumbers(string path)
        {
            List<String> result = new List<string>();
            string number;
            try
            {
                using (StreamReader numbers = new StreamReader(path))
                {
                    while ((number = numbers.ReadLine()) != null)
                        result.Add(number);
                }
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine("Insufficient memory to read new line. (Maybe giant lines somewhere?)");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            catch (ArgumentException e)
            {
                Console.WriteLine("Path to Personal Data is empty.");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File {0} cannot be found. Please try again.", path);
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine("Directory, in which the file is, cannot be found. Please try again.");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (IOException e)
            {
                Console.WriteLine("Occured IO error.");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            return result.ToArray();
        }  
    }
}
