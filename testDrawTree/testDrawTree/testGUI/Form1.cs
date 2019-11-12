using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using testGUI.ObjectPrj;
using testGUI.FileWork;

namespace testGUI {
    public partial class Form1 : Form {
        private static Bank bank = null;
        private static Company company = null;
        private static List<Company> companies = new List<Company>();
        private static List<float[]> prodecties = new List<float[]>();

        private List<List<string>> dataInput = null;
        private List<List<string>> listIteration;
        private string textGroup = "";
        private string textGroupLabel = "";
        private int iterator = 0;
        private FileReader fileReader = FileReader.CreateFileReader;
        private FileWriter fileWriter = FileWriter.CreateFileWriter;

        public Form1() {
            InitializeComponent();
            Init();
        }

        private void Init() {
            textGroup = this.firstNode.Text;
            textGroupLabel = this.firstNodeSrChance.Text;

            dataGridView1.RowCount = 4;
            dataGridView2.RowCount = 5;
            dataGridView3.RowCount = 3;

            dataGridView3.RowHeadersWidth = 70;
            dataGridView2.RowHeadersWidth = 70;
            dataGridView1.RowHeadersWidth = 90;

            char[] header_value = { 'Н', 'С', 'В' };

            for (int i = 0; i < 3; i++) {
                dataGridView3.Rows[i].HeaderCell.Value = string.Format("{0}", header_value[i]);
                dataGridView2.Rows[i + 1].HeaderCell.Value = string.Format("{0}", header_value[i]);
            }
            dataGridView2.Rows[0].HeaderCell.Value = string.Format("{0}", "Имя");
            dataGridView2[0, 0].Value = "Прогноз";
            dataGridView2.Rows[4].HeaderCell.Value = string.Format("{0}", "Цена");


            dataGridView1.Rows[0].HeaderCell.Value = string.Format("{0}", "Банк");
            dataGridView1[0, 0].Value = "Банк 1";
            dataGridView1.Rows[1].HeaderCell.Value = string.Format("{0}", "Сумма");
            dataGridView1.Rows[2].HeaderCell.Value = string.Format("{0}", "Ставка");
            dataGridView1.Rows[3].HeaderCell.Value = string.Format("{0}", "Срок(мес)");

            this.secondNode.Visible = secondNodeMinChance.Visible = secondNodeSrChance.Visible = secondNodeMaxChance.Visible =
                secondNodeMinMoney.Visible = secondNodeSrMoney.Visible = secondNodeMaxMoney.Visible = label4.Visible =
                label5.Visible = mountAvr.Visible = martPrice.Visible = this.fullSumLabel.Visible = this.fullSum.Visible = false;

            WhereAmI(1);
        }

        private void button5_Click(object sender, EventArgs e) {
            try {
                if (IsNullOrEqualsValue(1, "0")) {
                    MessageBox.Show("Дорогие пользователи, милости прошу проверьте данные!\nМать вашу!!!!!!!\nЯ любя)♥♥♥♥");
                    return;
                }

                if (company == null || bank == null) {
                    fileWriter.Clear();
                  
                    bank = new Bank() {
                        money = Convert.ToSingle(dataGridView1.Rows[1].Cells[0].Value.ToString().Replace(".", ",")),
                        precent = Convert.ToSingle(dataGridView1.Rows[2].Cells[0].Value.ToString().Replace(".", ",")),
                        dateAvr = Convert.ToSingle(dataGridView1.Rows[3].Cells[0].Value.ToString().Replace(".", ","))
                    };

                    Company.CountProduct = Convert.ToSingle(this.textBox2.Text.Replace(".", ","));
                    for (int i = 0; i < Company.Prices.Length; i++) {
                        Company.Prices[i] = Convert.ToSingle(this.dataGridView3.Rows[i].Cells[0].Value.ToString().Replace(".", ","));
                        Prodection.ForecastNii[i] = Convert.ToSingle(listIteration[0][i].Replace(".", ","));
                    }

                    company = new Company() {
                        changes = new float[] {
                            Convert.ToSingle(dataGridView2.Rows[1].Cells[0].Value.ToString().Replace(".", ",")),
                            Convert.ToSingle(dataGridView2.Rows[2].Cells[0].Value.ToString().Replace(".", ",")),
                            Convert.ToSingle(dataGridView2.Rows[3].Cells[0].Value.ToString().Replace(".", ",")),
                        }
                    };
                }

                this.firstNode.Text = $"{textGroup} при {Prodection.ForecastNii[0]}";
                this.secondNode.Text = $"{textGroup} при {Prodection.ForecastNii[1]}";
                this.thirdNode.Text = $"{textGroup} при {Prodection.ForecastNii[2]}";
                this.notBuyProdectionNode.Text = $"{textGroup} без покупки прогнозов";
            } catch (NullReferenceException ex) {
                MessageBox.Show("Ошибка:\n " + ex);
                return;
            } catch (FormatException ex) {
                MessageBox.Show("Ошибка:\n " + ex);
                return;
            }

            this.secondNode.Visible = secondNodeMinChance.Visible = secondNodeSrChance.Visible = secondNodeMaxChance.Visible =
                    secondNodeMinMoney.Visible = secondNodeSrMoney.Visible = secondNodeMaxMoney.Visible = label4.Visible =
                    label5.Visible = mountAvr.Visible = martPrice.Visible = this.fullSumLabel.Visible = this.fullSum.Visible = true;

            companies.Add(company);
            InsertChance();
            Calculate();
            companies.Clear();
            prodecties.Clear();
            Prodection.M.Clear();
            bank = null;
            company = null;
        }

