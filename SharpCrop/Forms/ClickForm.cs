using SharpCrop.Provider;
using SharpCrop.Utils;
using SharpCrop.Utils.Gif;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

            drawForm = new DrawForm();
            drawForm.Show();
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
        /// Hide DrawForm and ClickForm.
        /// </summary>
        private void HideInterface()
        {

            Hide();
            drawForm.Hide();
            Application.DoEvents();

#if __MonoCS__
            Thread.Sleep(500);
#endif
        }

        /// <summary>
        /// Show the end notifcation.
        /// </summary>
        /// <param name="url"></param>
        private void EndNotifcation(string url = null)
        {
            if (ConfigHelper.Memory.Copy && url != null)
            {
                Clipboard.SetText(url);

#if __MonoCS__
                var form = new CopyForm(url);
                form.FormClosed += (object sender, FormClosedEventArgs e) => Application.Exit();
                form.Show();
#endif
            }

            ToastFactory.CreateToast("Uploaded successfully!", 3000, () =>
            {
#if !__MonoCS__
                Application.Exit();
#endif
            });
        }

        /// <summary>
        /// Upload a stream with a generated name.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private async Task<string> Upload(MemoryStream stream)
        {
            var name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "." + ConfigHelper.Memory.FormatExt;

            return await provider.Upload(name, stream);
        }

        /// <summary>
        /// Capture one Bitmap.
        /// </summary>
        /// <param name="r"></param>
        private async void CaptureImage(Rectangle r)
        {
            HideInterface();

            string url;

            using (var stream = new MemoryStream())
            {
                var bitmap = CaptureHelper.GetBitmap(r);

                bitmap.Save(stream, ConfigHelper.Memory.FormatType);

                url = await Upload(stream);
            }

            EndNotifcation(url);
        }

        /// <summary>
        /// Capture a lot of Bitmaps. REALLY-REALLY SLOW!
        /// </summary>
        /// <param name="r"></param>
        private async void CaptureGif(Rectangle r)
        {
            HideInterface();

            var run = true;

            ToastFactory.CreateToast("Click here to stop!", 1000 * 60, () => 
            {
                ToastFactory.CreateToast("Processing...");

                run = false;
            });

            using (var stream = new MemoryStream())
            {
                await Task.Run(async () =>
                {
                    var gif = new AnimatedGifEncoder();
                    var frames = new List<Bitmap>();
                    var duration = Stopwatch.StartNew();

                    Stopwatch delay = null;

                    while (run)
                    {
                        var wait = 0;

                        if (delay != null)
                        {
                            wait = Constants.GifDelay - (int)delay.ElapsedMilliseconds;
                            wait = wait < 0 ? 0 : wait;
                        }

                        delay = Stopwatch.StartNew();

                        frames.Add(CaptureHelper.GetBitmap(r));
                        await Task.Delay(wait);

                        delay.Stop();
                    }

                    duration.Stop();

                    gif.Start(stream);
                    gif.SetQuality(Constants.GifQuality);
                    gif.SetRepeat(Constants.GifRepeat);

                    for (var i = 0; i < frames.Count; i++)
                    {
                        gif.AddFrame(frames[i]);
                    }

                    gif.SetDelay((int)duration.ElapsedMilliseconds / frames.Count);
                    gif.Finish();
                });

                ToastFactory.CreateToast("Uplading...");
                EndNotifcation(await Upload(stream));
            }
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

            if (configShown)
            {
                return;
            }

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
                    CaptureGif(r);
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