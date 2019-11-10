using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using testGUI.ObjectPrj;
using testGUI.FileWork;

namespace testGUI {
    public partial class Form1 : Form {
        private static Bank bank = null;
        private static Company company = null;
        private static List<Company> companies = new List<Company>();
        private static List<float[]> prodecties = new List<float[]>();
        private List<string> dataFromFile = null;
        private List<List<string>> dataInput = null;
        private bool init = false;
        private string textGroup = "";
        private string textGroupLabel = "";
        private int iterator = 0;//смотрим на банк

        //мям
        private List<List<string>> listBank = new List<List<string>>();

        public Form1() {
            InitializeComponent();

            textGroup = this.firstNode.Text;
            textGroupLabel = this.firstNodeSrChance.Text;

            listBank.Add(new List<string>(4));//capacity

            dataGridView1.RowCount = 4;//варианты кредитов
            dataGridView2.RowCount = 5;//прогнозные значения
            dataGridView3.RowCount = 3; //прогнозная цена

            dataGridView3.RowHeadersWidth = 70;
            dataGridView2.RowHeadersWidth = 70;
            dataGridView1.RowHeadersWidth = 90;

            char[] header_value = { 'Н', 'С', 'В' };

            for (int i = 0; i < 3; i++) {
                dataGridView3.Rows[i].HeaderCell.Value = string.Format("{0}", header_value[i]);
                dataGridView2.Rows[i + 1].HeaderCell.Value = string.Format("{0}", header_value[i]);
            }
            dataGridView2.Rows[0].HeaderCell.Value = string.Format("{0}", "Имя");
            dataGridView2[0, 0].Value = "Прогноз 1";
            dataGridView2.Rows[4].HeaderCell.Value = string.Format("{0}", "Цена");


            dataGridView1.Rows[0].HeaderCell.Value = string.Format("{0}", "Банк");
            dataGridView1[0, 0].Value = "Банк 1";
            dataGridView1.Rows[1].HeaderCell.Value = string.Format("{0}", "Сумма");
            dataGridView1.Rows[2].HeaderCell.Value = string.Format("{0}", "Ставка");
            dataGridView1.Rows[3].HeaderCell.Value = string.Format("{0}", "Срок(мес)");

            this.dataGridView1.Click += new EventHandler(pictureBox1_Click);

            this.secondNode.Visible = secondNodeMinChance.Visible = secondNodeSrChance.Visible = secondNodeMaxChance.Visible =
                secondNodeMinMoney.Visible = secondNodeSrMoney.Visible = secondNodeMaxMoney.Visible = label4.Visible = 
                label5.Visible = mountAvr.Visible = martPrice.Visible = this.fullSumLabel.Visible = this.fullSum.Visible = false;

            this.prevCompany.Click += new EventHandler(prevBank_Click);
            this.add_forecast.Click += new EventHandler(add_bank_Click);
        }

