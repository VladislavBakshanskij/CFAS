using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CFAS.ObjectPrj;
using CFAS.FileWork;

namespace CFAS {
    public partial class Main : Form {
        private static Bank bank;
        private static Company company;
        private static List<Company> companies;
        private static List<float[]> prodecties;

        private const float DECILMALS = 1000.0f;

        private int iterator = 0;
        private string textGroup;
        private string textGroupLabel;
        private FileReader fileReader;
        private FileWriter fileWriter;
        private List<List<string>> dataInput;
        private List<List<string>> listIteration;
        private DrawingTree drawing;

        public Main() {
            InitializeComponent();
            Init();
        }

        private void Init() {
            bank = null;
            companies = null;
            dataInput = null;

            companies = new List<Company>();
            prodecties = new List<float[]>();

            fileReader = FileReader.CreateFileReader;
            fileWriter = FileWriter.CreateFileWriter;

            textGroup = this.firstNode.Text;
            textGroupLabel = this.firstNodeSrChance.Text;

            dataGridView1.RowCount = 4;
            dataGridView2.RowCount = 5;
            dataGridView3.RowCount = 3;

            dataGridView3.RowHeadersWidth = 70;
            dataGridView2.RowHeadersWidth = 70;
            dataGridView1.RowHeadersWidth = 90;

            char[] headerValue = { 'Н', 'С', 'В' };

            for (int i = 0; i < 3; i++) {
                dataGridView3.Rows[i].HeaderCell.Value = string.Format("{0}", headerValue[i]);
                dataGridView2.Rows[i + 1].HeaderCell.Value = string.Format("{0}", headerValue[i]);
            }

            dataGridView2[0, 0].Value = string.Format("{0}", "Прогноз");
            dataGridView2.Rows[0].HeaderCell.Value = string.Format("{0}", "Имя");
            dataGridView2.Rows[4].HeaderCell.Value = string.Format("{0}", "Цена");

            dataGridView1[0, 0].Value = string.Format("{0}", "Банк 1");
            dataGridView1.Rows[0].HeaderCell.Value = string.Format("{0}", "Банк");
            dataGridView1.Rows[1].HeaderCell.Value = string.Format("{0}", "Сумма");
            dataGridView1.Rows[2].HeaderCell.Value = string.Format("{0}", "Ставка");
            dataGridView1.Rows[3].HeaderCell.Value = string.Format("{0}", "Срок(мес)");

            this.secondNode.Visible = secondNodeMinChance.Visible = secondNodeSrChance.Visible = secondNodeMaxChance.Visible =
                secondNodeMinMoney.Visible = secondNodeSrMoney.Visible = secondNodeMaxMoney.Visible = label4.Visible =
                label5.Visible = mountAvr.Visible = martPrice.Visible = this.fullSumLabel.Visible = this.fullSum.Visible = false;

            this.add_forecast.Click += add_bank_Click;
            this.button1.Click += drawTree_Click;
        }

        private void button5_Click(object sender, EventArgs e) {
            try {
                if (company == null || bank == null) {
                    fileWriter.Clear();

                    bank = new Bank() {
                        money = Floor(Convert.ToSingle(dataGridView1.Rows[1].Cells[0].Value.ToString().Replace(".", ","))),
                        precent = Floor(Convert.ToSingle(dataGridView1.Rows[2].Cells[0].Value.ToString().Replace(".", ","))),
                        dateAvr = Floor(Convert.ToSingle(dataGridView1.Rows[3].Cells[0].Value.ToString().Replace(".", ",")))
                    };

                    Company.CountProduct = Convert.ToSingle(this.textBox2.Text.Replace(".", ","));

                    for (int i = 0; i < Company.Prices.Length; i++) {
                        Company.Prices[i] = Convert.ToSingle(this.dataGridView3.Rows[i].Cells[0].Value.ToString().Replace(".", ","));
                        Predictions.ForecastNii[i] = Convert.ToSingle(listIteration[0][i].Replace(".", ","));
                    }

                    company = new Company() {
                        changes = new float[] {
                            Convert.ToSingle(dataGridView2.Rows[1].Cells[0].Value.ToString().Replace(".", ",")),
                            Convert.ToSingle(dataGridView2.Rows[2].Cells[0].Value.ToString().Replace(".", ",")),
                            Convert.ToSingle(dataGridView2.Rows[3].Cells[0].Value.ToString().Replace(".", ",")),
                        }
                    };
                }

                this.firstNode.Text = $"{textGroup} при {Predictions.ForecastNii[0]}";
                this.secondNode.Text = $"{textGroup} при {Predictions.ForecastNii[1]}";
                this.thirdNode.Text = $"{textGroup} при {Predictions.ForecastNii[2]}";
                this.notBuyProdectionNode.Text = $"{textGroup} без покупки прогнозов";
            } catch (NullReferenceException) {
                MessageBox.Show("Ошибка:\n Введите данные");
                return;
            } catch (FormatException ex) {
                MessageBox.Show("Ошибка:\n " + ex.Message);
                return;
            }

            this.secondNode.Visible = secondNodeMinChance.Visible = secondNodeSrChance.Visible = secondNodeMaxChance.Visible =
                    secondNodeMinMoney.Visible = secondNodeSrMoney.Visible = secondNodeMaxMoney.Visible = label4.Visible =
                    label5.Visible = mountAvr.Visible = martPrice.Visible = this.fullSumLabel.Visible = this.fullSum.Visible = true;
            
            drawing = new DrawingTree();
            DrawingTree.draw = true;
            
            companies.Add(company);
            InsertChance();
            Calculate();
            companies.Clear();
            prodecties.Clear();
            Predictions.M.Clear();
            bank = null;
            company = null;
        }

