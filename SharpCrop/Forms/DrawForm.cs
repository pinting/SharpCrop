using SharpCrop.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    public partial class DrawForm : Form
    {
        private Point MouseMovePoint = Point.Empty;
        private bool IsMouseDown = false;

        public Point MouseDownPoint = Point.Empty;
        public Point MouseUpPoint = Point.Empty;
        public MouseButtons MouseButton;

        /// <summary>
        /// A nonclickable form which background is transparent - so drawing is possible.
        /// </summary>
        public DrawForm()
        {
            InitializeComponent();

            ClientSize = Screen.PrimaryScreen.Bounds.Size;
            Location = new Point(0, 0);

#if __MonoCS__
            this.TransparencyKey = System.Drawing.Color.Black;
            this.BackColor = System.Drawing.Color.Black;
            this.Opacity = 0.20D;
#endif
        }

        /// <summary>
        /// Reset mouse position.
        /// </summary>
        public void Reset()
        {
            IsMouseDown = false;
        }

        /// <summary>
        /// Listen for mouse down events and save the coords.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            MouseUpPoint = MouseMovePoint = MouseDownPoint = e.Location;
            MouseButton = e.Button;
            IsMouseDown = true;
        }

        /// <summary>
        /// Listen for mouse up events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            MouseUpPoint = e.Location;
            IsMouseDown = false;
        }

        /// <summary>
        /// Change the last coord as the mouse moves.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            MouseMovePoint = e.Location;
            MouseButton = e.Button;

            Invalidate();
        }

        /// <summary>
        /// Paint the rectangle if the mouse button is down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!IsMouseDown)
            {
                return;
            }

            switch(MouseButton)
            {
                case MouseButtons.Left:
                    e.Graphics.FillRectangle(Constants.LeftColor, CaptureHelper.GetRect(MouseDownPoint, MouseMovePoint));
                    break;
                case MouseButtons.Right:
                    e.Graphics.FillRectangle(Constants.RightColor, CaptureHelper.GetRect(MouseDownPoint, MouseMovePoint));
                    break;
            }
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
