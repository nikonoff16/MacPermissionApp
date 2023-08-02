using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PermissionApp;

class Services
{
    public static string SetPermissions(string appPath, Enum permissionName)  
    {
        var appId = Services.GetAppId(appPath);
        return GetPermissions(appId, permissionName.ToString());
    }
    private static string GetPermissions(string appId, string permissionName) 
    {
       
        if (File.Exists("tccplus"))
        {
            return StartProcess("./tccplus", $"add {permissionName} {appId}");;
        }
        else if (String.IsNullOrEmpty(permissionName))
        {
            return "Received an empty permission name. Nothing to add I guess.";
        }
        else 
        {
            return "";
        }
    }

    private static string FindId(string output)
    {
            const string pattern = "identifier ['\"]\\w+.*?.\\w+['\"]";
            var result = "";

            const RegexOptions options = RegexOptions.Multiline;
            
            foreach (var m in Regex.Matches(output, pattern, options).Cast<Match>())
            {
                Console.WriteLine($"Internal app identifier '{m.Value}' has been found.");
                result = m.Value;
                result = result.Replace("identifier ", "");
                result = result.Replace("\"", "");
            }
            return result;
    }

    private static string GetAppId(string appUrl) 
    {
        var arguments = "-dr - " + "\"" + appUrl + "\"";
        var rawAppInfo = StartProcess("codesign", arguments);
        var test = FindId(rawAppInfo);
        return test;
    }

    private static string StartProcess(string appName, string arguments) 
    {
        var psi = new ProcessStartInfo
        {
            FileName = appName,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        using var process = Process.Start(psi);
        if (process == null) return "Some errors has been raised while execution";
        using StreamReader reader = process.StandardOutput;

        return reader.ReadToEnd();

    }
}