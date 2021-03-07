using Genshine_Gacha_Recorder_Win.Utils;
using Genshine_Gacha_Recorder_Win.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Genshine_Gacha_Recorder_Win.ViewModels
{
    public class DataService
    {
        public static readonly Dictionary<int, string> GachaTypeIdToName = new Dictionary<int, string>
            {
                { 301, "角色活动祈愿" },
                { 302, "武器活动祈愿" },
                { 200, "常驻祈愿" },
                { 100, "新手祈愿" }
            };
        private static readonly string AppPath = AppDomain.CurrentDomain.BaseDirectory;

        private string GachaUrl;
        private string Uid;
        private readonly Dictionary<int, List<GachaItemModel>> GachaInfoNew;
        private readonly Dictionary<int, List<GachaItemModel>> GachaInfoOld;

        public bool IsOk { get; set; }

        public Dictionary<int, List<GachaItemModel>> GachaInfo
        {
            get
            {
                return GachaInfoNew;
            }
        }

        /// <summary>
        /// Get gacha name by id.
        /// </summary>
        /// <param name="id">gacha type id</param>
        /// <returns>gacha name</returns>
        public static string GetNameByTypeId(int id)
        {
            return GachaTypeIdToName[id];
        }

        public DataService()
        {
            UpdateBaseInfo();

            GachaInfoOld = new Dictionary<int, List<GachaItemModel>>();
            GachaInfoNew = new Dictionary<int, List<GachaItemModel>>();
            foreach (var key in GachaTypeIdToName.Keys)
            {
                GachaInfoOld.Add(key, new List<GachaItemModel>());
                GachaInfoNew.Add(key, new List<GachaItemModel>());
            }
            GetGachaInfoFromFile();
            MergeGachaInfo();
        }

        public void UpdateBaseInfo()
        {
            Uid = GetUid().Trim();

            string UrlPath = AppPath+ $"{Uid}" + $"\\url.txt";
            if(File.Exists(UrlPath))
            {
                GachaUrl = File.ReadAllText(UrlPath);
                GachaUrl = @"https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog?" + GachaUrl[GachaUrl.IndexOf("authkey_ver")..].Replace("#/", "").Replace("#/log", "");
                var response = Requests.Get(GachaUrl, Encoding.UTF8);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = response.GetResponseStream();
                    using StreamReader sr = new StreamReader(stream);
                    string jsonString = sr.ReadToEnd();

                    using JsonDocument rootDocument = JsonDocument.Parse(jsonString);
                    JsonElement retcodeElement = rootDocument.RootElement.GetProperty("retcode");
                    if(Convert.ToInt32(retcodeElement.ToString()) !=0)
                    {
                        GachaUrl = GetGachaRecordUrl();
                    }
                }
                else
                {
                    GachaUrl = GetGachaRecordUrl();
                }
            }
            else
            {
                GachaUrl = GetGachaRecordUrl();
            }

            File.WriteAllText(UrlPath, GachaUrl);

            if (GachaUrl == null || Uid == null)
            {
                IsOk = false;
            }
            else
            {
                IsOk = true;
            }
        }

        public void GetGachaInfoFromFile()
        {
            string path = AppPath + $"{Uid}";
            foreach (int GachaType in GachaTypeIdToName.Keys)
            {
                string GachaInfoFilePath = $"{path}\\{GachaType}.json";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(GachaInfoFilePath))
                {
                    string GachaInfoJsonString = File.ReadAllText(GachaInfoFilePath);
                    GachaInfoOld[GachaType].Clear();
                    GachaInfoOld[GachaType].AddRange(JsonSerializer.Deserialize<List<GachaItemModel>>(GachaInfoJsonString));
                }
            }
        }

        public void GetGachaInfoFromWeb()
        {

            foreach (int GachaType in GachaTypeIdToName.Keys)
            {
                GachaInfoNew[GachaType].Clear();
                GachaInfoNew[GachaType].AddRange(GetGachaInfo(GachaUrl, GachaType));
                GachaInfoNew[GachaType].Reverse();
            }
        }

        public void MergeGachaInfo()
        {
            foreach (int GachaType in GachaTypeIdToName.Keys)
            {
                List<GachaItemModel> GachaInfo = GachaInfoNew[GachaType];
                List<GachaItemModel> GachaInfoOrigin = GachaInfoOld[GachaType];

                if (GachaInfoOrigin.Count == 0)
                {
                    GachaInfoNew[GachaType].AddRange(GachaInfo);
                    continue;
                }
                if (GachaInfo.Count == 0)
                {
                    GachaInfoNew[GachaType].AddRange(GachaInfoOrigin);
                    continue;
                }

                int FirstOverlapIndex = GachaInfoOrigin.FindIndex((item) =>
                {
                    return GachaInfo[0].Equals(item);
                });

                if (FirstOverlapIndex > 0)
                {
                    GachaInfo.InsertRange(0, GachaInfoOrigin.GetRange(0, FirstOverlapIndex - 1));
                }
            }
        }

        public void WriteToFile()
        {
            string path = AppPath + $"{Uid}";
            foreach (int GachaType in GachaTypeIdToName.Keys)
            {
                string GachaInfoFilePath = $"{path}\\{GachaType}.json";
                string jsonString = JsonSerializer.Serialize(GachaInfoNew[GachaType], new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                //File.WriteAllText(GachaInfoFilePath, Regex.Unescape(jsonString), Encoding.Unicode);
                File.WriteAllText(GachaInfoFilePath, jsonString, Encoding.UTF8);
            }
        }


        private static List<GachaItemModel> GetGachaInfo(string GachaUrl, int GachaType)
        {
            var GachaRecords = new List<GachaItemModel>();
            int Size = 20;
            for (int Page = 1; ; ++Page)
            {
                List<GachaItemModel> items = GetGachaItems(GachaUrl, GachaType, Page, Size);
                if (items == null)
                {
                    throw new TimeoutException();
                }

                if (items.Count == 0)
                {
                    break;
                }

                GachaRecords.AddRange(items);

                if (Page % 5 == 0)
                {
                    System.Threading.Thread.Sleep(500);
                }
            }

            int id = GachaRecords.Count;
            GachaRecords.ForEach((item) =>
            {
                item.Id = id;
                --id;
            });
            return GachaRecords;
        }

        /// <summary>
        /// Get Size Gacha Items of Page as GachaItemModel and return as List<>
        /// </summary>
        /// <param name="GachaUrl">祈愿记录URL</param>
        /// <param name="GachaType">祈愿类型ID</param>
        /// <param name="Page">页码</param>
        /// <param name="Size">容量(1-20)</param>
        /// <returns>List contains gacha items</returns>
        private static List<GachaItemModel> GetGachaItems(string GachaUrl, int GachaType, int Page, int Size)
        {
            GachaUrl = $"{GachaUrl}&gacha_type={GachaType}&page={Page}&size={Size}";

            var response = Requests.Get(GachaUrl, Encoding.UTF8);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream stream = response.GetResponseStream();
                using StreamReader sr = new StreamReader(stream);
                string jsonString = sr.ReadToEnd();

                using JsonDocument rootDocument = JsonDocument.Parse(jsonString);
                JsonElement rootElement = rootDocument.RootElement;
                JsonElement dataElement = rootElement.GetProperty("data");
                JsonElement listElement = dataElement.GetProperty("list");

                List<GachaItemModel> ret = new List<GachaItemModel>();
                foreach (JsonElement item in listElement.EnumerateArray())
                {
                    ret.Add(new GachaItemModel()
                    {
                        Time = DateTime.Parse(item.GetProperty("time").ToString()),
                        Name = item.GetProperty("name").ToString(),
                        Rank = int.Parse(item.GetProperty("rank_type").ToString())
                    });
                }

                return ret;
            }

            return null;
        }

        /// <summary>
        /// 获取查看祈愿记录的URL
        /// </summary>
        /// <returns>祈愿记录URL</returns>
        private static string GetGachaRecordUrl()
        {
            const string Tag = "OnGetWebViewPageFinish:";
            string UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string GenshineLogPath = $"{UserPath}/AppData/LocalLow/miHoYo/原神/output_log.txt";
            string line = null;

            if (File.Exists(GenshineLogPath))
            {
                using FileStream fs = new FileStream(GenshineLogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith(Tag))
                    {
                        break;
                    }
                }
            }

            if (line == null)
            {
                line = null;
            }
            else
            {
                line = @"https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog?" + line[line.IndexOf("authkey_ver")..].Replace("#/", "").Replace("#/log", "");
            }
            return line;
        }

        private static string GetUid()
        {
            string UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string GenshineUidPath = $"{UserPath}/AppData/LocalLow/miHoYo/原神/UidInfo.txt";

            if (File.Exists(GenshineUidPath))
            {
                using FileStream fs = new FileStream(GenshineUidPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using StreamReader sr = new StreamReader(fs);
                return sr.ReadToEnd();
            }
            else
            {
                return null;
            }
        }

    }
}
