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
    public sealed partial class Item : UserControl
    {
        // position

            public double LocationX { get; set; }
            public double LocationY { get; set; }

        // effects

        // size

        public Item()
        {
            this.InitializeComponent();
            Width = 110;
            Height = 120;
        }

        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
