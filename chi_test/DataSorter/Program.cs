using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataSorter
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1: Read input data:
            string[] inputFiles = Directory.GetFiles("Input");

            if (!inputFiles.Any())
            {
                throw new FileNotFoundException("No input files are found");
            }

            var personParser = new PersonParser();

            var persons = new List<Person>();

            foreach (var file in inputFiles)
            {
                string content = File.ReadAllText(file);
                string[] records = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var record in records)
                {
                    var person = personParser.Parse(record);

                    if (person != null)
                    {
                        persons.Add(person);
                    }
                }
            }

            // 2: Sort
            // 2.1: Output 1 - sorted by Gender (Females before Males) then LastName ascending.
            Sort(persons, Output1CompareFunc);

            WriteOutput1(persons);

            Console.WriteLine("===================================");

            // 2.2: Output 2 - sorted by Date, ascending.
            Sort(persons, Output2CompareFunc);

            WriteOutput2(persons);

            // 2.3: Output 3 - sorted by last name, descending.
            Console.WriteLine("===================================");

            Sort(persons, Output3CompareFunc);

            WriteOutput3(persons);

            Console.ReadKey();
        }

        private static void Sort(List<Person> persons, Func<Person, Person, bool> comparerFunc)
        {
            int length = persons.Count;

            Person temp = persons[0];

            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (!persons[i].CustomCompare(persons[j], comparerFunc))
                    {
                        temp = persons[i];
                        persons[i] = persons[j];
                        persons[j] = temp;
                    }
                }
            }
        }

        private static void WriteOutput1(List<Person> persons)
        {
            Console.WriteLine("Output 1 (sorted by Gender (Females before Males) then LastName ascending)");

            foreach (var person in persons)
            {
                Console.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", person.Gender, person.LastName, person.FirstName, person.MiddleInitial, person.FavoriteColor, person.DateOfBirth.ToString("MM/dd/yyyy")));
            }
        }

        private static void WriteOutput2(List<Person> persons)
        {
            Console.WriteLine("Output 2 (sorted by Date, ascending)");

            foreach (var person in persons)
            {
                Console.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", person.DateOfBirth.ToString("MM/dd/yyyy"), person.LastName, person.FirstName, person.MiddleInitial, person.Gender, person.FavoriteColor));
            }
        }

        private static void WriteOutput3(List<Person> persons)
        {
            Console.WriteLine("Output 3 (sorted by last name, descending)");

            foreach (var person in persons)
            {
                Console.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", person.LastName, person.FirstName, person.MiddleInitial, person.Gender, person.FavoriteColor, person.DateOfBirth.ToString("MM/dd/yyyy")));
            }
        }

        // Sort by Gender (Females before Males) then LastName ascending
        private static bool Output1CompareFunc(Person person1, Person person2)
        {
            if (person1.Gender > person2.Gender)
            {
                return true;
            }

            if (person1.Gender < person2.Gender)
            {
                return false;
            }

            return string.CompareOrdinal(person1.LastName, person2.LastName) < 0;
        }

        private static bool Output2CompareFunc(Person person1, Person person2)
        {
            return person1.DateOfBirth < person2.DateOfBirth;
        }

        private static bool Output3CompareFunc(Person person1, Person person2)
        {
            return string.CompareOrdinal(person1.LastName, person2.LastName) >= 0;
        }
    }
}
