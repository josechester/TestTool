using Injectoclean.Tools.BLE;
using System;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Injectoclean.Tools.BLE.SetupCJ4;

namespace Injectoclean
{

    public sealed partial class DiscoverBleServer : Page
    {
        private MainPage rootPage = MainPage.Current;
        private BLEContainer tester,Device;
        private String IdMB, IdHD;
        public DiscoverBleServer()
        {
            this.InitializeComponent();
            InitializeComponent();
            ComboBoxFile.Items.Add("HD");
            ComboBoxFile.Items.Add("MB");
            tester = new BLEContainer(rootPage.Log, rootPage.messageScreen);
            Device = new BLEContainer(rootPage.Log, rootPage.messageScreen);
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
            if (!AppSettings.Values.ContainsKey("MB"))
                AppSettings.Values.Add("MB", "35316");
            IdMB = (String)AppSettings.Values["MB"];
            if (!AppSettings.Values.ContainsKey("HD"))
                AppSettings.Values.Add("HD", "33409");
              IdHD= (String)AppSettings.Values["HD"];
            BRunTest.IsEnabled = false;
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
 
        }

        private async void BRunTest_Click(object sender, RoutedEventArgs e)
        {
            rootPage.Log.LogMessageNotification("");
            if (!tester.IsConnected() || !Device.IsConnected())
            {
                rootPage.Log.LogMessageError("Please first connect to both devices");
                return;
            }
            if (ComboBoxFile.SelectedIndex == -1)
            {
                rootPage.Log.LogMessageError("Please select a test to run");
                return;
            }
          //run on converted device
            await SetupCJ4.SetupTest(Device.Comunication, Programs.Tester, rootPage.messageScreen);
            //run TestD.CJ4 on tester and get responses
            //await SetupCJ4.SetupTest(Device.Comunication, Programs.Test, rootPage.messageScreen);
            getmessages();
        }
        private async void getmessages()
        {
            for (int i = 0; i < 100; i++)
            {
                Byte[][] responses = Device.Comunication.GetResponses(100, 20);
                if (responses != null)
                {
                    for (int j = 0; j < responses.Length; j++)
                        printonshell(responses[i].ToString());
                }
            }
        }

        private void Bconect_Device_Click(object sender, RoutedEventArgs e)
        {
            BRunTest.IsEnabled = false;
            rootPage.Log.LogMessageNotification("");
            if (txt_id_device.Text.Length == 5 && txt_id_device.Text.Length < 7)
                Device.connect(txt_id_device.Text);
            else
                rootPage.NotifyUser("Error input a correct id on Device to test S/N", NotifyType.ErrorMessage);
        }
        private void printonshell(String line)
        {
            shell.Text += line + String.Format(Environment.NewLine);
        }

        private void BconfigDevice_Click(object sender, RoutedEventArgs e)
        {
            rootPage.Log.LogMessageNotification("");
            if (!Device.IsConnected())
            {
                rootPage.Log.LogMessageError("Please first connect the device");
                return;
            }
            if (ComboBoxFile.SelectedIndex == -1)
            {
                rootPage.Log.LogMessageError("Please select a test to run");
                return;
            }
            switch (ComboBoxFile.SelectedIndex)
            {
                case -1:
                    rootPage.NotifyUser("Please select a correct test Type", NotifyType.ErrorMessage);
                    break;
                case 0:
                    SetupCJ4.SetupTest(Device.Comunication, Programs.HD, rootPage.messageScreen);
                    BRunTest.IsEnabled = true;
                    break;
                case 1:
                    SetupCJ4.SetupTest(Device.Comunication, Programs.MB, rootPage.messageScreen);
                    BRunTest.IsEnabled = true;
                    break;
            }
            
        }

        private void BCheckConnection_Click(object sender, RoutedEventArgs e)
        {
            switch (ComboBoxFile.SelectedIndex)
            {
                case -1:
                    rootPage.NotifyUser("Please select a correct test Type", NotifyType.ErrorMessage);
                    break;
                case 0:
                    tester.connect(IdHD);
                    break;
                case 1:
                    tester.connect(IdMB);
                    break;
            }
            
        }

        public byte[] CommandBuilder(String line)
        {

            String[] array = line.Split(' ');
            Byte[] temp = new byte[array.Length];
            Byte[] Command = Enumerable.Repeat((byte)0x00, 15).ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                temp[i] = (byte)Convert.ToInt32(array[i], 16);
                Command[i] = temp[i];
                Command[14] += Command[i];
            }
            Command[14] -= Command[0];
            return Command;
        }
    }
}