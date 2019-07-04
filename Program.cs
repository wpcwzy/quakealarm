using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using AngleSharp;
using Baidu.Aip.Speech;

namespace quakealarm
{
    class Program
    {
        static void alarm(string voiceContent)
        {
            Console.WriteLine(voiceContent);
            var API_KEY="3TOIOWY3Kxqj3qIpn2epvsX8";
            var SECRET_KEY="8ZUENcYUfR6E9mpthFwgcqm8TCoMvWTP";
            var client = new Tts(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间
            var option=new Dictionary<string,object>()
            {
                {"spd",4},
                {"vol",10}
            };
            var result=client.Synthesis(voiceContent,option);
            if(result.Success)
            {
            File.WriteAllBytes("voice.mp3",result.Data);
            Console.WriteLine("Playing voice");
            Process.Start(@"mplayer",@"voice.mp3");
            }
        }
        static void Main(string[] args)
        {
            //setup
            var lastTime="";
            var address="http://news.ceic.ac.cn/index.html";
            var config=Configuration.Default.WithDefaultLoader();
            while(true)
            {
                //get web document
                var document=BrowsingContext.New(config).OpenAsync(address);
                var table=document.Result.QuerySelectorAll("td");
                
                var length=Convert.ToDouble(table[0].InnerHtml);
                var time=table[1].InnerHtml;
                var pos=table[5].QuerySelector("a").InnerHtml;

                Console.WriteLine(DateTime.Now);

                if(time!=lastTime)
                {
                    Console.WriteLine("New earthquake");
                    lastTime=time;
                    if((pos.Contains("市") || pos.Contains("县"))&& length>=5.5)
                    {
                        alarm(string.Format("注意，中国地震台网在{0}时检测到{1}发生{2}级地震",time,pos,length));
                    }
                    else
                    {
                        alarm(string.Format("注意，中国地震台网在{0}时检测到{1}发生{2}级地震",time,pos,length));
                        Console.WriteLine("Skip");
                    }
                }
                else
                {
                    Console.WriteLine("Safe");
                }
                Console.WriteLine("==================");
                Thread.Sleep(10000);
            }
        }
    }
}