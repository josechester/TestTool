using System;
using System.Threading.Tasks;

namespace Injectoclean.Tools.BLE
{
    public interface ILockScreen
    {
        Task Show(String title);
        void Close();
        Task setTitle(String title);
        Task set(String title, String content, int timeout);
        Task SetwithButton(String title, String content, String CloseButtonName);
    }
}
