using System;
using System.ComponentModel;

namespace Genshine_Gacha_Recorder_Win.Models
{
    public class Gacha5xItemModel:IComparable<GachaItemModel>, IEquatable<GachaItemModel>, System.ComponentModel.INotifyPropertyChanged
    {
        private DateTime time;
        private string name;
        private int interval;

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
        public int Interval
        {
            get
            {
                return interval;
            }
            set
            {
                interval = value; if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(GachaItemModel obj)
        {
            if (Time > obj.Time)
            {
                return -1;
            }
            else if (Time < obj.Time)
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
            if(other==null)
            {
                return false;
            }
            return Time == other.Time && Name == other.Name && Interval == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals( obj as GachaItemModel);
        }

        public override int GetHashCode()
        {
            return time.GetHashCode() + name.GetHashCode() + interval.GetHashCode();
        }
    }

}
