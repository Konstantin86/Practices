using System;
using System.Diagnostics;

namespace algs.Search
{
    public abstract class SearchBase<T, TValue> : IAlgBase
    {
        protected TValue _value;
        protected string _position;
        private TimeSpan _elapsed;
        protected T _dataStructure;

        protected SearchBase(T dataStructure)
        {
            _dataStructure = dataStructure;
        }

        public TValue Search(TValue value)
        {
            _value = value;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var searchResult = ExecuteSearch();
            sw.Stop();
            _elapsed = sw.Elapsed;

            return searchResult;
        }

        protected abstract TValue ExecuteSearch();

        protected abstract string Algorythm { get; }

        public void WriteOutput(bool showResultArray = true)
        {
            {
                Console.WriteLine("========{0}========", Algorythm);

                if (showResultArray)
                {
                    Console.WriteLine("Searched value {0} is on the {1} position", _value, _position);
                }

                Console.WriteLine(_elapsed);
            }
        }
    }
}