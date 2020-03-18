using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace CFAS.FileWork
{
    public class FileWriter : FileWorker
    {
        private static readonly Lazy<FileWriter> fileWriter = new Lazy<FileWriter>(() => new FileWriter());
        public static FileWriter CreateFileWriter => fileWriter.Value;

        private FileWriter() : base()
        {
            filePath = Environment.CurrentDirectory + @"\output.xlsx";
        }

        public void AddProdection(List<string> text)
        {
            prodectionForFile.Add(text);
        }

        public void AddChance(List<string> text)
        {
            chanceForFile.Add(text);
        }

        public void AddM(string text)
        {
            MForFile.Add(text);
        }

        public void AddForecastNii(string text)
        {
            forecastNiiForFile.Add(text);
        }

        public void AddBankData(string text)
        {
            bankData.Add(text);
        }

        public void Clear()
        {
            ClearChance();
            ClearProdection();
            ClearM();
            ClearForecast();
        }

        public void ClearChance()
        {
            chanceForFile.Clear();
        }

        public void ClearProdection()
        {
            prodectionForFile.Clear();
        }

        public void ClearM()
        {
            MForFile.Clear();
        }

        public void ClearForecast()
        {
            forecastNiiForFile.Clear();
        }

        public void ClearBankData()
        {
            bankData.Clear();
        }

        /// <summary>
        /// Сохраняет в файл по пути FilePath
        /// </summary>
        public void Save()
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                Workbook workbook = application.Workbooks.Add();
                Worksheet worksheet = workbook.ActiveSheet;

                worksheet.Cells[1, 2].Value = CellSourceName["Прогноз"];
                worksheet.Cells[1, 15].Value = CellSourceName["Без"];

                string[] bankData = new string[3] {
                    "Кредит", "Выплата", "Итого"
                };
                char[] id = new char[] { 'Н', 'С', 'В' };

                for (int i = 0; i < bankData.Length; ++i)
                {
                    worksheet.Cells[i + 7, 1].Value = bankData[i];
                    worksheet.Cells[i + 7, 2].Value = this.bankData[i].ToString();
                    worksheet.Cells[1, i * 4 + 3].Value = this.forecastNiiForFile[i].ToString();
                    worksheet.Cells[i + 2, 1].Value = id[i].ToString();
                }

                worksheet.Cells[10, 1].Value = CellSourceName["Март"];
                worksheet.Cells[10, 2].Value = this.bankData[0];

                for (int i = 0; i < prodectionForFile.Count - 1; ++i)
                {
                    for (int j = 0; j < prodectionForFile[i].Count; ++j)
                        worksheet.Cells[j + 2, i * 4 + 3].Value = this.prodectionForFile[i][prodectionForFile[i].Count - j - 1].ToString();

                    worksheet.Cells[5, i * 4 + 2].Value = CellSourceName["Июнь"];
                    worksheet.Cells[5, i * 4 + 3].Value = this.MForFile[i].ToString();
                }

                for (int j = 0; j < chanceForFile[0].Count; ++j)
                    worksheet.Cells[j + 2, 2].Value = chanceForFile[0][j].ToString();
                for (int i = 0; i < chanceForFile[1].Count; ++i)
                    worksheet.Cells[i + 2, 14].Value = chanceForFile[1][i].ToString();

                for (int i = 0; i < 2; ++i)
                    for (int j = 0; j < chanceForFile[i + 1].Count; ++j)
                        worksheet.Cells[j + 2, i * 4 + 6].Value = chanceForFile[i + 2][j].ToString();

                for (int i = 0; i < chanceForFile[4].Count; i++)
                    worksheet.Cells[i + 2, 2].Value = chanceForFile[4][chanceForFile[4].Count - i - 1].ToString();

                workbook.SaveAs(filePath);
                workbook.Close();
                application.Quit();
                MessageBox.Show("Сохранение завершено");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка\n " + ex.Message);
            }
        }

        /// <summary>
        /// Сохраняет в файл по пути filePath
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void Save(string filePath)
        {
            FilePath = filePath;
            Save();
        }
    }
}