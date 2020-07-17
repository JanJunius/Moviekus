using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviekus.Web
{
    public class Bootstrapper : Moviekus.Bootstrapper
    {
        public static void Init()
        {
            var instance = new Bootstrapper();
        }
    }
}
