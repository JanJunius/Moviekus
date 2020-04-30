﻿using System;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NullableDatePicker : ContentView
    {
        private bool InitializeNotSetCheckbox;

        public NullableDatePicker()
        {
            InitializeComponent();

            DateFrom = DateTo = MoviekusDefines.MinDate;

            // Die Checkbox muss beim Öffnen einmalig initialisiert werden
            InitializeNotSetCheckbox = true;

            chkNotSet.CheckedChanged += (sender, args) =>
            {
                if (args.Value == true)
                    DateFrom = DateTo = MoviekusDefines.MinDate;
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
            defaultBindingMode: BindingMode.OneWay);

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
            set { SetValue(DateFromProperty, value); }
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

                if (InitializeNotSetCheckbox && DateFrom != DateTime.MinValue)
                {
                    chkNotSet.IsChecked = DateFrom == MoviekusDefines.MinDate;
                    InitializeNotSetCheckbox = false;
                }
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
                datePickerTo.IsVisible = lblFrom.IsVisible = lblTo.IsVisible = ShowRange;
            }
            else datePickerFrom.IsVisible = datePickerTo.IsVisible = lblFrom.IsVisible = lblTo.IsVisible = false;
            
            lblNotSet.Text = DateFrom == MoviekusDefines.MinDate ? "Nicht gesetzt" : "Zurücksetzen";
        }
    }
}