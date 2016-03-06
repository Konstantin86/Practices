using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DataSorter
{
    public class PersonParser
    {
        private readonly Dictionary<char, Func<string, Person>> _separatorParsers;

        CultureInfo _culture = new CultureInfo("en-US");

        public PersonParser()
        {
            _separatorParsers = new Dictionary<char, Func<string, Person>>
            {
                { '|', ParseByPipe },
                { ',', ParseByComma },
                { ' ', ParseBySpace },
            };
        }

        public Person Parse(string record)
        {
            var separatorParser = _separatorParsers.First(sp => record.Contains(sp.Key));

            Person person = separatorParser.Value(record);

            return person;
        }

        private Person ParseByPipe(string record)
        {
            Person person = null;

            var fields = record.Split('|');

            if (!fields.Any())
            {
                return null;
            }

            Gender gender;

            if (fields.Length < 4 || !Enum.TryParse(fields[3], out gender))
            {
                gender = Gender.Female;
            }

            DateTime dateOfBirth;

            if (fields.Length < 6 || !DateTime.TryParseExact(fields[5], "M/d/yyyy", _culture, DateTimeStyles.AssumeUniversal, out dateOfBirth))
            {
                dateOfBirth = DateTime.MinValue;
            }

            person = new Person
            {
                LastName = fields[0],
                FirstName = fields.Length > 1 ? fields[1] : string.Empty,
                MiddleInitial = fields.Length > 2 ? fields[2] : string.Empty,
                Gender = gender,
                FavoriteColor = fields.Length > 4 ? fields[4] : string.Empty,
                DateOfBirth = dateOfBirth
            };

            return person;
        }

        private Person ParseByComma(string record)
        {
            Person person = null;

            var fields = record.Split(',');

            if (!fields.Any())
            {
                return null;
            }

            Gender gender;

            if (fields.Length < 3 || !Enum.TryParse(fields[2], out gender))
            {
                gender = Gender.Female;
            }

            DateTime dateOfBirth;

            if (fields.Length < 5 || !DateTime.TryParseExact(fields[4], "M/d/yyyy", _culture, DateTimeStyles.AssumeUniversal, out dateOfBirth))
            {
                dateOfBirth = DateTime.MinValue;
            }

            person = new Person
            {
                LastName = fields[0],
                FirstName = fields.Length > 1 ? fields[1] : string.Empty,
                Gender = gender,
                FavoriteColor = fields.Length > 3 ? fields[3] : string.Empty,
                DateOfBirth = dateOfBirth
            };

            return person;
        }

        private Person ParseBySpace(string record)
        {
            Person person = null;

            var fields = record.Split(' ');

            if (!fields.Any())
            {
                return null;
            }

            Gender gender;

            if (fields.Length < 4 || !Enum.TryParse(fields[3], out gender))
            {
                gender = Gender.Female;
            }

            DateTime dateOfBirth;

            if (fields.Length < 5 || !DateTime.TryParseExact(fields[4], "M/d/yyyy", _culture, DateTimeStyles.AssumeUniversal, out dateOfBirth))
            {
                dateOfBirth = DateTime.MinValue;
            }

            person = new Person
            {
                LastName = fields[0],
                FirstName = fields.Length > 1 ? fields[1] : string.Empty,
                MiddleInitial = fields.Length > 2 ? fields[2] : string.Empty,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                FavoriteColor = fields.Length > 5 ? fields[5] : string.Empty
            };

            return person;
        }
    }
}