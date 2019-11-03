using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


/*
 
 
    Изменить отрисовку на создание Label и брать данные из основной формы    
     
  */

namespace testGUI {
    public partial class DrawingTree : Form {
        public static bool draw = false;
        public List<Label> chancies = new List<Label>();
        public List<Label> nameNode = new List<Label>();
        public float x;
        public float len = 0;
        public bool refresh = false;

        public DrawingTree() {
            InitializeComponent();
            this.Paint += new PaintEventHandler(TreeDraw);
            x = this.Width / 2;
            len = this.Height * 0.4089997f;
        }

        private void TreeDraw(object sender, PaintEventArgs e) {
            if (draw) {
                this.Controls.Clear();
                TreeDraw(x, 0, len, 60, 10, 25, e.Graphics);
            }
            if (refresh) {
                refresh = false;
                Refresh();
            }
        }

        private void TreeDraw(float x, float y, float lenNode, float angle, float offSet, float radius, Graphics g) {
            g.Clear(Color.White);
            float centerX = x;
            float centerY = y;
            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Black);

            float rightX = centerX + (float)Math.Cos(angle * Math.PI * 2 / 360.0) * lenNode;
            float rightY = centerY + (float)Math.Sin(angle * Math.PI * 2 / 360.0) * lenNode;

            g.DrawLine(pen, centerX, centerY + offSet, rightX, rightY);

            g.DrawLine(pen, rightX, rightY, rightX, rightY + lenNode);

            float leftX = centerX - (float)Math.Cos(angle * Math.PI * 2 / 360.0) * lenNode;
            float leftY = centerY + (float)Math.Sin(angle * Math.PI * 2 / 360.0) * lenNode;

            g.DrawLine(pen, centerX, centerY + offSet, leftX, leftY);
            g.DrawLine(pen, leftX, leftY, leftX, leftY + lenNode);

            g.DrawLine(pen, leftX, leftY, leftX + angle, leftY + lenNode);
            g.DrawLine(pen, leftX, leftY, leftX - angle, leftY + lenNode);

            g.DrawEllipse(pen, leftX - angle - offSet, leftY + lenNode - offSet, radius, radius);
            g.FillEllipse(new SolidBrush(Color.Red), leftX - angle - offSet, leftY + lenNode - offSet, radius, radius);
        }

        private void AddLabel(Point point, string text) {
            Controls.Add(new Label() {
                Location = point,
                Text = text
            });
        }

        private void DrawingTree_Resize(object sender, EventArgs e) {
            refresh = true;
            x = this.Width / 2;
            len = this.Height * 0.4089997f;
        }
    }
}
