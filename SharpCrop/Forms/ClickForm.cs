using SharpCrop.Utils;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System;

namespace SharpCrop.Forms
{
    /// <summary>
    /// A clickable form which is totally transparent - so no drawing is possible here.
    /// </summary>
    public partial class ClickForm : Form
    {
        private bool config = false;
        private DrawForm drawForm;
        private Controller controller;

        /// <summary>
        /// Consturct a new ClickForm with the given provider.
        /// </summary>
        public ClickForm(Controller controller)
        {
            this.controller = controller;

            InitializeComponent();

            ClientSize = Screen.PrimaryScreen.Bounds.Size;
            Location = new Point(0, 0);
            Opacity = 0.005;

            drawForm = new DrawForm();
        }

        /// <summary>
        /// Focus on ClickForm and DrawForm when shown.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            drawForm.Show();
            drawForm.Focus();
            Focus();
        }

        /// <summary>
        /// Show ConfigForm and hide ClickForm and DrawForm.
        /// </summary>
        private void ShowConfig()
        {
            config = true;

            drawForm.Hide();
            Hide();

            var form = new ConfigForm();

            form.Show();
            form.FormClosed += (object sender, FormClosedEventArgs ev) =>
            {
                config = false;

                drawForm.Reset();
                drawForm.Show();
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
        /// Call DrawForm when mouse is up and proceed to upload.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            drawForm.CallOnMouseUp(e);

            if (config)
            {
                return;
            }

            var r = CaptureHelper.GetRect(drawForm.MouseDownPoint, drawForm.MouseUpPoint);

            if (r.X < 0 || r.Y < 0 || r.Width < 1 || r.Height < 1)
            {
                return;
            }

            Hide();
            drawForm.Hide();
            Application.DoEvents();

#if __MonoCS__
            Thread.Sleep(500);
#else
            Thread.Sleep(50);
#endif

            switch (drawForm.MouseButton)
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
        /// Call DrawForm on paint event. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            drawForm.CallOnPaint(e);
        }

        /// <summary>
        /// Call DrawForm when mouse is down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            drawForm.CallOnMouseDown(e);
        }

        /// <summary>
        /// Call DrawForm when mouse moves.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            drawForm.CallOnMouseMove(e);
        }
    }
}