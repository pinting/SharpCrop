using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpCrop
{
    public partial class DrawForm : Form
    {
        private Grabber grabber = new Grabber();
        private Point mouseDownPoint = Point.Empty;
        private Point mousePoint = Point.Empty;
        private bool mouseDown = false;

        public DrawForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;

            DoubleBuffered = true;
            TopMost = true;

            BackColor = Color.White;
            TransparencyKey = Color.White;
            Opacity = 0.75;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouseDown = true;
            mousePoint = mouseDownPoint = e.Location;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseDown = false;
            Opacity = 0;
            grabber.GetScreenshot(mouseDown, mousePoint);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mousePoint = e.Location;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (mouseDown)
            {
                Rectangle rectangle = new Rectangle(
                    Math.Min(mouseDownPoint.X, mousePoint.X),
                    Math.Min(mouseDownPoint.Y, mousePoint.Y),
                    Math.Abs(mouseDownPoint.X - mousePoint.X),
                    Math.Abs(mouseDownPoint.Y - mousePoint.Y));

                e.Graphics.FillRectangle(Brushes.RoyalBlue, rectangle);
            }
        }

        public void CallOnMouseDown(MouseEventArgs e)
        {
            OnMouseDown(e);
        }

        public void CallOnMouseUp(MouseEventArgs e)
        {
            OnMouseUp(e);
        }

        public void CallOnMouseMove(MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        public void CallOnPaint(PaintEventArgs e)
        {
            OnPaint(e);
        }
    }
}
