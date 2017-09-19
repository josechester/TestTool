﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Security.Cryptography;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using static SDKTemplate.GattAttributes.InmediateAlert;

namespace SDKTemplate
{
    class Discover
    {
        private MainPage rootPage;
        private ObservableCollection<BluetoothLEDeviceDisplay> ResultCollection;
        private List<BluetoothLEAttributeDisplay> InmediateAlert = new List<BluetoothLEAttributeDisplay>();
        private DeviceWatcher deviceWatcher;
        private String id;
        //flags sync functions
        private bool findbyid = false;
        private bool watcheron = false;
        public Discover(MainPage rootpage)
        {
            this.rootPage = rootpage;
        }
        private static BluetoothLEDeviceDisplay Deviceinfo = null;
        public void Clear()
        {
            ResultCollection = null;
            findbyid = false;

        }
        private void stop()
        {
            if (watcheron == true)
            {
                StopBleDeviceWatcher();
                watcheron = false;
            }
        }

        private void start()
        {
            if (watcheron != true)
            {
                watcheron = true;
                StartBleDeviceWatcher();   
            }
        }
        public void Getnearlydevices(ref ObservableCollection<BluetoothLEDeviceDisplay> ResultCollection)
        {
            rootPage.NotifyUser("", NotifyType.StatusMessage);
            this.ResultCollection = ResultCollection;
            ResultCollection.Clear();
            findbyid = false;
            start();
          
        }
        public void GetService(String id)
        {
            rootPage.NotifyUser("", NotifyType.StatusMessage);
            ResultCollection = new ObservableCollection<BluetoothLEDeviceDisplay>();
            findbyid = true;
            char[] a = id.ToCharArray();
            switch (a.Length)
            {
                case 5:
                    this.id = ":0" + a[0] + ":" + a[1] + a[2] + ":" + a[3] + a[4];
                    break;
                case 6:
                    this.id = ":" + a[0] + a[1] + ":" + a[2] + a[3] + ":" + a[4] + a[5];
                    break;
                default:
                    this.id = id;
                    break;
            }
            if (Deviceinfo != null)
            {
                if (Deviceinfo.Id.Contains(this.id) == true)
                {
                    CheckConnectionAsync();
                    return;
                }
                else
                    Deviceinfo = null;
            }
            start();
                

        }
        public void GetService(BluetoothLEDeviceDisplay deviceinfo)
        {
            //rootPage.NotifyUser("Connecting ...", NotifyType.StatusMessage);
           
            if (watcheron == true)
            {
                StopBleDeviceWatcher();
                watcheron = false;
            }
            if (deviceinfo == null)
                rootPage.NotifyUser("Exeption DeviceInfo is null", NotifyType.ErrorMessage);
            else
            {
                Deviceinfo = deviceinfo;
                CheckConnectionAsync();
            }
        }

       
        #region isAliveTemplate
        //just template
        /* private async void IsAlive()
         {
             BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(Deviceinfo.Id);
             GattDeviceServicesResult Gattservices = await bluetoothLeDevice.GetGattServicesAsync();
             BluetoothLEAttributeDisplay inmdiateAlert = null;
             var services = Gattservices.Services;
             int i = 0;
             foreach (GattDeviceService service in Gattservices.Services)
             {
                 if (i == 3)
                 {
                     var characteristics = await service.GetCharacteristicsAsync();
                     foreach (GattCharacteristic c in characteristics.Characteristics)
                     {
                         inmdiateAlert = new BluetoothLEAttributeDisplay(c);
                     }
                 }
                 i++;
             }
             var writeBuffer = CryptographicBuffer.DecodeFromHexString("06");

             try
             {
                 GattCommunicationStatus result = await inmdiateAlert.characteristic.WriteValueAsync(writeBuffer);

                 if (result == GattCommunicationStatus.Success)
                 {
                     //ok
                 }
                 else
                 {
                     //false
                 }
             }
             catch (Exception ex) when ((uint)ex.HResult == 0x80650003 || (uint)ex.HResult == 0x80070005)
             {

             }
         }*/
        #endregion
        private async void CheckConnectionAsync()
        {
            Tools.MessageScreen dialog = new Tools.MessageScreen("Connecting");
            dialog.Show();
            if (Deviceinfo.IsPaired==false)
                    await Deviceinfo.DeviceInformation.Pairing.PairAsync();
            BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(Deviceinfo.Id);
            GattDeviceServicesResult Gattservices = await bluetoothLeDevice.GetGattServicesAsync();
            BluetoothLEAttributeDisplay inmdiateAlert = null;
            var services = Gattservices.Services;
            int i = 0;
            
            foreach (GattDeviceService service in Gattservices.Services)
            {
                if (service.Uuid==GattAttributes.InmediateAlert.guid)
                {
                    var characteristics = await service.GetCharacteristicsAsync();
                    foreach (GattCharacteristic c in characteristics.Characteristics)
                        inmdiateAlert = new BluetoothLEAttributeDisplay(c);
                }
                i++;
            }
            dialog.Close();
            try
            {
                GattCommunicationStatus result = await inmdiateAlert.characteristic.WriteValueAsync(Key.Esc);
                dialog.Close();
                if ( result == GattCommunicationStatus.Success)
                {
                    rootPage.NotifyUser("Device Connected", NotifyType.StatusMessage);
                    rootPage.Deviceinfo = Deviceinfo;
                }
                else
                {
                    rootPage.NotifyUser("Device Could't get connection", NotifyType.ErrorMessage);
                    rootPage.Deviceinfo = null;
                    Deviceinfo = null;
                }
            }
            catch (Exception ex) when ((uint)ex.HResult == 0x80650003 || (uint)ex.HResult == 0x80070005)
            {
                rootPage.NotifyUser(ex.Message.ToString(), NotifyType.ErrorMessage);
            }
            if (watcheron == true)
            {
                 StopBleDeviceWatcher();
                 watcheron = false;
            }
         }
         #region Device discovery
         
