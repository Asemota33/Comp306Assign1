using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Lab1Comp306MichaelAsemota
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CreateBucket createBucket;
        public static MainWindow mainWindow;
        public static ObjectLevelOperations objectLevelOperations;

        protected override void OnStartup(StartupEventArgs e)
        {
            createBucket = new CreateBucket();
            mainWindow = new MainWindow();
            objectLevelOperations = new ObjectLevelOperations();
        }
    }
}
