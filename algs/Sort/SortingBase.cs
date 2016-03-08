using System;
using System.Diagnostics;

namespace algs.Sort
{
    public abstract class SortBase<T> : IAlgBase where T : IComparable
    {
        protected T[] _array;
        private TimeSpan _elapsed;

        protected SortBase(T[] array)
        {
            _array = array;
        }

        public void Sort()
        {
            var sw = new Stopwatch();
            sw.Start();
            ExecuteSort();
            sw.Stop();
            _elapsed = sw.Elapsed;
        }

        protected abstract void ExecuteSort();

        public abstract string Algorythm { get; }

        public void WriteOutput(bool showResultArray = false)
        {
            Console.WriteLine("========{0}========", Algorythm);

            if (showResultArray)
            {
                Console.WriteLine(string.Join(",", _array));
            }

            Console.WriteLine(_elapsed);
        }
    }
}