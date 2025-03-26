using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace project.Models
{
    /// <summary>
    /// Třída reprezentující dané měření
    /// </summary>
    public class MeasureData
    {

        public string? Name { get; private set; }

        public IReadOnlyList<Data> FileData => _fileData;
        private readonly List<Data> _fileData = new();

        /// <summary>
        /// Funkce pro urceni zda instance obsahuje nejaka data
        /// </summary>
        /// <returns>Vraci true pokud data obsahuje jinak False</returns>
        public bool AnyData() => _fileData.Any();

        /// <summary>
        /// Funcke pro filtraci dat na zaklade teploty
        /// </summary>
        /// <param name="temperature">teplota pro filtraci</param>
        /// <returns>Vraci filtrovana data</returns>
        public IEnumerable<Data> GetDataByTemperature(double temperature) =>
            _fileData.Where(d => d.Temperature == temperature);

        /// <summary>
        /// Funcke pro filtraci dat na zaklade frekvence
        /// </summary>
        /// <param name="frequency">frekvence pro filtraci</param>
        /// <returns>Vraci filtrovana data</returns>
        public IEnumerable<Data> GetDataByFrequency(double frequency) =>
            _fileData.Where(d => d.Frequency == frequency);

        /// <summary>
        /// Nacteni dat ze souboru
        /// </summary>
        /// <param name="filePath">Cesta k souboru</param>
        /// <returns></returns>
        public async Task<bool> LoadData(string? filePath)
        {
            // Kontrola existence souboru
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Neplatná cesta k souboru!");
                return false;
            }

            // Načtení řádků souboru
            var lines = await File.ReadAllLinesAsync(filePath);

            // Kontrola, zda soubor obsahuje dostatek dat
            if (lines.Length < 5)
            {
                Console.WriteLine("Neplatný obsah dat v měření!");
                return false;
            }

            int index = 0;
            Name = lines[index++]; // Název měření

            // Přeskočení nepotřebných řádků, dokud nenajdeme hlavičku
            while (index < lines.Length && !lines[index].StartsWith(" ")) index++;
            if (index >= lines.Length) return false;

            // Rozdělení hlavičky do pole
            string[] header = lines[index++].Split('\t').Select(h => h.Trim()).ToArray();

            // Dynamické vyhledání indexů klíčových sloupců
            int freqIndex = Array.FindIndex(header, h => h.Contains("Freq", StringComparison.OrdinalIgnoreCase));
            int tempIndex = Array.FindIndex(header, h => h.Contains("Temp", StringComparison.OrdinalIgnoreCase));

            if (freqIndex == -1 || tempIndex == -1)
            {
                Console.WriteLine("Chybí klíčové sloupce v hlavičce!");
                return false;
            }

            // Načtení jednotlivých řádků dat
            for (int i = index; i < lines.Length; i++)
            {
                string[] values = lines[i].Split('\t');

                if (values.Length < header.Length ||
                    !double.TryParse(values[freqIndex], NumberStyles.Any, CultureInfo.InvariantCulture, out double freq) ||
                    !double.TryParse(values[tempIndex], NumberStyles.Any, CultureInfo.InvariantCulture, out double temp))
                {
                    continue;
                }

                // Uložení dalších hodnot do slovníku
                var extraData = new Dictionary<string, double>();
                for (int j = 0; j < header.Length && j < values.Length; j++)
                {
                    if (j != freqIndex && j != tempIndex && double.TryParse(values[j], NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                    {
                        extraData[header[j]] = value;
                    }
                }

                // Přidání nového záznamu do seznamu
                _fileData.Add(new Data(i, freq, temp, extraData));
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Měření: {Name}");

            var extraKeys = _fileData.SelectMany(d => d.extraValues.Keys).Distinct().ToArray();

            List<string> header = new() { "Frequency", "Temperature" };
            header.AddRange(extraKeys);
            sb.AppendLine(string.Join('\t', header));

            foreach (var data in _fileData)
            {
                List<string> values = new() { data.Frequency.ToString("E2"), data.Temperature.ToString("E2") };
                values.AddRange(extraKeys.Select(k => data.extraValues.ContainsKey(k) ? data.extraValues[k].ToString("E2") : "-"));
                sb.AppendLine(string.Join('\t', values));
            }

            return sb.ToString();
        }
    }
}
