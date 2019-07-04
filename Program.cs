using System;
using AngleSharp;

namespace quakealarm
{
    class Program
    {
        static void Main(string[] args)
        {
            //get web document
            var address="http://news.ceic.ac.cn/index.html";
            var config=Configuration.Default.WithDefaultLoader();
            var document=BrowsingContext.New(config).OpenAsync(address);
            var table=document.Result.QuerySelectorAll("td");
            
            var length=table[0].InnerHtml;
            var time=table[1].InnerHtml;
            var pos=table[5].QuerySelector("a").InnerHtml;

            
        }
    }
}
