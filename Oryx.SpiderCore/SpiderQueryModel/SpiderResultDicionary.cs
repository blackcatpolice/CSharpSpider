using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oryx.SpiderCore.SpiderQueryModel
{
    public class SpiderResultDicionary
    {
        public string QueryName { get; set; }

        public List<SpiderQueryResult> QueryResult { get; set; }
    }
}
