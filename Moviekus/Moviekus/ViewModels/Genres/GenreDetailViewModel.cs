using Acr.UserDialogs;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using Moviekus.Views.Validation;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Genres
{
    public class GenreDetailViewModel : BaseViewModel
    {
        public event EventHandler<Genre> OnGenreChanged;

        private IGenreService GenreService;

        public Genre Genre { get; set; }

        public GenreDetailViewModel(IGenreService genreService)
        {
            GenreService = genreService;
        }

        public ICommand DeleteCommand => new Command(async () =>
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Title = "Genre löschen",
                Message = "Dieses Genre wirklich löschen?",
                OkText = "Ja",
                CancelText = "Nein"
            });
            if (result)
            {
                await GenreService.DeleteAsync(Genre);
                await Navigation.PopAsync();
            }
        });

        public bool Validate()
        {
            return FormValidator.IsFormValid(Genre, Navigation.NavigationStack.Last());
        }

        public async Task SaveChanges()
        {
            Genre = await GenreService.SaveChangesAsync(Genre);
        }

        public async Task UndoChanges()
        {
            // Zurücksetzen aller Änderungen wenn Eingaben ungültig und Page verlassen wird
            Genre origionalGenre = await Resolver.Resolve<IGenreService>().GetAsync(Genre.Id);
            if (origionalGenre != null)
            {
                Genre = origionalGenre;
                OnGenreChanged?.Invoke(this, Genre);
            }
        }

    }
}