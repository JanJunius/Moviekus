using NLog.Config;
using System;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NullableDatePicker : ContentView
    {
        public NullableDatePicker()
        {
            InitializeComponent();

            DateFrom = DateTo = MoviekusDefines.MinDate;

            chkNotSet.CheckedChanged += (sender, args) =>
            {
                if (args.Value == true)
                {
                    DateFrom = DateTo = MoviekusDefines.MinDate;
                }                    
                else
                {
                    if (DateFrom == MoviekusDefines.MinDate) DateFrom = DateTime.Today;
                    if (DateTo == MoviekusDefines.MinDate) DateTo = DateTime.Today;
                }
                ShowRelevantControls();
            };
        }

        public static readonly BindableProperty ShowRangeProperty =
        BindableProperty.Create(
            propertyName: nameof(ShowRange),
            returnType: typeof(bool),
            declaringType: typeof(NullableDatePicker),
            propertyChanged: ShowRangePropertyChanged,
            defaultBindingMode: BindingMode.TwoWay);

        private static void ShowRangePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var ctrl = (NullableDatePicker)bindable;
            ctrl.ShowRange = (bool)newValue;
            ctrl.ShowRelevantControls();
        }

        public bool ShowRange
        {
            get { return (bool)GetValue(ShowRangeProperty); }
            set { SetValue(ShowRangeProperty, value); }
        }

        public static readonly BindableProperty DateFromProperty =
        BindableProperty.Create(
            propertyName: nameof(DateFrom),
            returnType: typeof(DateTime),
            declaringType: typeof(NullableDatePicker),
            defaultBindingMode: BindingMode.TwoWay);

        public DateTime DateFrom
        {
            get { return (DateTime)GetValue(DateFromProperty); }
            set 
            { 
                SetValue(DateFromProperty, value);
            }
        }

        public static readonly BindableProperty DateToProperty =
        BindableProperty.Create(
            propertyName: nameof(DateTo),
            returnType: typeof(DateTime),
            declaringType: typeof(NullableDatePicker),
            defaultBindingMode: BindingMode.TwoWay);

        public DateTime DateTo
        {
            get { return (DateTime)GetValue(DateToProperty); }
            set { SetValue(DateToProperty, value); }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == DateFromProperty.PropertyName)
            {
                datePickerFrom.Date = DateFrom;
                ShowRelevantControls();

                if (DateFrom != DateTime.MinValue)
                    chkNotSet.IsChecked = DateFrom == MoviekusDefines.MinDate;
            }
            if (propertyName == DateToProperty.PropertyName)
            {
                datePickerTo.Date = DateTo;
                ShowRelevantControls();
            }

        }

        private void ShowRelevantControls()
        {
            if (DateFrom != MoviekusDefines.MinDate)
            {
                datePickerFrom.IsVisible = true;
                datePickerTo.IsVisible = lblTo.IsVisible = ShowRange;
            }
            else datePickerFrom.IsVisible = datePickerTo.IsVisible = lblTo.IsVisible = false;
            
            lblNotSet.Text = DateFrom == MoviekusDefines.MinDate ? "Nicht gesetzt" : "Zurücksetzen";
        }

        private void OnDateFromSelected(object sender, DateChangedEventArgs args)
        {
            DateFrom = args.NewDate;
        }
 
        private void OnDateToSelected(object sender, DateChangedEventArgs args)
        {
            DateTo = args.NewDate;
        }

    }
}