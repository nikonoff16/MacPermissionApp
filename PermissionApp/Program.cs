using System.CommandLine;

namespace PermissionApp;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var appPathArgument = new Argument<string>
            (name: "path", description: "Path to the program that needs to add permissions");

        var addSinglePermissionArgument = new Argument<string>
            (name: "permission", description: "Single permission to add", getDefaultValue: () => "NotAnPermission");
        
        var addPermissionsOption = new Option<string[]>(
                name: "--add-multiple",
                description: "Strings to search for when deleting entries.")
            { IsRequired = false, AllowMultipleArgumentsPerToken = true};
        addPermissionsOption.AddAlias("-m");

        var defaultCommand = new Command("default", "Set permission to use a microphone and a camera on the device.")
            {
                appPathArgument
            };

        var addCommand = new Command("add", "Set permission or permissions");
        addCommand.AddArgument(appPathArgument);
        addCommand.AddArgument(addSinglePermissionArgument);
        addCommand.AddOption(addPermissionsOption);


        var rootCommand = new RootCommand();
        rootCommand.AddCommand(defaultCommand);
        rootCommand.AddCommand(addCommand);

        defaultCommand.SetHandler((appPathArgumentValue) =>
        {
            Console.WriteLine(Services.SetPermissions(appPathArgumentValue, UtilCommands.Camera));
            Console.WriteLine(Services.SetPermissions(appPathArgumentValue, UtilCommands.Microphone));
            Console.WriteLine(Services.SetPermissions(appPathArgumentValue, UtilCommands.ScreenCapture));
        }, appPathArgument);
        
        addCommand.SetHandler((appPathArgumentValue, singlePermission, permissionsArray) =>
        {
            if (permissionsArray.Length == 0)
            {
                SetPermissionByString(appPathArgumentValue, singlePermission);
            }
            else
            {
                foreach (var permission in permissionsArray)
                {
                    SetPermissionByString(appPathArgumentValue, permission);
                }    
            }

            
        }, appPathArgument, addSinglePermissionArgument, addPermissionsOption);
        return await rootCommand.InvokeAsync(args);
    }

    private static void SetPermissionByString(string appPathArgumentValue, string permission)
    {
        if (Enum.IsDefined(typeof(UtilCommands), permission))
        {
            var realPermission = (UtilCommands)Enum.Parse(typeof(UtilCommands), permission);
            Console.WriteLine(Services.SetPermissions(appPathArgumentValue, realPermission));
        }
        else
        {
            Console.WriteLine($"Permission {permission} does not exists.");
        }
    }
}
