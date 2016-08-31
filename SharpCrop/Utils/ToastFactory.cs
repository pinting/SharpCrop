using SharpCrop.Forms;
using System;
using System.Windows.Forms;

namespace SharpCrop.Utils
{
    /// <summary>
    /// ToastFactory creates new ToastForms for the user and handles there numbers.
    /// </summary>
    public static class ToastFactory
    {
        private static int index = 1;

        /// <summary>
        /// Create a new toast.
        /// </summary>
        /// <param name="text">Text of the toast</param>
        /// <param name="duration">Duration in ms</param>
        /// <param name="onClose">Closing event callback</param>
        public static void CreateToast(string text, int duration = 3000, Action onClose = null)
        {
            var form = new ToastForm(text, duration, index++);

            form.FormClosed += (object sender, FormClosedEventArgs e) => 
            {
                index--;

                if (onClose != null)
                {
                    onClose();
                }
            };

            form.Show();
        }
    }
}
