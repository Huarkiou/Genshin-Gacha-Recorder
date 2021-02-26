using Genshine_Gacha_Recorder_Win;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genshine_Gacha_Recorder_Win.ViewModels
{
    class GachaItemsViewModel
    {
        private readonly ViewModels.DataService GachaInfoModel;

        public readonly Dictionary<int,ObservableCollection<Models.GachaItemModel>> Info_Record;
        public readonly Dictionary<int,ObservableCollection<Models.GachaResultModel>> Info_Result;

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

            Info_Record= new Dictionary<int, ObservableCollection<Models.GachaItemModel>>();
            Info_Result = new Dictionary<int, ObservableCollection<Models.GachaResultModel>>();
            SaveToVM();

        }

        public void Update()
        {
            if (IsOkToLoadData())
            {
                GachaInfoModel.GetGachaInfoFromFile();
                GachaInfoModel.GetGachaInfoFromWeb();
                GachaInfoModel.MergeGachaInfo();
                GachaInfoModel.WriteToFile();
            }
        }

        public void SaveOnePondType(int type)
        {
            if (!Info_Record.ContainsKey(type))
            {
                Info_Record[type] = new ObservableCollection<Models.GachaItemModel>();
            }
            else
            {
                Info_Record[type].Clear();
            }

            if (!Info_Result.ContainsKey(type))
            {
                Info_Result[type] = new ObservableCollection<Models.GachaResultModel>
                {
                    new Models.GachaResultModel
                    {
                        Rank = 5,
                        Sum = 0,
                        Probability = 0
                    },
                    new Models.GachaResultModel
                    {
                        Rank = 4,
                        Sum = 0,
                        Probability = 0
                    }
                };
            }

            Info_Result[type][0].Sum = 0;
            Info_Result[type][1].Sum = 0;
            foreach (Models.GachaItemModel item in GachaInfoModel.GachaInfo[type])
            {
                if (item.Rank==Info_Result[type][0].Rank)
                {
                    Info_Result[type][0].Sum++;
                }
                else if (item.Rank==Info_Result[type][1].Rank)
                {
                    Info_Result[type][1].Sum++;
                }
                Info_Record[type].Add(item);
            }

            if (Info_Record[type].Count > 0)
            {
                Info_Result[type][0].Probability = (double)Info_Result[type][0].Sum / Info_Record[type].Count;
                Info_Result[type][1].Probability = (double)Info_Result[type][1].Sum / Info_Record[type].Count;
            }
        }

        public void SaveToVM()
        {
            foreach(int type in ViewModels.DataService.GachaTypeIdToName.Keys)
            {
                SaveOnePondType(type);
            }
        }
    }
}
