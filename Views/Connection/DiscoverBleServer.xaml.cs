
using Injectoclean.Tools.BLE;
using Injectoclean.Tools.Developers;
using Injectoclean.Tools.UserHelpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Injectoclean
{

    public sealed partial class DiscoverBleServer : Page
    {
        private MainPage rootPage = MainPage.Current;

        private ObservableCollection<BluetoothLEDeviceDisplay> ResultCollection = new ObservableCollection<BluetoothLEDeviceDisplay>();
        private Discover discover;

        public DiscoverBleServer()
        {
            InitializeComponent();
            discover = new Discover(MainPage.Current.iDeviceinfo, MainPage.Current.Log,MainPage.Current.messageScreen);
           // discover.GetService("32602");

        }
        private void EnumerateButton_Click(object sender, RoutedEventArgs e)
        {
            rootPage.NotifyUser(String.Empty, NotifyType.StatusMessage);
            discover.Getnearlydevices(ref ResultCollection);
        }
        private void BConect_click(object sender, RoutedEventArgs e)
        {
            if (txt_id.Text.Length == 5 && txt_id.Text.Length < 7)
            {
                discover.GetService(txt_id.Text);
            }

            else
                rootPage.NotifyUser("Error input a correct ID or select by device", NotifyType.ErrorMessage);
            // SetupCJ4.ExecuteTest(MainPage.Current.Comunication, MainPage.Current.messageScreen);
        }

        private void ConectById_Checked(object sender, RoutedEventArgs e)
        {
            discover.Clear();
            ResultCollection.Clear();
            //Connect.visibility = Visibility.Visible;
            Bconect.Visibility = Visibility.Visible;
            txt_id.Visibility = Visibility.Visible;
        }

        private void Bydevice_Checked(object sender, RoutedEventArgs e)
        {
           
            Bconect.Visibility = Visibility.Collapsed;
            txt_id.Visibility = Visibility.Collapsed;
        }


   

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.discover != null)
            {
                discover.Clear();
            }
        }
    }
}