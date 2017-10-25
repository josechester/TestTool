using Injectoclean.Tools.Ford.Data;
using System;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Injectoclean.Tools.BLE;
using System.Linq;
using Injectoclean.Tools.Ford.GenericVin;
using System.Threading.Tasks;
using Windows.UI.Core;
// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Injectoclean
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Scenario_Consol : Page
    {
        private MainPage rootPage;
        private static AutoResetEvent resetEvent = new AutoResetEvent(false);
        //private SetupCJ4 setup;
        ComunicationManager comunication=MainPage.Current.Comunication;
        public Scenario_Consol()
        {
            
            InitializeComponent();
            //setup = new SetupCJ4(MainPage.Current.Comunication, "pass.cj4", MainPage.Current.messageScreen);
            SetupCJ4.ExecuteTest(MainPage.Current.Comunication, MainPage.Current.messageScreen);
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {

   

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
