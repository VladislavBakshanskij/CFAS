using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using testGUI.ObjectPrj;

namespace testGUI {
    public partial class DrawingTree : Form {
        public static bool draw = false;
        public static List<Label> forecastNii = new List<Label>();
        public static List<Label> nameNode = new List<Label>();
        public static List<List<Label>> chancies = new List<List<Label>>();
        public static List<Label> prodections = new List<Label>();
        public static List<Label> M = new List<Label>();
        public static List<Label> pricies = new List<Label>();
        public static Label FullSum;
        public static Label Root;

        public float x;
        public float len = 0;
        public bool refresh = false;

        public DrawingTree() {
            InitializeComponent();
            this.Paint += new PaintEventHandler(TreeDraw);
            Init();
            for (int i = 0; i < Prodection.ForecastNii.Length; i++) {
                forecastNii.Add(new Label() { Text = Prodection.ForecastNii[i].ToString() });
            }
        }

        private void Init() {
            x = this.Width / 2;
            len = this.Height * 0.13f + 60;
        }

        private void TreeDraw(object sender, PaintEventArgs e) {
            if (draw) {
                this.Controls.Clear();
                TreeDraw(x, 0, len, 20, 10, 25, e.Graphics);
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
            Root.Location = new Point((int)x, (int)y);
            AddLabel(Root);

            Pen pen = new Pen(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Black);
            float leftNode = lenNode + 15;

            #region Left
            //Left
            float leftX = centerX - (float)Math.Cos(ToRadian(angle)) * leftNode;
            float leftY = centerY + (float)Math.Sin(ToRadian(angle)) * leftNode;

            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)leftX, (int)leftY));
            Label la = new Label { Text = Root.Text, Location = new Point((int)leftX, (int)leftY), Width = Root.Width };
            AddLabel(la);

            float leftAngle = angle + 30;
            centerX = leftX;
            centerY = leftY;

            float leftCenterX = centerX - (float)Math.Cos(ToRadian(leftAngle)) * leftNode;
            float leftCenterY = centerY + (float)Math.Sin(ToRadian(leftAngle)) * leftNode;

            leftX = leftCenterX;
            leftY = leftCenterY;

            DrawTwoLine(g, pen, centerX, centerY, leftNode, leftAngle, new Label[] {
                new Label() { Text = "Март", Width = 20 + Root.Width },
                new Label() { Text = "Июнь", Width = 20 + Root.Width },
            }, new Label[] {
                new Label(){ Text = (
                        Convert.ToSingle(prodections[0].Text) * Convert.ToSingle(chancies[0][0].Text) +
                        Convert.ToSingle(prodections[1].Text) * Convert.ToSingle(chancies[0][1].Text) +
                        Convert.ToSingle(prodections[2].Text) * Convert.ToSingle(chancies[0][2].Text)
                ).ToString(),
                    Width = Root.Width + 10
                },
                new Label(){ Text = Root.Text , Width = Root.Width }
            });

            leftX = centerX + (float)Math.Cos(ToRadian(leftAngle)) * leftNode;
            g.DrawLine(pen, new Point((int)(leftX - 30), (int)leftY), new Point((int)(leftX - 30), (int)(leftY + leftNode)));
            Label l = new Label() { Text = Root.Text, Location = new Point((int)(leftX - 30), (int)(leftY + leftNode)), Width = Root.Width };
            AddLabel(l);

            DrawThreeLine(g, pen, leftCenterX, leftCenterY, leftNode, leftAngle, 0);
            //addLabel

            #endregion

            #region Right
            //Right
            centerX = x;
            centerY = y;

            float rightNode = lenNode - 20;
            float rightX = centerX + (float)Math.Cos(ToRadian(angle)) * rightNode;
            float rightY = centerY + (float)Math.Sin(ToRadian(angle)) * rightNode;

