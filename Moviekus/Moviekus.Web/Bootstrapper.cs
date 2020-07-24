using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviekus.Web
{
    /// <summary>
    /// Der Bootstrapper initialisiert AutoFac
    /// AutoFac wird in allen Projekten als Inversion of Control Container verwendet
    /// Da die Services auch im Web-Projekt verwendet werden, muss AutoFac auch hier initialisiert werden,
    /// obwohl ASP seinen eigenen IoC-Container verwendet, siehe Startup
    /// TODO: Lösung finden, ob und wie man beide zusammenlegen kann
    /// </summary>
    public class Bootstrapper : Moviekus.Bootstrapper
    {
        public static void Init()
        {
            var instance = new Bootstrapper();
        }
    }
}
