using SharpCrop.Provider;
using SharpCrop.Utils;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    public partial class ClickForm : Form
    {
        private bool configShown = false;

        private DrawForm drawForm;
        private IProvider provider;

        /// <summary>
        /// A clickable form which is totally transparent - so no drawing is possible here.
        /// </summary>
        public ClickForm(IProvider provider)
        {
            this.provider = provider;

            InitializeComponent();

            ClientSize = Screen.PrimaryScreen.Bounds.Size;
            Location = new Point(0, 0);
            Opacity = 0.005;

            drawForm = new DrawForm(this);
            drawForm.Show();
        }

        /// <summary>
        /// Grab bitmap and upload it to the saved Dropbox account.
        /// </summary>
        /// <param name="r">Bitmap position, size</param>
        private async void CaptureImage(Rectangle r)
        {
            // Hide the UI and prepare to capture and upload
            if (configShown)
            {
                return;
            }

            Hide();
            drawForm.Hide();
            Application.DoEvents();

#if __MonoCS__
            await Task.Delay(500);
#endif

            // Capture and start the upload process
            using (var stream = new MemoryStream())
            {
                var name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "." + ConfigHelper.Memory.FormatExt;
                var bitmap = CaptureHelper.GetBitmap(r);

                bitmap.Save(stream, ConfigHelper.Memory.FormatType);

                var url = await provider.Upload(name, stream);

                // Copy the URL if needed
                if (ConfigHelper.Memory.Copy)
                {
#if __MonoCS__
                    var form = new CopyForm(url);
                    form.FormClosed += (object sender, FormClosedEventArgs e) => Application.Exit();
                    form.Show();
#else
                    Clipboard.SetText(url);
#endif
                }
            }

            // Uploaded notification
            ToastFactory.CreateToast("Uploaded successfully!", 3000, () => 
            {
#if !__MonoCS__
                Application.Exit();
#endif
            });
        }

        /// <summary>
        /// Show ConfigForm and hide ClickForm and DrawForm.
        /// </summary>
        private void ShowConfig()
        {
            configShown = true;

            drawForm.Hide();
            Hide();

            var form = new ConfigForm();

            form.Show();
            form.FormClosed += (object sender, FormClosedEventArgs ev) =>
            {
                configShown = false;

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
                    Application.Exit();
                    break;
                case Keys.F1:
                    ShowConfig();
                    break;
            }
        }

        /// <summary>
        /// Call DrawForm when mouse is up and upload.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            drawForm.CallOnMouseUp(e);

            var r = CaptureHelper.GetRect(drawForm.MouseDownPoint, drawForm.MouseUpPoint);

            if (r.X < 0 || r.Y < 0 || r.Width < 1 || r.Height < 1)
            {
                return;
            }

            switch(drawForm.MouseButton)
            {
                case MouseButtons.Left:
                    CaptureImage(r);
                    break;
                case MouseButtons.Right:
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
    }
}