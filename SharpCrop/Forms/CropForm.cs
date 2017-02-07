using SharpCrop.Utils;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Threading.Tasks;

namespace SharpCrop.Forms
{
    /// <summary>
    /// CropForm helps the user to select a rectangle which gonna be captured and uploaded.
    /// </summary>
    public partial class CropForm : Form
    {
        private readonly Controller controller;
        private readonly Rectangle screen;
        private readonly int index;

        private MouseButtons mouseButtonUsed = MouseButtons.Left;
        private Point mouseMovePoint = Point.Empty;
        private Point mouseDownPoint = Point.Empty;
        private Point mouseUpPoint = Point.Empty;
        private bool isMouseDown;

        /// <summary>
        /// Consturct a new CropForm with the given provider.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="screen">Screen bounds</param>
        /// <param name="index">Screen index in Screen.AllScreens</param>
        public CropForm(Controller controller, Rectangle screen, int index = -1)
        {
            this.controller = controller;
            this.screen = screen;
            this.index = index;
            
            Location = screen.Location;
            ClientSize = Size = screen.Size;

            InitializeComponent();
            RefreshBackground();
            MakeClickable();

            controller.ConfigForm.VisibleChanged += (sender, ev) =>
            {
                if(controller.ConfigForm.Visible)
                {
                    return;
                }

                if (ConfigHelper.Memory.NoTransparency)
                {
                    Task.Run(() =>
                    {
                        // Wait for the form to disappear
                        Thread.Sleep(500);
                        Invoke(new Action(() =>
                        {
                            RefreshBackground();
                            ShowAll();
                        }));
                    });
                }
                else
                {
                    ShowAll();
                }
            };
        }

        /// <summary>
        /// Reset the mouse position.
        /// </summary>
        private void ResetMouse()
        {
            mouseMovePoint = mouseDownPoint = mouseUpPoint = Point.Empty;
            isMouseDown = false;
        }

        /// <summary>
        /// When CropForm shown.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            MakeClickable();
            ResetMouse();

            if(controller.CropForms.Count == 1)
            {
                Focus();
            }
        }

        /// <summary>
        /// When CropForm lose focus.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            MakeClickable();
            ResetMouse();

            if (controller.CropForms.Count == 1)
            {
                Focus();
            }
        }

        /// <summary>
        /// Consturct a new ConfigForm and hide CropForm.
        /// </summary>
        private void ShowConfig()
        {
            HideAll();
            controller.ConfigForm.Show();
        }
        
        /// <summary>
        /// Listen for key presses.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
                case Keys.F1:
                    ShowConfig();
                    break;
            }
        }

        /// <summary>
        /// Listen for mouse up event and proceed to upload.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MakeClickable();

            if(!isMouseDown)
            {
                return;
            }

            mouseUpPoint = mouseMovePoint = e.Location;
            mouseButtonUsed = e.Button;
            isMouseDown = false;
            
            var region = CaptureHelper.GetRectangle(mouseDownPoint, mouseUpPoint);

            if (region.X < 0 || region.Y < 0 || region.Width < 1 || region.Height < 1)
            {
                return;
            }

            HideAll();
            Application.DoEvents();
            CaptureHelper.SetManualScaling(index);

#if __MonoCS__
            Thread.Sleep(500);
#else
            Thread.Sleep(50);
#endif

            switch (e.Button)
            {
                case MouseButtons.Left:
                    controller.CaptureImage(region, screen.Location);
                    break;
                case MouseButtons.Right:
                    controller.CaptureGif(region, screen.Location);
                    break;
            }
        }

        /// <summary>
        /// Listen for mouse down events and save the coords.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MakeInvisible();

            mouseUpPoint = mouseMovePoint = mouseDownPoint = e.Location;
            mouseButtonUsed = e.Button;
            isMouseDown = true;
        }

        /// <summary>
        /// Request repaint as the mouse moves.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            mouseMovePoint = e.Location;
            mouseButtonUsed = e.Button;

            Invalidate();
        }
        /// <summary>
        /// Paint the rectangle if the mouse button is down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!isMouseDown)
            {
                return;
            }

            var draw = new Action<Brush, Rectangle>((color, r) => 
            {
                if (!ConfigHelper.Memory.NoTransparency)
                {
                    e.Graphics.FillRectangle(color, r);
                    return;
                }

                var path = new Point[]
                {
                    new Point { X = r.X, Y = r.Y },
                    new Point { X = r.X + r.Width , Y = r.Y },
                    new Point { X = r.X + r.Width , Y = r.Y + r.Height },
                    new Point { X = r.X , Y = r.Y + r.Height },
                    new Point { X = r.X, Y = r.Y }
                };

                e.Graphics.DrawLines(new Pen(color, Constants.PenWidth), path);
            });

            var region = CaptureHelper.GetRectangle(mouseDownPoint, mouseMovePoint);

            switch (mouseButtonUsed)
            {
                case MouseButtons.Left:
                    draw(Constants.LeftColor, region);
                    break;
                case MouseButtons.Right:
                    draw(Constants.RightColor, region);
                    break;
            }
        }

        /// <summary>
        /// Hide every CropForm - if more than one monitor is used.
        /// </summary>
        private void HideAll()
        {
            foreach (var form in controller.CropForms)
            {
                form.Hide();
            }
        }

        /// <summary>
        /// Show every CropForm.
        /// </summary>
        private void ShowAll()
        {
            foreach (var form in controller.CropForms)
            {
                form.Show();
            }
        }

        /// <summary>
        /// Refresh the background if transparency is disabled.
        /// </summary>
        private void RefreshBackground()
        {
            if (!ConfigHelper.Memory.NoTransparency)
            {
                return;
            }

            CaptureHelper.SetManualScaling(index);

            var bitmap = CaptureHelper.GetBitmap(new Rectangle(Point.Empty, screen.Size), screen.Location);

            BackgroundImage = CaptureHelper.ResizeBitmap(bitmap, Width, Height);
            Opacity = 1.0D;
        }

        /// <summary>
        /// Make the form invisible and enable clicking.
        /// </summary>
        private void MakeClickable()
        {
#if !__MonoCS__
            if(ConfigHelper.Memory.NoTransparency)
            {
                return;
            }

            TransparencyKey = Color.White;
            Opacity = 0.005D;
#endif
        }

        /// <summary>
        /// Make the background invisible (not the selecting rectangle)
        /// and disable clicking.
        /// </summary>
        private void MakeInvisible()
        {
#if !__MonoCS__
            if (ConfigHelper.Memory.NoTransparency)
            {
                return;
            }

            TransparencyKey = Color.Black;
            Opacity = 0.75D;
#endif
        }

#if !__MonoCS__
        
        /// <summary>
        /// Keep focus for other windows while topmost.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                if(!ConfigHelper.Memory.NoFocus)
                {
                    return base.CreateParams;
                }

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
        protected override bool ShowWithoutActivation => ConfigHelper.Memory.NoFocus || base.ShowWithoutActivation;

#endif

    }
}