using System;
using System.ComponentModel;

namespace Genshine_Gacha_Recorder_Win.Models
{
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
            return Time == other.Time && Rank == other.Rank && Name == other.Name && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals( obj as GachaItemModel);
        }

        public override int GetHashCode()
        {
            return time.GetHashCode() + name.GetHashCode() + rank.GetHashCode() + id.GetHashCode();
            throw new NotImplementedException();
        }
    }

}
