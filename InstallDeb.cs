using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

public class InstallDeb
{
    public enum AptErrorType
    {
        DependencyError,
        PackageBroken,
        None,
        HeldPackage
    }

    public static AptErrorType DetectError(string error)
    {
        error = error.ToLower();

        if (error.Contains("depends:") || error.Contains("dependências"))
        {
            return AptErrorType.DependencyError;
        } else if (error.Contains("unmet dependencies") || error.Contains("impossível corrigir"))
        {
            return AptErrorType.PackageBroken;
        } else if (error.Contains("Held broken packages") || error.Contains("hold"))
        {
            return AptErrorType.HeldPackage;
        }

        return AptErrorType.None;
    }

    public InstallDeb(string pathway)
    {
        bool IsDependencyError(string error)
        {
            return error.Contains("Depends:") ||
                error.Contains("dependências desencontradas") ||
                error.Contains("but it is not installable");
        }

        

        string path = pathway;

        var psi = new ProcessStartInfo
            {
                FileName = "pkexec",
                Arguments = $"apt install -y \"{path}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(psi);

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
            {
                new NotificationWindow(error, "ERROR", "Red").Show();

            if (IsDependencyError(error))
            {
                var extractDependencies = ExtractDependencies(error);
                string dependency = Convert.ToString(extractDependencies);
                new DependsError(error, "DEPENDENCY ERROR", dependency).Show();
            }
            }
            else
            {
                var notific = new NotificationWindow("Instalado com sucesso!", "Notification", "Lime");
                notific.Timer();
            }
    }

    public static List<string> ExtractDependencies(string error)
    {
        var dependencies = new List<string>();

        var matches = Regex.Matches(error, @"Depends:\s*([a-zA-Z0-9\-\._]+)(?:.s\*\(.*?\))?");

        foreach (Match match in matches)
        {
            dependencies.Add(match.Groups[1].Value);
        }

        return dependencies;
    }
}