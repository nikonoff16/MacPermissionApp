using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.XPath;
// See https://aka.ms/new-console-template for more information
RunApp(args);

void RunApp(string[] args) 
{
    if (args.Length > 0) 
    {
        Console.WriteLine($"The argument is {args[0]}");
        var appId = GetAppId(args[0]);
        Console.WriteLine(appId);
        Console.WriteLine(GetPermissions(appId));
    } 
    else 
    {
        Console.WriteLine("There's no any arguments with this program");
    }
}

string GetPermissions(string appId, bool micro=true, bool camera=true) 
{
    if (File.Exists("tccplus"))
    {
        string result = "";
        if (micro) 
        {
            result += StartProcess("./tccplus", $"add Microphone {appId}");
        }
        if (camera) 
        {
            result += StartProcess("./tccplus", $"add Camera {appId}");
        }
        return result;
    }
    else 
    {
        return "";
    }
}

string FindId(string output)
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

string GetAppId(string appUrl) 
{
    var arguments = "-dr - " + "\"" + appUrl + "\"";
    var rawAppInfo = StartProcess("codesign", arguments);
    var test = FindId(rawAppInfo);
    return test;
}

string StartProcess(string appName, string arguments) 
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
