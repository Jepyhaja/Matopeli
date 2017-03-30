using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

        //audio
        private MediaElement mediaElement;

        public GamePage()
        {
            this.InitializeComponent();

            // snake

            // foods, food

            // keypresshandler

            //gamelooptimer

            //audio
            LoadAudio();

        }

        private async void LoadAudio()
        {
            StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file =
                await folder.GetFileAsync("mehu.wav");
            var stream = await file.OpenAsync(FileAccessMode.Read);

            mediaElement = new MediaElement();
            mediaElement.AutoPlay = false;
            mediaElement.SetSource(stream, file.ContentType);
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
