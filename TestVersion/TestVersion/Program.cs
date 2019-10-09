using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVersion {
    public class Program {
        public static void Main(string[] args) {
            Bank bank = null;
            Company company = null;

            Console.WriteLine("Показать пример?[yes\\no]");
            string answer = Console.ReadLine().ToLower();
            if (answer.Equals("no")) {
                NumberFormatInfo numberFormatInfo = new NumberFormatInfo() {
                    NumberDecimalSeparator = "."
                };
                Console.WriteLine("Информация о банке");

                Console.WriteLine("Введите Сумму взятую в Кредит");
                float sumMoney = float.Parse(Console.ReadLine(), numberFormatInfo);

                Console.WriteLine("Введите срок выплаты");
                float date = float.Parse(Console.ReadLine(), numberFormatInfo);

                Console.WriteLine("Введите годовой процент");
                float proc = float.Parse(Console.ReadLine(), numberFormatInfo);
                bank = new Bank {
                    money = sumMoney,
                    dateAvr = date,
                    procent = proc
                };

                Console.WriteLine("Информация о производителе");

                Console.WriteLine("Количество товара");
                float count = float.Parse(Console.ReadLine(), numberFormatInfo);

                Console.WriteLine("Максимальная стоимость товара");
                float max = float.Parse(Console.ReadLine(), numberFormatInfo);

                Console.WriteLine("Средняя стоимость");
                float sr = float.Parse(Console.ReadLine(), numberFormatInfo);

                Console.WriteLine("Минимальная стоимость");
                float min = float.Parse(Console.ReadLine(), numberFormatInfo);
                company = new Company {
                    countProduct = count,
                    maxPrice = max,
                    srPrice = sr,
                    minPrice = min
                };
                company.changes = new float[3];
                string[] chageName = new string[] { "Минимальный", "Средний", "МаксимальныйЫ" };

                for (int i = 0; i < 3; i++) {
                    Console.WriteLine("Введите " + chageName[i] + " шанс");
                    company.changes[i] = float.Parse(Console.ReadLine(), numberFormatInfo);
                    if (company.changes[i] > 1) {
                        Console.WriteLine("Вероятность меньше 1");
                        company.changes[i] = float.Parse(Console.ReadLine(), numberFormatInfo);
                    }
                }

                if (!company.WTF) {
                    Console.WriteLine("Вероятность в сумме должна давать 1");
                    for (int i = 0; i < 3; i++) {
                        Console.WriteLine("Введите " + chageName[i] + " шанс");
                        company.changes[i] = float.Parse(Console.ReadLine(), numberFormatInfo);
                        if (company.changes[i] > 1) {
                            Console.WriteLine("Вероятность меньше 1");
                            company.changes[i] = float.Parse(Console.ReadLine(), numberFormatInfo);
                        }
                    }
                }
            } else {
                bank = new Bank();
                company = new Company();
            }

            var changes = Prodection.DoSomething(bank, company);
            foreach (var item in changes) Console.WriteLine(item + "\n");
            Console.WriteLine("Ожидается " + Prodection.M);
            Console.ReadKey();
        }
    }
}
