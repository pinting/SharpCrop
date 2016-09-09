using SharpCrop.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SharpCrop.Utils
{
    /// <summary>
    /// ToastFactory creates new ToastForms for the user and handles there numbers.
    /// </summary>
    public static class ToastFactory
    {
        private static readonly Dictionary<int, Form> toasts = new Dictionary<int, Form>();
        private static int index = 1;
        private static int counter;

        /// <summary>
        /// Create a new toast.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color">Background color</param>
        /// <param name="duration">Toast duration (0 is infinite)</param>
        /// <param name="onClose">On close callback</param>
        /// <returns></returns>
        public static int Create(string text, Color color, int duration = 3000, Action onClose = null)
        {
            var form = new ToastForm(text, color, duration, index++);
            var currentId = counter++;

            form.FormClosed += (sender, e) =>
            {
                index--;
                toasts.Remove(currentId);

                onClose?.Invoke();
            };

            form.Show();
            toasts.Add(currentId, form);

            return currentId;
        }

        /// <summary>
        /// Create a new toast with white background.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="duration"></param>
        /// <param name="onClose"></param>
        /// <returns></returns>
        public static int Create(string text, int duration = 3000, Action onClose = null)
        {
            return Create(text, Color.White, duration, onClose);
        }

        /// <summary>
        /// Close an existing toast.
        /// </summary>
        /// <param name="id"></param>
        public static void Remove(int id)
        {
            if(toasts.ContainsKey(id))
            {
                toasts[id].Close();
            }
        }
    }
}
