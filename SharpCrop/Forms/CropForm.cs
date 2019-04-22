using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SharpCrop.Models;
using SharpCrop.Services;

namespace SharpCrop.Forms
{
    /// <summary>
    /// CropForm helps the user to select a rectangle which gonna be captured and uploaded.
    /// </summary>
    public class CropForm : Form
    {
        private readonly Controller controller;
        private readonly Rectangle screen;

        private MouseButtons mouseButtonUsed = MouseButtons.Left;
        private Point mouseMovePoint = Point.Empty;
        private Point mouseDownPoint = Point.Empty;
        private Point mouseUpPoint = Point.Empty;
        private bool isMouseDown;
        private Rectangle lastRegion;

        /// <summary>
        /// Construct a new CropForm with the given provider.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="screen">Screen bounds</param>
        public CropForm(Controller controller, Rectangle screen)
        {
            this.controller = controller;
            this.screen = screen;
            
            InitializeComponent();
            
            Location = screen.Location;
            ClientSize = Size = screen.Size;

            RefreshBackground();
            MakeClickable();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.Manual;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.Black;
            DoubleBuffered = true;
            Opacity = 0.25D;
            Text = "SharpCrop";
            TopMost = false;
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

            RefreshBackground();
            MakeClickable();
            ResetMouse();

            if(FormService.CropForms.Count == 1)
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

            RefreshBackground();
            MakeClickable();
            ResetMouse();

            if (FormService.CropForms.Count == 1)
            {
                Focus();
            }
        }

        /// <summary>
        /// Construct a new ConfigForm and hide CropForm.
        /// </summary>
        private void ShowConfig()
        {
            FormService.HideCropForms();
            FormService.ConfigForm.Show();
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
            
            // IMPORTANT: Convert the local (window) points to absolute (screen) points
            var region = CaptureService.GetRectangle(
                PointToScreen(mouseDownPoint),
                PointToScreen(mouseUpPoint));

            if (region.Width < 1 || region.Height < 1)
            {
                return;
            }

            FormService.HideCropForms();
            Application.DoEvents();

            // Wait the form to be hidden
            Thread.Sleep(200);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    controller.CaptureImage(region);
                    break;
                case MouseButtons.Right:
                    controller.CaptureVideo(region);
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
        /// Draw the select rectangle on the screen.
        /// It uses the lastRegion variable to repaint only the difference.
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="color"></param>
        /// <param name="r"></param>
        private void DrawSelectRectangle(Graphics gfx, Brush color, Rectangle r)
        {
            if (SettingsService.Current.NoTransparency)
            {

                var path = new[]
                {
                    new Point { X = r.X, Y = r.Y },
                    new Point { X = r.X + r.Width , Y = r.Y },
                    new Point { X = r.X + r.Width , Y = r.Y + r.Height },
                    new Point { X = r.X , Y = r.Y + r.Height },
                    new Point { X = r.X, Y = r.Y }
                };

                gfx.DrawLines(new Pen(color, Config.PenWidth), path);

                return;
            }
            
            var old = lastRegion;
            var b = new SolidBrush(BackColor);

            if (old.X < r.X)
            {
                gfx.FillRectangle(b, old.X, old.Y, r.X - old.X, old.Height);
            }

            if (old.Y < r.Y)
            {
                gfx.FillRectangle(b, old.X, old.Y, old.Width, r.Y - old.Y);
            }

            if (old.Y + old.Height > r.Y + r.Height)
            {
                gfx.FillRectangle(b, old.X, r.Y + r.Height, old.Width, old.Y + old.Height - (r.Y + r.Height));
            }

            if (old.X + old.Width > r.X + r.Width)
            {
                gfx.FillRectangle(b, r.X + r.Width, old.Y, old.X + old.Width - (r.X + r.Width), old.Height);
            }

            gfx.FillRectangle(color, r.X, r.Y, r.Width, r.Height);
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

            var region = CaptureService.GetRectangle(mouseDownPoint, mouseMovePoint);

            if (mouseButtonUsed == MouseButtons.Left)
            {
                DrawSelectRectangle(e.Graphics, Config.LeftColor, region);
            }
            else if (mouseButtonUsed == MouseButtons.Right)
            {
                DrawSelectRectangle(e.Graphics, Config.RightColor, region);
            }

            lastRegion = region;
        }

        /// <summary>
        /// Refresh the background if transparency is disabled.
        /// </summary>
        private void RefreshBackground()
        {
            if (!SettingsService.Current.NoTransparency)
            {
                return;
            }

            var bitmap = CaptureService.CaptureBitmap(new Rectangle(screen.Location, screen.Size));

            BackgroundImage = bitmap;
            Opacity = 1.0D;
        }

        /// <summary>
        /// Make the form invisible and enable clicking.
        /// </summary>
        private void MakeClickable()
        {
            if(VersionService.GetPlatform() != PlatformType.Windows || SettingsService.Current.NoTransparency)
            {
                return;
            }

            TransparencyKey = Color.White;
            Opacity = 0.005D;
        }

        /// <summary>
        /// Make the background invisible (not the selecting rectangle) and disable clicking.
        /// </summary>
        private void MakeInvisible()
        {
            if (VersionService.GetPlatform() != PlatformType.Windows || SettingsService.Current.NoTransparency)
            {
                return;
            }

            TransparencyKey = Color.Black;
            Opacity = 0.75D;
        }
    }
}