        private void InsertChance() {
            float max = company.changes.Max();
            companies.AddRange(new Company[] {
                new Company() {
                    changes = new float[] {
                        Floor(max / 2),
                        Floor(1 - max),
                        Floor(max / 2)
                    } 
                }, new Company() {
                    changes = new float[] {
                        Floor((1 - max) / 2),
                        Floor(max),
                        Floor((1 - max) / 2)
                    }
                }, new Company() {
                    changes = (from item in company.changes.Reverse() select Floor(item)).ToArray()
                }, new Company() {
                    changes = new float[] {
                        Floor(1 - max),
                        Floor(1 - max),
                        Floor((1 - max) / 2)
                    }
                }
            });
        }

        private float Floor(float value) {
            char[] arr = value.ToString().ToCharArray();
            int countNumber = 0;
            bool isStart = false;

            for (int i = 0; i < arr.Length; i++) {
                if (arr[i] == ',') isStart = true;
                if (isStart && arr[i] != '0') countNumber++;
            }

            return countNumber > 2 ? ((int)(value * DECILMALS)) / DECILMALS : value;
        }

        private void Calculate() {
            if (company == null || bank == null || !company.CheckChance) return;

            DrawingTree.M?.Clear();
            DrawingTree.predictions?.Clear();
            DrawingTree.forecastNii?.Clear();
            DrawingTree.chancies?.Clear();
            
            foreach (Company item in companies)
                prodecties.Add(Predictions.GetPrediction(bank, item, Convert.ToSingle(this.textBox2.Text.Replace(".", ","))).Sort());
            for (int i = 0; i < prodecties[0].Length; ++i)
                DrawingTree.predictions.Add(new System.Windows.Controls.Label() { Content = prodecties[0][i].ToString() });
            foreach (float item in Predictions.ForecastNii)
                DrawingTree.forecastNii.Add(new System.Windows.Controls.Label() { Content = item.ToString() });

            #region Node
            //
            // ----------------------------------------------------------------------------
            //
            this.notBuyProdectionNodeMaxChance.Text = textGroupLabel + " " + companies[1].changes[0].ToString();
            this.notBuyProdectionNodeMinChance.Text = textGroupLabel + " " + companies[1].changes[1].ToString();
            this.notBuyProdectionNodeSrChance.Text = textGroupLabel + " " + companies[1].changes[2].ToString();

            //Left
            List<System.Windows.Controls.Label> lst = new List<System.Windows.Controls.Label>();

            for (int i = 0; i < companies[1].changes.Length; ++i)
                lst.Add(new System.Windows.Controls.Label() { Content = companies[1].changes[i].ToString() });
            
            DrawingTree.chancies.Add(lst);
            lst = new List<System.Windows.Controls.Label>();

            this.notBuyProdectionNodeMaxMoney.Text = prodecties[1][0].ToString();
            this.notBuyProdectionNodeMinMoney.Text = prodecties[1][1].ToString();
            this.notBuyProdectionNodeSrMoney.Text = prodecties[1][2].ToString();
            //
            // 1 node
            //
            this.firstNodeMaxChance.Text = textGroupLabel + " " + companies[4].changes[0].ToString();
            this.firstNodeSrChance.Text = textGroupLabel + " " + companies[4].changes[1].ToString();
            this.firstNodeMinChance.Text = textGroupLabel + " " + companies[4].changes[2].ToString();

            for (int i = 0; i < companies[4].changes.Length; ++i)
                lst.Add(new System.Windows.Controls.Label() { Content = companies[4].changes[i].ToString() });
            
            DrawingTree.chancies.Add(lst);
            lst = new List<System.Windows.Controls.Label>();

            this.firstNodeMaxMoney.Text = prodecties[0][0].ToString();
            this.firstNodeSrMoney.Text = prodecties[0][1].ToString();
            this.firstNodeMinMoney.Text = prodecties[0][2].ToString();
            //
            // 2 node
            //
            this.secondNodeMaxChance.Text = textGroupLabel + " " + companies[2].changes[0].ToString();
            this.secondNodeMinChance.Text = textGroupLabel + " " + companies[2].changes[1].ToString();
            this.secondNodeSrChance.Text = textGroupLabel + " " + companies[2].changes[2].ToString();

            for (int i = 0; i < companies[2].changes.Length; ++i)
                lst.Add(new System.Windows.Controls.Label() { Content = companies[2].changes[i].ToString() });
            
            DrawingTree.chancies.Add(lst);
            lst = new List<System.Windows.Controls.Label>();

            this.secondNodeMaxMoney.Text = prodecties[2][0].ToString();
            this.secondNodeMinMoney.Text = prodecties[2][1].ToString();
            this.secondNodeSrMoney.Text = prodecties[2][2].ToString();
            //
            // 3 node
            //
            this.thirdNodeMaxChance.Text = textGroupLabel + " " + companies[0].changes[0].ToString();
            this.thirdNodeSrChance.Text = textGroupLabel + " " + companies[0].changes[1].ToString();
            this.thirdNodeMinChance.Text = textGroupLabel + " " + companies[0].changes[2].ToString();

            for (int i = 0; i < companies[0].changes.Length; ++i) 
                lst.Add(new System.Windows.Controls.Label() { Content = companies[0].changes[i].ToString() });
            
            DrawingTree.chancies.Add(lst);

            this.thirdNodeMaxMoney.Text = prodecties[3][0].ToString();
            this.thirdNodeSrMoney.Text = prodecties[3][1].ToString();
            this.thirdNodeMinMoney.Text = prodecties[3][2].ToString();
            //
            // ----------------------------------------------------------------------------
            //
            #endregion

            this.firstNode_M.Text = bank.money > Predictions.M[3] ? bank.money.ToString() : Predictions.M[3].ToString();
            DrawingTree.M.Add(new System.Windows.Controls.Label { Content = firstNode_M.Text });

            this.secondNode_M.Text = bank.money > Predictions.M[2] ? bank.money.ToString() : Predictions.M[2].ToString();
            DrawingTree.M.Add(new System.Windows.Controls.Label { Content = secondNode_M.Text });

            this.thirdNode_M.Text = this.notBuyProdectionNode_M.Text
                            = this.notBuyProdectionLabel.Text = bank.money > Predictions.M[1] ? bank.money.ToString() : Predictions.M[1].ToString();
            DrawingTree.M.Add(new System.Windows.Controls.Label { Content = thirdNode_M.Text });

            this.mountAvr.Text = Predictions.MountPrice.ToString();
            this.martPrice.Text = bank.money.ToString();

            float prodectionMoney = bank.money + Convert.ToSingle(listIteration[0][listIteration[0].Count - 1].Replace(".", ","))
                                + Convert.ToSingle(this.dataGridView2.Rows[dataGridView2.RowCount - 1].Cells[0].Value.ToString().Replace(".", ","));
            this.notBuyProdectionLabel.Text = bank.money.ToString();
            this.fullSum.Text = prodectionMoney.ToString();

            DrawingTree.FullSum = new System.Windows.Controls.Label() { Content = fullSum.Text };
            DrawingTree.Root = new System.Windows.Controls.Label() { Content = this.notBuyProdectionLabel.Text };

            Write();
        }

