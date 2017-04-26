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
    public sealed partial class  Snake : UserControl 
    {

        //speed
        public double speed = 25;

        //direction
        public string direction { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }


        public Snake()
        {
            this.InitializeComponent();

        }



        //update location

        public void SetLocation(double X, double Y)
        {
            SetValue(Canvas.LeftProperty, X);
            SetValue(Canvas.TopProperty, Y);
        }

        
    }
}
