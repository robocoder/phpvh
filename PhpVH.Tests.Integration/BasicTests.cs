using Components;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH.Tests.Integration
{
    [TestFixture(Category = "BasicTests")]
    public class BasicTests
    {
        private Lazy<TestSettings> _settings = new Lazy<TestSettings>(ResolveTestSettings);

        public TestSettings Settings
        {
            get { return _settings.Value; }
        }

        private static TestSettings ResolveTestSettings()
        {
            return ServiceLocator.Default.ResolveOrCreate<TestSettings>(TestSettings.Load);
        }

        [Test(Description = "Connectivity test"), Category("Connectivity")]
        public void TestConnectivity()
        {
            TcpClient client = null;

            try
            {
                client = new TcpClient();
                client.Connect(Settings.Host, Settings.Port);
                Assert.IsTrue(client.Connected, "TCP client not connected.");
            }
            finally
            {
                if (client != null)
                {
                    client.Close();                    
                }
            }            
        }

        [Test(Description = "HTTP test"), Category("HttpConnectivity")]
        public void TestHttp()
        {
            new WebClient().DownloadData("http://" + Settings.Host + ":" + Settings.Port);
        }

        [Test(Description = "Webroot test"), Category("Webroot")]
        public void TestWebroot()
        {
            Assert.IsTrue(
                Directory.Exists(Settings.Webroot), 
                "Could not find webroot directory {0}.", 
                Settings.Webroot);

            try
            {
                var tmpFile = Path.Combine(Settings.Webroot, Guid.NewGuid() + ".tmp");
                File.WriteAllText(tmpFile, "foo");
                File.Delete(tmpFile);
            }
            catch (IOException e)
            {
                var msg = string.Format("Could not create file in webroot \"{0}\".", Settings.Webroot);
                throw new AssertionException(msg, e);
            }

            var phpFiles = Directory
                .GetFiles(Settings.Webroot, "*", SearchOption.AllDirectories)
                .Where(x => Path.GetExtension(x).ToLower() == ".php");

            if (!phpFiles.Any())
            {
                var msg = string.Format("Could not find PHP files in webroot \"{0}\".", Settings.Webroot);
                throw new AssertionException(msg);
            }
        }

        [Test(Description = "PHP test"), Category("PhpTest")]
        public void TestPhp()
        {
            var versionTester = new PhpVersionTester(Settings.ToScanConfig()) { ThrowOnFail = true };
            versionTester.CheckVersion();
        }
    }
}
