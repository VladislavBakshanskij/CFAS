using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CFAS {
    public partial class DrawingTree : Window {
        public static bool draw = false;
        public static List<Label> forecastNii = new List<Label>();
        public static List<List<Label>> chancies = new List<List<Label>>();
        public static List<Label> predictions = new List<Label>();
        public static List<Label> M = new List<Label>();
        public static Label FullSum;
        public static Label Root;

        private const double OFFSET = 18;

        public DrawingTree() {
            InitializeComponent();
            System.Drawing.Icon icon = Properties.Resources.dollar;
            System.Drawing.Bitmap bitmap = icon.ToBitmap();
            this.Icon = bitmap.ToImageSource();
            this.ResizeMode = ResizeMode.NoResize;
            FontSize = 12.5;
        }

        public void DrawTree() => DrawTree(508, OFFSET / 2.0, 20, 123.57);

        private void DrawTree(double x, double y, double angle, double lenNode) {
            if (draw) {
                Root.Margin = new Thickness(x - 10, y - 6, 0, 0);
                AddLabel(Root);
                DrawLeftNode(x, y, angle, lenNode);
                DrawRightNode(x, y, angle, lenNode);
                draw = false;
            }
        }

        #region Left
        private void DrawLeftNode(double centerX, double centerY, double angle, double lenNode) {
            double leftNode = lenNode + 15;
            double leftX = centerX - Math.Cos(ToRadian(angle)) * leftNode - 80;
            double leftY = centerY + Math.Sin(ToRadian(angle)) * leftNode;
            double leftAngle = angle + 30;
            double leftCenterX = leftX - Math.Cos(ToRadian(leftAngle)) * leftNode;
            double leftCenterY = leftY + Math.Sin(ToRadian(leftAngle)) * leftNode;

            Line leftLine = new Line() {
                X1 = centerX,
                Y1 = centerY + OFFSET,
                X2 = leftX,
                Y2 = leftY,
                Stroke = new SolidColorBrush(Color.FromRgb(1, 1, 1)),
            };

            AddUIElement(leftLine);
            AddLabel(new Label[] {
                new Label() {
                    Content = "Прогноз не заказан",
                    Margin = new Thickness(leftX - OFFSET * 0.35, leftY - OFFSET * 1.3, 0, 0),
                    RenderTransform = new RotateTransform(353),
                    FontSize = 13
                },new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(leftX - 10, leftY - 5, 0, 0)
                }
            });

            DrawLine(leftX, leftY + OFFSET, leftCenterX, leftCenterY, Color.FromRgb(1, 1, 1));
            AddLabel(new Label[] {
                new Label() {
                    Content = "Март",
                    Margin = new Thickness(leftCenterX - OFFSET, leftCenterY - OFFSET, 0, 0),
                    RenderTransform = new RotateTransform(310)
                },
                new Label(){
                   Content = (
                        Convert.ToSingle(predictions[0].Content) * Convert.ToSingle(chancies[0][0].Content) +
                        Convert.ToSingle(predictions[1].Content) * Convert.ToSingle(chancies[0][1].Content) +
                        Convert.ToSingle(predictions[2].Content) * Convert.ToSingle(chancies[0][2].Content)
                   ).ToString(),
                   Margin = new Thickness(leftCenterX - OFFSET, leftCenterY - 5, 0, 0)
                }
            });

            DrawLine(leftX, leftY + OFFSET, leftX + Math.Cos(ToRadian(leftAngle)) * leftNode - 30, leftCenterY, Color.FromRgb(1, 1, 1));
            AddLabel(new Label[] {
                new Label() {
                    Content = "Июнь",
                    Margin = new Thickness(leftX + Math.Cos(ToRadian(leftAngle)) * leftNode - 30, leftCenterY - OFFSET * 3, 0, 0),
                    RenderTransform = new RotateTransform(60)
                },
                new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(leftX + Math.Cos(ToRadian(leftAngle)) * leftNode - 30 - OFFSET, leftCenterY - 5, 0, 0)
                }
            });

            DrawThreeLine(leftCenterX, leftCenterY, leftNode, leftAngle, 0);

            leftX = leftX + Math.Cos(ToRadian(leftAngle)) * leftNode - 30;

            DrawLine(leftX, leftCenterY + OFFSET, leftX, leftCenterY + leftNode, Color.FromRgb(1, 1, 1));
            AddLabel(new Label() {
                Content = Root.Content,
                Margin = new Thickness(leftX - OFFSET, leftCenterY + leftNode, 0, 0)
            });
        }
        #endregion

        #region Right
        private void DrawRightNode(double centerX, double centerY, double angle, double lenNode) {
            double rightNode = lenNode - 20;
            double rightX = centerX + Math.Cos(ToRadian(angle)) * rightNode + 80;
            double rightY = centerY + Math.Sin(ToRadian(angle)) * rightNode;
            double rightAngle = angle + 10;

            Line rightLine = new Line() {
                X1 = centerX,
                Y1 = centerY + OFFSET,
                X2 = rightX,
                Y2 = rightY,
                Stroke = new SolidColorBrush(Color.FromRgb(1, 1, 1)),
            };

            FullSum.Margin = new Thickness(rightX - 10, rightY - 5, 0, 0);
            AddLabel(new Label[] {
                new Label() {
                    Content = "Прогноз заказан " + Convert.ToString(Convert.ToSingle(FullSum.Content.ToString()) - Convert.ToSingle(Root.Content.ToString())),
                    Margin = new Thickness(rightX - OFFSET * 8, rightY - OFFSET * 2, 0, 0),
                    RenderTransform = new RotateTransform(5),
                    FontSize = 13
                }, FullSum,
            });
            AddUIElement(rightLine);

            rightAngle -= 10;

            double rightCenterX = rightX + Math.Cos(ToRadian(rightAngle)) * rightNode + 20;
            double rightCenterY = rightY + Math.Sin(ToRadian(rightAngle)) * rightNode;

            DrawLine(rightX, rightY + OFFSET, rightCenterX + 60, rightCenterY + 50, Color.FromRgb(1, 1, 1));
            AddLabel(new Label[] {
                new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(rightCenterX + 40, rightCenterY + 45, 0, 0),
                }, new Label() {
                    Content = forecastNii[1].Content,
                    Margin = new Thickness(rightCenterX - 25, rightCenterY - 10, 0, 0),
                    RenderTransform = new RotateTransform(15)
                }
            });

            DrawLine(rightX, rightY + OFFSET, rightX, rightY + rightNode, Color.FromRgb(1, 1, 1));
            AddLabel(new Label[]{
                new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(rightX - OFFSET, rightY + rightNode - 7, 0, 0),
                }, new Label() {
                    Content = forecastNii[2].Content,
                    Margin = new Thickness(rightX + OFFSET * 1.5, rightY + rightNode - OFFSET * 2 - 8, 0, 0),
                    RenderTransform = new RotateTransform(90)
                }
            });

            DrawLine(rightX, rightY + OFFSET, rightX - Math.Cos(ToRadian(rightAngle)) * rightNode - 80, rightCenterY, Color.FromRgb(1, 1, 1));
            AddLabel(new Label[] {
                new Label() {
                    Content = Convert.ToString(
                        Convert.ToSingle(predictions[0].Content.ToString()) * Convert.ToSingle(chancies[1][0].Content.ToString()) +
                        Convert.ToSingle(predictions[1].Content.ToString()) * Convert.ToSingle(chancies[1][1].Content.ToString()) +
                        Convert.ToSingle(predictions[2].Content.ToString()) * Convert.ToSingle(chancies[1][2].Content.ToString())
                    ),
                    Margin = new Thickness(rightX - Math.Cos(ToRadian(rightAngle)) * rightNode - 80 - OFFSET, rightCenterY - 6, 0, 0),
                }, new Label() {
                    Content = forecastNii[0].Content,
                    Margin = new Thickness(rightX - Math.Cos(ToRadian(rightAngle)) * rightNode - 80 + OFFSET, rightCenterY - OFFSET - OFFSET / 2, 0, 0),
                    RenderTransform = new RotateTransform(360)
                }
            });

            rightAngle += 45;

            DrawTwoLine(rightCenterX + 60, rightCenterY + 50, rightNode, rightAngle);
            DrawThreeLine(rightCenterX + 60 + Math.Cos(ToRadian(rightAngle)) * rightNode,
                rightCenterY + 50 + Math.Sin(ToRadian(rightAngle)) * rightNode, rightNode, rightAngle, 2);
            AddLabel(new Label[] {
                new Label() {
                    Content = Convert.ToString(
                        Convert.ToSingle(predictions[0].Content.ToString()) * Convert.ToSingle(chancies[2][0].Content.ToString()) +
                        Convert.ToSingle(predictions[1].Content.ToString()) * Convert.ToSingle(chancies[2][1].Content.ToString()) +
                        Convert.ToSingle(predictions[2].Content.ToString()) * Convert.ToSingle(chancies[2][2].Content.ToString())
                    ),
                    Margin = new Thickness(rightCenterX + 60 + Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET,
                                            rightCenterY + 50 + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 0.2, 0, 0)
                }, new Label() {
                    Content = "Март",
                    Margin = new Thickness(
                        rightCenterX + 60 + Math.Cos(ToRadian(rightAngle)) * rightNode,
                        rightCenterY + 50 + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 3, 0, 0
                    ),
                    RenderTransform = new RotateTransform(60)
                }, new Label() {
                    Content = "Июнь",
                    Margin = new Thickness(
                        rightCenterX + 60 - Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET,
                        rightCenterY + 50 + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 1.3, 0, 0
                    ),
                    RenderTransform = new RotateTransform(300)
                }
            });
            DrawLine(rightCenterX + 60 - Math.Cos(ToRadian(rightAngle)) * rightNode,
                rightCenterY + 50 + Math.Sin(ToRadian(rightAngle)) * rightNode + OFFSET,
                rightCenterX + 60 - Math.Cos(ToRadian(rightAngle)) * rightNode,
                rightCenterY + 50 + Math.Sin(ToRadian(rightAngle)) * rightNode + rightNode, Color.FromRgb(1, 1, 1)
            );
            AddLabel(new Label[] {
                new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(rightCenterX + 60 - Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET * 0.8,
                                            rightCenterY + 50 + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 0.25, 0, 0)
                }, new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(rightCenterX + 60 - Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET,
                                                rightCenterY + 50 + Math.Sin(ToRadian(rightAngle)) * rightNode + rightNode, 0, 0)
                }
            });

            DrawTwoLine(rightX, rightY + rightNode, rightNode, rightAngle);
            DrawThreeLine(rightX - Math.Cos(ToRadian(rightAngle)) * rightNode,
                    rightY + rightNode + Math.Sin(ToRadian(rightAngle)) * rightNode,
                    rightNode, rightAngle, 3);
            AddLabel(new Label[] {
                new Label() {
                    Content = Convert.ToString(
                        Convert.ToSingle(predictions[0].Content.ToString()) * Convert.ToSingle(chancies[3][0].Content.ToString()) +
                        Convert.ToSingle(predictions[1].Content.ToString()) * Convert.ToSingle(chancies[3][1].Content.ToString()) +
                        Convert.ToSingle(predictions[2].Content.ToString()) * Convert.ToSingle(chancies[3][2].Content.ToString())
                    ),
                    Margin = new Thickness(rightX - Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET,
                                            rightY + rightNode + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 0.2, 0, 0)
                }, new Label() {
                    Content = "Март",
                    Margin = new Thickness(
                        rightX - Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET,
                        rightY + rightNode + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 1.1, 0, 0
                    ),
                    RenderTransform = new RotateTransform(300)
                }, new Label() {
                    Content = "Июнь",
                    Margin = new Thickness(
                        rightX + Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET / 2,
                        rightY + rightNode + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 3.5, 0, 0
                    ),
                    RenderTransform = new RotateTransform(60)
                }
            });
            DrawLine(rightX + Math.Cos(ToRadian(rightAngle)) * rightNode,
                rightY + rightNode + Math.Sin(ToRadian(rightAngle)) * rightNode + OFFSET,
                rightX + Math.Cos(ToRadian(rightAngle)) * rightNode,
                rightY + rightNode + Math.Sin(ToRadian(rightAngle)) * rightNode + rightNode, Color.FromRgb(1, 1, 1)
            );
            AddLabel(new Label[] {
                new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(rightX + Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET * 0.8,
                                            rightY + rightNode + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 0.25, 0, 0)
                }, new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(rightX + Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET,
                                                rightY + rightNode + Math.Sin(ToRadian(rightAngle)) * rightNode + rightNode, 0, 0)
                }
            });

            DrawTwoLine(rightX - Math.Cos(ToRadian(rightAngle - 45)) * rightNode - 80, rightCenterY, rightNode, rightAngle);
            DrawThreeLine(rightX - Math.Cos(ToRadian(rightAngle - 45)) * rightNode - 80 - Math.Cos(ToRadian(rightAngle)) * rightNode,
                rightCenterY + Math.Sin(ToRadian(rightAngle)) * rightNode, rightNode, rightAngle, 1);
            AddLabel(new Label[] {
                new Label() {
                    Content = Convert.ToString(
                        Convert.ToSingle(predictions[0].Content.ToString()) * Convert.ToSingle(chancies[1][0].Content.ToString()) +
                        Convert.ToSingle(predictions[1].Content.ToString()) * Convert.ToSingle(chancies[1][1].Content.ToString()) +
                        Convert.ToSingle(predictions[2].Content.ToString()) * Convert.ToSingle(chancies[1][2].Content.ToString())
                    ),
                    Margin = new Thickness(rightX - Math.Cos(ToRadian(rightAngle - 45)) * rightNode - 80 - Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET,
                                            rightCenterY + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 0.2, 0, 0)
                }, new Label() {
                    Content = "Март",
                    Margin = new Thickness(
                        rightX - Math.Cos(ToRadian(rightAngle - 45)) * rightNode - 80 - Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET,
                        rightCenterY + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 1.1, 0, 0
                    ),
                    RenderTransform = new RotateTransform(300)
                }, new Label() {
                    Content = "Июнь",
                    Margin = new Thickness(
                        rightX - Math.Cos(ToRadian(rightAngle - 45)) * rightNode - 80 + Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET / 2,
                        rightCenterY + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 3.5, 0, 0
                    ),
                    RenderTransform = new RotateTransform(60)
                }
            });
            DrawLine(rightX - Math.Cos(ToRadian(rightAngle - 45)) * rightNode - 80 + Math.Cos(ToRadian(rightAngle)) * rightNode,
                rightCenterY + Math.Sin(ToRadian(rightAngle)) * rightNode + OFFSET,
                rightX - Math.Cos(ToRadian(rightAngle - 45)) * rightNode - 80 + Math.Cos(ToRadian(rightAngle)) * rightNode,
                rightCenterY + Math.Sin(ToRadian(rightAngle)) * rightNode + rightNode + OFFSET * 2, Color.FromRgb(1, 1, 1)
            );
            AddLabel(new Label[] {
                new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(rightX - Math.Cos(ToRadian(rightAngle - 45)) * rightNode - 80 + Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET * 0.8,
                                                rightCenterY + Math.Sin(ToRadian(rightAngle)) * rightNode - OFFSET * 0.25, 0, 0)
                }, new Label() {
                    Content = Root.Content,
                    Margin = new Thickness(rightX - Math.Cos(ToRadian(rightAngle - 45)) * rightNode - 80 + Math.Cos(ToRadian(rightAngle)) * rightNode - OFFSET,
                                                rightCenterY + Math.Sin(ToRadian(rightAngle)) * rightNode + rightNode + OFFSET * 2, 0, 1)
                }
            });
        }
        #endregion 

        private void DrawTwoLine(double centerX, double centerY, double lenNode, double angle) {
            double x = centerX - Math.Cos(ToRadian(angle)) * lenNode;
            double y = centerY + Math.Sin(ToRadian(angle)) * lenNode;

            DrawLine(centerX, centerY + OFFSET, x, y, Color.FromRgb(1, 1, 1));

            x = centerX + Math.Cos(ToRadian(angle)) * lenNode;

            DrawLine(centerX, centerY + OFFSET, x, y, Color.FromRgb(1, 1, 1));
        }

        private void DrawThreeLine(double centerX, double centerY, double lenNode, double angle, int index) {
            double x = centerX - Math.Cos(ToRadian(angle)) * lenNode;
            double y = centerY + Math.Sin(ToRadian(angle)) * lenNode;
            DrawLine(centerX, centerY + OFFSET, x, y, Color.FromRgb(0, 255, 0));
            AddLabel(new Label() {
                Content = predictions[0].Content,
                Margin = new Thickness(x - OFFSET, y, 0, 0),
            });
            AddLabel(new Label() {
                Content = chancies[index][0].Content,
                Margin = new Thickness(x - OFFSET - 5, y - OFFSET, 0, 0),
                RenderTransform = new RotateTransform(310)
            });

            DrawLine(centerX, centerY + OFFSET, centerX, centerY + lenNode, Color.FromRgb(255, 255, 0));
            AddLabel(new Label() {
                Content = predictions[1].Content,
                Margin = new Thickness(centerX - OFFSET, centerY + lenNode, 0, 0)
            });
            AddLabel(new Label() {
                Content = chancies[index][1].Content,
                Margin = new Thickness(centerX + OFFSET + 5, centerY + lenNode - OFFSET * 2, 0, 0),
                RenderTransform = new RotateTransform(90)
            });

            x = centerX + Math.Cos(ToRadian(angle)) * lenNode;

            DrawLine(centerX, centerY + OFFSET, x, y, Color.FromRgb(255, 0, 0));
            AddLabel(new Label() {
                Content = predictions[2].Content,
                Margin = new Thickness(x - OFFSET, y, 0, 0)
            });
            AddLabel(new Label() {
                Content = chancies[index][2].Content,
                Margin = new Thickness(x + OFFSET - 5, y - OFFSET - OFFSET, 0, 0),
                RenderTransform = new RotateTransform(60)
            });
        }

        private void DrawLine(double x1, double y1, double x2, double y2, Color color) {
            AddUIElement(new Line() {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = new SolidColorBrush(color)
            });
        }

        private void AddUIElement(UIElement el) {
            this.grid1.Children.Add(el);
        }

        private void AddLabel(params Label[] labels) {
            if (labels is null) return;

            foreach (Label label in labels) {
                AddLabel(label);
            }
        }

        private void AddLabel(Label label) {
            AddUIElement(label);
        }

        private double ToRadian(double angle) {
            return Math.PI * angle * 2 / 360.0;
        }

        private void Window_Closed(object sender, EventArgs e) {
            this.grid1.Children.Clear();
        }
    }
}