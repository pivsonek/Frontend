using System.Collections.ObjectModel;

namespace project;

public class GraphManager
{
    public ObservableCollection<GraphModel> Graphs { get; private set; } = new();

    public GraphManager()
    {
        // Inicializace seznamu grafů
        Graphs.Add(new GraphModel { Name = "Graf 1" });
        Graphs.Add(new GraphModel { Name = "Graf 2" });
        Graphs.Add(new GraphModel { Name = "Graf 3" });
        Graphs.Add(new GraphModel { Name = "Graf 4" });
    }

    /// <summary>
    /// Přidá nový graf do seznamu.
    /// </summary>
    public void AddGraph()
    {
        int nextNumber = Graphs.Count + 1;
        Graphs.Add(new GraphModel { Name = $"Graf {nextNumber}" });
    }

    /// <summary>
    /// Odebere graf ze seznamu.
    /// </summary>
    public void RemoveGraph(GraphModel graph)
    {
        if (Graphs.Contains(graph))
        {
            Graphs.Remove(graph);
        }
    }

    /// <summary>
    /// Přepne stav zvětšení grafu.
    /// </summary>
    public void ToggleGraphSize(GraphModel graph)
    {
        if (graph != null)
        {
            graph.IsExpanded = !graph.IsExpanded;
        }
    }
}
