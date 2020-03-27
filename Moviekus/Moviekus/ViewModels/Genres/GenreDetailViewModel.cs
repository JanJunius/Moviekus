﻿using Acr.UserDialogs;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.ViewModels;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Genres
{
    public class GenreDetailViewModel : BaseViewModel
    {
        private IService<Genre> GenreService;

        public Genre Genre { get; set; }

        public GenreDetailViewModel(GenreService genreService)
        {
            GenreService = genreService;
        }

        public ICommand SaveCommand => new Command(async () =>
        {
            await GenreService.SaveChangesAsync(Genre);
            await Navigation.PopAsync();
        });

        public ICommand DeleteCommand => new Command(async () =>
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
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

    }
}