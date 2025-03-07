using Microsoft.Maui.Storage;
using System;
using System.Threading.Tasks;

namespace project;

/// <summary>
/// Zpracovává načítání souborů v aplikaci.
/// </summary>
public class FileHandler
{
    public async Task<string?> PickFileAsync()
    {
        var result = await FilePicker.PickAsync();
        return result?.FullPath;
    }

    public async Task<string?> ReadFileContentAsync(string filePath)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            return await File.ReadAllTextAsync(filePath);
        }
        return null;
    }
}

