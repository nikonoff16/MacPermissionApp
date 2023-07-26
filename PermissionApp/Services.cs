using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PermissionApp;

class Services
{
    public static string GetPermissions(string appId, bool micro=true, bool camera=true, bool screenRecord=false) 
    {
        if (File.Exists("tccplus"))
        {
            string result = "";
            if (micro) 
            {
                result += StartProcess("./tccplus", $"add {UtilCommands.Microphone} {appId}");
            }
            if (camera) 
            {
                result += StartProcess("./tccplus", $"add {UtilCommands.Camera} {appId}");
            }
            if (screenRecord) 
            {
                result += StartProcess("./tccplus", $"add {UtilCommands.ScreenCapture} {appId}");
            }
            return result;
        }
        else 
        {
            return "";
        }
    }

    static string FindId(string output)
    {
            string pattern = "identifier ['\"]\\w+.*?.\\w+['\"]";
            var result = "";

            RegexOptions options = RegexOptions.Multiline;
            
            foreach (Match m in Regex.Matches(output, pattern, options).Cast<Match>())
            {
                Console.WriteLine($"'{m.Value}' found at index {m.Index}.");
                result = m.Value;
                result = result.Replace("identifier ", "");
                result = result.Replace("\"", "");
            }
            return result;
    }

    public static string GetAppId(string appUrl) 
    {
        var arguments = "-dr - " + "\"" + appUrl + "\"";
        var rawAppInfo = StartProcess("codesign", arguments);
        var test = FindId(rawAppInfo);
        return test;
    }

    public static string StartProcess(string appName, string arguments) 
    {
        var psi = new ProcessStartInfo();
        psi.FileName = appName;
        psi.Arguments = arguments;
        psi.UseShellExecute = false;
        psi.RedirectStandardOutput = true;

        using var process = Process.Start(psi);
        using StreamReader reader = process.StandardOutput;

        return reader.ReadToEnd();
    }
}