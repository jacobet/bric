using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Brix
{
    public class e2
    {
        Dictionary<string, int> dict;

        // ctor
        public e2()
        {
            dict = new Dictionary<string, int>();
        }

        public void Run()
        {
            Console.WriteLine("Enter file path");
            string path = Console.ReadLine();
            Console.WriteLine("File is loading now");
            ReadFile(path);

            Console.WriteLine("Input of a 5 characters long alphanumerical string");
            string str;
            do
            {
                str = Console.ReadLine();

            } while (!(str.Any(c => char.IsDigit(c) || char.IsUpper(c))) || str.Length != 5);

            string obj = Order(str);
            if (dict.ContainsKey(obj))
                Console.WriteLine($"Result of the search: {str} match {dict[obj] + 1} times");
            else
                Console.WriteLine($"{str} don't matched in file");


        }

        private void ReadFile(string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    AddDict(ln);
                }
                file.Close();
            }
        }

        private string Order(string ln)
        {
            List<char> list = new List<char>();
            foreach (char c in ln)
            {
                list.Add(c);
            }
            list.Sort();

            string obj = string.Join("", list);
            return obj;
        }
        private void AddDict(string ln)
        {
            string obj = Order(ln);
            if (!dict.ContainsKey(obj))
            {
                dict.Add(obj, 0);
            }
            else
            {
                int i = dict[obj];
                dict[obj] = ++i;
            }
        }

    }
}
