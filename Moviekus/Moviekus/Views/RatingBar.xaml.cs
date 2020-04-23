using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Moviekus.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingBar : ContentView
    {
        private static string emptyStarImage = "star_empty.png";
        private static string fillStarImage = "star.png";
        private static int imageHeight = 30;
        private static int imageWidth = 30;

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
              defaultValue: 0,
              defaultBindingMode: BindingMode.TwoWay,
              propertyChanged: RatingPropertyChanged
            );


        public RatingBar()
        {
            InitializeComponent();

            ratingbar.Children.Add(StarContainer);

            star1 = new Image() { HeightRequest = imageHeight, WidthRequest = imageWidth };
            star2 = new Image() { HeightRequest = imageHeight, WidthRequest = imageWidth };
            star3 = new Image() { HeightRequest = imageHeight, WidthRequest = imageWidth };
            star4 = new Image() { HeightRequest = imageHeight, WidthRequest = imageWidth };
            star5 = new Image() { HeightRequest = imageHeight, WidthRequest = imageWidth };

            StarContainer.Children.Clear();
            StarContainer.Children.Add(star1);
            StarContainer.Children.Add(star2);
            StarContainer.Children.Add(star3);
            StarContainer.Children.Add(star4);
            StarContainer.Children.Add(star5);

            star1.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(x => Rating = 1)
            }); 

            star2.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(x => Rating = 2)               
            });

            star3.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(x => Rating = 3)
            });
            star4.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(x => Rating = 4)
            });
            star5.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(x => Rating = 5)
            });            
        }

        /*
        #region  Image Height Width Property
        public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
        propertyName: "ImageHeight",
        returnType: typeof(double),
        declaringType: typeof(RatingBar),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: ImageHeightPropertyChanged
         );

        public double ImageHeight
        {
            get { return (double)base.GetValue(ImageHeightProperty); }
            set { base.SetValue(ImageHeightProperty, value); }
        }

        private void ImageHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // set all images height  equal
            star1.HeightRequest = (double)newValue;
            star2.HeightRequest = (double)newValue;
            star3.HeightRequest = (double)newValue;
            star4.HeightRequest = (double)newValue;
            star5.HeightRequest = (double)newValue;
        }


        //image width
        public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
        propertyName: "ImageWidth",
        returnType: typeof(double),
        declaringType: typeof(RatingBar),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: ImageWidthPropertyChanged
         );

        public double ImageWidth
        {
            get { return (double)base.GetValue(ImageWidthProperty); }
            set { base.SetValue(ImageWidthProperty, value); }
        }

        private static void ImageWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // set all images width  equal
            star1.WidthRequest = (double)newValue;
            star2.WidthRequest = (double)newValue;
            star3.WidthRequest = (double)newValue;
            star4.WidthRequest = (double)newValue;
            star5.WidthRequest = (double)newValue;
        }
        #endregion

        #region Horizontal Vertical Alignment 
        public static readonly BindableProperty HorizontalOptionsProperty = BindableProperty.Create(
        propertyName: "HorizontalOptions",
        returnType: typeof(LayoutOptions),
        declaringType: typeof(RatingBar),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: HorizontalOptionsPropertyChanged
         );

        public LayoutOptions HorizontalOptions
        {
            get { return (LayoutOptions)base.GetValue(HorizontalOptionsProperty); }
            set { base.SetValue(HorizontalOptionsProperty, value); }
        }

        private static void HorizontalOptionsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            control.stkRatingbar.HorizontalOptions = (LayoutOptions)newValue;
        }

        //VERTICLE option set

        public static readonly BindableProperty VerticalOptionsProperty = BindableProperty.Create(
        propertyName: "VerticalOptions",
        returnType: typeof(LayoutOptions),
        declaringType: typeof(RatingBar),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: VerticalOptionsPropertyChanged
         );

        public LayoutOptions VerticalOptions
        {
            get { return (LayoutOptions)base.GetValue(VerticalOptionsProperty); }
            set { base.SetValue(VerticalOptionsProperty, value); }
        }

        private static void VerticalOptionsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            control.stkRatingbar.VerticalOptions = (LayoutOptions)newValue;
        }
        #endregion
        */

        // this function will replace empty star with fill star
        private void fillStar()
        {
            switch (Rating)
            {
                case 1:
                    star1.Source = null;
                    
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

        private static void RatingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (RatingBar)bindable;
            //control.Rating = (int)newValue;
            control.SetValue(RatingProperty, (int)newValue);
            control.fillStar();                
        }

 
    }

}