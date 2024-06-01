# kantor

## Overview

Kantor is a currency exchange application developed using .NET MAUI and LiveCharts API. It allows users to view the exchange rates of various currencies against the Polish Zloty (PLN) and provides a graphical representation of these exchange rates over the last 30 days.
Features

    * Currency Selection: Choose from a list of currencies to see their exchange rate against PLN.
    * Real-Time Data: Fetches the latest exchange rates and gold prices from the NBP API.
    * Interactive Chart: Displays historical exchange rate data in a line chart.
    * Currency Conversion: Enter a value in PLN to see its equivalent in the selected currency.

## Dependencies

    * LiveChartsCore.SkiaSharpView
    * LiveChartsCore
    * LiveChartsCore.Defaults
    * System.Text.Json
    * LiveChartsCore.SkiaSharpView.Maui
    * System.Collections.ObjectModel
    * System.Net.Http
    * System.Threading.Tasks

## Getting Started
Prerequisites

    * .NET SDK
    * MAUI workload installed

Installation

1. Clone the repository:

         git clone https://github.com/wojciechszymanski-dev/kantor.git

2. Navigate to the project directory:

         cd kantor

 3. Restore the dependencies:

         dotnet restore

Running the Application

      dotnet build
      dotnet run

Project Structure

    * MainPage.xaml: Contains the UI elements of the main page.
    * MainPage.xaml.cs: Contains the logic for handling UI interactions and fetching data from the NBP API.
    * Models: Contains data models for deserializing JSON responses from the API.

## Code Explanation
Initialization

The MainPage class initializes the UI components and sets up the currency list and the main layout grid.
UI Setup

    * InitializeUI(): Configures the main layout, adding a scrollable list of currency buttons and a grid for displaying exchange information.
    * CreateInnerGrid(): Sets up the inner grid with initial labels and styling.

Event Handling

    * Button Click Event: Each currency button sets its background color when clicked and calls ShowExchangeMenu() to display the exchange details.
    * Text Changed Event: Validates and updates the converted currency value when the input text changes.

Data Fetching

    * FetchAndDisplayData(): Fetches the exchange rates or gold prices from the NBP API, parses the JSON response, and updates the chart with the latest data.

Chart Display

    * ShowExchangeMenu(): Dynamically creates UI elements for currency exchange, including an entry for PLN value and a label for the converted value. It also fetches and displays the exchange rate data in a chart.

Usage

    1. Select Currency: Click on any currency button to view its exchange rate against PLN.
    2. View Chart: The application will display a line chart showing the historical exchange rates for the last 30 days.
    3. Currency Conversion: Enter a value in the provided input box to see its equivalent in the selected currency.

## API Reference

The application uses the NBP API to fetch exchange rates and gold prices:

    * Exchange Rates: https://api.nbp.pl/api/exchangerates/rates/A/{currency}/last/30/?format=json
    * Gold Prices: https://api.nbp.pl/api/cenyzlota/last/30/?format=json

## License

This project is licensed under the MIT License.
