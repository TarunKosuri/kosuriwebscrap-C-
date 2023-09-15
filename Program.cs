using System;
using System.Linq;
using System.Net.Http.Headers;
using ConsoleApp1;
using System.Xml.Linq;
using HtmlAgilityPack;
using OpenQA.Selenium;

namespace FlipkartWebScrape
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.flipkart.com/search?q=books&otracker=search&otracker1=search&marketplace=FLIPKART&as-show=on&as=off";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            var productNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, '_4ddWXP')]");
            List<ProductInfo> items = new List<ProductInfo>();
            if (productNodes != null)
            {
                       foreach (var productNode in productNodes)
                        {
                        string productName = productNode.SelectSingleNode(".//a[contains(@class, 's1Q9rs')]").InnerText;
                        string price = productNode.SelectSingleNode(".//div[contains(@class, '_30jeq3')]").InnerText;
                        var offerNode = productNode.SelectSingleNode(".//div[contains(@class, '_3Ay6Sb')]");
                        string offer = offerNode != null ? offerNode.InnerText : "offer not applicable";
                        var ratingNode = productNode.SelectSingleNode(".//div[contains(@class, '_3LWZlK')]");
                        string rating = ratingNode != null ? ratingNode.InnerText : "NA";
                        items.Add(new ProductInfo { productName = productName, price = price, offer = offer, rating = rating });
                        if (!string.IsNullOrWhiteSpace(productName) && !string.IsNullOrWhiteSpace(price) && !string.IsNullOrWhiteSpace(offer) && !string.IsNullOrWhiteSpace(rating))
                        {
                            Console.WriteLine("Product Name: " + productName.Trim());
                            Console.WriteLine("Price: " + price.Trim());
                            Console.WriteLine("offer:" + offer.Trim());
                            Console.WriteLine("rating" + rating.Trim());
                            Console.WriteLine();
                        }
                    }
            }
            else
            {
                Console.WriteLine("No products found.");
            }
            ExportToCSV(items);
        }
        static void ExportToCSV(List<ProductInfo> items)
        {
            var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };
            using (var writer = new System.IO.StreamWriter("C:\\Users\\iray\\Desktop\\New folder (2)\\items1.csv"))
            using (var csv = new CsvHelper.CsvWriter(writer, config))
            {
                csv.WriteRecords(items);
            }
        }
    }  
}    

