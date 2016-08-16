using SharpCrop.Forms;
using System;
using System.Windows.Forms;

namespace SharpCrop.Utils
{
    public static class ToastFactory
    {
        public static void CreateToast(string text, int duration = 3000, Action onClose = null)
        {
            var form = new ToastForm(text, duration);

            if(onClose != null)
            {
                form.FormClosed += (object sender, FormClosedEventArgs e) => 
                {
                    onClose();
                };
            }

            form.Show();
        }
    }
}
