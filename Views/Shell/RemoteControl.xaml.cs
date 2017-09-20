﻿using SDKTemplate.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace SDKTemplate.Views.Shell
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class RemoteControl : Page
    {
        private MainPage rootPage;
        RemoteShell comunication;
        public RemoteControl()
        {
            this.InitializeComponent();
            rootPage = MainPage.Current;
            comunication = new RemoteShell(rootPage);
       }

    private void Send_Click(object sender, RoutedEventArgs e)
    {
        comunication.SetupCJ4();
    }

    private void up_Click(object sender, RoutedEventArgs e)
    {
        comunication.RemoteKey(Key.Up);
    }

    private void left_Click(object sender, RoutedEventArgs e)
    {
        comunication.RemoteKey(Key.Left);
    }

    private void down_Click(object sender, RoutedEventArgs e)
    {
        comunication.RemoteKey(Key.Down);
    }

    private void right_Click(object sender, RoutedEventArgs e)
    {
        comunication.RemoteKey(Key.Right);
    }

    private void enter_Click(object sender, RoutedEventArgs e)
    {
        comunication.RemoteKey(Key.Enter);
    }

    private void reset_Click(object sender, RoutedEventArgs e)
    {
        comunication.RemoteKey(Key.Reset);
    }

    private void escape_Click(object sender, RoutedEventArgs e)
    {
        comunication.RemoteKey(Key.Esc);
    }

}
}
