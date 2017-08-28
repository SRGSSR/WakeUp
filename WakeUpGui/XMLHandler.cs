using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using System.ComponentModel;

namespace WakeUpGui
{
    class XMLHandler 
    {
        private string xmlUrl;
        private string path;
        private TimeSpan interval;
        private string lastdata = string.Empty;

        private WebClient webclient;
        private ILogger logger;
        private Dictionary<string, LocalProcess> processes;
        private CancellationTokenSource source;
        public delegate void ChangedEventHandler(object sender, EventArgs e);
        public event ChangedEventHandler Changed;

        private static readonly String defaultConfigValue = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><processes>	<process>		<name> notepad</name>		<fullname> Notepad</fullname>		<reason> Test purpose</reason>		<date> 25.04.2017 </date>	</process></processes>";

        //constructor
        public XMLHandler(string xmlUrl,string path, ILogger logger, int interval)
        {
            this.xmlUrl = xmlUrl;
            this.webclient = new WebClient();
            this.path = path;
            this.interval = TimeSpan.FromMinutes(interval);
            this.logger = logger;
            this.source = new CancellationTokenSource();
            //returns null if the found is not found
            this.lastdata = getLocalConfiguration();
            this.getConfiguredProcesses(lastdata);
        }

        //define that if this methode is called something has changed and the subscribers have to run their methode
        public void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }


        //cancels the tasks for the download of the xml
        public void Cancel()
        {
            source.Cancel();
        }

        //starts the task for download the xml file
        public void Start()
        {
            Task.Factory.StartNew(RunPeriodicallyAsync, source.Token);
        }

        //returns the proccess configured in the xml
        public Dictionary<string, LocalProcess> getProcess()
        {
            return processes;
        }

        //downloads the newest xml file from the webserver
        private string downloadConfiguredProcesses()
        {
            try
            {
                return webclient.DownloadString(xmlUrl);
            }

            catch (Exception ex)
            {
                logger.log("Download failed " + ex.Message);
                return "FAILED";
            }
        }

        //writes the data to a local configuration
        public void writeLocalConfiguration(string data)
        {
            try
            {
                File.WriteAllText(path,data);
            }

            catch (Exception ex)
            {
                logger.log("Save failed " + ex.Message);
            }
        }

        //reads the local file
        public string getLocalConfiguration()
        {
            try
            {
                return File.ReadAllText(path);
            }

            catch (System.IO.FileNotFoundException)
            {
                logger.log(path + " not found");
                return "";
            }

            catch (Exception ex)
            {
                logger.log("Read failed " + ex.Message);
                return "";
            }
        }

        //converts the xml file to a dictionary
        public void getConfiguredProcesses(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                xml = defaultConfigValue;
            }

            var processdictionary = new Dictionary<string, LocalProcess>();

            try
            {
                foreach (var p in XElement.Parse(xml).Elements("process"))
                {
                    var ap = new LocalProcess
                    (
                        p.Element("name").Value.Trim(),
                        p.Element("fullname").Value.Trim(),
                        p.Element("reason").Value.Trim(),
                        Convert.ToDateTime(p.Element("date").Value.Trim())
                    );

                    if (!processdictionary.ContainsKey(ap.getName()))
                    {
                        processdictionary.Add(ap.getName(), ap);
                    }
                }

                processes = processdictionary;
            }

            catch (Exception ex)
            {
                logger.log("Parse failed " + ex.Message);
            }
        }

        private void getData()
        {
            var fromWeb = downloadConfiguredProcesses();

            //verify if the webserver didn't failed, and the data has changed and the xml is not empty
            getConfiguredProcesses(fromWeb);

            if ((!fromWeb.StartsWith(@"FAILED", StringComparison.Ordinal)) && fromWeb != lastdata)
            {
                writeLocalConfiguration(fromWeb);
                lastdata = fromWeb;
                OnChanged(EventArgs.Empty);
            }

        }

        //tasks which runs periodically and downloads the file from the webserver
        public async Task RunPeriodicallyAsync()
        {
            while (!(source.Token.IsCancellationRequested))
            {
                getData();

                try
                {
                    await Task.Delay(interval, source.Token);
                }

                catch (Exception ex)
                {
                    logger.log("Failed to delay " + ex.Message);
                }
            }
        }
    }
}
