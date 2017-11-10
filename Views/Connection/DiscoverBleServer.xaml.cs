using Injectoclean.Tools.BLE;
using System;
using System.Linq;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using static Injectoclean.Tools.BLE.SetupCJ4;

namespace Injectoclean
{

    public sealed partial class DiscoverBleServer : Page
    {
        private SolidColorBrush red = new SolidColorBrush();
        private SolidColorBrush green = new SolidColorBrush();
        private MainPage rootPage = MainPage.Current;
        private BLEContainer tester, Device;
        private String IdMB, IdHD;

        private void sethdcolor(SolidColorBrush brush) {

            MBP7.Fill = brush;
            MBP10.Fill = brush;
            MBm7.Fill = brush;
            MBP13.Fill = brush;
            MBCAN.Fill = brush;
        }
        private void setmbcolor(SolidColorBrush brush)
        {
            HDJ17.Fill = brush;
            HDJ19.Fill = brush;
        }
        private void reset()
        {
            sethdcolor(red);
            setmbcolor(red);
            ComboBoxFile.SelectedIndex = -1;
            BRunTest.IsEnabled = false;
            HDview.Visibility = Visibility.Collapsed;
            MBview.Visibility = Visibility.Collapsed;
        }
        public DiscoverBleServer()
        {
            this.InitializeComponent();
            InitializeComponent();

            red.Color = Colors.Red;
            green.Color = Colors.Green;

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
            IdHD = (String)AppSettings.Values["HD"];
            reset();

        }
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private async void BRunTest_Click(object sender, RoutedEventArgs e)
        {
            rootPage.Log.LogMessageNotification("");
            if (!tester.IsConnected() || !Device.IsConnected())
            {
                rootPage.Log.LogMessageError("Porfavor primero conectese al dispocitivo y valide la conexion al tester");
                return;
            }
            if (ComboBoxFile.SelectedIndex == -1)
            {
                rootPage.Log.LogMessageError("Porfavor seleccione un tipo de configuracion");
                return;
            }
            //run on converted device
            await SetupCJ4.SetupTest(Device.Comunication, Programs.Tester, rootPage.messageScreen);
            //run TestD.CJ4 on tester and get responses
            await SetupCJ4.SetupTest(tester.Comunication, Programs.Test, rootPage.messageScreen);
            shell.Visibility = Visibility.Visible; 
            getmessages();
        }
        private async void getmessages()
        {
            for (int i = 0; i < 100; i++)
            {
                Byte[][] responses = tester.Comunication.GetResponses(100, 1);
                if (responses != null)
                {
                    for (int j = 0; j < responses.Length; j++)
                    {
                        printonshell(tester.Comunication.GetstringFromBytes(responses[j]));
                        printonshell(System.Text.Encoding.ASCII.GetString(responses[j]));
                        switch (ComboBoxFile.SelectedIndex)
                        {
                            case 0:
                                checkHD(responses[j]);
                                break;
                            case 1:
                                checkMB(responses[j]);
                                break;
                        }
                    }     
                }
            }
        }
        void checkHD(Byte[] mess)
        {
            if (mess[1] != 'j' || mess.Length<6)
                return;
            if (HDJ17.Fill == red)
            {
                if (mess[2] == '1' && mess[3] == '7' && mess[4] == '0' && mess[5] == '8')
                {
                    HDJ17.Fill = green;
                    return;
                }
            }
            if (HDJ19.Fill == red)
            {
                if (mess[2] == '1' && mess[3] == '9' && mess[4] == '3' && mess[5] == '9')
                {
                    HDJ19.Fill = green;
                    return;
                }
            }
        }
        void checkMB(Byte[] mess)
        {
            if (mess.Length < 5)
                return;
            switch (System.Text.Encoding.ASCII.GetString(mess).Substring(0, 5))
            {
                case "isop07":
                    MBP7.Fill = green;
                    break;
                case "isop10":
                    MBP10.Fill = green;
                    break;
                case "isop13":
                    MBP13.Fill = green;
                    break;
                case "isopm7":
                    MBm7.Fill = green;
                    break;
                case "isomc":
                    MBCAN.Fill = green;
                    break;
            }
            return;
        }
        private void Bconect_Device_Click(object sender, RoutedEventArgs e)
        {
            reset();
            rootPage.Log.LogMessageNotification("");
            if (txt_id_device.Text.Length == 5 && txt_id_device.Text.Length < 7)
                Device.connect(txt_id_device.Text);
            else
                rootPage.NotifyUser("Error ingrese un S/N valido", NotifyType.ErrorMessage);
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
                rootPage.Log.LogMessageError("Porfavor primero conectese al dispocitivo");
                return;
            }
            switch (ComboBoxFile.SelectedIndex)
            {
                case -1:
                    rootPage.NotifyUser("Porfavor seleccione un tipo de configuracion", NotifyType.ErrorMessage);
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
            sethdcolor(red);
            setmbcolor(red);
            BRunTest.IsEnabled = false;
            HDview.Visibility = Visibility.Collapsed;
            MBview.Visibility = Visibility.Collapsed;
            switch (ComboBoxFile.SelectedIndex)
            {
                case -1:
                    rootPage.NotifyUser("Porfavor seleccione un tipo de configuracion", NotifyType.ErrorMessage);
                    break;
                case 0:
                    tester.connect(IdHD);
                    HDview.Visibility = Visibility.Visible;
                    break;
                case 1:
                    tester.connect(IdMB);
                    MBview.Visibility = Visibility.Visible;
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