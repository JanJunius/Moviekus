using NLog;
using NLog.Targets;
using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moviekus.ViewModels
{
    public class LogViewerViewModel : BaseViewModel
    {
        public string LogContent { get; private set; }

        public ICommand LoadLogCommand => new Command(() =>
        {
            var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("logfile");
            // Need to set timestamp here if filename uses date. 
            // For example - filename="${basedir}/logs/${shortdate}/trace.log"
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
            string fileName = fileTarget.FileName.Render(logEventInfo);
            if (File.Exists(fileName))
                LogContent = File.ReadAllText(fileName);
            else LogContent = $"Die Logdatei '{fileName}' enthält keine Einträge bzw. existiert nicht.";
            RaisePropertyChanged(nameof(LogContent));

        });
    }
}