        private void InsertChance() {
            float max = company.changes.Max();
            companies.Add(new Company() {
                changes = new float[] {
                    max / 2,
                    1 - max,
                    max / 2
                }
            });
            companies.Add(new Company {
                changes = new float[] {
                    (1 - max) / 2,
                    max,
                    (1 - max) / 2
                }
            });
            companies.Add(new Company() { changes = company.changes.Reverse() });
        }

        private void Calculate() {
            if (company == null || bank == null || !company.CheckChance || !Prodection.CheckForecastNii) return;
            
            foreach (var item in companies) {
                prodecties.Add(Prodection.DoProdection(bank, item).Sort());
            }

            DrawingTree.prodections = new List<Label>();
            for (int i = 0; i < prodecties[0].Length; ++i) {
                DrawingTree.prodections.Add(new Label() { Text = prodecties[0][i].ToString() });
            }
            
            #region Node
            //
            // ----------------------------------------------------------------------------
            //
            this.notBuyProdectionNodeMaxChance.Text = textGroupLabel + " " + companies[1].changes[0].ToString();
            this.notBuyProdectionNodeMinChance.Text = textGroupLabel + " " + companies[1].changes[1].ToString();
            this.notBuyProdectionNodeSrChance.Text = textGroupLabel + " " + companies[1].changes[2].ToString();

            //Left
            List<Label> lst = new List<Label>();
            for (int i = 0; i < companies[1].changes.Length; ++i) {
                lst.Add(new Label() { Text = companies[1].changes[i].ToString() });
            }
            DrawingTree.chancies.Add(lst);
            lst = new List<Label>();
            //

            this.notBuyProdectionNodeMaxMoney.Text = prodecties[1][0].ToString();
            this.notBuyProdectionNodeMinMoney.Text = prodecties[1][1].ToString();
            this.notBuyProdectionNodeSrMoney.Text = prodecties[1][2].ToString();
            //
            // 1 node
            //
            this.firstNodeMaxChance.Text = textGroupLabel + " " + companies[0].changes[0].ToString();
            this.firstNodeSrChance.Text = textGroupLabel + " " + companies[0].changes[1].ToString();
            this.firstNodeMinChance.Text = textGroupLabel + " " + companies[0].changes[2].ToString();

            for (int i = 0; i < companies[0].changes.Length; ++i) {
                lst.Add(new Label() { Text = companies[0].changes[i].ToString() });
            }
            DrawingTree.chancies.Add(lst);
            lst = new List<Label>();

            this.firstNodeMaxMoney.Text = prodecties[0][0].ToString();
            this.firstNodeSrMoney.Text = prodecties[0][1].ToString();
            this.firstNodeMinMoney.Text = prodecties[0][2].ToString();
            //
            // 2 node
            //
            this.secondNodeMaxChance.Text = textGroupLabel + " " + companies[2].changes[0].ToString();
            this.secondNodeMinChance.Text = textGroupLabel + " " + companies[2].changes[1].ToString();
            this.secondNodeSrChance.Text = textGroupLabel + " " + companies[2].changes[2].ToString();
            
            for (int i = 0; i < companies[2].changes.Length; ++i) {
                lst.Add(new Label() { Text = companies[2].changes[i].ToString() });
            }
            DrawingTree.chancies.Add(lst);
            lst = new List<Label>();

            this.secondNodeMaxMoney.Text = prodecties[2][0].ToString();
            this.secondNodeMinMoney.Text = prodecties[2][1].ToString();
            this.secondNodeSrMoney.Text = prodecties[2][2].ToString();
            //
            // 3 node
            //
            this.thirdNodeMaxChance.Text = textGroupLabel + " " + companies[3].changes[0].ToString();
            this.thirdNodeSrChance.Text = textGroupLabel + " " + companies[3].changes[1].ToString();
            this.thirdNodeMinChance.Text = textGroupLabel + " " + companies[3].changes[2].ToString();

            for (int i = 0; i < companies[3].changes.Length; ++i) {
                lst.Add(new Label() { Text = companies[3].changes[i].ToString() });
            }
            DrawingTree.chancies.Add(lst);

            this.thirdNodeMaxMoney.Text = prodecties[3][0].ToString();
            this.thirdNodeSrMoney.Text = prodecties[3][1].ToString();
            this.thirdNodeMinMoney.Text = prodecties[3][2].ToString();
            //
            // ----------------------------------------------------------------------------
            //
            #endregion

            this.firstNode_M.Text = bank.money > Prodection.M[3] ? bank.money.ToString() : Prodection.M[3].ToString();
            DrawingTree.M.Add(firstNode_M);

            this.secondNode_M.Text = bank.money > Prodection.M[2] ? bank.money.ToString() : Prodection.M[2].ToString();
            DrawingTree.M.Add(secondNode_M);

            this.thirdNode_M.Text = this.notBuyProdectionNode_M.Text 
                            = this.notBuyProdectionLabel.Text = bank.money > Prodection.M[1] ? bank.money.ToString() : Prodection.M[1].ToString();
            DrawingTree.M.Add(thirdNode_M);

            this.mountAvr.Text = Prodection.MountPrice.ToString();
            this.martPrice.Text = bank.money.ToString();

            float prodectionMoney = bank.money + Convert.ToSingle(listIteration[0][listIteration[0].Count - 1].Replace(".", ",")) 
                                + Convert.ToSingle(this.dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value.ToString().Replace(".", ","));
            this.notBuyProdectionLabel.Text = bank.money.ToString();
            this.fullSum.Text = prodectionMoney.ToString();

            DrawingTree.FullSum = fullSum;
            DrawingTree.Root = this.notBuyProdectionLabel;

            Write();
        }