        private void button5_Click(object sender, EventArgs e) {
            try {
                if ((company == null || bank == null) && dataFromFile == null) {
                    //Input
                    bank = new Bank() {
                        money = Convert.ToSingle(dataGridView1.Rows[1].Cells[0].Value.ToString(), new NumberFormatInfo() { NumberDecimalSeparator = "." }),
                        precent = Convert.ToSingle(dataGridView1.Rows[2].Cells[0].Value.ToString(), new NumberFormatInfo() { NumberDecimalSeparator = "." }),
                        dateAvr = Convert.ToSingle(dataGridView1.Rows[3].Cells[0].Value.ToString(), new NumberFormatInfo() { NumberDecimalSeparator = "." })
                    };

                    Company.CountProduct = Convert.ToSingle(this.textBox2.Text);
                    for (int i = 0; i < Company.Prices.Length; i++) {
                        Company.Prices[i] = Convert.ToSingle(this.dataGridView3.Rows[i].Cells[0].Value.ToString(), new NumberFormatInfo() { NumberDecimalSeparator = "." });
                        Prodection.ForecastNii[i] = Convert.ToSingle(dataInput[0][i + 1], new NumberFormatInfo() { NumberDecimalSeparator = "." });
                    }

                    company = new Company() {
                        changes = new float[] {
                            Convert.ToSingle(dataGridView2.Rows[1].Cells[0].Value.ToString(), new NumberFormatInfo() { NumberDecimalSeparator = "." }),
                            Convert.ToSingle(dataGridView2.Rows[2].Cells[0].Value.ToString(), new NumberFormatInfo() { NumberDecimalSeparator = "." }),
                            Convert.ToSingle(dataGridView2.Rows[3].Cells[0].Value.ToString(), new NumberFormatInfo() { NumberDecimalSeparator = "." }),
                        }
                    };
                } else if (company == null || bank == null) {
                    //File
                    company = new Company();
                    Company.CountProduct = Convert.ToSingle(dataFromFile[1]);
                    for (int i = 0; i < Company.Prices.Length; i++) {
                        Company.Prices[i] = Convert.ToSingle(dataFromFile[i + 2]);
                        Prodection.ForecastNii[i] = Convert.ToSingle(dataFromFile[i + 5]);
                        company.changes[i] = Convert.ToSingle(dataFromFile[i + 9]);
                    }

                    bank = new Bank() {
                        money = Convert.ToSingle(dataFromFile[dataFromFile.Count - 3]),
                        dateAvr = Convert.ToSingle(dataFromFile[dataFromFile.Count - 2]),
                        precent = Convert.ToSingle(dataFromFile[dataFromFile.Count - 1])
                    };
                }

                //Name group
                this.firstNode.Text = $"{textGroup} при {Prodection.ForecastNii[0]}";
                this.secondNode.Text = $"{textGroup} при {Prodection.ForecastNii[1]}";
                this.thirdNode.Text = $"{textGroup} при {Prodection.ForecastNii[2]}";
                this.notBuyProdectionNode.Text = $"{textGroup} без покупки прогнозов";
            } /* Отлавливание исключений */catch (NullReferenceException ex) {
                MessageBox.Show(ex.Message);
                return;
            } catch (FormatException ex) {
                MessageBox.Show(ex.Message);
                return;
            }

            this.secondNode.Visible = secondNodeMinChance.Visible = secondNodeSrChance.Visible = secondNodeMaxChance.Visible =
                    secondNodeMinMoney.Visible = secondNodeSrMoney.Visible = secondNodeMaxMoney.Visible = label4.Visible =
                    label5.Visible = mountAvr.Visible = martPrice.Visible = this.fullSumLabel.Visible = this.fullSum.Visible = true;

            companies.Add(company);
            InsertChance();
            Calculate();

            company = null;
            bank = null;
            init = false;

            companies?.Clear();
            prodecties?.Clear();
            dataInput?.Clear();
            dataFromFile?.Clear();
            dataInput = null;
            dataFromFile = null;
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
            if (company == null || bank == null || !company.CheckChance) return;
            foreach (var item in companies)
                prodecties.Add(Prodection.GetProdection(bank, item).Sort());
            //
            // ----------------------------------------------------------------------------
            //
            this.notBuyProdectionNodeMaxChance.Text = textGroupLabel + " " + companies[1].changes[0].ToString();
            this.notBuyProdectionNodeMinChance.Text = textGroupLabel + " " + companies[1].changes[1].ToString();
            this.notBuyProdectionNodeSrChance.Text = textGroupLabel + " " + companies[1].changes[2].ToString();

            this.notBuyProdectionNodeMaxMoney.Text =  prodecties[1][0].ToString();
            this.notBuyProdectionNodeMinMoney.Text = prodecties[1][1].ToString();
            this.notBuyProdectionNodeSrMoney.Text = prodecties[1][2].ToString();
            //
            // 1 node
            //
            this.firstNodeMaxChance.Text = textGroupLabel + " " + companies[0].changes[0].ToString();
            this.firstNodeSrChance.Text = textGroupLabel + " " + companies[0].changes[1].ToString();
            this.firstNodeMinChance.Text = textGroupLabel + " " + companies[0].changes[2].ToString();

            this.firstNodeMaxMoney.Text = prodecties[0][0].ToString();
            this.firstNodeSrMoney.Text = prodecties[0][1].ToString();
            this.firstNodeMinMoney.Text = prodecties[0][2].ToString();
            //
            // 2 node
            //
            this.secondNodeMaxChance.Text = textGroupLabel + " " + companies[2].changes[0].ToString();
            this.secondNodeMinChance.Text = textGroupLabel + " " + companies[2].changes[1].ToString();
            this.secondNodeSrChance.Text = textGroupLabel + " " + companies[2].changes[2].ToString();

            this.secondNodeMaxMoney.Text = prodecties[2][0].ToString();
            this.secondNodeMinMoney.Text = prodecties[2][1].ToString();
            this.secondNodeSrMoney.Text = prodecties[2][2].ToString();
            //
            // 3 node
            //
            this.thirdNodeMaxChance.Text = textGroupLabel + " " + companies[3].changes[0].ToString();
            this.thirdNodeSrChance.Text = textGroupLabel + " " + companies[3].changes[1].ToString();
            this.thirdNodeMinChance.Text = textGroupLabel + " " + companies[3].changes[2].ToString();

            this.thirdNodeMaxMoney.Text = prodecties[3][0].ToString();
            this.thirdNodeSrMoney.Text = prodecties[3][1].ToString();
            this.thirdNodeMinMoney.Text = prodecties[3][2].ToString();
            //
            // ----------------------------------------------------------------------------
            //

            this.firstNode_M.Text = bank.money > Prodection.M[3] ? bank.money.ToString() : Prodection.M[3].ToString();
            this.secondNode_M.Text = bank.money > Prodection.M[2] ? bank.money.ToString() : Prodection.M[2].ToString();
            this.thirdNode_M.Text = this.notBuyProdectionNode_M.Text = this.notBuyProdectionLabel.Text = bank.money > Prodection.M[1] ? bank.money.ToString() : Prodection.M[1].ToString();

            this.mountAvr.Text = Prodection.MountPrice.ToString();
            this.martPrice.Text = bank.money.ToString();

            /*float m1 = Convert.ToSingle(this.firstNode_M.Text);
            float m2 = Convert.ToSingle(this.secondNode_M.Text);
            float m3 = Convert.ToSingle(this.thirdNode_M.Text);

            float half = (m1 + m2 + m3) / 3;*/
            float prodectionMoney = bank.money + (dataFromFile == null ? Convert.ToSingle(dataInput[0][4], new NumberFormatInfo() { NumberDecimalSeparator = "." }) : Convert.ToSingle(dataFromFile[8]));
            this.notBuyProdectionLabel.Text = this.fullSum.Text = prodectionMoney.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
           /* DrawingTree drawingTree = new DrawingTree();
            DrawingTree.draw = true;
            drawingTree.Show();*/
        }