        private void Write() {
            var lst = new List<string>();
            foreach (float[] values in prodecties) {
                lst = new List<string>();
                foreach (float value in values)
                    lst.Add(value.ToString());
                fileWriter.AddProdection(lst);
            }

            foreach (Company c in companies) {
                lst = new List<string>();
                foreach (float chance in c.changes)
                    lst.Add(chance.ToString());
                fileWriter.AddChance(lst);
            }

            foreach (float forecast in Predictions.ForecastNii)
                fileWriter.AddForecastNii(forecast.ToString());

            fileWriter.AddBankData(bank.money.ToString());
            fileWriter.AddBankData(Predictions.MountPrice.ToString());
            fileWriter.AddBankData(this.fullSum.Text);

            fileWriter.AddM(this.firstNode_M.Text);
            fileWriter.AddM(this.secondNode_M.Text);
            fileWriter.AddM(this.thirdNode_M.Text);
            fileWriter.AddM(this.notBuyProdectionNode_M.Text);
        }

        private void add_bank_Click(object sender, EventArgs e) {
            Button btn = (Button)sender;
            if (iterator >= 1) return;
            if (btn.Name.Equals(add_forecast.Name)) {
                dataInput = new List<List<string>>();
                listIteration = listIteration ?? new List<List<string>>();
                var list = new List<string>();
                
                for (int i = 1; i < 5; ++i) {
                    list.Add(dataGridView2.Rows[i].Cells[0].Value.ToString());
                    dataGridView2.Rows[i].Cells[0].Value = "";
                }

                dataInput.Add(list);
                listIteration.Clear();
                listIteration.Add(list);

                for (int i = 0; i < 3; i++)
                    Predictions.ForecastNii[i] = Convert.ToSingle(list[i + 1].Replace(".", ","));
                
                ++iterator;
                WhereAmI(iterator + 1, dataInput.Count + 1);
            }
        }

