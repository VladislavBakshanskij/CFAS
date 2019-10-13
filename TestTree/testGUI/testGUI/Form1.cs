<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testGUI {
    public partial class Form1 : Form {
        private Bank bank;
        private Company company;
        private bool draw = false;

        public Form1() {
            InitializeComponent();

            dataGridView1.RowCount = 3;
            dataGridView2.RowCount = 4;
            dataGridView3.RowCount = 3;

            dataGridView3.RowHeadersWidth = 70;
            dataGridView2.RowHeadersWidth = 70;
            dataGridView1.RowHeadersWidth = 90;

            char[] header_value = { 'Н', 'С', 'В' };

            for (int i = 0; i < 3; i++) {
                dataGridView3.Rows[i].HeaderCell.Value = string.Format("{0}", header_value[i]);
                dataGridView2.Rows[i].HeaderCell.Value = string.Format("{0}", header_value[i]);
            }

            dataGridView1.Rows[0].HeaderCell.Value = string.Format("{0}", "Сумма");
            dataGridView1.Rows[1].HeaderCell.Value = string.Format("{0}", "Ставка");
            dataGridView1.Rows[2].HeaderCell.Value = string.Format("{0}", "Срок(мес)");
            dataGridView2.Rows[3].HeaderCell.Value = string.Format("{0}", "Цена");

            this.groupBox4.Visible = minChance.Visible = srChance.Visible = maxChance.Visible = 
                minMoney.Visible = srMoney.Visible = maxMoney.Visible = label4.Visible = label5.Visible = mountAvr.Visible = martPrice.Visible = false;

            panel1.Paint += new PaintEventHandler(paint);
        }

        private void DrawTree(Graphics g, int lenNode, int angle, int radius, int offSet) {
            float centerX = panel1.Width / 2;
            float centerY = 0;
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Black);

            g.DrawEllipse(pen, centerX - offSet, centerY, radius, radius);
            g.FillEllipse(brush, centerX - offSet, centerY, radius, radius);

            float rightX = centerX + (float)Math.Cos(angle * Math.PI * 2 / 360.0) * lenNode;
            float rightY = centerY + (float)Math.Sin(angle * Math.PI * 2 / 360.0) * lenNode;
            g.DrawLine(pen, centerX, centerY + offSet, rightX, rightY);

            g.DrawLine(pen, rightX, rightY, rightX, rightY + lenNode);

            g.DrawEllipse(pen, rightX - offSet, rightY - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.WhiteSmoke), rightX - offSet, rightY - offSet, radius, radius);

            g.DrawEllipse(pen, rightX - offSet, rightY + lenNode - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.Yellow), rightX - offSet, rightY + lenNode - offSet, radius, radius);

            float leftX = centerX - (float)Math.Cos(angle * Math.PI * 2 / 360.0) * lenNode;
            float leftY = centerY + (float)Math.Sin(angle * Math.PI * 2 / 360.0) * lenNode;
            g.DrawLine(pen, centerX, centerY + offSet, leftX, leftY);
            g.DrawLine(pen, leftX, leftY, leftX, leftY + lenNode);
            g.DrawLine(pen, leftX, leftY, leftX + angle, leftY + lenNode);
            g.DrawLine(pen, leftX, leftY, leftX - angle, leftY + lenNode);
            
            g.DrawEllipse(pen, leftX - offSet, leftY - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.WhiteSmoke), leftX - offSet, leftY - offSet, radius, radius);

            g.DrawEllipse(pen, leftX - offSet, leftY + lenNode - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.Yellow), leftX - offSet, leftY + lenNode - offSet, radius, radius);

            g.DrawEllipse(pen, leftX + angle - offSet, leftY + lenNode - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.Green), leftX + angle - offSet, leftY + lenNode - offSet, radius, radius);

            g.DrawEllipse(pen, leftX - angle - offSet, leftY + lenNode - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.Red), leftX - angle - offSet, leftY + lenNode - offSet, radius, radius);
        }

        private void paint(object sender, PaintEventArgs e) {
            if (draw) DrawTree(e.Graphics, 200, 60, 25, 10);
        }

        private void проектToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Открыт компонент меню\nЧуть позже будет сохранение и открытие файлов");
        }

        private void button5_Click(object sender, EventArgs e) {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo() { NumberDecimalSeparator = "." };

            bank = new Bank() {
                money = float.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString(), numberFormatInfo),
                precent = float.Parse(dataGridView1.Rows[1].Cells[0].Value.ToString(), numberFormatInfo),
                dateAvr = float.Parse(dataGridView1.Rows[2].Cells[0].Value.ToString(), numberFormatInfo)
            };

            company = new Company() {
                countProduct = float.Parse(this.textBox2.Text),
                minPrice = float.Parse(dataGridView3.Rows[0].Cells[0].Value.ToString(), numberFormatInfo),
                srPrice = float.Parse(dataGridView3.Rows[1].Cells[0].Value.ToString(), numberFormatInfo),
                maxPrice = float.Parse(dataGridView3.Rows[2].Cells[0].Value.ToString(), numberFormatInfo),
                changes = new float[] {
                    float.Parse(dataGridView2.Rows[0].Cells[0].Value.ToString(), numberFormatInfo),
                    float.Parse(dataGridView2.Rows[1].Cells[0].Value.ToString(), numberFormatInfo),
                    float.Parse(dataGridView2.Rows[2].Cells[0].Value.ToString(), numberFormatInfo),
                }
            };
            Calculate();
            //draw = true;
        }

        private void Calculate() {
            if (company == null || bank == null || !company.WTF) return;
            this.groupBox4.Visible = minChance.Visible = srChance.Visible = maxChance.Visible =
                minMoney.Visible = srMoney.Visible = maxMoney.Visible = label4.Visible = label5.Visible = mountAvr.Visible = martPrice.Visible = true;

            var res = Prodection.DoSomething(bank, company);
            mountAvr.Text = Prodection.MountPrice.ToString();
            martPrice.Text = bank.money.ToString();

            minMoney.Text = res[0].ToString();
            srMoney.Text = res[1].ToString();
            maxMoney.Text = res[2].ToString();

            minChance.Text += " " + company.changes[0].ToString();
            srChance.Text += " " + company.changes[1].ToString();
            maxChance.Text += " " + company.changes[2].ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e) {

        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testGUI {
    public partial class Form1 : Form {
        private Bank bank;
        private Company company;
        private bool draw = false;

        public Form1() {
            InitializeComponent();

            dataGridView1.RowCount = 3;
            dataGridView2.RowCount = 4;
            dataGridView3.RowCount = 3;

            dataGridView3.RowHeadersWidth = 70;
            dataGridView2.RowHeadersWidth = 70;
            dataGridView1.RowHeadersWidth = 90;

            char[] header_value = { 'Н', 'С', 'В' };

            for (int i = 0; i < 3; i++) {
                dataGridView3.Rows[i].HeaderCell.Value = string.Format("{0}", header_value[i]);
                dataGridView2.Rows[i].HeaderCell.Value = string.Format("{0}", header_value[i]);
            }

            dataGridView1.Rows[0].HeaderCell.Value = string.Format("{0}", "Сумма");
            dataGridView1.Rows[1].HeaderCell.Value = string.Format("{0}", "Ставка");
            dataGridView1.Rows[2].HeaderCell.Value = string.Format("{0}", "Срок(мес)");
            dataGridView2.Rows[3].HeaderCell.Value = string.Format("{0}", "Цена");

            this.groupBox4.Visible = minChance.Visible = srChance.Visible = maxChance.Visible = 
                minMoney.Visible = srMoney.Visible = maxMoney.Visible = label4.Visible = label5.Visible = mountAvr.Visible = martPrice.Visible = false;

            panel1.Paint += new PaintEventHandler(paint);
        }

        private void DrawTree(Graphics g, int lenNode, int angle, int radius, int offSet) {
            float centerX = panel1.Width / 2;
            float centerY = 0;
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Black);

            g.DrawEllipse(pen, centerX - offSet, centerY, radius, radius);
            g.FillEllipse(brush, centerX - offSet, centerY, radius, radius);

            float rightX = centerX + (float)Math.Cos(angle * Math.PI * 2 / 360.0) * lenNode;
            float rightY = centerY + (float)Math.Sin(angle * Math.PI * 2 / 360.0) * lenNode;
            g.DrawLine(pen, centerX, centerY + offSet, rightX, rightY);

            g.DrawLine(pen, rightX, rightY, rightX, rightY + lenNode);

            g.DrawEllipse(pen, rightX - offSet, rightY - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.WhiteSmoke), rightX - offSet, rightY - offSet, radius, radius);

            g.DrawEllipse(pen, rightX - offSet, rightY + lenNode - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.Yellow), rightX - offSet, rightY + lenNode - offSet, radius, radius);

            float leftX = centerX - (float)Math.Cos(angle * Math.PI * 2 / 360.0) * lenNode;
            float leftY = centerY + (float)Math.Sin(angle * Math.PI * 2 / 360.0) * lenNode;
            g.DrawLine(pen, centerX, centerY + offSet, leftX, leftY);
            g.DrawLine(pen, leftX, leftY, leftX, leftY + lenNode);
            g.DrawLine(pen, leftX, leftY, leftX + angle, leftY + lenNode);
            g.DrawLine(pen, leftX, leftY, leftX - angle, leftY + lenNode);
            
            g.DrawEllipse(pen, leftX - offSet, leftY - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.WhiteSmoke), leftX - offSet, leftY - offSet, radius, radius);

            g.DrawEllipse(pen, leftX - offSet, leftY + lenNode - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.Yellow), leftX - offSet, leftY + lenNode - offSet, radius, radius);

            g.DrawEllipse(pen, leftX + angle - offSet, leftY + lenNode - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.Green), leftX + angle - offSet, leftY + lenNode - offSet, radius, radius);

            g.DrawEllipse(pen, leftX - angle - offSet, leftY + lenNode - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.Red), leftX - angle - offSet, leftY + lenNode - offSet, radius, radius);
        }

        private void paint(object sender, PaintEventArgs e) {
            if (draw) DrawTree(e.Graphics, 200, 60, 25, 10);
        }

        private void проектToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Открыт компонент меню\nЧуть позже будет сохранение и открытие файлов");
        }

        private void button5_Click(object sender, EventArgs e) {
            NumberFormatInfo numberFormatInfo = new NumberFormatInfo() { NumberDecimalSeparator = "." };

            bank = new Bank() {
                money = float.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString(), numberFormatInfo),
                precent = float.Parse(dataGridView1.Rows[1].Cells[0].Value.ToString(), numberFormatInfo),
                dateAvr = float.Parse(dataGridView1.Rows[2].Cells[0].Value.ToString(), numberFormatInfo)
            };

            company = new Company() {
                countProduct = float.Parse(this.textBox2.Text),
                minPrice = float.Parse(dataGridView3.Rows[0].Cells[0].Value.ToString(), numberFormatInfo),
                srPrice = float.Parse(dataGridView3.Rows[1].Cells[0].Value.ToString(), numberFormatInfo),
                maxPrice = float.Parse(dataGridView3.Rows[2].Cells[0].Value.ToString(), numberFormatInfo),
                changes = new float[] {
                    float.Parse(dataGridView2.Rows[0].Cells[0].Value.ToString(), numberFormatInfo),
                    float.Parse(dataGridView2.Rows[1].Cells[0].Value.ToString(), numberFormatInfo),
                    float.Parse(dataGridView2.Rows[2].Cells[0].Value.ToString(), numberFormatInfo),
                }
            };
            Calculate();
            //draw = true;
        }

        private void Calculate() {
            if (company == null || bank == null || !company.WTF) return;
            this.groupBox4.Visible = minChance.Visible = srChance.Visible = maxChance.Visible =
                minMoney.Visible = srMoney.Visible = maxMoney.Visible = label4.Visible = label5.Visible = mountAvr.Visible = martPrice.Visible = true;

            var res = Prodection.DoSomething(bank, company);
            mountAvr.Text = Prodection.MountPrice.ToString();
            martPrice.Text = bank.money.ToString();

            minMoney.Text = res[0].ToString();
            srMoney.Text = res[1].ToString();
            maxMoney.Text = res[2].ToString();

            minChance.Text += " " + company.changes[0].ToString();
            srChance.Text += " " + company.changes[1].ToString();
            maxChance.Text += " " + company.changes[2].ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e) {

        }
    }
}
>>>>>>> Stashed changes
