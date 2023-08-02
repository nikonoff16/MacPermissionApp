using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Parsing;
using Spectre.Console;

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

        var addCommand = new Command("add", "Set permission or permissions")
        {
            appPathArgument,
            addSinglePermissionArgument,
            addPermissionsOption
        };


        var rootCommand = new RootCommand(description: "Mac Permission App")
        {
            defaultCommand,
            addCommand
        };

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
        
        var parser = new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .UseHelp(ctx =>
            {
                ctx.HelpBuilder.CustomizeSymbol(addCommand,
                    firstColumnText: "add <path> <permission> \nOR \nadd <path> -m <perm1> <perm2> etc.",
                    secondColumnText: $"First command allows to add single permission to an app," +
                                      $"syntax is quite plain.\n" +
                                      $"The second allows to add many commands, just type it one by one " +
                                      $"and divide them by space.\n" +
                                      $"List of all available permissions is here: " +
                                      $"{string.Join(", ", Enum.GetNames(typeof(UtilCommands)))}.");
                ctx.HelpBuilder.CustomizeLayout(
                    _ =>
                        HelpBuilder.Default
                            .GetLayout()
                            .Skip(1) // Skip the default command description section.
                            .Prepend(
                                _ => Spectre.Console.AnsiConsole.Write(
                                    new FigletText(rootCommand.Description!))
                            ));
            })
            .Build();

        
        
        return await parser.InvokeAsync(args);
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
