using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//Moving
using System.Runtime.InteropServices;

namespace testGUI
{
    public partial class OMG : Form
    {
        public OMG()
        {
            InitializeComponent();
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            if (this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)) == false)
            {
                this.BackColor = Color.Black;
            }
        }

        private void CloseButt_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MaxButt_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            MaxButt.Visible = false;
            MinButt.Visible = true;
        }

        private void MinButt_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            MinButt.Visible = false;
            MaxButt.Visible = true;
        }

        private void HideButt_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void OMG_Load(object sender, EventArgs e)
        {

        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void BarControl_MouseDown(object sender, MouseEventArgs e)
        {
            
            ReleaseCapture();
            MaxButt.Visible = true;
            SendMessage(this.Handle, 0x112, 0xf012, 0);
            MinButt.Visible = false;

        }
    }
}
