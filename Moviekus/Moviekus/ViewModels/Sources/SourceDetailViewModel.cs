using Acr.UserDialogs;
using Microsoft.Data.Sqlite;
using Moviekus.Models;
using Moviekus.Services;
using Moviekus.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Sources
{
    public class SourceDetailViewModel : BaseViewModel
    {
        private IService<Source> SourceService;

        public Source Source { get; set; }

        public SourceDetailViewModel(SourceService sourceService)
        {
            SourceService = sourceService;
        }

        public ICommand DeleteCommand => new Command(async () =>
        {
            var result = await UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
            {
                Title = "Quelle löschen",
                Message = "Diese Quelle und ALLE verbundenen Filme wirklich löschen?",
                OkText = "Ja",
                CancelText = "Nein"
            });
            if (result)
            {
                try
                {
                    await SourceService.DeleteAsync(Source);
                }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Error(ex);
                    string errorMsg = ex.Message;
                    if (ex.InnerException != null && ex.InnerException is SqliteException)
                    {
                        SqliteException sqlException = ex.InnerException as SqliteException;
                        if (sqlException.SqliteErrorCode == 19)
                            errorMsg = "Quelle kann nicht gelöscht werden, da sie noch verwendet wird.";
                        else errorMsg = sqlException.Message;
                    }
                    await UserDialogs.Instance.AlertAsync(new AlertConfig
                    {
                        Title = "Quelle löschen",
                        Message = errorMsg
                    });
                }

                await Navigation.PopAsync();
            }
        });

        public override async void OnViewDisappearing()
        {
            base.OnViewDisappearing();

            await SourceService.SaveChangesAsync(Source);
            await Navigation.PopAsync();
        }
    }
}
