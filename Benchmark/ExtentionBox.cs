using System;

namespace Benchmark
{
    public static class ExtentionBox
    {
        public static byte[] FillWithRandomValues(this byte[] array)
        {
            Random rand = new Random();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (byte) rand.Next(256);
            }

            return array;
        }
    }
}
