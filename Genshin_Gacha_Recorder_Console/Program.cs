using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Genshine_Gacha_Recorder
{
    public class Program
    {
        static void Main(string[] args)
        {
            string GachaUrl;
            GachaUrl = GetGachaRecordUrl();

            Console.WriteLine("读取数据中...");

            var d = new Dictionary<int, string>
            {
                { 301, "角色活动祈愿" },
                { 302, "武器活动祈愿" },
                { 200, "常驻祈愿" },
                { 100, "新手祈愿" }
            };

            string uid = GetUid();
            string path = System.AppDomain.CurrentDomain.BaseDirectory + $"{uid}".Trim();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            foreach (int GachaType in d.Keys)
            {
                string GachaInfoFilePath = $"{path}\\{GachaType}.json";

                List<GachaItem> GachaInfo = GetGachaInfo(GachaUrl, GachaType);
                GachaInfo.Reverse();

                // 读取本地数据并合并
                if (File.Exists(GachaInfoFilePath))
                {
                    string GachaInfoJsonString = File.ReadAllText(GachaInfoFilePath);
                    List<GachaItem> GachaInfoOrigin = JsonSerializer.Deserialize<List<GachaItem>>(GachaInfoJsonString);

                    int LastOverlapIndex = GachaInfo.FindIndex((item) =>
                    {
                        return GachaInfoOrigin[^1].Equals(item);
                    });

                    GachaInfo.RemoveRange(0, LastOverlapIndex + 1);

                    GachaInfoOrigin.AddRange(GachaInfo);
                    GachaInfo = GachaInfoOrigin;
                }

                string jsonString = JsonSerializer.Serialize(GachaInfo, options);
                File.WriteAllText(GachaInfoFilePath,Regex.Unescape(jsonString),Encoding.Unicode);

                DisplayResult(d[GachaType], GachaInfo);
            }

            Console.ReadKey();
        }

        public static void DisplayResult(string type, List<GachaItem> items)
        {
            int sum_5x=0, sum_4x= 0;
            int interval = 1;

            Console.WriteLine("\n" + type + ":\n");

            foreach(var item in items)
            {
                if (item.rank == 5)
                {
                    Console.Write($"{item.name}({interval}) ");
                    interval = 1;

                    ++sum_5x;
                }

                if (item.rank == 4)
                {
                    ++sum_4x;
                }

                ++interval;
            }

            int total = items.Count;
            Console.WriteLine($"\n5星:{sum_5x * 100.0 / total:F2}%");
            Console.WriteLine($"4星:{sum_4x * 100.0 / total:F2}%");
            Console.WriteLine($"共计抽数:{total} 五星数:{sum_5x} 四星数:{sum_4x}");
        }

        public static string GetUid()
        {
            string UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string GenshineUidPath = $"{UserPath}/AppData/LocalLow/miHoYo/原神/UidInfo.txt";

            using FileStream fs = new FileStream(GenshineUidPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader sr = new StreamReader(fs);
            return sr.ReadToEnd();
        }

        public static List<GachaItem> GetGachaInfo(string GachaUrl, int GachaType)
        {
            var GachaRecords = new List<GachaItem>();
            int Size = 6;
            for (int Page = 1; ; ++Page)
            {
                GachaListItem[] items = GetGachaListItems(GachaUrl, GachaType, Page, Size);
                if(items.Length== 0)
                {
                    break;
                }

                foreach(var item in items)
                {
                    GachaRecords.Add(new GachaItem() { time = DateTime.Parse(item.time), rank = int.Parse(item.rank_type), name = item.name });
                    //Console.WriteLine($"{item.time} {item.rank_type}星 {item.name}");
                }

                if (Page%10==0)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }

            int id = GachaRecords.Count;
            GachaRecords.ForEach((item) =>
            {
                item.id = id;
                --id;
            });
            return GachaRecords;
        }

        public static GachaListItem[] GetGachaListItems(string GachaUrl, int GachaType, int Page, int Size)
        {
            GachaUrl = $"{GachaUrl}&gacha_type={GachaType}&page={Page}&size={Size}";

            var response = Requests.Get(GachaUrl, Encoding.UTF8);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream stream = response.GetResponseStream();
                using StreamReader sr = new StreamReader(stream);
                string jsonString = sr.ReadToEnd();
                GachaJson js = JsonSerializer.Deserialize<GachaJson>(jsonString);
                return js.data.list;
            }

            return null;
        }

        public static string GetGachaRecordUrl()
        {
            const string Tag = "OnGetWebViewPageFinish:";
            string UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string GenshineLogPath = $"{UserPath}/AppData/LocalLow/miHoYo/原神/output_log.txt";
            string line = null;

            while (line == null)
            {
                try
                {
                    using FileStream fs = new FileStream(GenshineLogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith(Tag))
                        {
                            break;
                        }
                    }
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("请打开祈愿历史记录记录界面!");
                System.Threading.Thread.Sleep(1000);
            }

            line = @"https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog?" + line[line.IndexOf("authkey_ver")..].Replace("#/log", "");
            return line;
        }

    }
}
