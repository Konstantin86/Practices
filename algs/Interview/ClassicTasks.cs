using System;
using System.Collections.Generic;

namespace algs.Interview
{
    public class ClassicTasks
    {
        public static string RevertString(string str)
        {
            var cArray = str.ToCharArray();

            for (int i = 0; i < cArray.Length / 2; i++)
            {
                var s = cArray[i];
                cArray[i] = cArray[cArray.Length - 1 - i];
                cArray[cArray.Length - 1 - i] = s;
            }

            return new string(cArray);
        }

        /// <summary>
        /// Receives array of int values and returns distinct array of int values
        /// </summary>
        /// <param name="array"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static int[] ArrayUnique(int[] array, int size)
        {
            var result = new List<int>();

            for (int i = 0; i < size; i++)
            {
                result.Add(array[i]);

                for (int y = i + 1; y < size; y++)
                    if (array[i] == array[y]) // Duplicated element has been found
                    {
                        for (int shift = y; shift < size - 1; shift++) // Shift all the rest elements to -1
                            array[shift] = array[shift + 1];

                        size -= 1;

                        if (array[i] == array[y]) // If next element is duplicate move to previous
                            y--;
                    }
            }

            return result.ToArray();
            //array.Distinct();     //.Net analogue of implementation of the classis algorythm.
        }

        /// <summary>
        /// Returns string representation of the number
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string IntToString(int i)
        {
            var str = new Stack<char>();

            do
            {
                str.Push(Convert.ToChar((i % 10).ToString()));
            } while ((i /= 10) > 1);

            return new string(str.ToArray());
        }
    }
}