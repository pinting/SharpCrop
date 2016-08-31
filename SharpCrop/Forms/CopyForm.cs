using System.Windows.Forms;

namespace SharpCrop.Forms
{
    /// <summary>
    /// A simple form which shows a URL to the user. This is needed, because Clipboard is
    /// not working in Mono - so the user must copy the output manually.
    /// </summary>
    public partial class CopyForm : Form
    {
        public CopyForm(string url)
        {
            InitializeComponent();

            linkBox.Text = url;
        }
    }
}
