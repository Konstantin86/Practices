using System;
using System.Collections.Generic;
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
        public void WriteOutput(bool showResultArray = false)
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

    class LinearSearch<T, TValue> : SearchBase<T, TValue> where T : IList<TValue> where TValue : IComparable
    {
        public LinearSearch(T dataStructure) : base(dataStructure)
        {
        }

        protected override TValue ExecuteSearch()
        {
            for (int i = 0; i < _dataStructure.Count - 1; i++)
                if (_dataStructure[i].CompareTo(_value) == 0)
                {
                    _position = i.ToString();
                    return _dataStructure[i];
                }

            _position = "-1";
            return default(TValue);
        }

        protected override string Algorythm => "LinearSearch";
    }