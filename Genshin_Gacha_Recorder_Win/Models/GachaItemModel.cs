using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Genshine_Gacha_Recorder_Win.Models
{
    public class GachaResponseItem
    {
        public string uid { get; set; }
        public string gacha_type{ get; set; }
        public string item_id{ get; set; }
        public string count{ get; set; }
        public string time{ get; set; }
        public string name{ get; set; }
        public string lang{ get; set; }
        public string item_type{ get; set; }
        public string rank_type{ get; set; }
    }
    public class GachaResponseData
    {
        public string page { get; set; }
        public string size { get; set; }
        public string total { get; set; }
        public GachaResponseItem[] list { get; set; }
        public string region { get; set; }
    }
    public class GachaResponse
    {
        public int retcode { get; set; }
        public string message { get; set; }
        public GachaResponseData data { get; set; }
    }

    public class GachaItemModel:IComparable<GachaItemModel>, IEquatable<GachaItemModel>, System.ComponentModel.INotifyPropertyChanged
    {
        private DateTime time;
        private int rank;
        private string name;
        private int id;

        public DateTime Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
                }
            }
        }
        public int Rank
        {
            get { return rank; }
            set
            {
                rank = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
                }
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value; if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
                }
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value; if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(GachaItemModel obj)
        {
            if(Time>obj.Time)
            {
                return -1;
            }
            else if(Time<obj.Time)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public bool Equals(GachaItemModel other)
        {
            return Time == other.Time&& Rank==other.Rank && Name==other.Name && Id==other.Id;
        }

    }

}
