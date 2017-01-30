using System;
using System.Linq;

namespace algs.Sort
{
    public class MergeSort<T> : SortBase<T> where T : IComparable
    {
        public MergeSort(T[] array) : base(array)
        {
        }

        protected override void ExecuteSort()
        {
            _array = DoSort(_array);
        }

        private T[] DoSort(T[] array)
        {
            if (array.Length == 1)
                return array;

            int midPoint = array.Length/2;
            return Merge(DoSort(array.Take(midPoint).ToArray()), DoSort(array.Skip(midPoint).ToArray()));
        }

        private static T[] Merge(T[] arr1, T[] arr2)
        {
            int a = 0, b = 0;
            T[] merged = new T[arr1.Length + arr2.Length];
            for (int i = 0; i < arr1.Length + arr2.Length; i++)
            {
                if (b < arr2.Length && a < arr1.Length)
                    if (arr1[a].CompareTo(arr2[b]) > 0 && b < arr2.Length)
                        merged[i] = arr2[b++];
                    else
                        merged[i] = arr1[a++];
                else
                    if (b < arr2.Length)
                        merged[i] = arr2[b++];
                    else
                        merged[i] = arr1[a++];
            }
            return merged;
        }

        protected override string Algorythm => "MergeSort";
    }
}