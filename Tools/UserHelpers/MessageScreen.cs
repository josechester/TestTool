using System;
using Injectoclean.Tools.BLE;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Injectoclean.Tools.UserHelpers
{
    public class MessageScreen : ILockScreen
    {
        private ContentDialog dialog;
        public void Close()
        {
            dialog.Hide();
        }

        public async Task setTitle(string title)
        {
            if (dialog != null)
                dialog.Hide();
            dialog = new ContentDialog();
            dialog.Title = title;
            ProgressRing ring = new ProgressRing();
            ring.IsActive = true;
            dialog.Content = ring;
             dialog.ShowAsync();
            await Task.Delay(1000);



        }

        public async Task SetwithButton(string title, string content, string CloseButtonName)
        {
            if (dialog != null)
                dialog.Hide();
            dialog = new ContentDialog();
            dialog.Title = title;
            dialog.Content = content;
            dialog.CloseButtonText = CloseButtonName;
            dialog.ShowAsync();
            await Task.Delay(1000);
        }

        public async Task Show(string title)
        {
            if(dialog!=null)
                dialog.Hide();
            dialog = new ContentDialog();
            dialog.Title = title;
            ProgressRing ring = new ProgressRing();
            ring.IsActive = true;
            dialog.Content = ring;
            dialog.ShowAsync();
            await Task.Delay(1000);
        }
       
        public MessageScreen()
        {
        }

        public async Task set(string title, string content, int timeout)
        {
            if (dialog != null)
                dialog.Hide();
            dialog = new ContentDialog();
            dialog.Title = title;
            dialog.Content = content;
             dialog.ShowAsync();
            await Task.Delay(timeout);
            this.Close();
        }
    }
}