        // <3 <3 <3
        private void add_bank_Click(object sender, EventArgs e) {
            Button btn = (Button)sender;
            if (btn.Name.Equals(add_forecast.Name)) {
                if (!init) {
                    dataInput = new List<List<string>>();
                    var list = new List<string>();
                    foreach (DataGridViewRow data in this.dataGridView2.Rows) {
                        list.Add(data.Cells[0].Value.ToString());
                        data.Cells[0].Value = "";
                    }
                    dataInput.Add(list);
                    for (int i = 0; i < 3; i++) {
                        Prodection.ForecastNii[i] = Convert.ToSingle(list[i + 1], new NumberFormatInfo() { NumberDecimalSeparator = "." });
                    }
                    init = true;
                }
                return;
            }

            if (iterator < listBank.Count - 1) iterator = listBank.Count - 1;//это если мы где-то в жопке и создаем новый столбик
            if (NotNullValue(1) == 1) {
                for (int i = 0; i < 4; i++) listBank[iterator].Add(dataGridView1[0, i].Value.ToString());
                listBank.Add(new List<string>(4));
                WhereAmI(listBank.Count, listBank.Count, 1);
                iterator++;
                ClearDGV(1);
                dataGridView1[0, 0].Value = "Банк " + listBank.Count;
            }
        }

