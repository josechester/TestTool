using Injectoclean.Tools.Developers;
using Injectoclean.Tools.UserHelpers;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Injectoclean.Views;

namespace Injectoclean
{
    public partial class MainPage : Page
    {
        public const string FEATURE_NAME = "MENU";

        List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario() { Title="Inicio", ClassType=typeof(DiscoverBleServer) },
            new Scenario() { Title="Configuracion", ClassType=typeof(Config) }
        };

        public Log Log = new Log();
        public MessageScreen messageScreen => new MessageScreen();

        public class Scenario
        {
            public string Title { get; set; }
            public Type ClassType { get; set; }
        }
    }
}
