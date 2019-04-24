using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Windows.Forms;

namespace NetworkChecker
{
    class Program : WindowsFormsApplicationBase
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Program().Run(args);
        }

        private Program()
        {
            IsSingleInstance = true;
        }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            EnableVisualStyles = true;
            Application.Run(new CustomApplicationContext());
            return false;
        }
    }
}
