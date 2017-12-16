using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oryx.SpiderCore
{
    public static class ThreadManager
    {
        public static Queue<Action> ThreadActionQueue = new Queue<Action>();
    }
}
