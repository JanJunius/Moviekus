using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingBar : ContentView
    {
        private static string emptyStarImage = "star_empty.png";
        private static string fillStarImage = "star.png";

        private Image star1;
        private Image star2;
        private Image star3;
        private Image star4;
        private Image star5;

        private StackLayout StarContainer = new StackLayout() { Orientation = StackOrientation.Horizontal };

        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }

            set
            {
                if (Rating != value)
                    SetValue(RatingProperty, value);
            }
        }

        public static readonly BindableProperty RatingProperty = BindableProperty.Create(
              propertyName: "Rating",
              returnType: typeof(int),
              declaringType: typeof(RatingBar),
              defaultValue: -1,
              defaultBindingMode: BindingMode.TwoWay,
              propertyChanged: RatingPropertyChanged
            );

        public int StarSize
        {
            get { return (int)GetValue(StarSizeProperty); }
            set { SetValue(StarSizeProperty, value); }
        }

        public static readonly BindableProperty StarSizeProperty = BindableProperty.Create(
              propertyName: "StarSize",
              returnType: typeof(int),
              declaringType: typeof(RatingBar),
              defaultValue: 30,
              defaultBindingMode: BindingMode.OneWay,
              propertyChanged: StarSizePropertyChanged
            );

        public RatingBar()
        {
            InitializeComponent();

            ratingbar.Children.Add(StarContainer);

            InitStars();
        }


        // this function will replace empty star with fill star
        private void fillStar()
        {
            switch (Rating)
            {
                case 0:
                    star1.Source = emptyStarImage;
                    star2.Source = emptyStarImage;
                    star3.Source = emptyStarImage;
                    star4.Source = emptyStarImage;
                    star5.Source = emptyStarImage;
                    break;
                case 1:
                    star1.Source = fillStarImage;
                    star2.Source = emptyStarImage;
                    star3.Source = emptyStarImage;
                    star4.Source = emptyStarImage;
                    star5.Source = emptyStarImage;
                    break;
                case 2:
                    star1.Source = fillStarImage;
                    star2.Source = fillStarImage;
                    star3.Source = emptyStarImage;
                    star4.Source = emptyStarImage;
                    star5.Source = emptyStarImage;
                    break;
                case 3:
                    star1.Source = fillStarImage;
                    star2.Source = fillStarImage;
                    star3.Source = fillStarImage;
                    star4.Source = emptyStarImage;
                    star5.Source = emptyStarImage;
                    break;
                case 4:
                    star1.Source = fillStarImage;
                    star2.Source = fillStarImage;
                    star3.Source = fillStarImage;
                    star4.Source = fillStarImage;
                    star5.Source = emptyStarImage;
                    break;
                case 5:
                    star1.Source = fillStarImage;
                    star2.Source = fillStarImage;
                    star3.Source = fillStarImage;
                    star4.Source = fillStarImage;
                    star5.Source = fillStarImage;
                    break;

            }
        }

        private void InitStars()
        {
            star1 = new Image() { HeightRequest = StarSize, WidthRequest = StarSize };
            star2 = new Image() { HeightRequest = StarSize, WidthRequest = StarSize };
            star3 = new Image() { HeightRequest = StarSize, WidthRequest = StarSize };
            star4 = new Image() { HeightRequest = StarSize, WidthRequest = StarSize };
            star5 = new Image() { HeightRequest = StarSize, WidthRequest = StarSize };

            StarContainer.Children.Clear();
            StarContainer.Children.Add(star1);
            StarContainer.Children.Add(star2);
            StarContainer.Children.Add(star3);
            StarContainer.Children.Add(star4);
            StarContainer.Children.Add(star5);

            star1.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (Rating == 1) Rating = 0;
                    else Rating = 1;
                })
            });

            star2.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (Rating == 2) Rating = 0;
                    else Rating = 2;
                })
            });

            star3.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (Rating == 3) Rating = 0;
                    else Rating = 3;
                })
            });
            star4.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (Rating == 4) Rating = 0;
                    else Rating = 4;
                })
            });
            star5.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (Rating == 5) Rating = 0;
                    else Rating = 5;
                })
            });
        }

        private static void RatingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            control.SetValue(RatingProperty, (int)newValue);
            control.fillStar();                
        }

        private static void StarSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            control.InitStars();
        }

    }

}