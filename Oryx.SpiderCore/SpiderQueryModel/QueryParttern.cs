using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oryx.SpiderCore.SpiderQueryModel
{
    public class QueryParttern : ICloneable
    {
        public string LastUrl { get; set; }

        public string CurrentUrl { get; set; }

        public string PartternName { get; set; }

        public string NextUrlParttern { get; set; }

        public QueryParttern NextParttern { get; set; }

        public List<Parttern> QueryTarget { get; set; }

        public ConfigLoadMore LoadMore { get; set; }
        public string PageParameter { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class ConfigLoadMore
    {
        [JsonProperty("excuteTime")]
        public int ExcuteTime { get; set; }

        [JsonProperty("loedMorePartern")]
        public string LoadMoreParttern { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }

        [JsonProperty("watingTimeSeconds")]
        public int WatingTimeSeconds { get; set; }
    }

    public class Parttern
    {
        public string PartternName { get; set; }

        public List<string> Query { get; set; }

        public PartterContentType ContentType { get; set; }
    }

    public enum PartterContentType
    {
        Text,
        Html,
        Attribute,
        Content
    }
}
