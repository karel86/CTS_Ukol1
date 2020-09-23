using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;

namespace Ukol1
{
    class Program
    {
        #region constants
        private static readonly string cBaseUrl = "https://www.cts-tradeit.cz";
        private static readonly string cDashboardUrl = "https://www.cts-tradeit.cz/kariera/";
        #endregion

        #region member variables

        #endregion

        #region property getters/setters

        #endregion

        #region constructors

        #endregion

        #region action methods
        static void Main(string[] args)
        {
            ConnectToCTSDashboard();
        }
        #endregion

        #region event handlers

        #endregion

        #region private member functions
        private static void ConnectToCTSDashboard()
        {
            var web = new HtmlWeb();

            Console.WriteLine($"Connecting to {cDashboardUrl}.");
            var doc = web.Load(cDashboardUrl);
            var pozice = doc.DocumentNode.SelectNodes("//a[contains(@class, 'card card-lg card-link-bottom')]");

            if (pozice == null)
            {
                Console.WriteLine("Pozměněna struktura webu. Hledání nebude pokračovat.");
                return;
            }

            foreach (var item in pozice)
            {
                var href = item.Attributes.FirstOrDefault(a => a.Name == "href");
                if (href != null)
                {
                    ConnectToCTSPosition($"{cBaseUrl}{href.Value}");
                }
            }

            Console.WriteLine("Done!");
        }

        private static void ConnectToCTSPosition(string url)
        {
            var web = new HtmlWeb();

            Console.WriteLine($"Connecting to position {url}.");
            var doc = web.Load(url);

            string positionName = doc.DocumentNode.SelectSingleNode("//h1")?.InnerText;
            string storyText = doc.DocumentNode.SelectSingleNode("//div[@class='story__text']")?.InnerText;

            if (positionName == null)
            {
                Console.WriteLine($"Nenalezen název pozice pro url {url}.");
                return;
            }

            if (storyText == null)
            {
                Console.WriteLine($"Nenalezen story text pro url {url}.");
                return;
            }

            positionName = positionName.Replace('/', '-');

            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\{positionName}.txt", storyText.Trim());

        }
        #endregion

    }
}
