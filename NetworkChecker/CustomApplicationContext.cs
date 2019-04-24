using System;
using System.ComponentModel;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;

namespace NetworkChecker
{
    class CustomApplicationContext : ApplicationContext
    {
        private const int PingInterval = 2000;

        private Container components;
        private NotifyIcon notifyIcon;
        private Timer timer;
        private readonly Icon greenIcon, yellowIcon, redIcon;

        public CustomApplicationContext()
        {
            this.greenIcon = LoadIcon("signal-green.ico");
            this.yellowIcon = LoadIcon("signal-yellow.ico");
            this.redIcon = LoadIcon("signal-red.ico");

            InitializeContext();
            this.timer.Enabled = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
        }

        private Icon LoadIcon(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string fullResourceName = $"{assembly.GetName().Name}.{resourceName}";
            using (var stream = assembly.GetManifestResourceStream(fullResourceName))
                return new Icon(stream);
        }

        private void InitializeContext()
        {
            this.components = new Container();
            this.notifyIcon = new NotifyIcon(this.components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Text = "Try ping...",
                Icon = redIcon,
                Visible = true
            };

            var exitItem = this.notifyIcon.ContextMenuStrip.Items.Add("Exit");
            exitItem.Click += (sender, e) => ExitThread();

            this.timer = new Timer(this.components)
            {
                Interval = PingInterval
            };
            this.timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (CanPing("www.google.com"))
            {
                this.notifyIcon.Text = "Ping via DNS succesful.";
                this.notifyIcon.Icon = greenIcon;
            }
            else if (CanPing("8.8.8.8"))
            {
                this.notifyIcon.Text = "Ping successful via IP-address only.";
                this.notifyIcon.Icon = yellowIcon;
            }
            else
            {
                this.notifyIcon.Text = "Ping failed.";
                this.notifyIcon.Icon = redIcon;
            }
        }

        private bool CanPing(string address)
        {
            try
            {
                return new Ping().Send(address).Status == IPStatus.Success;
            }
            catch (PingException)
            {
                return false;
            }
        }
    }
}
