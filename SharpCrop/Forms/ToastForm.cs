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
        private readonly int margin = 5;
        private int duration;

        /// <summary>
        /// Construct a new ToastForm. Index is managed by ToastFactory.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="duration"></param>
        public ToastForm(string text, int duration, int index = 1)
        {
            InitializeComponent();

            Location = new Point(
                Screen.PrimaryScreen.Bounds.Width - (Width + margin), 
                Screen.PrimaryScreen.Bounds.Height - (Height + margin) * index);

            this.duration = duration;
            label.Text = text;
        }

        /// <summary>
        /// Start duration timer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Task.Run(async () =>
            {
                await Task.Delay(duration);

                if (!IsDisposed)
                {
                    Invoke(new Action(() => Close()));
                }
            });
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
        /// Do not steal focus from other windows.
        /// </summary>
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
    
        /// <summary>
        /// Keep focus for other windows while topmost.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams baseParams = base.CreateParams;

                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                baseParams.ExStyle |= (int)(WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW);

                return baseParams;
            }
        }
    }
}
