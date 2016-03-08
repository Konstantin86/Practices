using System;

namespace algs.Sort
{
    class BubbleSort<T> : SortBase<T> where T : IComparable
    {
        public BubbleSort(T[] array) : base(array)
        {
        }

        protected override void ExecuteSort()
        {
            for (int i = 0; i < _array.Length - 1; i++)
                for (int y = 0; y < _array.Length - 1; y++)
                {
                    if (_array[y].CompareTo(_array[y+1]) > 0)
                    {
                        T temp = _array[y];
                        _array[y] = _array[y + 1];
                        _array[y + 1] = temp;
                    }
                }
        }

        protected override string Algorythm => "BubbleSort";
    }
}