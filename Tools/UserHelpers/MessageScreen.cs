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

        public void setTitle(string title)
        {
            dialog.Title = title;
            ProgressRing ring =new ProgressRing();
            ring.IsActive = true;
            dialog.Content = ring;
            dialog.CloseButtonText = "";

        }

        public void SetwithButton(string title, string content, string CloseButtonName)
        {
            dialog.Title = title;
            dialog.Content = content;
            dialog.CloseButtonText = CloseButtonName;

        }

        public void Show(string title)
        {
            if(dialog!=null)
                dialog.Hide();
            dialog = new ContentDialog();
            dialog.Title = title;
            ProgressRing ring = new ProgressRing();
            ring.IsActive = true;
            dialog.Content = ring;
            dialog.ShowAsync();
        }
       
        public MessageScreen()
        {
        }

        public async Task set(string title, string content, int timeout)
        {
            dialog.CloseButtonText = "";
            dialog.Title = title;
            dialog.Content = content;
            await Task.Delay(timeout);
            this.Close();
        }
    }
}
