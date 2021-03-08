using Genshine_Gacha_Recorder_Win;
using Genshine_Gacha_Recorder_Win.Models;
using Genshine_Gacha_Recorder_Win.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Genshine_Gacha_Recorder_Win.ViewModels
{
    class GachaItemsViewModel
    {
        public readonly ViewModels.DataService GachaInfoModel;

        public readonly Dictionary<int, ObservableCollection<Models.GachaItemModel>> Info_Records;
        public readonly Dictionary<int, ObservableCollection<Models.GachaResultModel>> Info_Results;
        public readonly Dictionary<int, ObservableCollection<Models.Gacha5xItemModel>> Info_5x_Items;

        public bool IsOkToLoadData()
        {
            GachaInfoModel.UpdateBaseInfo();
            return GachaInfoModel.IsOk;
        }

        public GachaItemsViewModel()
        {
            GachaInfoModel = new ViewModels.DataService();

            GachaInfoModel.GetGachaInfoFromFile();
            GachaInfoModel.MergeGachaInfo();

            Info_Records = new Dictionary<int, ObservableCollection<Models.GachaItemModel>>();
            Info_Results = new Dictionary<int, ObservableCollection<Models.GachaResultModel>>();
            Info_5x_Items = new Dictionary<int, ObservableCollection<Models.Gacha5xItemModel>>();

            Save();

        }

        public void Update()
        {
            if (IsOkToLoadData())
            {
                GachaInfoModel.StatusMessage = "正在读取数据...";
                GachaInfoModel.GetGachaInfoFromFile();
                GachaInfoModel.GetGachaInfoFromWeb();
                GachaInfoModel.MergeGachaInfo();
                GachaInfoModel.WriteToFile();
                GachaInfoModel.StatusMessage = "数据更新时间:" + DateTime.Now.ToString();
            }
        }

        public void SaveOneType(int type)
        {
            if (!Info_Records.ContainsKey(type))
            {
                Info_Records[type] = new ObservableCollection<Models.GachaItemModel>();
            }
            else
            {
                Info_Records[type].Clear();
            }

            if (!Info_5x_Items.ContainsKey(type))
            {
                Info_5x_Items[type] = new ObservableCollection<Models.Gacha5xItemModel>();
            }
            else
            {
                Info_5x_Items[type].Clear();
            }

            if (!Info_Results.ContainsKey(type))
            {
                Info_Results[type] = new ObservableCollection<Models.GachaResultModel>
                {
                    new Models.GachaResultModel
                    {
                        Rank = 5,
                        Sum = 0,
                        DianLeJiFa = -1,
                        Probability = 0
                    },
                    new Models.GachaResultModel
                    {
                        Rank = 4,
                        Sum = 0,
                        DianLeJiFa = -1,
                        Probability = 0
                    },
                    new Models.GachaResultModel
                    {
                        Rank = 3,
                        Sum = 0,
                        DianLeJiFa = 0,
                        Probability = 0
                    }
                };
            }

            Info_Results[type][0].DianLeJiFa = -1;
            Info_Results[type][1].DianLeJiFa = -1;

            Info_Results[type][0].Sum = 0;
            Info_Results[type][1].Sum = 0;

            foreach (Models.GachaItemModel item in GachaInfoModel.GachaInfo[type])
            {
                ++Info_Results[type][0].DianLeJiFa;
                ++Info_Results[type][1].DianLeJiFa;

                // 五星
                if (item.Rank == Info_Results[type][0].Rank)
                {
                    Info_5x_Items[type].Add(new Gacha5xItemModel()
                    {
                        Interval = Info_Results[type][0].DianLeJiFa + 1,
                        Name = item.Name,
                        Time = item.Time
                    });

                    Info_Results[type][0].DianLeJiFa = -1;
                    Info_Results[type][0].Sum++;
                    Info_Results[type][0].Average = (double)item.Id / Info_Results[type][0].Sum;
                }
                // 四星
                else if (item.Rank == Info_Results[type][1].Rank)
                {
                    Info_Results[type][1].DianLeJiFa = -1;
                    Info_Results[type][1].Sum++;
                    Info_Results[type][1].Average = (double)item.Id / Info_Results[type][1].Sum;
                }


                Info_Records[type].Add(item);
            }

            if (Info_Records[type].Count > 0)
            {
                Info_Results[type][0].Probability = (double)Info_Results[type][0].Sum / Info_Records[type].Count;
                Info_Results[type][1].Probability = (double)Info_Results[type][1].Sum / Info_Records[type].Count;

                Info_Results[type][2].Sum = Info_Records[type].Count - Info_Results[type][0].Sum - Info_Results[type][1].Sum;
                Info_Results[type][2].Probability = 1 - Info_Results[type][0].Probability - Info_Results[type][1].Probability;
                Info_Results[type][2].Average = (double)Info_Records[type].Count / Info_Results[type][2].Sum;
            }
        }

        public void Save()
        {
            foreach (int type in ViewModels.DataService.GachaTypeIdToName.Keys)
            {
                SaveOneType(type);
            }
        }

    }
}
