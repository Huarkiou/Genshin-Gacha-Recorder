﻿using System.ComponentModel;

namespace Genshine_Gacha_Recorder_Win.Models
{
    public class GachaResultModel : System.ComponentModel.INotifyPropertyChanged

    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int rank;
        public int Rank
        {
            get
            {
                return rank;
            }
            set
            {
                rank = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Rank)));
                }
            }
        }

        private int sum;
        public int Sum
        {
            get
            {
                return sum;
            }
            set
            {
                sum = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Sum)));
                }
            }
        }

        private double probability;
        public double Probability
        {
            get
            {
                return probability;
            }
            set
            {
                probability = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Probability)));
                }
            }
        }
    }
}