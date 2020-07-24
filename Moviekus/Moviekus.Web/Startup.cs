using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moviekus.EntityFramework;
using Moviekus.ServiceContracts;
using Moviekus.Services;

namespace Moviekus.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Die Options sind nur notwendig, um die Startseite auf den Movies-Ordner umzubiegen
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Movies/Index", "");
            });

            // Blazor wird in der RatingComponent verwendet
            services.AddServerSideBlazor();

            services.AddDbContext<MoviekusDbContext>();

            // Inversion of Control
            // Die Services werden nie direkt angesprochen, sondern immer über die zugehörigen Interfaces.
            // Die Zuordnung wer was implementiert erfolgt hier.
            // Eine Komponente wie z.B. ein PageModel definiert einen Konstruktor mit dem Interfacetyp, den sie benötigt
            // und erhält diesen dann abhängig von dieser Konfiguration geliefert.
            // Auf diese Weise kann man hier eine Konfiguration vollständig ersetzen, indem man z.B. als implementierende
            // Klasse eine MOC-Version zum Testen angibt.
            services.AddSingleton(typeof(IGenreService), typeof(GenreService));
            services.AddSingleton(typeof(IFilterService), typeof(FilterService));
            services.AddSingleton(typeof(IMovieService), typeof(MovieService));
            services.AddSingleton(typeof(ISettingsService), typeof(SettingsService));
            services.AddSingleton(typeof(ISourceService), typeof(SourceService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
            });
        }
    }
}
