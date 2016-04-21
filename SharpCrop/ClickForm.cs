using System;
using System.Drawing;
using System.Windows.Forms;

namespace SharpCrop
{
    public partial class ClickForm : Form
    {
        private DrawForm drawForm = new DrawForm();

        public ClickForm()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;
            TopMost = true;

            BackColor = Color.Black;
            Opacity = 0.005;

            drawForm.Show();
        }

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
    }
}