        private void Write() {
            var lst = new List<string>();
            foreach (float[] values in prodecties) {
                lst = new List<string>();
                foreach (float value in values) {
                    lst.Add(value.ToString());
                }
                fileWriter.AddProdection(lst);
            }

            foreach (Company c in companies) {
                lst = new List<string>();
                foreach (float chance in c.changes) {
                    lst.Add(chance.ToString());
                }
                fileWriter.AddChance(lst);
            }

            foreach (float forecast in Prodection.ForecastNii) {
                fileWriter.AddForecastNii(forecast.ToString());
            }

            fileWriter.AddBankData(bank.money.ToString());
            fileWriter.AddBankData(Prodection.MountPrice.ToString());
            fileWriter.AddBankData(this.fullSum.Text);

            fileWriter.AddM(this.firstNode_M.Text);
            fileWriter.AddM(this.secondNode_M.Text);
            fileWriter.AddM(this.thirdNode_M.Text);
            fileWriter.AddM(this.notBuyProdectionNode_M.Text);
        }

        private void Add() {
            dataInput = new List<List<string>>();
            listIteration = listIteration ?? new List<List<string>>();
            var list = new List<string>();
            for (int i = 1; i < 5; ++i) {
                list.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                dataGridView2.Rows[i].Cells[0].Value = "";
            }
            dataInput?.Add(list);
            listIteration?.Clear();
            listIteration?.Add(list);
            for (int i = 0; i < 3; i++) {
                Prodection.ForecastNii[i] = Convert.ToSingle(list[i + 1].Replace(".", ","));
            }
            ++iterator;
            WhereAmI(iterator + 1);
        }

        private void nextBank_Click(object sender, EventArgs e) {
            Next();
        }

        private void Next() {
            if (IsNullOrEqualsValue(2)) return;
            if (iterator != 1) {
                if (listIteration == null) {
                    Add();
                    return;
                }

                var lst = new List<string>();
                for (int i = 0; i < 4; ++i) {
                    lst.Add(dataGridView2.Rows[i + 1].Cells[0].Value.ToString());
                }

                for (int i = 0; i < listIteration[iterator].Count; ++i) {
                    dataGridView2.Rows[i + 1].Cells[0].Value = listIteration[iterator][i];
                }

                dataGridView2.Rows[0].Cells[0].Value = "Прогноз";
                listIteration.RemoveAt(iterator);
                listIteration.Insert(iterator, lst);

                WhereAmI(iterator + 2);
                ++iterator;
            } else MessageBox.Show("Вы дошли до конца списка!");
        }

