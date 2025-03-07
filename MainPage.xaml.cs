using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace project;

public partial class MainPage : ContentPage
{
    private GraphManager _graphManager = new();
    private FileHandler _fileHandler = new();
    private GraphModel? _expandedGraph = null; // Uchovává aktuálně zvětšený graf

    private ObservableCollection<GraphModel> _graphs;
    public ObservableCollection<GraphModel> Graphs
    {
        get => _graphs;
        set
        {
            _graphs = value;
            OnPropertyChanged(nameof(Graphs));
        }
    }

    public ObservableCollection<string> DataItems { get; set; } = new();

    private bool _isFrequencyEnabled = true;
    private bool _isTemperatureEnabled = true;

    public bool IsFrequencyEnabled
    {
        get => _isFrequencyEnabled;
        set
        {
            _isFrequencyEnabled = value;
            OnPropertyChanged(nameof(IsFrequencyEnabled));
        }
    }

    public bool IsTemperatureEnabled
    {
        get => _isTemperatureEnabled;
        set
        {
            _isTemperatureEnabled = value;
            OnPropertyChanged(nameof(IsTemperatureEnabled));
        }
    }

    public MainPage()
    {
        InitializeComponent();
        _graphs = _graphManager.Graphs;
        BindingContext = this;
    }

    private async void OnLoadFileClicked(object sender, EventArgs e)
    {
        string? filePath = await _fileHandler.PickFileAsync();
        if (!string.IsNullOrEmpty(filePath))
        {
            string? content = await _fileHandler.ReadFileContentAsync(filePath);
            FilePreviewLabel.Text = $"Soubor: {filePath}\nObsah:\n{content}";
        }
    }


    private void OnAddGraphClicked(object sender, EventArgs e)
    {
        _graphManager.AddGraph();
    }

    private void OnResizeGraphClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is GraphModel graph)
        {
            // Pokud už je tento graf zvětšený, tak ho zmenšíme
            if (graph.IsExpanded)
            {
                graph.IsExpanded = false;
                return;
            }

            // Jinak nejprve všechny ostatní grafy nastavíme na nezvětšené
            foreach (var g in _graphs)
            {
                g.IsExpanded = false;
            }

            // A zvětšíme pouze vybraný graf
            graph.IsExpanded = true;
        }
    }


    private async void OnExportGraphClicked(object sender, EventArgs e)
    {
        // TODO: Implementace exportu grafu
        if (sender is Button button && button.BindingContext is GraphModel graph)
        {
            await DisplayAlert("Export", $"Exportuji graf: {graph.Name}", "OK");
        }
        else
        {
            await DisplayAlert("Chyba", "Nelze exportovat graf.", "OK");
        }
    }

    private void OnTemperatureTextChanged(object sender, TextChangedEventArgs e)
    {
        Console.WriteLine($"Teplota změněna: {e.NewTextValue}");

        // Pokud uživatel zadá teplotu, zamkne se frekvence
        IsFrequencyEnabled = string.IsNullOrWhiteSpace(e.NewTextValue);
    }

    private void OnFrequencyTextChanged(object sender, TextChangedEventArgs e)
    {
        Console.WriteLine($"Frekvence změněna: {e.NewTextValue}");

        // Pokud uživatel zadá frekvenci, zamkne se teplota
        IsTemperatureEnabled = string.IsNullOrWhiteSpace(e.NewTextValue);
    }

    private void OnTemperatureSearch(object sender, EventArgs e)
    {
        Console.WriteLine("Hledání teploty bylo spuštěno.");
    }

    private void OnFrequencySearch(object sender, EventArgs e)
    {
        Console.WriteLine("Hledání frekvence bylo spuštěno.");
    }

    private async void OnHelpClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Nápověda",
            "Tato aplikace umožňuje analyzovat data na základě teploty a frekvence.\n\n" +
            "1. Zadejte teplotu nebo frekvenci.\n" +
            "2. Druhá hodnota se automaticky zamkne.\n" +
            "3. Klikněte na tlačítko hledání pro analýzu.\n" +
            "4. Můžete přidávat a upravovat grafy.\n\n" +
            "Pro další informace kontaktujte podporu.",
            "OK");
    }

}
