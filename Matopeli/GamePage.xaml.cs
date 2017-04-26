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
    /// Page containing most game logic
    /// </summary>

    public sealed partial class GamePage : Page
    {
        
        // snake
        private Snake snake;

        // list of snakes, we draw body with from this
        private List<Snake> snakes;
        
        // x, y for snake
        private Point point;
        
        // private snake placement list
        private List<Point> points;

        // item
        private Item item;

        //score
        int score = 0;
        
        // snake length
        int length = 2; // item eaten -> length ++
        // how many items have been eaten
        int itemsEaten = 0; // rect with item --> ++

        // keypresshandler
        private bool UpPressed;
        private bool DownPressed;
        private bool LeftPressed;
        private bool RightPressed;
       
        // flags to restrict players movements and to keep snake moving always
        public bool goingUp; 
        public bool goingDown;
        public bool goingLeft;
        public bool goingRight;

        // flag to stop timerstart in keyTimer
        public bool TimerOn = false;

        // gamelooptimer contains all the methods like move, render etc
        private DispatcherTimer timer;

        // keypresshandler and the start argument for timer
        private DispatcherTimer keyTimer;
        private MediaElement mediaElement;
        private MediaElement mediaElement2;

        public GamePage()
        {
            this.InitializeComponent();

            snakes = new List<Snake>(); // initialize list of snakes
            points = new List<Point>(); // initialize list of coordinate points

            // this dictates the starting position and length of the snake
            point = new Point(300, 300); // (X,Y) 
            points.Add(point);
            foreach (Point point in points)  // not neccessarily needed here
            {
                snake = new Snake       // create snake so we can draw it
                {
                    LocationX = point.X,  // useless (I guess)
                    LocationY = point.Y   // useless (I guess)
                };
                snakes.Insert(0, snake);        // insert snake to the first slot of the snakes list
                GameBG.Children.Insert(0, snakes[0]);   // draw the first snake in the snakes list to the canvas
                snakes[0].SetLocation(point.X, point.Y); // set location for the snake drawn

            }

            // spawn first item
            itemSpawn();

            // Load and start the music
            LoadAudio();

            // key listener
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            // start gameloop 10FPS
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan (0,0,0,0,1000/10);
            timer.Tick += timer_tick;
            //timer.start(); is inside keyTimer_tick method
            

            // start keypresshandler 60FPS
            keyTimer = new DispatcherTimer();
            keyTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            keyTimer.Tick += keyTimer_tick;
            keyTimer.Start();
        }

        // load audio
        private void LoadAudio()
        {
            LoadMusic();
            LoadRage();
        }

        private async void LoadMusic()
        {
            StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile Musicfile =
                await folder.GetFileAsync("cheekiBreeki_asset.mp3");
            var stream = await Musicfile.OpenAsync(FileAccessMode.Read);

            mediaElement = new MediaElement();
            mediaElement.AutoPlay = false;
            mediaElement.IsLooping = true;
            mediaElement.SetSource(stream, Musicfile.ContentType);
            
        }

        private async void LoadRage() // ladataan ragesound
        {
            StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile Ragefile =
                await folder.GetFileAsync("RAGE_asset.mp3");
            var stream = await Ragefile.OpenAsync(FileAccessMode.Read);

            mediaElement2 = new MediaElement();
            mediaElement2.AutoPlay = false;
            mediaElement2.SetSource(stream, Ragefile.ContentType);

        }


        private void checkCollision()
        {

            // checks if snake is hitting the wall
            if (snake.LocationX > GameBG.Width - 25 || snake.LocationX < 0 || snake.LocationY < 0 || snake.LocationY > GameBG.Height - 25)
            {
                gameOver();
            }
            
            Rect SnakeRect = new Rect(snake.LocationX, snake.LocationY, snake.ActualWidth, snake.ActualHeight);

            Rect ItemRect = new Rect(item.LocationX, item.LocationY, item.ActualWidth, item.ActualHeight);

            // check if item and snake are colliding
            SnakeRect.Intersect(ItemRect);


            if (!SnakeRect.IsEmpty)
            {
                itemsEaten++;

                GameBG.Children.Remove(item);

                itemSpawn();

                length++;

                // gradual speedup logic:
                if (length < 15)
                {
                    timer.Interval -= new TimeSpan(0, 0, 0, 0, 1000 / (100 * item.speedUp - (length + itemsEaten)));
                }
                if (length > 15 && length < 20)
                {
                    timer.Interval -= new TimeSpan(0, 0, 0, 0, 1000 / (200 * item.speedUp - (length + itemsEaten)));
                }
                if (length > 20 && length < 25)
                {
                    timer.Interval -= new TimeSpan(0, 0, 0, 0, 1000 / (350 * item.speedUp - (length + itemsEaten)));
                }
                if (length > 25)
                {
                    timer.Interval -= new TimeSpan(0, 0, 0, 0, 1000 / (450 * item.speedUp - (length + itemsEaten)));
                }

                score += ((itemsEaten + length) * item.speedUp);
            }


                for ( int i=1; i < snakes.Count (); i++) // loop tru the whole snake-list
            {
                if (snakes[0].LocationX == snakes[i].LocationX && snakes[0].LocationY == snakes[i].LocationY) //if the coordinates match any of the parts in the list
                {
                    gameOver();
                }
            }

        }


        

        private void keyTimer_tick(object sender, object e){

                // keypresshandler tick

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

            if ((UpPressed == true || DownPressed == true || LeftPressed == true || RightPressed == true) && TimerOn == false) {
                timer.Start(); // start gamelogic after an arrowkey is pressed

                // start music
                mediaElement.Play();

                TimerOn = true;
            }

        }


        // Gameloop 10FPS
        private void timer_tick(object sender, object e)
        {

            Move();

            renderSnake();

            removeTail();
            

            textBlock.Text = score.ToString(); // only for you my love(debug)
            
            checkCollision();
            
        }

        private void gameOver()
        {
            // gameOver(); Show label with reset button and score
            timer.Stop();
            //stop music
            mediaElement.Stop();
            mediaElement2.Play(); // play rage
        }




       private void renderSnake() // draws latest point as a snake 
        {
            
            snake = new Snake // create snake 
            {
                LocationX = point.X, // the X cords of the latest point
                LocationY = point.Y    // the Y cords of the latest point
            };
                snakes.Insert(0, snake); // add snake to snakes as first
                GameBG.Children.Insert(0, snakes[0]); // add as first to the canvas
                snakes[0].SetLocation(point.X, point.Y); // draw snake to canvas
            }
        
        private void removeTail() // this removes last point and last snake and corresponding snake from canvas
        {
            if (points.Count > length) // this argument dictates snakes length
            {
                GameBG.Children.Remove(snakes.Last());  // remove drawn snake form canvas, must be done first
                snakes.Remove(snakes.Last());           
                points.Remove(points.Last());           // remove last point to keep the wanted length
            }
        }

        private void Move() // create new points for the render function to utilize later
        {

            if (goingUp == true) {
                point = new Point(points[0].X, points[0].Y - snake.speed); 
                points.Insert(0, point);
               
            };
            if (goingDown == true)
            {
                point = new Point(points[0].X, points[0].Y + snake.speed);
                points.Insert(0, point);
                
            };
            if (goingLeft == true)
            {
                point = new Point(points[0].X - snake.speed, points[0].Y);
                points.Insert(0, point);
                
            };
            if (goingRight == true)
            {
                point = new Point(points[0].X + snake.speed, points[0].Y);
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
            item.LocationY = random.Next(0, Convert.ToInt32(itemY) - 120);


            Random rand = new Random();
            item.currentFrame = rand.Next(0, 5);
            GameBG.Children.Add(item);
            item.SetLocation();
            
        }
        // basic setup for getting arrow keys to function

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
