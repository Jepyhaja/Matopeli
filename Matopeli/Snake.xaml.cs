using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Matopeli
{
    public sealed partial class Snake : UserControl
    {

        //speed
        private double speed = 25;

        //direction
        private int directionX = 1; // 1 or -1
        private int directionY = 1; // 1 or -1

        

        public double LocationX { get; set; }
        public double LocationY { get; set; }


        public Snake()
        {
            this.InitializeComponent();

        }

        // moveX
        public void moveX(int DirectionX)
        {
            //speed = 5
            
           

        }
        public void moveY(int DirectionY)
        {
            //speed = 5
            

        }
        public void move(int DirectionY, int DirectionX)
        {
            LocationX -= directionX * speed * DirectionX;
            LocationY -= directionY * speed * DirectionY;
        }


        //update location

        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
        
    }
}
