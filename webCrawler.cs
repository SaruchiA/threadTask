using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Http;
using System.Windows.Forms;
using System.Net.Http.HttpMessageInvoker;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace threadTask
{
    class webCrawler
   {
            static void Main(string[] args)
            {
                var crawler = new WebCrawler();
                crawler.Start();
            }

            public class WebCrawler
            {
                private readonly HttpClient client;
                private const string SiteUrl = "http://stackoverflow.com";
                private CrawlList crawlList;
                private const int MaxWorkers = 5;
                private int workers;

                public WebCrawler()
                {
                    client = new HttpClient();
                    crawlList = new CrawlList();
                }

                public void Start()
                {
                    crawlList = new CrawlList();
                    crawlList.AddUrl(SiteUrl);

                    do
                    {
                        if (workers >= MaxWorkers) continue;

                        if (!crawlList.HasNext()) continue;

                        Interlocked.Increment(ref workers);

                        Debug.Write("Workers " + workers);
                        ProcessUrl(crawlList.GetNext());                       

                    } while (crawlList.HasNext() || workers > 0);

                }

                private async void ProcessUrl(string url)
                {
                    Debug.Print("Processing " + url);
                    await client.GetAsync(url).ContinueWith(ProcessResponse);
                }

                private async void ProcessResponse(Task<HttpResponseMessage> response)
                {
                    Debug.Print("Processing response ");
                    var html = await response.Result.Content.ReadAsStringAsync();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var internalLinks = doc.DocumentNode.SelectNodes("//a[@href]").Where(x => x.Attributes["href"].Value.StartsWith("/")).Select(x => SiteUrl + x.Attributes["href"].Value).ToList();
                    internalLinks.ForEach(x => crawlList.AddUrl(x));

                    Interlocked.Decrement(ref workers);
                }
            }

            public class CrawlList
            {
                private readonly ConcurrentBag<string> urlsToCrawl;
                private readonly ConcurrentBag<string> urlsCompleted;

                public CrawlList()
                {
                    urlsToCrawl = new ConcurrentBag<string>();
                    urlsCompleted = new ConcurrentBag<string>();
                }

                public bool HasNext()
                {
                    return urlsToCrawl.Any();
                }

                public string GetNext()
                {
                    string url;
                    urlsToCrawl.TryTake(out url);
                    urlsCompleted.Add(url);
                    return url;
                }

                public void AddUrl(string url)
                {
                    if (!UrlAlreadyAdded(url))
                    {
                        urlsToCrawl.Add(url);
                        Debug.Print("Adding Url " + url);
                    }
                }

                public bool UrlAlreadyAdded(string url)
                {
                    return urlsToCrawl.Contains(url) || urlsCompleted.Contains(url);
                }
            }
        }
    
}
