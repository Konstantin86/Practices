using System;
using System.Collections.Generic;

namespace algs.Search
{
    // Complexity: O(log(n))
    class BinarySearch<T, TValue> : SearchBase<T, TValue> where T : IList<TValue> where TValue : IComparable
    {
        public BinarySearch(T dataStructure) : base(dataStructure)
        {
        }

        protected override TValue ExecuteSearch()
        {
            int low = 0;
            int high = _dataStructure.Count - 1;

            while (low <= high)
            {
                int middle = (low + high) / 2;

                if (_dataStructure[middle].CompareTo(_value) == 0)
                {
                    _position = middle.ToString();
                    return _dataStructure[middle];
                }
                if (_value.CompareTo(_dataStructure[middle]) < 0)
                    high = middle - 1;
                else
                    low = middle + 1;
            }

            _position = "-1";
            return default(TValue);
        }

        protected override string Algorythm => "BinarySearch";
    }
}