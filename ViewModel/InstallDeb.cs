using System.Collections.Generic;
using System.Diagnostics;
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
        } 
        else if (error.Contains("unmet") || error.Contains("impossível"))
        {
            return AptErrorType.PackageBroken;
        } 
        else if (error.Contains("Held") || error.Contains("hold"))
        {
            return AptErrorType.HeldPackage;
        }

        return AptErrorType.None;
    }

    public InstallDeb(string pathway)
    {
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

            if (process == null)
            {
                new NotificationWindow("Erro ao iniciar processo.", "ERROR", "Red").Show();
                return;
            }

            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                ErrorRepair(error);
            }
            else
            {
                var notific = new NotificationWindow("Instalado com sucesso!", "Notification", "Lime");
                notific.Timer();
            }
    }

    public static List<string> ExtractDependencies(string error)
    {
        var dependencies = new HashSet<string>();

        var matches = Regex.Matches(error, @"Depends:\s*([a-zA-Z0-9\-\._]+)(?:\s*\(.*?\))?");

        foreach (Match match in matches)
        {
            dependencies.Add(match.Groups[1].Value);
        }

        return new List<string>(dependencies);
    }

    public static void ErrorRepair(string error)
    {
        var dep = ExtractDependencies(error);
        var type = DetectError(error);

        switch (type)
        {
            case AptErrorType.DependencyError:
                new DependsError("ERRO DE DEPENDÊNCIA", error, dep, type).Show();
            break;

            case AptErrorType.HeldPackage:
                new DependsError("ERRO DE PACOTE", error, dep, type).Show();
            break;

            case AptErrorType.PackageBroken:
                new DependsError("ERRO DE PACOTE", error, dep, type).Show();
            break;
        }
    }
}