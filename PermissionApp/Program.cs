using System.CommandLine;

namespace PermissionApp;

class Program
{
    static async Task Main(string[] args)
    {
        var appPathArgument = new Argument<string>
            ("path", "Path to the program that needs to add permissions");

        var rootCommand = new RootCommand();
        rootCommand.Add(appPathArgument);

        rootCommand.SetHandler((appPathArgumentValue) =>
            {
                Console.WriteLine($"<message> argument = {appPathArgumentValue}");
                var appId = Services.GetAppId(appPathArgumentValue);
                Console.WriteLine(appId);
                Console.WriteLine(Services.GetPermissions(appId));
            },
            appPathArgument);

        await rootCommand.InvokeAsync(args);
    }
}
