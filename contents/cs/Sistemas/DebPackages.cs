using System.Diagnostics;

public class DebPackages
{
    public string GetFile(string file)
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

    public (string output, string error) GetPackage(string package)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dpkg",
            ArgumentList = {"-s", package},
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = Process.Start(psi);
        string packageName = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        return(packageName, error);
    }
}




            