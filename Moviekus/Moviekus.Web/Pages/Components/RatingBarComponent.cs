using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Moviekus.Web.Pages.Components
{
    /*
     * Beim Hinzufügen von Razor Components (Blazor) zu einem Razor Web Projekt sind einige Dinge zu beachten:
     * 1. Die Components müssen im Pages-Ordner liegen (oder einem Unterordner davon)
     * 2. In _Layout.cshtml muss im Header dieser Eintrag stehen: <base href="~/" />
     * 3. In _Layout.cshtml muss ganz unten folgender Eintrag stehen: <script src="_framework/blazor.server.js"></script>
     * 4. Unter dem Root-Ordner muss die Datei _Imports.razor angelegt werden mit dem Inhalt siehe Datei
     * 5. In Startup.ConfigureServices muss dieser Aufruf stehen: services.AddServerSideBlazor();
     * 6. In Startup.Configure muss dieser Aufruf stehen: endpoints.MapBlazorHub();
     * */
    public class RatingBarComponent : ComponentBase
    {
        [Parameter] public int Rating { get; set; }
        
        [Parameter] public bool Enabled { get; set; }

        public void OnStarClick(int starId)
        {
            if (!Enabled)
                return;

            if (Rating == starId)
                Rating = 0;
            else Rating = starId;
        }
    }
}
