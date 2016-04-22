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

        /// <summary>
        /// A nonclickable form which background is transparent - so drawing is possible.
        /// </summary>
        public DrawForm()
        {
            SuspendLayout();
            
            Name = "DrawForm";
            Text = "ClickForm";
            ClientSize = Screen.PrimaryScreen.Bounds.Size;
            Location = new Point(0, 0);

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;

            DoubleBuffered = true;
            TopMost = true;

            BackColor = Color.White;
            TransparencyKey = Color.White;
            Opacity = 0.75;
        }

        /// <summary>
        /// Listen for mouse down events and save the coords.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            isMouseDown = true;
            dest = source = e.Location;
        }

        /// <summary>
        /// Listen for mouse up events.
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// Change the last coord as the mouse moves.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            dest = e.Location;
            Invalidate();
        }

        /// <summary>
        /// Paint the rectangle if the mouse button is down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (isMouseDown)
            {
                e.Graphics.FillRectangle(Brushes.RoyalBlue, GetRect(source, dest));
            }
        }

        /// <summary>
        /// Private helper function to construct a rectangle from two points.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        private Rectangle GetRect(Point source, Point dest)
        {
            return new Rectangle(
                Math.Min(source.X, dest.X),
                Math.Min(source.Y, dest.Y),
                Math.Abs(source.X - dest.X),
                Math.Abs(source.Y - dest.Y));
        }

        #region Public Events

        /* These methods are needed to make internal functions public to the ClickForm */

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
