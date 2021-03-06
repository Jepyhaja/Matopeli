﻿using System;
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
    public sealed partial class Item : UserControl
    {
        // position
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        // item that is shown
        public int currentFrame = 0; 
        public int frameHeigth = 120;

        // item properties
        public int speedUp { get; set; }

        

        public Item()
        {
            this.InitializeComponent();

            Random rand = new Random();
            // frame
            currentFrame = rand.Next(0, 5);
            if (currentFrame == 0)
            {
                speedUp = 1;
            }
            if (currentFrame == 1)
            {
                speedUp = 2;
            }
            if (currentFrame == 2)
            {
                speedUp = 3;
            }
            if (currentFrame == 3)
            {
                speedUp = 4;
            }
            if (currentFrame == 4)
            {
                speedUp = 5;
            }
            if (currentFrame == 5)
            {
                speedUp = 6;
            }
            // set offset
            SpriteSheetOffset.Y = currentFrame * -frameHeigth;
        }

        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
