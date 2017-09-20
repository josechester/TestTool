﻿using SDKTemplate.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static SDKTemplate.GattAttributes.InmediateAlert;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace SDKTemplate
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Scenario_Consol : Page
    {
        private MainPage rootPage;
        RemoteShell comunication;
        private static AutoResetEvent resetEvent = new AutoResetEvent(false);
        public Scenario_Consol()
        {
            
            this.InitializeComponent();
            rootPage = MainPage.Current;
            comunication = new RemoteShell(rootPage);
            comunication.SetupCJ4();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            
        }

        
    }
}