        private void remove_bank_Click(object sender, EventArgs e) {
            DialogResult DeleteBank = MessageBox.Show("Вы действительно хотите удалить вариант кредита?", "Удаление кредита", MessageBoxButtons.YesNo);
            if (DeleteBank == DialogResult.Yes) {
                if (listBank.Count > 2 && iterator != listBank.Count - 1) {
                    listBank.RemoveAt(iterator);
                    for (int i = 0; i < 4; i++)
                        dataGridView1[0, i].Value = listBank[iterator][i];
                } else if (listBank.Count == 2 && iterator == 0) {
                    listBank.RemoveAt(iterator);
                    for (int i = 0; i < 4; i++)
                        dataGridView1[0, i].Value = listBank[iterator][i];
                } else if (iterator == listBank.Count - 1 && listBank.Count > 1) {
                    listBank.RemoveAt(iterator);
                    for (int i = 0; i < 4; i++)
                        dataGridView1[0, i].Value = listBank[iterator][i];
                    iterator--;
                } //ЗА ПРЕДЕЛАМИ ИНДЕКС, ТВОЮ МАТЬ!!!  почему ему не нравится [iterator--]? тк на каждой итерации цикла переменая дикрментируется
                else ClearDGV(1);
                WhereAmI(iterator + 1, listBank.Count, 1);
            }
        }

        private void nextBank_Click(object sender, EventArgs e) {
            //это делал ты, я это оставлю здесь ~ <3 

            /* if (iterator + 1 >= listBank.Count - 1) {
                 for (int i = 0; i < 4; i++) this.dataGridView1[0, i].Value = "";  ???????????????
             } else {*/
            if (listBank.Count == 0) MessageBox.Show("Внесен только один кредит!");
            else if (NotNullValue(1) == 1 && iterator != listBank.Count - 1) {
                for (int i = 0; i < 4; i++)
                    listBank[iterator].Insert(i, dataGridView1[0, i].Value.ToString());//не хочет без tostr()
                while (listBank[iterator].Count > 4) listBank[iterator].RemoveAt(4);
                ClearDGV(1);
                iterator++;
                WhereAmI(iterator + 1, listBank.Count, 1);
                for (int i = 0; i < 4; i++) dataGridView1[0, i].Value = listBank[iterator][i];
            } else MessageBox.Show("Вы дошли до конца списка!");

        }

        private void prevBank_Click(object sender, EventArgs e) {
            Button btn = (Button)sender;
            if (listBank.Count > 0 && iterator == 0) MessageBox.Show("Вы дошли до начала списка!");
            else if (NotNullValue(1) == 1) {
                for (int i = 0; i < 4; i++)
                    listBank[iterator].Insert(i, dataGridView1[0, i].Value.ToString());//не хочет без tostr()
                while (listBank[iterator].Count > 4) listBank[iterator].RemoveAt(4);
                ClearDGV(1);
                iterator--;
                WhereAmI(iterator + 1, listBank.Count, 1);
                for (int i = 0; i < 4; i++) dataGridView1[0, i].Value = listBank[iterator][i];
            }
        }

        //еще больше switch/case <3 <3 <3
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

        public int NotNullValue(int x) {
            int z = 1;
            switch (x) {
                case 1:
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "") {
                            MessageBox.Show("Заполните пустые значения в таблице кредитов!");
                            z = 0;
                            break;
                        }
                    break;
                case 2:
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "") {
                            MessageBox.Show("Заполните пустые значения в таблице прогнозов!");
                            z = 0;
                            break;
                        }
                    break;
                case 3:
                    foreach (DataGridViewRow row in dataGridView3.Rows)
                        if (row.Cells[0].Value == null || row.Cells[0].Value.ToString() == "") {
                            MessageBox.Show("Заполните пустые значения в таблице прогнозных цен!");
                            z = 0;
                            break;
                        }
                    break;
            }
            return z;
        }

        public void WhereAmI(int x, int y, int z) {
            switch (z) {
                case 1: label_bank.Text = x + "/" + y; break;
                case 2: label_forecast.Text = x + "/" + y; break;
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e) {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
                OpenFile(this.openFileDialog1.FileName);
        }
        
        private void OpenFile(string FileName) {
            FileReader fileReader = FileReader.CreateFileReader;
            dataFromFile = fileReader.ReadFromFile(FileName);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveFile(Environment.CurrentDirectory + @"\tree.xlsx");
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                SaveFile(this.saveFileDialog1.FileName);
        }

        private void SaveFile(string fileName) {
            FileWriter fileWriter = FileWriter.CreateFileWriter;
            //Чуть позже выгрузка
            fileWriter.Save(fileName);
        }
    }
}