         private void StopBleDeviceWatcher()
         {
             if (deviceWatcher != null)
             {
                 // Unregister the event handlers.
                 deviceWatcher.Added -= DeviceWatcher_Added;
                 deviceWatcher.Updated -= DeviceWatcher_Updated;
                 deviceWatcher.Removed -= DeviceWatcher_Removed;
                 deviceWatcher.EnumerationCompleted -= DeviceWatcher_EnumerationCompleted;
                 deviceWatcher.Stopped -= DeviceWatcher_Stopped;

                 // Stop the watcher.
                 deviceWatcher.Stop();
                 deviceWatcher = null;
             }

         }

         /// <summary>
         ///     Starts a device watcher that looks for all nearby BT devices (paired or unpaired). Attaches event handlers and
         ///     populates the collection of devices.
         /// </summary>
         private void StartBleDeviceWatcher()
         {
             // Additional properties we would like about the device.
             string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

             // BT_Code: Currently Bluetooth APIs don't provide a selector to get ALL devices that are both paired and non-paired.
             deviceWatcher =
                     DeviceInformation.CreateWatcher(
                         "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")",
                         requestedProperties,
                         DeviceInformationKind.AssociationEndpoint);

             // Register event handlers before starting the watcher.
             deviceWatcher.Added += DeviceWatcher_Added;
             deviceWatcher.Updated += DeviceWatcher_Updated;
             deviceWatcher.Removed += DeviceWatcher_Removed;
             deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
             deviceWatcher.Stopped += DeviceWatcher_Stopped;

             // Start over with an empty collection.
             ResultCollection.Clear();

             // Start the watcher.
             deviceWatcher.Start();
         }

         private BluetoothLEDeviceDisplay FindBluetoothLEDeviceDisplay(string id)
         {
             foreach (BluetoothLEDeviceDisplay bleDeviceDisplay in ResultCollection)
             {
                 if (bleDeviceDisplay.Id == id)
                 {
                     return bleDeviceDisplay;
                 }
             }
             return null;
         }
        private BluetoothLEDeviceDisplay FindBluetoothLEbyPartofId(string id)
        {
            String temp;
            foreach (BluetoothLEDeviceDisplay bleDeviceDisplay in ResultCollection)
            {
                temp = bleDeviceDisplay.Id;
                if (temp.Contains(id) == true)
                {
                    rootPage.NotifyUser("Device Finded,Please wait...", NotifyType.StatusMessage);
                    return bleDeviceDisplay;
                }
            }
            return null;
        }

        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
         {
             // We must update the collection on the UI thread because the collection is databound to a UI element.
             await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                 if (sender == deviceWatcher)
                 {
                     // Make sure device name isn't blank or already present in the list.
                     if (deviceInfo.Name != string.Empty && FindBluetoothLEDeviceDisplay(deviceInfo.Id) == null)
                     {
                         ResultCollection.Add(new BluetoothLEDeviceDisplay(deviceInfo));
                         //to make quickly the conection
                         if (findbyid == true && deviceInfo.Id.Contains(this.id) == true)
                         {
                             Deviceinfo = FindBluetoothLEDeviceDisplay(deviceInfo.Id);
                             findbyid = false;
                             CheckConnectionAsync();
                         }

                     }
                 }
             });
         }

         private async void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
         {

             // We must update the collection on the UI thread because the collection is databound to a UI element.
             await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                 if (sender == deviceWatcher)
                 {
                     BluetoothLEDeviceDisplay bleDeviceDisplay = FindBluetoothLEDeviceDisplay(deviceInfoUpdate.Id);
                     if (bleDeviceDisplay != null)
                     {
                         bleDeviceDisplay.Update(deviceInfoUpdate);
                     }
                 }
             });
         }

         private async void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
         {
             // We must update the collection on the UI thread because the collection is databound to a UI element.
             await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                 if (sender == deviceWatcher)
                 {
                     // Find the corresponding DeviceInformation in the collection and remove it.
                     BluetoothLEDeviceDisplay bleDeviceDisplay = FindBluetoothLEDeviceDisplay(deviceInfoUpdate.Id);
                     if (bleDeviceDisplay != null)
                     {
                         ResultCollection.Remove(bleDeviceDisplay);
                     }
                 }
             });
         }

         private async void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object e)
         {
             // We must update the collection on the UI thread because the collection is databound to a UI element.
             await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
             {
                 // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                 if (sender == deviceWatcher)
                 {
                    /* rootPage.NotifyUser($"{ResultCollection.Count} devices found. Enumeration completed.",
                         NotifyType.StatusMessage);*/
                }
            });
        }

        private async void DeviceWatcher_Stopped(DeviceWatcher sender, object e)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Protect against race condition if the task runs after the app stopped the deviceWatcher.
                if (sender == deviceWatcher)
                {
                    /* rootPage.NotifyUser($"No longer watching for devices.",
                             sender.Status == DeviceWatcherStatus.Aborted ? NotifyType.ErrorMessage : NotifyType.StatusMessage);*/
                }
            });
        }
        #endregion  
    }
}
