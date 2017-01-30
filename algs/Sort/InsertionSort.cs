using System;

namespace algs.Sort
{
    class InsertionSort<T> : SortBase<T> where T : IComparable
    {
        public InsertionSort(T[] array) : base(array)
        {
        }

        protected override void ExecuteSort()
        {
            _array = DoSort(_array);
        }

        public static T[] DoSort(T[] array)
        {
            for (var i = 0; i < array.Length - 1; i++)
            {
                var y = i + 1;
                while (y > 0)
                {
                    if (array[y - 1].CompareTo(array[y]) > 0)
                    {
                        var temp = array[y - 1];
                        array[y - 1] = array[y];
                        array[y] = temp;
                    }

                    y--;
                }
            }

            return array;
        }

        protected override string Algorythm => "InsertionSort";
    }
}