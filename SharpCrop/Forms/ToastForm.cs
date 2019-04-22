using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpCrop.Models;
using SharpCrop.Services;

namespace SharpCrop.Forms
{
    /// <summary>
    /// ToastForm is responsible for the right-bottom side toast boxes.
    /// </summary>
    public class ToastForm : Form
    {
        private const int margin = 5;
        private Label label;

        /// <summary>
        /// Construct a new ToastForm. Index is managed by ToastFactory.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        /// <param name="index"></param>
        public ToastForm(string text, Color color, int duration = 0, int index = 1)
        {
            InitializeComponent();

            label.Text = text;
            BackColor = color;

            Location = new Point(
                Screen.PrimaryScreen.Bounds.Width - (Width + margin),
                Screen.PrimaryScreen.Bounds.Height - (Height + margin) * index);

            if (duration <= 0)
            {
                return;
            }

            Task.Run(async () =>
            {
                await Task.Delay(duration);

                if (!IsDisposed)
                {
                    Invoke(new Action(Close));
                }
            });
        }
        
        private void InitializeComponent()
        {
            SuspendLayout();
            
            label = new Label();
            label.Location = new Point(15, 15);
            label.Size = new Size(100, 20);
            label.AutoSize = true;
            label.TabIndex = 0;
            label.TextAlign = ContentAlignment.MiddleCenter;
            
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(500, 60);
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Opacity = 0.9D;
            ControlBox = false;
            TopMost = true;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;
            
            Controls.Add(label);
            ResumeLayout(false);
            PerformLayout();

        }

        /// <summary>
        /// Access virtual property in constructor.
        /// </summary>
        public sealed override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        /// <summary>
        /// Close on click.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Close();
        }

        /// <summary>
        /// Keep focus for other windows while topmost.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var baseParams = base.CreateParams;

                if (VersionService.GetPlatform() != PlatformType.Windows)
                {
                    return baseParams;
                }

                const int wsExNoactivate = 0x08000000;
                const int wsExToolwindow = 0x00000080;

                baseParams.ExStyle |= wsExNoactivate | wsExToolwindow;

                return baseParams;
            }
        }

        /// <summary>
        /// Do not steal focus from other windows.
        /// </summary>
        protected override bool ShowWithoutActivation => true;
    }
}
