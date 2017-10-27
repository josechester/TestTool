using Injectoclean.Tools.BLE;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Injectoclean.Tools.BLE.SetupCJ4;

namespace Injectoclean
{

    public sealed partial class DiscoverBleServer : Page
    {
        private MainPage rootPage = MainPage.Current;
        private BLEContainer tester,Device;

        public DiscoverBleServer()
        {
            InitializeComponent();
            ComboBoxFile.Items.Add("HD");
            ComboBoxFile.Items.Add("MB");
            tester = new BLEContainer(rootPage.Log, rootPage.messageScreen);
            Device = new BLEContainer(rootPage.Log, rootPage.messageScreen);

        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
 
        }

        private void Bconect_tester_Click(object sender, RoutedEventArgs e)
        {
            rootPage.Log.LogMessageNotification("");
            if (txt_id_tester.Text.Length == 5 && txt_id_tester.Text.Length < 7)
                tester.connect(txt_id_tester.Text);
            else
                rootPage.NotifyUser("Error input a correct id on tester S/N", NotifyType.ErrorMessage);
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
            String program;
            if (ComboBoxFile.SelectedIndex == 1)
                program = Programs.HD;
            else
                program = Programs.MB;

            await SetupCJ4.SetupTester(tester.Comunication, program, rootPage.messageScreen);
            //await SetupCJ4.SetupTest(Device.Comunication, Programs.Test, rootPage.messageScreen);
            //getmessages();
        }
        private async void getmessages()
        {
            for (int i = 0; i < 100; i++)
            {
                Byte[][] responses = tester.Comunication.GetResponses(100, 20);
                for (int j = 0; j < responses.Length; j++)
                    printonshell(responses[i].ToString());
            }
        }

        private void Bconect_Device_Click(object sender, RoutedEventArgs e)
        {
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