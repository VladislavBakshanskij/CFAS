<<<<<<< Updated upstream
using System;

namespace testGUI {
    public static class Prodection {
        public static float M { get; private set; } = 0;//Мат. ожидание
        public static float MountPrice { get; private set; }

        public static float[] DoSomething(Bank bank, Company company) {
            float mountPrice = ((bank.precent / 12 * bank.dateAvr) / 100) * bank.money;
            MountPrice = mountPrice;
            Console.WriteLine("Выплата в месяц " + mountPrice);
            float[] res = {
                company.minPrice * company.countProduct - mountPrice,
                company.srPrice * company.countProduct - mountPrice,
                company.maxPrice * company.countProduct - mountPrice,
            };
            for (int i = 0; i < res.Length; i++) {
                M += res[i] * company.changes[i];
                //Console.WriteLine(res[i]);
            }
            return res;
        }
    }
=======
using System;

namespace testGUI {
    public static class Prodection {
        public static float M { get; private set; } = 0;//Мат. ожидание
        public static float MountPrice { get; private set; }

        public static float[] DoSomething(Bank bank, Company company) {
            float mountPrice = ((bank.precent / 12 * bank.dateAvr) / 100) * bank.money;
            MountPrice = mountPrice;
            Console.WriteLine("Выплата в месяц " + mountPrice);
            float[] res = {
                company.minPrice * company.countProduct - mountPrice,
                company.srPrice * company.countProduct - mountPrice,
                company.maxPrice * company.countProduct - mountPrice,
            };
            for (int i = 0; i < res.Length; i++) {
                M += res[i] * company.changes[i];
                //Console.WriteLine(res[i]);
            }
            return res;
        }
    }
>>>>>>> Stashed changes
}