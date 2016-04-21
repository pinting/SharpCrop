using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpCrop
{
    public partial class DrawForm : Form
    {
        private Point source = Point.Empty;
        private Point dest = Point.Empty;
        private bool isMouseDown = false;

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

            isMouseDown = true;
            dest = source = e.Location;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            isMouseDown = false;

            var r = GetRect(source, dest);

            if (r.X >= 0 && r.Y >= 0 && r.Width >= 1 && r.Height >= 1)
            {
                Opacity = 0;
                Grabber.GetScreenshot(r);
                Application.Exit();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            dest = e.Location;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!isMouseDown)
            {
                return;
            }

            e.Graphics.FillRectangle(Brushes.RoyalBlue, GetRect(source, dest));
        }

        private Rectangle GetRect(Point source, Point dest)
        {
            return new Rectangle(
                Math.Min(source.X, dest.X),
                Math.Min(source.Y, dest.Y),
                Math.Abs(source.X - dest.X),
                Math.Abs(source.Y - dest.Y));
        }

        #region Public Events

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

        #endregion
    }
}
