using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Genshine_Gacha_Recorder
{
    public class GachaListItem
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
    public class GachaData
    {
        public string page { get; set; }
        public string size { get; set; }
        public string total { get; set; }
        public GachaListItem[] list { get; set; }
        public string region { get; set; }
    }
    public class GachaJson
    {
        public int retcode { get; set; }
        public string message { get; set; }
        public GachaData data { get; set; }
    }

    public class GachaItem:IComparable<GachaItem>, IEquatable<GachaItem>
    {
        public DateTime time { get; set; }
        public int rank { get; set; }
        public string name { get; set; }
        public int id { get;set;}

        public int CompareTo(GachaItem obj)
        {
            if(time>obj.time)
            {
                return -1;
            }
            else if(time<obj.time)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public bool Equals(GachaItem other)
        {
            return time == other.time&& rank==other.rank && name==other.name && id==other.id;
        }

    }

}
