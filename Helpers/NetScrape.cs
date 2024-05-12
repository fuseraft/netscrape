using System.Net;
using HtmlAgilityPack;
using Netscrape.Services.Interfaces;

namespace Netscrape.Helpers;

public class NetScrape
{
    private readonly IHttpClientFacade _httpClientFacade;

    public NetScrape(IHttpClientFacade httpClientFacade)
    {
        _httpClientFacade = httpClientFacade;
    }

    public async Task<HtmlDocument?> LoadHtmlFromUrlAsync(string url)
    {
        try
        {
            string htmlContent = await _httpClientFacade.GetAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            return htmlDocument;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching or parsing HTML: {ex.Message}");
            return null;
        }
    }

    public void PrintAllLinks(HtmlDocument? htmlDocument)
    {
        if (htmlDocument == null)
        {
            Console.WriteLine("No HTML document provided.");
            return;
        }

        var nodes = htmlDocument.DocumentNode.SelectNodes("//a");
        if (nodes != null)
        {
            HashSet<string> visited = new();
            foreach (var node in nodes)
            {
                var href = NormalizeHref(node.GetAttributeValue("href", "#"));
                PrintLinkInfo(node, href, visited);
            }
        }
        else
        {
            Console.WriteLine("No links found.");
        }
    }

    private void PrintLinkInfo(HtmlNode node, string href, HashSet<string> visited)
    {
        var innerText = WebUtility.HtmlDecode(node.InnerText.Trim());
        if (string.IsNullOrEmpty(innerText) || string.IsNullOrEmpty(href))
        {
            return;
        }

        var domain = ExtractDomain(href);
        if (visited.Contains(domain))
        {
            return;
        }

        visited.Add(domain);

        if (innerText.Contains(domain))
        {
            innerText = innerText[..innerText.IndexOf(domain)];
        }
        
        var blurb = WebUtility.HtmlDecode(node.ParentNode.ParentNode.LastChild.InnerText.Trim());
        Console.WriteLine(new string('-', 62));
        Console.WriteLine($"\ntitle: {innerText}\nurl:   {href}\n");
        Console.WriteLine($"{WordWrap.Wrap(blurb, 60)}\n");
    }

    private string NormalizeHref(string href)
    {
        if (CanIgnore(href))
        {
            return string.Empty;
        }
        
        href = href.Replace("/url?q=", string.Empty);
        int endIndex = href.IndexOf("&amp;sa=");
        if (endIndex > 0)
        {
            href = href[..endIndex];
        }

        return href;
    }

    private string ExtractDomain(string url)
    {
        try
        {
            return new Uri(url).Host;
        }
        catch (UriFormatException)
        {
            return string.Empty;
        }
    }

    private bool CanIgnore(string href)
    {
        return href.StartsWith("/search?")
            || href.StartsWith("/?sa")
            || href.StartsWith("/setprefs?")
            || href.StartsWith("/advanced_search")
            || href.Contains("/imgres?")
            || href.StartsWith("/url?q=/")
            || href.Contains("maps.google.com")
            || href.Contains("policies.google.com")
            || href.Contains("google.com/preferences")
            || href.Contains("accounts.google.com")
            || href.Contains("support.google.com")
            || href.Contains("google.com/search")
            || !href.StartsWith("/url?q=");
    }
}