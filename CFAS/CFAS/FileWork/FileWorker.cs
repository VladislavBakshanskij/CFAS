using System.Collections.Generic;

namespace CFAS.FileWork {
    public abstract class FileWorker {
        public Dictionary<string, string> CellSourceName = new Dictionary<string, string>() {
            ["Н"] = "H",
            ["С"] = "С",
            ["В"] = "В",
            ["Прогноз"] = "Прогноз",
            ["Без"] = "Без покупки",
            ["Март"] = "Ожидается в марте",
            ["Июнь"] = "Ожидается в июне",
            ["Кредит"] = "Кредит",
            ["Выплата"] = "Выплата",
            ["Итого"] = "Итого"
        };
        
        protected string filePath;
        protected List<List<string>> prodectionForFile = new List<List<string>>();
        protected List<List<string>> chanceForFile = new List<List<string>>();
        protected List<string> MForFile = new List<string>();
        protected List<string> forecastNiiForFile = new List<string>();
        protected List<string> bankData = new List<string>();

        public string FilePath {
            get => filePath;
            set {
                if (value is string) {
                    filePath = value;
                }
            }
        }

        protected FileWorker() {
        }
    }
}
