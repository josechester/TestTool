using Injectoclean.Tools.Developers;
using Injectoclean.Tools.UserHelpers;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Injectoclean
{
    public partial class MainPage : Page
    {
        public const string FEATURE_NAME = "BLE";

        List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario() { Title="Discover servers", ClassType=typeof(DiscoverBleServer) }
         };

        public Log Log = new Log();
        public MessageScreen messageScreen = new MessageScreen();

        public class Scenario
        {
            public string Title { get; set; }
            public Type ClassType { get; set; }
        }
    }
}
