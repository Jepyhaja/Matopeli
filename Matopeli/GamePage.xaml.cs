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
        private List<Snake> snakes;
        // x, y for snake
        private Point point;
        // private snake placement list
        private List<Point> points;

        // item
        private Item item;

        //Snake lenght
        int length = 5;

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
            points = new List<Point>();

            // create point
           
            point = new Point(300, 300);
            points.Add(point);
            foreach(Point point in points)
            {
                snake = new Snake
                {
                    LocationX = point.X,
                    LocationY = point.Y
                };
                snakes.Insert(0, snake);
                GameBG.Children.Insert(0, snakes[0]);
                snakes[0].SetLocation(point.X, point.Y);

            }
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


            Rect SnakeRect = new Rect(snake.LocationX, snake.LocationY, snake.ActualWidth, snake.ActualHeight);

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

             if (UpPressed && goingDown == false)
            {
                goingUp = true;
                goingDown = false;
                goingLeft = false;
                goingRight = false;
            }
            if (DownPressed && goingUp == false)
            {
                goingDown = true;
                goingUp = false;
                goingLeft = false;
                goingRight = false;
            }
            if (LeftPressed && goingRight == false)
            {
                goingLeft = true;
                goingDown = false;
                goingUp = false;
                goingRight = false;
            }
            if (RightPressed && goingLeft == false)
            {
                goingRight = true;
                goingDown = false;
                goingLeft = false;
                goingUp = false;
            }

            

        }


        //Gameloop 10FPS
        private void timer_tick(object sender, object e)
        {

            Move();

            Rendersnake();

            // madon collision handler seiniin
            if (snake.LocationX > GameBG.Width - 25 || snake.LocationX < 0 || snake.LocationY < 0 || snake.LocationY > GameBG.Height - 25)
            {
                //gameOver();
                timer.Stop();
            }
            
            textBlock.Text = points.Count.ToString() ;

            checkCollision();
            
        }
        /// <summary>
        /// https://gamedev.stackexchange.com/questions/24817/c-creating-a-simple-snake-game
        /// </summary>

       private void Rendersnake()
        {


            if (points.Count > length) // length = 5
            {
               
                    GameBG.Children.Remove(snakes.Last());
                    GameBG.Children.Remove(snake);
                    snakes.Remove(snakes.Last());
                    points.Remove(points.Last());
                
            }

            snake = new Snake // create snake 
            {
                LocationX = point.X, //the X cords of the latest point
                LocationY = point.Y    //the Y cords of the latest point
            };
            snakes.Insert(0, snake); // add snake to snakes as first
            GameBG.Children.Insert(0, snakes[0]); // add as first to the canvas
            snakes[0].SetLocation(point.X, point.Y); // draw snake to canvas

            

         }









        private void Move()
        {

            if (goingUp == true) {
                point = new Point(points[0].X, points[0].Y - snake.speed);
                snake = new Snake
                {
                    LocationX = point.X,
                    LocationY = point.Y

                };
                snakes.Insert(0, snake);
                snakes.Remove(snakes[snakes.Count - 1]);
                points.Insert(0, point);
               
            };
            if (goingDown == true)
            {
                point = new Point(points[0].X, points[0].Y + snake.speed);
                snake = new Snake
                {
                    LocationX = point.X,
                    LocationY = point.Y

                };
                snakes.Insert(0, snake);
                snakes.Remove(snakes[snakes.Count - 1]);
                points.Insert(0, point);
                
            };
            if (goingLeft == true)
            {
                point = new Point(points[0].X - snake.speed, points[0].Y);
                snake = new Snake
                {
                    LocationX = point.X,
                    LocationY = point.Y

                };
                snakes.Insert(0, snake);
                snakes.Remove(snakes[snakes.Count - 1]);
                points.Insert(0, point);
                
            };
            if (goingRight == true)
            {
                point = new Point(points[0].X + snake.speed, points[0].Y);
                snake = new Snake
                {
                    LocationX = point.X,
                    LocationY = point.Y

                };
                snakes.Insert(0, snake);
                snakes.Remove(snakes[snakes.Count - 1]);
                points.Insert(0, point);
                
            };
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
        
        private void textBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "moro";
        }
    }
}
