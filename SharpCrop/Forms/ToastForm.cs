using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    /// <summary>
    /// ToastForm is responsible for the right-bottom side toast boxes.
    /// </summary>
    public partial class ToastForm : Form
    {
        private const int margin = 5;

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

        /// <summary>
        /// Access virtual property in constructor.
        /// </summary>
        public sealed override Color BackColor
        {
            get
            {
                return base.BackColor;
            }

            set
            {
                base.BackColor = value;
            }
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

#if !__MonoCS__

        /// <summary>
        /// Keep focus for other windows while topmost.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var baseParams = base.CreateParams;

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

#endif

    }
}
