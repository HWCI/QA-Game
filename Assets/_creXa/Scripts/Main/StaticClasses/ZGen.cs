using UnityEngine;
using System.Collections;

namespace creXa.GameBase
{
    public static class ZGen
    {
        public static int[] GetRandomIntArray(int length, int startidx = 0, int randomtimes = -1)
        {
            if (length <= 0) return null;
            if (randomtimes == -1) randomtimes = length;

            int[] rtn = new int[length];

            for (int i = 0; i < rtn.Length; i++)
                rtn[i] = startidx + i;

            for (int i = 0; i < rtn.Length; i++)
            {
                int x = Random.Range(0, rtn.Length);
                int tmp = rtn[i];
                rtn[i] = rtn[x];
                rtn[x] = tmp;
            }

            return rtn;
        }

        public static int[] GetCulmulativeIntArray(int[] array, bool positiveFilter = true)
        {
            int total = 0;
            int[] rtn = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                if (positiveFilter && array[i] < 0)
                    rtn[i] = total;
                else
                    rtn[i] = array[i] + total;
                total += array[i];
            }
            return rtn;
        }

        public static int GetIdxWithProb(int[] probs)
        {
            int[] cprobs = GetCulmulativeIntArray(probs);

            return GetIdxWithCulmulativeProb(cprobs);
        }

        public static int GetIdxWithCulmulativeProb(int[] cprobs)
        {
            int rnd = Random.Range(0, cprobs[cprobs.Length - 1]);

            for (int i = 0; i < cprobs.Length; i++)
                if (rnd < cprobs[i]) return i;

            return -1;
        }

        public static T[] GetRandomSelectedArray<T>(int length, T[] selectors, int[] probs = null)
        {
            if (selectors == null || selectors.Length == 0) return null;
            T[] rtn = new T[length];

            if (probs == null)
            {
                probs = new int[selectors.Length];
                for (int i = 0; i < probs.Length; i++)
                    probs[i] = 1;
            }

            int[] cprobs = GetCulmulativeIntArray(probs);

            for (int i = 0; i < rtn.Length; i++)
                rtn[i] = selectors[GetIdxWithCulmulativeProb(cprobs)];

            return rtn;
        }

        public static string Zeroes(int length)
        {
            string rtn = "";
            for (int i = 0; i < length; i++)
                rtn += "0";
            return rtn;
        }
    }
}
