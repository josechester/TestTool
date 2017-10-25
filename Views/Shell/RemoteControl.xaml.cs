using Injectoclean.Tools.Developers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Injectoclean.Tools.BLE.GattAttributes.InmediateAlert;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Injectoclean.Views.Shell
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class RemoteControl : Page
    {
        private MainPage rootPage;
        
        public RemoteControl()
        {
            this.InitializeComponent();
            rootPage = MainPage.Current;
            
        }

        private void up_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Comunication.SendCommand(Key.Up);
        }

        private void left_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Comunication.SendCommand(Key.Left);
        }

        private void down_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Comunication.SendCommand(Key.Down);
        }

        private void right_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Comunication.SendCommand(Key.Right);
        }

        private void enter_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Comunication.SendCommand(Key.Enter);
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Comunication.SendCommand(Key.Reset);
        }

        private void escape_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Comunication.SendCommand(Key.Esc);
        }
    }
}
