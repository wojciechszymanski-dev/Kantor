namespace kantor
{
    public partial class MainPage : ContentPage
    {
        Dictionary<int, string> currencies = new Dictionary<int, string> {
               {0, "GOLD" },
               {1, "EUR" },
               {2, "USD" },
               {3, "CHF" },
               {4, "GBP" },
               {5, "JPY" },
               {6, "CAD" },
               {7, "TRY" },
               {8, "RUB" },
               {9, "MXN" },
            };

        public MainPage()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            outerGrid.AddColumnDefinition(new ColumnDefinition(10));
            outerGrid.AddColumnDefinition(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));
            outerGrid.AddColumnDefinition(new ColumnDefinition(10));
            outerGrid.AddColumnDefinition(new ColumnDefinition(new GridLength(8, GridUnitType.Star)));
            outerGrid.AddColumnDefinition(new ColumnDefinition(10));

            outerGrid.AddRowDefinition(new RowDefinition(10));
            outerGrid.AddRowDefinition(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            outerGrid.AddRowDefinition(new RowDefinition(10));

            ScrollView scrollView = new ScrollView { 
                BackgroundColor = Colors.Black,
            };
            outerGrid.Add(scrollView, 1, 1);

            StackLayout currencyList = new StackLayout();

            for (int i = 0; i < 10; i++)
            {
                Button button = new Button
                {
                    HeightRequest = 100,
                    FontSize = 20,
                    TextColor = Colors.White,
                    Text = $"PLN -> {currencies[i]}",
                    BackgroundColor = Color.FromArgb($"#333"),
                    Margin = new Thickness(0,0,0,5),
                    CornerRadius = 0,
                };

                button.Clicked += (sender, e) =>
                {
                    foreach (Button b in currencyList)
                    {
                        if (b != button)
                        {
                            b.BackgroundColor = Color.FromArgb("#333");
                        }
                    }
                    button.BackgroundColor = Color.FromArgb("#007aff");
                };
                currencyList.Add(button);
            }

            scrollView.Content = currencyList;

            Grid innerGrid = new Grid { 
                BackgroundColor = Color.FromArgb("#222"),
            };

            Label label = new Label { 
                Text = "CURRENCY EXCHANGE",
                TextColor = Colors.White,
                FontSize = 80,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            innerGrid.Add(label);

            outerGrid.Add(innerGrid, 3, 1);
        }

        private void CurrencyApiHandler()
        {
            currencies = new Dictionary<int, string> {
               {0, "GOLD" },
               {1, "EUR" },
               {2, "USD" },
               {3, "CHF" },
               {4, "GBP" },
               {5, "JPY" },
               {6, "CAD" },
               {7, "TRY" },
               {8, "RUB" },
               {9, "MXN" },
            };
        }
    }

}
