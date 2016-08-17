using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace SharpCrop.Forms
{
    public partial class ToastForm : Form
    {
        private System.Timers.Timer timer;
        private double duration;

        /// <summary>
        /// Toast form which responsible for the right-bottom side toast boxes.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="duration"></param>
        public ToastForm(string text, double duration)
        {
            InitializeComponent();

            Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width, Screen.PrimaryScreen.Bounds.Height - Height);

            this.duration = duration;
            Desc.Text = text;
        }

        /// <summary>
        /// Start duration timer.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            timer = new System.Timers.Timer(duration);

            timer.Elapsed += delegate (Object source, ElapsedEventArgs ev)
            {
                Invoke(new Action(() => Close()));
            };

            timer.AutoReset = false;
            timer.Enabled = true;
        }

        /// <summary>
        /// Close on click.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            timer.Stop();
            Close();
        }
    }
}
