using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace Injectoclean.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Config : Page
    {
        public Config()
        {
            this.InitializeComponent();
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

            if (!AppSettings.Values.ContainsKey("MB"))
                AppSettings.Values.Add("MB", "35316");
            txt_id_MB.Text = (String)AppSettings.Values["MB"];

            if (!AppSettings.Values.ContainsKey("HD"))
                AppSettings.Values.Add("HD", "33409");
            txt_id_HD.Text = (String)AppSettings.Values["HD"];

            txt_id_HD.IsEnabled = false;
            txt_id_MB.IsEnabled = false;
        }
        
        private void Bconf_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Log.LogMessageNotification("");
            if (Bconf.Content.ToString() == "Edit")
            {
                txt_id_HD.IsEnabled = true;
                txt_id_MB.IsEnabled = true;
                Bconf.Content = "Save";
            }
            else
            {


                if (txt_id_HD.Text.Length < 5 || txt_id_HD.Text.Length > 6)
                {
                    MainPage.Current.NotifyUser("Error input a correct HD tester S/N", NotifyType.ErrorMessage);
                    return;
                }
                if (txt_id_MB.Text.Length < 5 || txt_id_MB.Text.Length > 6)
                { 
                    MainPage.Current.NotifyUser("Error input a correct MB tester S/N", NotifyType.ErrorMessage);
                    return;
                }
                ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
                AppSettings.Values.Clear();
                AppSettings.Values.Add("HD", txt_id_HD.Text);
                AppSettings.Values.Add("MB", txt_id_MB.Text);
                txt_id_HD.IsEnabled = false;
                txt_id_MB.IsEnabled = false;
                Bconf.Content = "Edit";
            }
        }
    }
}
