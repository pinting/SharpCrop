using System.Windows.Forms;

namespace SharpCrop.Provider.Forms
{
    public partial class WaitForm : Form
    {
        string link;

        public WaitForm(string url)
        {
            InitializeComponent();

            linkBox.Text = url;
            link = url;
        }

        private void Clicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(link);
        }
    }
}
