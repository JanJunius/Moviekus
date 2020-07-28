using Acr.UserDialogs;
using Microsoft.Data.Sqlite;
using Moviekus.Models;
using Moviekus.ServiceContracts;
using Moviekus.Views.Validation;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels.Sources
{
    public class SourceDetailViewModel : BaseViewModel
    {
        public event EventHandler<Source> OnSourceChanged;

        private ISourceService SourceService;

        public Source Source { get; set; }

        public SourceDetailViewModel(ISourceService sourceService)
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

        public bool Validate()
        {
            return FormValidator.IsFormValid(Source, Navigation.NavigationStack.Last());
        }

        public async Task SaveChanges()
        {
            Source = await SourceService.SaveChangesAsync(Source);
        }

        public async Task UndoChanges()
        {
            // Zurücksetzen aller Änderungen wenn Eingaben ungültig und Page verlassen wird
            Source origionalSource = await Resolver.Resolve<ISourceService>().GetAsync(Source.Id);
            if (origionalSource != null)
            {
                Source = origionalSource;
                OnSourceChanged?.Invoke(this, Source);
            }
        }

    }
}
