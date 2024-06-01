using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System.Diagnostics;
using LiveChartsCore.Defaults;
using System.Text.Json;
using LiveChartsCore.SkiaSharpView.Maui;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace kantor
{
    public partial class MainPage : ContentPage
    {
        Grid innerGrid = new Grid();
        Label label;
        StackLayout stack;
        bool removeLabel = false;
        static List<string> dates;
        static List<double> values = new();
        Dictionary<int, string> currencies = new Dictionary<int, string>
        {
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
            outerGrid.AddColumnDefinition(new ColumnDefinition(10));

            ScrollView scrollView = new ScrollView
            {
                BackgroundColor = Colors.Black,
            };
            outerGrid.Add(scrollView, 1, 1);

            StackLayout currencyList = new StackLayout();

            for (int i = 0; i < currencies.Count; i++)
            {
                Button button = new Button
                {
                    HeightRequest = 100,
                    FontSize = 20,
                    TextColor = Colors.White,
                    Text = $"PLN -> {currencies[i]}",
                    BackgroundColor = Color.FromArgb("#333"),
                    Margin = new Thickness(0, 0, 0, 5),
                    CornerRadius = 0,
                };

                button.Clicked += async (sender, e) =>
                {
                    foreach (Button b in currencyList.Children)
                        if (b != button) b.BackgroundColor = Color.FromArgb("#333");

                    button.BackgroundColor = Color.FromArgb("#007aff");
                    await ShowExchangeMenu(button.Text.Split("->")[1].Trim());
                };
                currencyList.Children.Add(button);
            }

            scrollView.Content = currencyList;

            CreateInnerGrid();
            outerGrid.Add(innerGrid, 3, 1);
        }

        private void CreateInnerGrid()
        {
            innerGrid = new Grid
            {
                BackgroundColor = Color.FromArgb("#222"),
            };

            label = new Label
            {
                Text = "CURRENCY EXCHANGE",
                TextColor = Colors.White,
                FontSize = 80,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10)
            };
            innerGrid.Add(label);
        }

        private async Task ShowExchangeMenu(string currency)
        {
            if (label != null) innerGrid.Remove(label);
            if (stack != null) innerGrid.Remove(stack);

            stack = new StackLayout();

            label = new Label
            {
                Text = "CURRENCY EXCHANGE",
                TextColor = Colors.White,
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 0, 0, 10)
            };

            stack.Children.Add(label);

            Entry entry = new Entry
            {
                WidthRequest = 100,
                Text = "0"
            };
            entry.TextChanged += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(entry.Text) && !int.TryParse(entry.Text, out _))
                    entry.Text = entry.Text.Substring(0, entry.Text.Length - 1);
            };

            stack.Children.Add(entry);

            await FetchAndDisplayData(currency);

            innerGrid.Add(stack);
        }

        public ISeries[] Series { get; set; } = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = values.ToArray(),
                Fill = null
            }
        };

        public async Task FetchAndDisplayData(string currency)
        {
            HttpClient client = new HttpClient();
            string apiUrl = (currency.Equals("GOLD"))
                                ? "https://api.nbp.pl/api/cenyzlota/last/30/?format=json" 
                                : $"https://api.nbp.pl/api/exchangerates/rates/A/{currency}/last/30/?format=json";
            string response = await client.GetStringAsync(apiUrl);

            JsonDocument doc = JsonDocument.Parse(response);
            JsonElement root = doc.RootElement;

            dates = new List<string>();
            values = new List<double>();

            if (!currency.Equals("GOLD"))
            {
                JsonElement rates = root.GetProperty("rates");
                foreach (JsonElement rate in rates.EnumerateArray())
                {
                    dates.Add(rate.GetProperty("effectiveDate").GetDateTime().ToString("MM-dd"));
                    values.Add(rate.GetProperty("mid").GetDouble());
                }
            }
            else
            {
                foreach (JsonElement element in root.EnumerateArray())
                {
                    dates.Add(element.GetProperty("data").GetDateTime().ToString("MM-dd"));
                    values.Add(element.GetProperty("cena").GetDouble());
                }
            }

            var chart = new CartesianChart
            {
                Series = new ObservableCollection<ISeries>
                {
                    new LineSeries<double>
                    {
                        Values = values
                    }
                },
                YAxes = new List<Axis>
                {
                    new Axis
                    {
                        Labeler = value => value.ToString("0.00")
                    }
                },
                XAxes = new List<Axis>
                {
                    new Axis
                    {
                        Labels = dates.ToArray(),
                        MinStep = 1,
                    }
                },
                WidthRequest = 800,
                HeightRequest = 400,
                ZoomMode = (LiveChartsCore.Measure.ZoomAndPanMode)X
            };

            stack.Children.Add(chart);
        }
    }
}
