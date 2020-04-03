using Autofac;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.Views.Movies;
using Moviekus.ViewModels;
using Moviekus.Views.Genres;
using Moviekus.Views.Sources;
using Moviekus.Views;

namespace Moviekus
{
    /*
     * The Bootstrapper will be inherited by each platform since this is where the execution of the app begins. 
     * This will also give us the option to add platform-specific configurations. 
     * To ensure that we inherit from the class, we define it as abstract.
    */
    public abstract class Bootstrapper
    {
        // The ContainerBuilder is a class defined in Autofac that takes care of creating the container 
        // for us after we are finished with the configuration.
        protected ContainerBuilder ContainerBuilder { get; private set; }

        public Bootstrapper()
        {
            Initialize();
            FinishInitialization();
        }

        /*
        The Initialize method scans the assembly for any types that inherit from the Page or ViewModel and adds them to the container.
        It also adds the TodoItemRepository as a singleton to the container.
        This means that each time we ask for a TodoItemRepository, we will get the same instance.
        The default behavior for Autofac(this may vary between libraries) is that we get a new instance for each resolution.
        */
        protected virtual void Initialize()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            ContainerBuilder = new ContainerBuilder();

            // Registriere alle Views (Page) und ViewModels (ViewModel)
            // Die Konstruktoren der jeweils registrierten Klassen definieren die Abhängigkeiten als Parameter
            // Bsp: public ItemView(ItemViewModel viewModel)
            // => Eine ItemView benötigt ein ItemViewModel            
            foreach (var type in currentAssembly.DefinedTypes
                      .Where(e =>
                             e.IsSubclassOf(typeof(Page)) ||
                             e.IsSubclassOf(typeof(BaseViewModel))))
            {
                ContainerBuilder.RegisterType(type.AsType());
            }
            ContainerBuilder.RegisterType<SourceService>().SingleInstance();
            ContainerBuilder.RegisterType<GenreService>().SingleInstance();
            ContainerBuilder.RegisterType<MovieService>().SingleInstance();
            ContainerBuilder.RegisterType<SettingsService>().SingleInstance();
        }

        private void FinishInitialization()
        {
            var container = ContainerBuilder.Build();
            Resolver.Initialize(container);
        }
    }
}
