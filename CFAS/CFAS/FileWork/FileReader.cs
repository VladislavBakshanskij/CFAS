using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using CFAS.ObjectPrj;

namespace CFAS.FileWork {
    public sealed class FileReader : FileWorker {
        private static readonly Lazy<FileReader> fileReader = new Lazy<FileReader>(() => new FileReader());
        public static FileReader CreateFileReader => fileReader.Value;
        
        private FileReader() : base() {
            filePath = Environment.CurrentDirectory + @"\input.xlsx";
        }

        /// <summary>
        /// Считыват из файла по заданному пути
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <returns>Список считанных данных</returns>
        public List<string> ReadAllText(string filePath) {
            FilePath = filePath;
            return ReadAllText();
        }

        /// <summary>
        /// Считыват из файла по пути FilePath
        /// </summary>
        /// <returns>Список считанных данных</returns>
        public List<string> ReadAllText() {
            List<string> res = new List<string>();
            Application application = new Application();
            Workbook workbook = application.Workbooks.Open(filePath);
            Worksheet worksheet = workbook.Sheets[1];

            for (int i = 1; ; i++) {
                if (string.IsNullOrWhiteSpace(worksheet.Cells[i, 2].Text.ToString())) break;
                else res.Add(worksheet.Cells[i, 2].Text.ToString());
            }

            workbook.Close();
            application.Quit();
            return res;
        }
    }
}
