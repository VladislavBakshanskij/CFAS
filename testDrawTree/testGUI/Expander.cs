using System;

namespace testGUI {
    public static class Expander {
        public static float[] Reverse(this float[] arr) {
            float[] array = (float[])arr.Clone();
            for (int i = 0; i < array.Length / 2; i++) {
                float t = array[i];
                array[i] = array[array.Length - i - 1];
                array[array.Length - i - 1] = t;
            }
            return array;
        }

        public static float Max(this float[] arr) {
            float max = arr[0];
            for (int i = 1; i < arr.Length; i++) {
                if (arr[i] > max) {
                    max = arr[i];
                }
            }
            return max;
        }

        public static float[] Sort(this float[] arr) {
            float[] array = (float[])arr.Clone();
            for (int i = 1; i < array.Length; i++) {
                int key = i - 1;
                float temp = array[i];
                while (key >= 0 && array[key] < temp) {
                    array[key + 1] = array[key];
                    array[key] = temp;
                    key--;
                }
            }
            return array;
        }
    }
}