        private void Prev() {
            if (iterator == 0) MessageBox.Show("Вы дошли до начала списка!");
            else if (!IsNullOrEqualsValue(2)) {
                var lst = new List<string>();
                for (int i = 0; i < 4; i++) {
                    lst.Add(dataGridView2.Rows[i + 1].Cells[0].Value.ToString());
                }
                for (int i = 0; i < listIteration[iterator - 1].Count; ++i) {
                    dataGridView2.Rows[i + 1].Cells[0].Value = listIteration[iterator - 1][i];
                }
                dataGridView2.Rows[0].Cells[0].Value = "Прогноз";
                listIteration.RemoveAt(iterator - 1);
                listIteration.Insert(iterator - 1, lst);
                WhereAmI(iterator);
                --iterator;
            }
        }

        private void prevBank_Click(object sender, EventArgs e) {
            Prev();
        }

        public void ClearDGV(int x) {
            switch (x) {
                case 1:
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                        row.Cells[0].Value = "";
                    break;
                case 2:
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                        row.Cells[0].Value = "";
                    break;
                case 3:
                    foreach (DataGridViewRow row in dataGridView3.Rows)
                        row.Cells[0].Value = "";
                    break;
            }
        }

        public bool IsNullOrEqualsValue(int x, string value = "") {
            switch (x) {
                case 1:
                    foreach (DataGridViewRow row in dataGridView1.Rows) {
                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString().Equals(value)) {
                            if (row.Cells[0].Value.ToString().Equals("")) {
                                MessageBox.Show("Заполните пустые значения в таблице кредитов!");
                            } else if (row.Cells[0].Value.ToString().Equals("0")) {
                                MessageBox.Show("Заполните правильно значения в таблице кредитов!"); ;
                            }

                            return true;
                        }
                    }
                    break;
                case 2:
                    foreach (DataGridViewRow row in dataGridView2.Rows) {
                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == value) {
                            MessageBox.Show("Заполните пустые значения в таблице прогнозов!");
                            return true;
                        }
                    }
                    break;
                case 3:
                    foreach (DataGridViewRow row in dataGridView3.Rows) {
                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == value) {
                            MessageBox.Show("Заполните пустые значения в таблице прогнозных цен!");
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public void WhereAmI(int x) {
            label_forecast.Text = x + "/" + 2;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                OpenFile(this.openFileDialog1.FileName);
        }

        private void OpenFile(string FileName) {
            List<string> dataFromFile = fileReader.ReadAllText(FileName);
            FillForm(dataFromFile);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFile(Environment.CurrentDirectory + @"\tree.xlsx");
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                SaveFile(this.saveFileDialog1.FileName);
        }

        private void SaveFile(string fileName) {
            fileWriter.Save(fileName);
        }

        private void FillForm(List<string> data) {
            if (data == null || data.Count == 0) return;
            int position = 0;
            textBox1.Text = data[0];
            ++position;

            textBox2.Text = data[1];
            ++position;

            List<string> lst = new List<string>();
            dataInput = dataInput ?? new List<List<string>>();
            listIteration = listIteration ?? new List<List<string>>();

            //Price
            for (int i = 0; i < 3; ++i, ++position) {
                dataGridView3.Rows[i].Cells[0].Value = data[position];
            }

            //ForecastNii
            for (int i = 0; i < 4; ++i, ++position) lst.Add(data[position]);
            dataInput?.Add(lst);
            ++iterator;
            WhereAmI(iterator + 1);
            ClearDGV(2);

            this.dataGridView2.Rows[0].Cells[0].Value = "Прогноз";
            //Chance Product
            for (int i = 0; i < 3; ++i, ++position) {
                this.dataGridView2.Rows[i + 1].Cells[0].Value = data[position];
            }
            this.dataGridView2.Rows[4].Cells[0].Value = "0";

            if (position < data.Count) {
                for (int i = 0; i < 3; ++i, ++position) {
                    this.dataGridView1.Rows[i + 1].Cells[0].Value = data[position];
                }
            }
            listIteration.AddRange(dataInput);
        }

        private void drawTree_Click(object sender, EventArgs e) {
            DrawingTree drawing = new DrawingTree();
            DrawingTree.draw = true;
            drawing.Show();
        }
    }
}
