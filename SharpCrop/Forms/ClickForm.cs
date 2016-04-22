using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpCrop
{
    public partial class ClickForm : Form
    {
        private DrawForm drawForm = new DrawForm();

        /// <summary>
        /// A clickable form which is totally transparent - so no drawing is possible here.
        /// </summary>
        public ClickForm()
        {
            SuspendLayout();
            
            Name = "ClickForm";
            Text = "SharpCrop";
            ClientSize = Screen.PrimaryScreen.Bounds.Size;
            Location = new Point(0, 0);
            
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;
            TopMost = true;

            BackColor = Color.Black;
            Opacity = 0.005;

            drawForm.Show();
        }

        /// <summary>
        /// Listen for key presses.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if(e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Hide Form from the Alt + Tab application switcher menu.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var Params = base.CreateParams;
                Params.ExStyle |= 0x80;
                return Params;
            }
        }

        #region Mouse Events

        /* These mouse events are binded to DrawForm */

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            drawForm.CallOnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            drawForm.CallOnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            drawForm.CallOnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            drawForm.CallOnPaint(e);
        }

        #endregion
    }
}