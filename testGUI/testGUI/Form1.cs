using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            dataGridView1.RowCount = 3;
            dataGridView2.RowCount = 4;
            dataGridView3.RowCount = 3;
            dataGridView3.RowHeadersWidth = 70;
            dataGridView2.RowHeadersWidth = 70;
            dataGridView1.RowHeadersWidth = 90;
            char[] header_value = { 'Н', 'С', 'В' };
            for (int i = 0; i < 3; i++)
            { dataGridView3.Rows[i].HeaderCell.Value =String.Format("{0}",header_value[i]); dataGridView2.Rows[i].HeaderCell.Value = String.Format("{0}", header_value[i]); }
            dataGridView2.Rows[3].HeaderCell.Value = String.Format("{0}", "Цена");
            dataGridView1.Rows[0].HeaderCell.Value = String.Format("{0}", "Сумма");
            dataGridView1.Rows[1].HeaderCell.Value = String.Format("{0}", "Ставка");
            dataGridView1.Rows[2].HeaderCell.Value = String.Format("{0}", "Срок(мес)");
        }

        private void проектToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
