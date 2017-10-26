using System.Configuration;
using System.Net;
using System.ServiceProcess;
using System.Timers;

namespace WindowsService
{
    public partial class PingService : ServiceBase
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Set up a timer to trigger every minute.  
            Timer timer = new Timer();
            var interval = ConfigurationManager.AppSettings["interval"];
            timer.Interval = int.Parse(interval); // 60 seconds  
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
            log.Info("Started");
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // do anything here, for example log something
             
            
            var urls = ConfigurationManager.AppSettings["urls"];

            if (!string.IsNullOrWhiteSpace(urls))
            {
                var urlList = urls.Split(',');
                using (var client = new WebClient())
                {
                    foreach (var url in urlList)
                    {
                        log.Info(url);
                        client.DownloadData(url);
                    }
                }
            }
        }

        protected override void OnStop()
        {
        }
    }
}
