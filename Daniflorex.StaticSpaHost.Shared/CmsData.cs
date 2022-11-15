using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniflorex.StaticSpaHost.Shared
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Fields
    {
        public string title { get; set; }
        public string text { get; set; }
    }

    public class Root
    {
        public System system { get; set; }
        public Fields fields { get; set; }
    }

    public class System
    {
        public string id { get; set; }
        public string name { get; set; }
        public string urlSegment { get; set; }
        public string type { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime editedAt { get; set; }
        public string contentType { get; set; }
        public string locale { get; set; }
        public object url { get; set; }
    }
}