        private void nextBank_Click(object sender, EventArgs e) => Next();

        private void Next() {
            if (dataInput == null) return;
            if (!IsNull(2) && iterator != dataInput.Count) {
                var lst = new List<string>();
                for (int i = 0; i < 4; ++i) {
                    lst.Add(dataGridView2.Rows[i + 1].Cells[0].Value.ToString());
                }
                for (int i = 0; i < listIteration[iterator].Count; ++i) dataGridView2.Rows[i + 1].Cells[0].Value = listIteration[iterator][i];
                dataGridView2.Rows[0].Cells[0].Value = "Прогноз";
                listIteration.RemoveAt(iterator);
                listIteration.Insert(iterator, lst);
                WhereAmI(iterator + 2, listIteration.Count + 1);
                ++iterator;
            } else MessageBox.Show("Вы дошли до конца списка!");
        }

        private void Prev() {
            if (dataInput.Count > 0 && iterator == 0) MessageBox.Show("Вы дошли до начала списка!");
            else if (!IsNull(2)) {
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
                WhereAmI(iterator, listIteration.Count + 1);
                --iterator;
            }
        }

        private void prevBank_Click(object sender, EventArgs e) => Prev();

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

        public bool IsNull(int x) {
            switch (x) {
                case 1:
                    foreach (DataGridViewRow row in dataGridView1.Rows) {
                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "") {
                            MessageBox.Show("Заполните пустые значения в таблице кредитов!");
                            return true;
                        }
                    }
                    break;
                case 2:
                    foreach (DataGridViewRow row in dataGridView2.Rows) {
                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "") {
                            MessageBox.Show("Заполните пустые значения в таблице прогнозов!");
                            return true;
                        }
                    }
                    break;
                case 3:
                    foreach (DataGridViewRow row in dataGridView3.Rows) {
                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "") {
                            MessageBox.Show("Заполните пустые значения в таблице прогнозных цен!");
                            return true;
                        }
                    }
                    break;
            }
            return false;
        }

        public void WhereAmI(int x, int y) => label_forecast.Text = x + "/" + y;

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                OpenFile(this.openFileDialog1.FileName);
        }

        private void OpenFile(string FileName) {
            List<string> dataFromFile = fileReader.ReadAllText(FileName);
            FillForm(dataFromFile);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e) => SaveFile(Environment.CurrentDirectory + @"\tree.xlsx");

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                SaveFile(this.saveFileDialog1.FileName);
        }

        private void SaveFile(string fileName) => fileWriter.Save(fileName);

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
            for (int i = 0; i < 3; ++i, ++position)
                dataGridView3.Rows[i].Cells[0].Value = data[position];

            //ForecastNii
            for (int i = 0; i < 4; ++i, ++position) lst.Add(data[position]);
            dataInput?.Add(lst);
            ++iterator;
            WhereAmI(iterator + 1, dataInput.Count + 1);
            ClearDGV(2);

            this.dataGridView2.Rows[0].Cells[0].Value = "Прогноз";
            //Chance Product
            for (int i = 0; i < 3; ++i, ++position)
                this.dataGridView2.Rows[i + 1].Cells[0].Value = data[position];
            this.dataGridView2.Rows[4].Cells[0].Value = "0";

            if (position < data.Count)
                for (int i = 0; i < 3; ++i, ++position)
                    this.dataGridView1.Rows[i + 1].Cells[0].Value = data[position];
            listIteration.AddRange(dataInput);
        }

        private void drawTree_Click(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            if (drawing == null) {
                drawing = new DrawingTree();
                DrawingTree.draw = true;
            }

            drawing.DrawTree();
            drawing.Show();
            drawing = null;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e) {
            this.add_forecast.Click -= add_bank_Click;
            this.button1.Click -= drawTree_Click;
        }
    }
}