            float rightAngle = angle + 10;

            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)rightX + 30, (int)rightY));
            //addLabel

            centerX = rightX + 30;
            centerY = rightY;

            DrawThreeLine(g, pen, centerX, centerY, rightNode, rightAngle);
            //addLabel
            
            float newX = (float)(centerX - (float)Math.Cos(ToRadian(rightAngle)) * rightNode);
            float newY = (float)(centerY + (float)Math.Sin(ToRadian(rightAngle)) * rightNode);
            DrawNode(g, pen, newX, newY, rightNode, rightAngle + 35, 0);
            DrawNode(g, pen, centerX, centerY + rightNode, rightNode, rightAngle + 35, 1);

            newX = (float)(centerX + (float)Math.Cos(ToRadian(rightAngle)) * rightNode);
            DrawNode(g, pen, newX, newY, rightNode, rightAngle + 35, 2);

            #endregion
        }

        private void DrawTwoLine(Graphics g, Pen pen, float centerX, float centerY, float lenNode, float angle, Label[] txt, params Label[] labels) {
            float x = centerX - (float)Math.Cos(ToRadian(angle)) * lenNode;
            float y = centerY + (float)Math.Sin(ToRadian(angle)) * lenNode;

            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)(x), (int)y));

            labels[0].Location = new Point((int)(x - 5), (int)y);
            AddLabel(labels[0]);

            txt[0].Location = new Point((int)(x / 2) + txt[0].Width * 2 + 2, (int)y / 2);
            AddLabel(txt[0]);

            x = centerX + (float)Math.Cos(ToRadian(angle)) * lenNode;
            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)(x - 30), (int)y));

            labels[1].Location = new Point((int)(x) - 35, (int)y - 2);
            AddLabel(labels[1]);

            txt[1].Location = new Point((int)(x / 2) + txt[1].Width * 2 + 2, (int)y / 2);
            AddLabel(txt[1]);
        }

        private void DrawThreeLine(Graphics g, Pen pen, float centerX, float centerY, float lenNode, float angle, int index) {
            float x = centerX - (float)Math.Cos(ToRadian(angle)) * lenNode;
            float y = centerY + (float)Math.Sin(ToRadian(angle)) * lenNode;
            int position = 0;

            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)(x), (int)(y)));

            AddLabel(new Label() {
                Text = prodections[position].Text,
                Location = new Point((int)(x), (int)(y)),
                Width = Root.Width
            });
            AddLabel(new Label() {
                Text = chancies[index][position].Text,
                Location = new Point((int)(x), (int)(y * 0.8)), Width = Root.Width
            });

            ++position;

            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)(centerX), (int)(centerY + lenNode)));

            AddLabel(new Label() {
                Text = chancies[index][position].Text,
                Location = new Point((int)(centerX), (int)((centerY + lenNode) * 0.8)),
                Width = Root.Width
            });
            AddLabel(new Label() {
                Text = prodections[position].Text,
                Location = new Point((int)(centerX), (int)(centerY + lenNode)),
                Width = Root.Width
            });

            ++position;

            x = centerX + (float)Math.Cos(ToRadian(angle)) * lenNode;
            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)(x), (int)(y)));

            AddLabel(new Label() {
                Text = prodections[position].Text,
                Location = new Point((int)(x), (int)(y)),
                Width = Root.Width
            });
            AddLabel(new Label() {
                Text = chancies[index][position].Text,
                Location = new Point((int)(x), (int)(y * 0.8)),
                Width = Root.Width
            });
        }

        private void DrawThreeLine(Graphics g, Pen pen, float centerX, float centerY, float lenNode, float angle) {
            float x = centerX - (float)Math.Cos(ToRadian(angle)) * lenNode;
            float y = centerY + (float)Math.Sin(ToRadian(angle)) * lenNode;
            int position = 0;

            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)(x), (int)(y)));

            AddLabel(new Label() {
                Text = forecastNii[position].Text,
                Location = new Point((int)(x), (int)(y * 0.8)),
                Width = Root.Width
            });

            ++position;

            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)(centerX), (int)(centerY + lenNode)));

            AddLabel(new Label() {
                Text = forecastNii[position].Text,
                Location = new Point((int)(x), (int)(y * 0.8)),
                Width = Root.Width
            });

            ++position;

            x = centerX + (float)Math.Cos(ToRadian(angle)) * lenNode;
            g.DrawLine(pen, new Point((int)centerX, (int)centerY), new Point((int)(x), (int)(y)));

            AddLabel(new Label() {
                Text = forecastNii[position].Text,
                Location = new Point((int)(x), (int)(y * 0.8)),
                Width = Root.Width
            });
        }

        private void DrawNode(Graphics g, Pen pen, float centerX, float centerY, float lenNode, float angle, int index) {
            DrawTwoLine(g, pen, centerX, centerY, lenNode, angle, new Label[] {
                new Label(){ Text = "Март", Width = Root.Width + 15},
                new Label(){ Text = "Июнь", Width = Root.Width + 15}
            }, new Label[] {
                new Label(){ Text = M[index].Text, Width = Root.Width },
                new Label(){ Text = Root.Text , Width = Root.Width }
            });

            float nodeX = (float)(centerX + (float)Math.Cos(ToRadian(angle)) * lenNode);
            float nodeY = (float)(centerY + (float)Math.Sin(ToRadian(angle)) * lenNode);

            g.DrawLine(pen, new Point((int)(nodeX - 30), (int)nodeY), new Point((int)(nodeX - 30), (int)(nodeY + lenNode)));

            nodeX = (float)(centerX - (float)Math.Cos(ToRadian(angle)) * lenNode);

            DrawThreeLine(g, pen, nodeX, nodeY, lenNode, angle + 5, index);
        }

        private double ToRadian(float angle) {
            return angle * Math.PI * 2 / 360.0;
        }

        private void AddLabel(Label label) {
            Controls.Add(label);
        }

        private void DrawingTree_Resize(object sender, EventArgs e) {
            refresh = true;
            Init();
        }
    }
}
