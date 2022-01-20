using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ConsoleApp1
{
    public class Class_Html
    {
        public Class_Html()
        {
            Main();
        }
        void Main()
        {
            var GetHtmlText = new HtmlWeb();
            //HtmlDocument cnblogs = GetHtmlText.Load("https://www.baidu.com", "GET");
            HtmlDocument cnblogs = GetHtmlText.Load("https://www.priest.ink/TestHtml/HtmlTest/FleckHrml.html", "GET");
            HtmlNodeCollection titleNodes = cnblogs.DocumentNode.SelectNodes("//a[@class='titlelnk']");
            var TitleList = cnblogs.DocumentNode.SelectNodes("//title");
            var TitleText = cnblogs.DocumentNode.SelectNodes("//title").FirstOrDefault()?.InnerText;
            foreach (var item in titleNodes)
            {
                Console.WriteLine(item.InnerText);
                Console.WriteLine(item.Attributes.FirstOrDefault(x => x.Name == "href")?.Value);
            }
        }

        public void BatchRequest(List<string> Urls,string invalidStr)
        {
            var GetHtmlText = new HtmlWeb();
            Urls.ForEach(url =>
            {
                HtmlDocument cnblogs = GetHtmlText.Load(url, "GET");
                var TitleText = cnblogs.DocumentNode.SelectNodes("//title").FirstOrDefault()?.InnerText;
                if (cnblogs.DocumentNode.InnerHtml.Contains(invalidStr))
                {
                    Console.WriteLine($"{url}》HtmlTitle:{TitleText}");
                }
            });
        }
    }
}
