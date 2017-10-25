using System;
using System.Threading.Tasks;
using static Injectoclean.Tools.BLE.GattAttributes.InmediateAlert;

namespace Injectoclean.Tools.BLE
{
    public static class SetupCJ4
    {
        public static async Task ExecuteSetup(ComunicationManager comunication, String program, ILockScreen dialog)
        {
            int limit = 5;
            if (dialog != null)
                dialog.Show("Restarting CJ4...");
            if (!comunication.IsReady())
                ComunicationManager.PutTaskDelay(1000);
            comunication.SendCommand(Key.Reset);
            await Task.Delay(1000);
            if (dialog != null)
                dialog.setTitle("Accesing Remote Shell...");
            if (!Shell.RemoteShellAccess(comunication, limit))
            {
                if (dialog != null)
                    dialog.SetwithButton("could'n conect to remote Shell", "Please use a update device to this function or if your device is up to day please contact support", "Ok");
                return;
            }
            if (dialog != null)
                dialog.setTitle("Accesing Files...");

            if (!Shell.CdToFiles(comunication, limit))
            {
                if (dialog != null)
                    dialog.SetwithButton("could'n access to files", "Please use a update device to this function or if your device is up to day please contact support", "Ok");
                return;
            }
            if (dialog != null)
                dialog.setTitle("Executing Program");

            if (!Shell.ExecuteFile(comunication, limit, program))
            {
                if (dialog != null)
                    dialog.SetwithButton("could'n execute program" + program, "Please use a update device to this function or if your device is up to day please contact support", "Ok");
            }
            else
            {
                if (dialog != null)
                    dialog.set("Sucess", "Program " + program + " is running", 1500);
            }
        }
        public static async Task ExecuteTest(ComunicationManager comunication, ILockScreen dialog)
        {
            int limit = 5;
            if (dialog != null)
                dialog.Show("Restarting CJ4...");
            if (!comunication.IsReady())
                ComunicationManager.PutTaskDelay(1000);
            comunication.SendCommand(Key.Reset);
            await Task.Delay(1000);
            if (dialog != null)
                dialog.setTitle("Accesing Remote Shell...");

            if (!Shell.RemoteShellAccess(comunication, limit))
            {
                if (dialog != null)
                    dialog.SetwithButton("could'n conect to remote Shell", "Please use a update device to this function or if your device is up to day please contact support", "Ok");
                return;
            }
            if (dialog != null)
                dialog.setTitle("Accesing Files...");

            if (!Shell.CdToFiles(comunication, limit))
            {
                if (dialog != null)
                    dialog.SetwithButton("could'n access to files", "Please please contact support", "Ok");
                return;
            }
            if (dialog != null)
                dialog.setTitle("Copying SetHd.CJ4");
            if (!Shell.CopyPCToFlash(comunication, "SetHd.CJ4", "\\Assets\\Data\\"))
            {
                if (dialog != null)
                    dialog.SetwithButton("could'n copy program", "Please contact support", "Ok");
                return;
            }
            if (dialog != null)
                dialog.setTitle("Copying SetMB.CJ4");
            if (!Shell.CopyPCToFlash(comunication, "SetMB.CJ4", "\\Assets\\Data\\"))
            {
                if (dialog != null)
                    dialog.SetwithButton("could'n copy program", "Please contact support", "Ok");
                return;
            }
            dialog.setTitle("Copying Test.CJ4");
            if (!Shell.CopyPCToFlash(comunication, "Test.CJ4", "\\Assets\\Data\\"))
            {
                if (dialog != null)
                    dialog.SetwithButton("could'n copy program", "Please contact support", "Ok");
                return;
            }
            if (dialog != null)
                dialog.setTitle("Opening test Program");

            if (!Shell.ExecuteFile(comunication, limit, "Test.cj4"))
            {
                if (dialog != null)
                    dialog.SetwithButton("could'n execute program Test.cj4", "Please contact support", "Ok");
            }
            else
            {
                if (dialog != null)
                    dialog.set("Files Copy Sucess", "Program Test.cj4 is running", 1500);
            }
        }
    }
}
