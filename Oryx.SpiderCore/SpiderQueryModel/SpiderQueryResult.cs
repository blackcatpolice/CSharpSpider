using Oryx.SpiderCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oryx.SpiderCore.SpiderQueryModel
{
    public class SpiderQueryResult : ISpiderQueryResult
    {
        public string KeyName { get; set; }

        public string Path { get; set; }

        public List<string> QueryResult { get; set; }
    }
}
