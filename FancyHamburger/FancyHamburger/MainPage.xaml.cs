using FancyHamburger.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FancyHamburger
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        enum States
        {
            Main,
            NavDrawOpen
        }

        public class NavMenuItem
        {
            public string MenuTitle { get; set; }
            public Lazy<View> Content { get; set; }
        }

        public ObservableCollection<NavMenuItem> MenuItems { get; set; }

        Storyboard _storyboard = new Storyboard();
        States _currentState = States.Main;
        uint animationSpeed = 250;
        public MainPage()
        {
            InitializeComponent();

            // setup our states in the Storyboard state machine.
            _storyboard.Add(States.Main, new[]
                {
                    new ViewTransition(MiddleBar, AnimationType.Layout, new Rectangle(0,7, 15, 3), animationSpeed, Easing.SinInOut),
                    new ViewTransition(MainContent, AnimationType.TranslationX, 0, animationSpeed, Easing.SinInOut),
                    new ViewTransition(MainContent, AnimationType.Scale, 1, animationSpeed, Easing.SinInOut),
                });

            _storyboard.Add(States.NavDrawOpen, new[]
                {
                    new ViewTransition(MiddleBar, AnimationType.Layout, new Rectangle(0,7, 30, 3), animationSpeed, Easing.SinInOut),
                    new ViewTransition(MainContent, AnimationType.TranslationX, 180, animationSpeed, Easing.SinInOut),
                    new ViewTransition(MainContent, AnimationType.Scale, .8, animationSpeed, Easing.SinInOut),
                });

            MenuItems = new ObservableCollection<NavMenuItem>()
            {
                new NavMenuItem() {MenuTitle="Payment", Content= new Lazy<View>(() => new Payment())},
                new NavMenuItem() {MenuTitle="Your Trips", Content= new Lazy<View>(() => new Trips())},
                new NavMenuItem() {MenuTitle="Coupon", Content= new Lazy<View>(() => new Coupon())},
                new NavMenuItem() {MenuTitle="Settings", Content= new Lazy<View>(() => new Settings())},
            };

            this.BindingContext = MenuItems;

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var newState = _currentState == States.Main ? States.NavDrawOpen : States.Main;
            _storyboard.Go(newState);
            _currentState = newState;
        }

        

        private async Task ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedItem = e.Item as NavMenuItem;
            if (selectedItem != null)
            {
                var newContent = selectedItem.Content.Value;
                MainContent.Content = newContent;
                _storyboard.Go(States.Main);
                _currentState = States.Main;
            }
        }
    }
}
