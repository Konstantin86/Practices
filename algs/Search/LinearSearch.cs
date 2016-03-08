using System;
using System.Collections.Generic;

namespace algs.Search
{
    // Complexity: O(n)
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
}