using System;
using System.Collections.Generic;

namespace testGUI.ObjectPrj {
    public static class Prodection {
        public static List<float> M { get; private set; } = new List<float>();//Мат. ожидание
        public static float MountPrice { get; private set; }
        public static float[] ForecastNii = new float[3];

        public static bool CheckForecastNii {
            get {
                float sum = 0;
                foreach (float item in ForecastNii) sum += item;
                if (sum > 0.98f) {
                    sum = (float)Math.Floor(sum);
                    return sum >= 0.99f && sum <= 1.1f;
                } else return false;
            }
        }

        public static float[] DoProdection(Bank bank, Company company) {
            MountPrice = ((bank.precent / 12 * bank.dateAvr) / 100) * bank.money;
            float m = 0;
            
            float[] res = new float[Company.Prices.Length];
            for (int i = 0; i < res.Length; i++) res[i] = Company.Prices[i] * Company.CountProduct - MountPrice;
            for (int i = 0; i < res.Length; i++) m += res[i] * company.changes[i];

            M.Add(m);
            return res;
        }
    }
}
