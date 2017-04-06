using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Matopeli
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {

        // snake
        private Snake snake;

        // items, item

        // keypresshandler
        private bool UpPressed;
        private bool DownPressed;
        private bool LeftPressed;
        private bool RightPressed;

        //snake movedirection
        public int currentDirX { get; set; }
        public int currentDirY { get; set; }

        public bool goingUp;
        public bool goingDown;
        public bool goingLeft;
        public bool goingRight;


        //gamelooptimer
        private DispatcherTimer timer;

        //audio

        public GamePage()
        {
            this.InitializeComponent();

            // create snake
            snake = new Snake
            {
                LocationX = GameBG.Width / 2,
                LocationY = GameBG.Height / 2
            };

            //add snake
            GameBG.Children.Add(snake);

            //key listener

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            //start gameloop
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan (0,0,0,0,1000/60);
            timer.Tick += timer_tick;
            timer.Start();
        }

        //Gameloop
        private void timer_tick(object sender, object e)
        {

            if (UpPressed && goingDown == false)
            {
                currentDirY = 1;
                currentDirX = 0;
                goingUp = true;
                goingLeft = false;
                goingRight = false;
                goingDown = false;
            }
            if (DownPressed && goingUp == false)
            {
                currentDirY = -1;
                currentDirX = 0;
                goingUp = false;
                goingLeft = false;
                goingRight = false;
                goingDown = true;
            }
            if (LeftPressed && goingRight == false)
            {
                currentDirY = 0;
                currentDirX = 1;
                goingUp = false;
                goingLeft = true;
                goingRight = false;
                goingDown = false;
            }
            if (RightPressed && goingLeft == false)
            {
                currentDirY = 0;
                currentDirX = -1;
                goingUp = false;
                goingLeft = false;
                goingRight = true;
                goingDown = false;
            }

            snake.move(currentDirY, currentDirX);

            snake.SetLocation();
 
        }


        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Up:
                    UpPressed = false;
                    break;
                case VirtualKey.Down:
                    DownPressed = false;
                    break;
                case VirtualKey.Left:
                    LeftPressed = false;
                    break;
                case VirtualKey.Right:
                    RightPressed = false;
                    break;

            }
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch(args.VirtualKey)
            {
                case VirtualKey.Up:
                    UpPressed = true;
                    break;
                case VirtualKey.Down:
                    DownPressed = true;
                    break;
                case VirtualKey.Left:
                    LeftPressed = true;
                    break;
                case VirtualKey.Right:
                    RightPressed = true;
                    break;

            }
        }

        private void BackButtonGP_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null) return;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }
    }
}
