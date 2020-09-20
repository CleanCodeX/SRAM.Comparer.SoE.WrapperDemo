using System;
using System.Diagnostics;
using System.IO;
using SramComparer.Services;

namespace SramComparer.SoE.DemoApp
{
    internal class SramComparerNetStarter
    {
        private static ICommandExecutor CommandExecutor => ServiceCollection.CommandExecutor!;

        internal static bool Start(string currentGameFilepath, string? commands = null)
        {
            var args = new string[1];
            args[0] = currentGameFilepath;

            if (commands is not null)
            {
                Array.Resize(ref args, 2);
                args[1] = $@" --cmd ""{commands}""";
            }
            
            var result = SoE.Program.Main(args);
            Console.ReadLine();

            return result == 0;
        }

        internal static bool RunSingleSramComparerCommandNet(string exeFilepath, string currentGameFilepath, string command)
        {
            var options = new Options { CurrentGameFilepath = currentGameFilepath};

            Debug.Assert(File.Exists(exeFilepath));

            var result = CommandExecutor.RunCommand(command, options, Console.Out);
            Console.ReadLine();

            return result;
        }
    }
}