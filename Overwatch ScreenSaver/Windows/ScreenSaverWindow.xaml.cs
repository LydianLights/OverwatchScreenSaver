using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Overwatch_ScreenSaver
{
    public partial class ScreenSaverWindow : Window
    {
        #region Initialization
        //initialize screensaver to screen bounds
        public ScreenSaverWindow(Rect bounds)
        {
            InitializeComponent();
            this.Left = bounds.Left;
            this.Top = bounds.Top;
            this.Width = bounds.Width;
            this.Height = bounds.Height;

            //set up timer for animation
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        //make the screensaver be far even as decided to use even go want to do look more like
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.None;
            this.Topmost = true;
        }
        #endregion


        #region Input Detection
        //close the screensaver when user input is detected
        Point mousePrevLocation;
        Point mouseCurrLocation;
        readonly int mouseMoveTolerance = 3;

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            //quit if mouse is moved a significant amount
            if (!Point.Equals(mousePrevLocation, new Point(0,0)))
            {
                mouseCurrLocation = e.GetPosition(this);

                if (Math.Abs(mousePrevLocation.X - mouseCurrLocation.X) > mouseMoveTolerance ||
                    Math.Abs(mousePrevLocation.Y - mouseCurrLocation.Y) > mouseMoveTolerance)
                {
                    Application.Current.Shutdown();
                }
            }

            mousePrevLocation = e.GetPosition(this);
        }
        #endregion


        #region Animation
        //spawn images and move them around based on the timer
        List<Image> imageList = new List<Image>();
        Vector velocity = new Vector(-1, 1);
        Random r = new Random();

        private void timer_Tick(object sender, EventArgs e)
        {
            MoveImages();
            if (r.Next(100) == 0)
            {
                AddImage();
            }
        }

        //spawn a new image and add it to the image list
        private void AddImage()
        {
            Image image = new Image();
            image.Source = Application.Current.FindResource("DVa") as BitmapImage;

            double scale = r.NextDouble() + 0.5;
            //scale = 1;
            image.Width = 276 * scale;
            //double.NaN means "auto"
            //image.Height = double.NaN;
            image.Height = 287.5 * scale;

            //choose a point to spawn at along the top or right border
            int nextSpawn = r.Next((int)(this.Width + this.Height));

            Point location;
            //check if the top border was chosen
            if (nextSpawn < this.Width)
            {
                location = new Point(nextSpawn, -image.Height);
            }
            //else spawn on right border
            else
            {
                location = new Point(this.Width, nextSpawn - this.Width - image.Height);
            }

            image.SetValue(Canvas.LeftProperty, location.X);
            image.SetValue(Canvas.TopProperty, location.Y);

            imageLayer.Children.Add(image);
            imageList.Add(image);
        }

        //cycle through the image list and move them based on global velocity
        private void MoveImages()
        {
            for (int i = 0; i < imageList.Count; i++)
            {
                Image image = imageList[i];
                double x = (double)image.GetValue(Canvas.LeftProperty) + velocity.X;
                double y = (double)image.GetValue(Canvas.TopProperty) + velocity.Y;

                image.SetValue(Canvas.LeftProperty, x);
                image.SetValue(Canvas.TopProperty, y);

                if (x < -image.Width || y > this.Height)
                {
                    RemoveImage(i);
                    i--;
                }
            }
        }

        private void RemoveImage(int index)
        {
            imageLayer.Children.RemoveAt(index);
            imageList.RemoveAt(index);
        }
        #endregion
    }
}
