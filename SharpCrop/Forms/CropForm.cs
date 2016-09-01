using SharpCrop.Utils;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System;

namespace SharpCrop.Forms
{
    /// <summary>
    /// CropForm helps the user to select a rectangle which gonna be captured and uploaded.
    /// </summary>
    public partial class CropForm : Form
    {
        private MouseButtons mouseButtonUsed = MouseButtons.Left;
        private Point mouseMovePoint = Point.Empty;
        private Point mouseDownPoint = Point.Empty;
        private Point mouseUpPoint = Point.Empty;
        private bool isMouseDown = false;
        private Controller controller;

        /// <summary>
        /// Consturct a new CropForm with the given provider.
        /// </summary>
        public CropForm(Controller controller)
        {
            this.controller = controller;

            InitializeComponent();

            ClientSize = Screen.PrimaryScreen.Bounds.Size;
            Location = new Point(0, 0);

            MakeClickable();
        }

        /// <summary>
        /// Reset the mouse position.
        /// </summary>
        private void ResetMouse()
        {
            mouseMovePoint = mouseDownPoint = mouseUpPoint = Point.Empty;
            isMouseDown = false;
        }

        /// <summary>
        /// When CropForm shown.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            MakeClickable();
            ResetMouse();
            Focus();
        }

        /// <summary>
        /// When CropForm lose focus.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            MakeClickable();
            ResetMouse();
            Focus();
        }

        /// <summary>
        /// Consturct a new ConfigForm and hide CropForm.
        /// </summary>
        private void ShowConfig()
        {
            var form = new ConfigForm();

            Hide();
            form.Show();
            form.FormClosed += (object sender, FormClosedEventArgs ev) =>
            {
                Show();
            };
        }
        
        /// <summary>
        /// Listen for key presses.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
                case Keys.F1:
                    ShowConfig();
                    break;
            }
        }

        /// <summary>
        /// Listen for mouse up event and proceed to upload.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MakeClickable();

            if(!isMouseDown)
            {
                return;
            }

            mouseUpPoint = mouseMovePoint = e.Location;
            mouseButtonUsed = e.Button;
            isMouseDown = false;
            
            var r = CaptureHelper.GetRect(mouseDownPoint, mouseUpPoint);

            if (r.X < 0 || r.Y < 0 || r.Width < 1 || r.Height < 1)
            {
                return;
            }

            Hide();
            Application.DoEvents();

#if __MonoCS__
            Thread.Sleep(500);
#else
            Thread.Sleep(50);
#endif

            switch (e.Button)
            {
                case MouseButtons.Left:
                    controller.CaptureImage(r);
                    break;
                case MouseButtons.Right:
                    controller.CaptureGif(r);
                    break;
            }
        }

        /// <summary>
        /// Listen for mouse down events and save the coords.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MakeInvisible();

            mouseUpPoint = mouseMovePoint = mouseDownPoint = e.Location;
            mouseButtonUsed = e.Button;
            isMouseDown = true;
        }

        /// <summary>
        /// Request repaint as the mouse moves.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            mouseMovePoint = e.Location;
            mouseButtonUsed = e.Button;

            Invalidate();
        }

        /// <summary>
        /// Paint the rectangle if the mouse button is down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!isMouseDown)
            {
                return;
            }

            var rect = CaptureHelper.GetRect(mouseDownPoint, mouseMovePoint);

            switch (mouseButtonUsed)
            {
                case MouseButtons.Left:
                    e.Graphics.FillRectangle(Constants.LeftColor, rect);
                    break;
                case MouseButtons.Right:
                    e.Graphics.FillRectangle(Constants.RightColor, rect);
                    break;
            }
        }

        /// <summary>
        /// Make the form invisible and enable clicking.
        /// </summary>
        private void MakeClickable()
        {
#if !__MonoCS__
            TransparencyKey = Color.White;
            Opacity = 0.005D;
#endif
        }

        /// <summary>
        /// Make the background invisible (not the selecting rectangle)
        /// and disable clicking.
        /// </summary>
        private void MakeInvisible()
        {
#if !__MonoCS__
            TransparencyKey = Color.Black;
            Opacity = 0.75D;
#endif
        }

        /// <summary>
        /// Do not steal focus from other windows.
        /// </summary>
        protected override bool ShowWithoutActivation
        {
            get
            {
                if (ConfigHelper.Memory.Focus)
                {
                    return base.ShowWithoutActivation;
                }

                return true;
            }
        }

        /// <summary>
        /// Keep focus for other windows while topmost.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                if(ConfigHelper.Memory.Focus)
                {
                    return base.CreateParams;
                }

                CreateParams baseParams = base.CreateParams;

                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                baseParams.ExStyle |= (int)(WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);

                return baseParams;
            }
        }
    }
}