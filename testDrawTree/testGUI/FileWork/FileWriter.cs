using System;
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace testGUI.FileWork {
    public class FileWriter : FileWorker {
        private static readonly Lazy<FileWriter> fileWriter = new Lazy<FileWriter>(() => new FileWriter());
        public static FileWriter CreateFileWriter => fileWriter.Value;

        private FileWriter() : base() {
            filePath = Environment.CurrentDirectory + @"\output.xlsx";
        }

        /// <summary>
        /// Записывает данные
        /// </summary>
        /// <param name="text">Записываемый текст</param>
        public void WriteToFile(string text) {
            Text += text;
        }
        
        /// <summary>
        /// Сохраняет в файл по пути FilePath
        /// </summary>
        public void Save() {
            if (!File.Exists(filePath)) File.Create(filePath);

            Application application = new Application();
            Workbook workbook = application.Workbooks.Add();
            Worksheet worksheet = workbook.ActiveSheet;

            

            workbook.SaveAs(filePath);
            application.Quit();
        }

        /// <summary>
        /// Сохраняет в файл по пути filePath
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void Save(string filePath) {
            FilePath = filePath;
            Save();
        }
    }
}
