using EasyTabs;
using System;
using System.Windows.Forms;

namespace ABrowser
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppContainer container = new AppContainer();
            container.Tabs.Add(
               new TitleBarTab(container)
               {
                   Content = new frmMain
                   {
                       Text = "New Tab"
                   }
               }
                );
            container.SelectedTabIndex = 0;

            TitleBarTabsApplicationContext context = new TitleBarTabsApplicationContext();
            context.Start(container);

            Application.Run(context);
        }
    }
}
