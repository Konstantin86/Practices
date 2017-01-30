using System;

namespace DataSorter
{
    public class Person
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public Gender Gender { get; set; }

        public string FavoriteColor { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool CustomCompare(Person person, Func<Person, Person, bool> compareFunc)
        {
            return compareFunc(this, person);
        }
    }
}