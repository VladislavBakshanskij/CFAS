using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CFAS.ObjectPrj {
    public static class Predictions {
        public static List<float> M { get; private set; } = new List<float>();//Мат. ожидание
        public static float MountPrice { get; private set; }
        public static float[] ForecastNii = new float[3];

        public static float[] GetPrediction(Bank bank, Company company) {
            MountPrice = ((bank.precent / 12 * bank.dateAvr) / 100) * bank.money;
            float m = 0;

            float[] res = new float[Company.Prices.Length];

            for (int i = 0; i < res.Length; i++) {
                res[i] = Company.Prices[i] * Company.CountProduct - MountPrice;
                    
            }

            for (int i = 0; i < res.Length; i++) m += res[i] * company.changes[i];

            M.Add(m);
            return res;
        }

        public static float[] GetPrediction(Bank bank, Company company, float countProduct) {
            MountPrice = ((bank.precent / 12 * bank.dateAvr) / 100) * bank.money;
            float m = 0;

            float[] res = new float[Company.Prices.Length];
            for (int i = 0; i < res.Length; i++) {
                res[i] = Company.Prices[i] * countProduct - MountPrice;
            }

            for (int i = 0; i < res.Length; i++) m += res[i] * company.changes[i];

            M.Add(m);
            return res;
        }
    }
}
