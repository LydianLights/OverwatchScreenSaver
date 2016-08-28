using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfScreenHelper;

namespace OverwatchScreenSaver
{
    //Three possible arguments
    // /p = show screensaver preview
    // /c = show screensaver config dialog
    // /s = show screensaver full-screen

    // /p and /c may be passed with a number representing the window handler (separated by either a space or a colon)
    // ex /p:1234567 or /c 1234567

    //If no arguments behave as if /s was passed

    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //store command line arguments
            string[] args = e.Args;

            //if no arguments given, treat like /s
            if (args.Length == 0)
            {
                args = new string[] { "/s" };
            }


            //format first argument
            string firstArgument = args[0].ToLower().Trim();
            string secondArgument = null;

            //handle arguments separated by a colon
            if (firstArgument.Length > 2)
            {
                secondArgument = firstArgument.Substring(3).Trim();
                firstArgument = firstArgument.Substring(0, 2);
            }
            else if (args.Length > 1)
            {
                secondArgument = args[1];
            }

            if (firstArgument == "/c")          //Configuration mode
            {
                RunConfig();
            }
            else if (firstArgument == "/p")     //Preview mode
            {
                RunPreview();
            }
            else if (firstArgument == "/s")     //Fullscreen mode
            {
                RunScreenSaver();
            }
            else        //Invalid argument
            {
                MessageBox.Show("Invalid command line argument '" + firstArgument + "'", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void RunScreenSaver()
        {
            //create the screensaver in each monitor
            foreach (Screen screen in Screen.AllScreens)
            {
                ScreenSaverWindow wnd = new ScreenSaverWindow(screen.Bounds);
                wnd.Show();
            }
        }

        private void RunPreview()
        {
            //TODO
            this.Shutdown();
        }

        private void RunConfig()
        {
            //TODO
            MessageBox.Show("NERF THIS", ";)", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            this.Shutdown();
        }
    }
}
