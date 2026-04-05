
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EncurtadorDownload;

public class FlatpakPackages
{
    public string GetFlatpakFile(string file)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dpkg-deb",
            ArgumentList = {"-f", file, "Package"},
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi);

        string txt = process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();

        return(txt);
    }

    public (string? output, string? error) GetPackage(string package)
    {
        try
        {
            string[] lines = File.ReadAllLines(package);

            string? id = lines
                .FirstOrDefault(l => l.StartsWith("Id="))
                ?.Replace("Id=","")
                .Trim();

                if (id == null)
                    return (null, "ID não encontrado.");
                
            return (id, null);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }
}