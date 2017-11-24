using System;
using System.Threading.Tasks;
using static Injectoclean.Tools.BLE.GattAttributes.InmediateAlert;

namespace Injectoclean.Tools.BLE
{
    public static class SetupCJ4
    {
        public class Programs
        {
            public const String MB = "SetMB.CJ4";
            public const String HD = "SetHD.CJ4";
            public const String Test = "TestEq.CJ4";
            public const String Tester = "Test.CJ4";
            public const String Pass = "pass.CJ4";
            public const String nissan = "PN.CJ4";
        }
        private static int limit=5;
        public static async Task ExecuteSetup(ComunicationManager comunication, String program, ILockScreen dialog)
        {
            if (dialog != null)
                await dialog.Show("Reiniciando dispositivo...");
            //if (!comunication.IsReady())
                //ComunicationManager.PutTaskDelay(1000);
            
            if (dialog != null)
                await dialog.setTitle("Accesando a Remote Shell...");
            comunication.SendCommand(Key.Reset);
            //await Task.Delay(1000);
            if (!Shell.RemoteShellAccess(comunication, limit))
            {
                if (dialog != null)
                    await dialog.SetwithButton("Error: No se pudo accesar a remote shell", "Porfavor utilize un dispositivo actualizado.", "Aceptar");
                return;
            }
            if (dialog != null)
                await dialog.setTitle("Accesando a Archivos");

            if (!Shell.CdToFiles(comunication, limit))
            {
                if (dialog != null)
                    await dialog.SetwithButton("Error: Acceso a archivos no obtenido", "Porfavor utiliza un dispositivo actualizado, si su dispositivo lo esta contacte a soporte tecnico", "Aceptar");
                return;
            }
            if (dialog != null)
                await dialog.setTitle("ejecutando Programa");

            if (!Shell.ExecuteFile(comunication, limit, program))
            {
                if (dialog != null)
                    await dialog.SetwithButton("Error: No se pudo ejecutar programa" + program, "Porfavor utiliza un dispositivo actualizado, si su dispositivo lo esta contacte a soporte tecnico", "Aceptar");
            }
            else
            {
                if (dialog != null)
                   await dialog.set("Exito", "Programa " + program + " esta ejecutandose", 1500);
            }
        }
        public static async Task SetupTester(ComunicationManager comunication,String program, ILockScreen dialog)
        {
            if (dialog != null)
                await dialog.Show("Reiniciando Dispositivo...");
            //if (!comunication.IsReady())
                //ComunicationManager.PutTaskDelay(1000);
            if (dialog != null)
                await dialog.setTitle("Accesando a Remote Shell...");
            comunication.SendCommand(Key.Reset);
            //await Task.Delay(1000);
            if (!Shell.RemoteShellAccess(comunication, limit))
            {
                if (dialog != null)
                    await dialog.SetwithButton("Error:", "Porfavor utiliza un dispositivo actualizado, si su dispositivo lo esta contacte a soporte tecnico", "Aceptar");
                return;
            }
            if (dialog != null)
                await dialog.setTitle("Accesando a Archivos");

            if (!Shell.CdToFiles(comunication, limit))
            {
                if (dialog != null)
                    await dialog.SetwithButton("Error: Acceso a archivos no obtenido", "Porfavor utiliza un dispositivo actualizado, si su dispositivo lo esta contacte a soporte tecnico", "Aceptar");
                return;
            }
            if (!Shell.ExecuteFile(comunication, limit, Programs.Tester))
            {
                if (dialog != null)
                    await dialog.setTitle("Copiando " + program);
                if (!Shell.CopyPCToFlash(comunication, program, "\\Assets\\Data\\"))
                {
                    if (dialog != null)
                        await dialog.SetwithButton("Error: No se pudo copiar programa", "Porfavor contacte a soporte tecnico", "Aceptar");
                    return;
                }
                await dialog.setTitle("Copiando " + Programs.Tester);
                if (!Shell.CopyPCToFlash(comunication, Programs.Tester, "\\Assets\\Data\\"))
                {
                    if (dialog != null)
                        await dialog.SetwithButton("Error: No se pudo copiar programa", "Porfavor contacte a soporte tecnico", "Aceptar");
                    return;
                }
                if (dialog != null)
                    await dialog.setTitle("Ejecutando programa para prueba");

                if (!Shell.ExecuteFile(comunication, limit, Programs.Tester))
                {
                    if (dialog != null)
                        await dialog.SetwithButton("Error: No se pudo ejecutar programa " + Programs.Tester, "Porfavor contacte a soporte tecnico", "Aceptar");
                }
            }
            if (dialog != null)
               await dialog.set("Exito", "Programa "+ Programs.Tester + "esta ejecutandose", 1500);
            
            //check if test is working if not execute the respective to copy to nfc then execute test.cj4
        }
        public static async Task SetupTest(ComunicationManager comunication, String program, ILockScreen dialog)
        {
            if (dialog != null)
                await dialog.Show("Reiniciando Dispositivo...");
            //if (!comunication.IsReady())
                //ComunicationManager.PutTaskDelay(1000);
            if (dialog != null)
                await dialog.setTitle("Accesando a Remote Shell...");
            comunication.SendCommand(Key.Reset);
            //await Task.Delay(1000);
            if (!Shell.RemoteShellAccess(comunication, limit))
            {
                if (dialog != null)
                    await dialog.SetwithButton("Error:", "Porfavor utiliza un dispositivo actualizado, si su dispositivo lo esta contacte a soporte tecnico", "Aceptar");
                return;
            }
            if (dialog != null)
                await dialog.setTitle("Accesando a Archivos");
            if (!Shell.CdToFiles(comunication, limit))
            {
                if (dialog != null)
                    await dialog.SetwithButton("Error: Acceso a archivos no obtenido", "Porfavor utiliza un dispositivo actualizado, si su dispositivo lo esta contacte a soporte tecnico", "Aceptar");
                return;
            }
            if (!Shell.ExecuteFile(comunication, limit, program))
            {
                if (dialog != null)
                    await dialog.setTitle("Copiando " + program);
                if (!Shell.CopyPCToFlash(comunication, program, "\\Assets\\Data\\"))
                {
                    if (dialog != null)
                        await dialog.SetwithButton("Error: No se pudo copiar programa", "Porfavor contacte a soporte tecnico", "Aceptar");
                    return;
                }
                if (!Shell.ExecuteFile(comunication, limit, program))
                {
                    if (dialog != null)
                        await dialog.SetwithButton("Error: No se pudo ejecutar programa " + program, "Porfavor contacte a soporte tecnico", "Aceptar");
                }
            }
            if (dialog != null)
                await dialog.set("Exito", "Programa " + program + " esta ejecutandose", 1500);
            
        }
    }
}
