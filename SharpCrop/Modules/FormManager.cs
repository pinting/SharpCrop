using System.Collections.Generic;
using System.Windows.Forms;
using SharpCrop.Forms;

namespace SharpCrop.Modules
{
    public static class FormManager
    {
        private static readonly List<Form> cropForms = new List<Form>();
        private static Form configForm;

        public static void Init(Controller controller)
        {
            // Create a new ConfigForm
            configForm = new ConfigForm();
            configForm.FormClosed += (s, e) => Application.Exit();
            configForm.Load += async (s, e) => await controller.RestoreProviders();

            // If there are any registered providers, the config will be hidden
            // Else the config will be closed (along with the whole application)
            configForm.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing && ProviderManager.RegisteredProviders.Count > 0)
                {
                    e.Cancel = true;
                    configForm.Hide();
                }
            };

            // Show crop forms if the config is hidden
            configForm.VisibleChanged += (s, e) =>
            {
                if (!configForm.Visible)
                {
                    ShowCropForms();
                }
            };

            // Create a CropForm for every screen and show them
            for (var i = 0; i < Screen.AllScreens.Length; i++)
            {
                var screen = Screen.AllScreens[i];
                var form = new CropForm(controller, screen.Bounds, i);

                form.FormClosed += (s, e) => Application.Exit();

                cropForms.Add(form);
            }
        }

        /// <summary>
        /// Hide every CropForm - if more than one monitor is used.
        /// </summary>
        public static void HideCropForms() => cropForms.ForEach(e => e.Hide());

        /// <summary>
        /// Show every CropForm.
        /// </summary>
        public static void ShowCropForms() => cropForms.ForEach(e => e.Show());

        /// <summary>
        /// Protect list from external modification.
        /// </summary>
        public static IReadOnlyList<Form> CropForms => cropForms;

        /// <summary>
        /// Protect variable from external modification.
        /// </summary>
        public static Form ConfigForm => configForm;
    }
}
