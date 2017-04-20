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


        //gameoverscreen
        private bool gameOver = false;




        // snake
        private Snake snake;

        // private snake list
        private List<Snake> snakes;

        // item
        private Item item;


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

        //timer for keypresshandeler
        private DispatcherTimer keyTimer;

        //audio

        public GamePage()
        {
            this.InitializeComponent();

            snakes = new List<Snake>();

            // create snake
            snake = new Snake
            {
                LocationX = GameBG.Width / 2,
                LocationY = GameBG.Height / 2
            };
           
            snakes.Add(snake);

            snake = new Snake
            {
                LocationX = (GameBG.Width / 2) + 25,
                LocationY = (GameBG.Height / 2)
            };
            snakes.Add(snake);

            
            //add snake
            GameBG.Children.Add(snakes[0]);
            GameBG.Children.Add(snakes[1]); // +25 px X-akselilla
            itemSpawn();
            //key listener

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            //start gameloop 10FPS
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan (0,0,0,0,1000/10);
            timer.Tick += timer_tick;
            timer.Start();

            //start keypresshandler 60FPS
            keyTimer = new DispatcherTimer();
            keyTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            keyTimer.Tick += keyTimer_tick;
            keyTimer.Start();
        }

        private void checkCollision()
        {


            Rect SnakeRect = new Rect(snakes[0].LocationX, snakes[0].LocationY, snakes[0].ActualWidth, snakes[0].ActualHeight);

            Rect ItemRect = new Rect(item.LocationX, item.LocationY, item.ActualWidth, item.ActualHeight);

            //intersect checker
            SnakeRect.Intersect(ItemRect);


            if (!SnakeRect.IsEmpty)
            {
                GameBG.Children.Remove(item);

                itemSpawn();
                
            }

        }


        private void keyTimer_tick(object sender, object e){

                // keypresshandelr tick

             if (UpPressed && snake.direction != "down")
            {
                snake.direction = "up";
            }
            if (DownPressed && snake.direction != "up")
            {
                snake.direction = "down";
            }
            if (LeftPressed && snake.direction != "right")
            {
                snake.direction = "left";
            }
            if (RightPressed && snake.direction != "left")
            {
                snake.direction = "right";
            }

            
        }


        //Gameloop 10FPS
        private void timer_tick(object sender, object e)
        {

            // madon collision handler seiniin
            if (snakes[0].LocationX > GameBG.Width - 25 || snakes[0].LocationX < 0 || snakes[0].LocationY < 0 || snakes[0].LocationY > GameBG.Height - 25)
            {
                //gameOver();
                timer.Stop();
            }


            Move();
            /*
            snakes[0].SetLocation();

            snakes[1].LocationX = snakes[0].LocationX + 25;

            snakes[1].LocationY = snakes[0].LocationY + 25;

            snakes[1].SetLocation();
            */

            /* for (int i = 0; i < snakes.Count; i++)
             {

                 snakes[i].SetLocation();

             }*/

            snakes[0].SetLocation();

            checkCollision();
            
        }

        private void Move()
        {
                
        switch(snake.direction) {
        case "up":
                snakes[0].LocationY -= snake.speed;
        break;
        case "down":
                snakes[0].LocationY += snake.speed;
        break;
        case "left":
                snakes[0].LocationX -= snake.speed;
        break;
        case "right":
                snakes[0].LocationX += snake.speed;
        break;

            }
        }

        public void itemSpawn()
        {
            double itemX = GameBG.Width;
            double itemY = GameBG.Height;
            Random random = new Random();
            item = new Item();
            item.LocationX = random.Next(0, Convert.ToInt32(itemX) - 100);
            item.LocationY = random.Next(0, Convert.ToInt32(itemY) - 100);


            Random rand = new Random();
            item.currentFrame = rand.Next(0, 5);
            GameBG.Children.Add(item);
            item.SetLocation();

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
