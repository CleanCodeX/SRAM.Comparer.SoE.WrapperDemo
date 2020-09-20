using System;
using System.Diagnostics;
using System.IO;
using App.Commons.Extensions;
using SramComparer.Services;

namespace SramComparer.SoE.DemoApp
{
    internal class Program
    {
        private static IConsolePrinter ConsolePrinter => ServiceCollection.ConsolePrinter;
        private static ICommandExecutor CommandExecutor => ServiceCollection.CommandExecutor!;

        private static void Main(string[] args)
        {
            if (args.Length < 2)
                Array.Resize(ref args, 2);

            var exeFilepath = args[0];
            var currentGameFilepath = args[1];

            Console.WriteLine("Arguments:");
            Console.WriteLine($"{{0}}: {exeFilepath}");
            Console.WriteLine($"{{1}}: {currentGameFilepath}");

            var result = true;
            while (result)
            {
                Console.WriteLine();
                Console.WriteLine("=".Repeat(100));
                Console.WriteLine("Enter starting method:");
                Console.WriteLine("1: via .NET");
                Console.WriteLine("2: via Cmd");
                Console.WriteLine("q: quit");

                var key = Console.ReadLine();

                try
                {
                    result = key switch
                    {
                        "1" => StartSramComparerNet(currentGameFilepath),
                        "2" => StartSramComparerCmd(exeFilepath, currentGameFilepath),
                        "q" => false,
                        // ReSharper disable once UnreachableSwitchArmDueToIntegerAnalysis
                        _ => true
                    };
                }
                catch (Exception ex)
                {
                    ConsolePrinter.PrintFatalError(ex.Message);
                }
            }
        }

        private static bool StartSramComparerNet(string currentGameFilepath, string? commands = null)
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

        private static bool RunSingleSramComparerCommandNet(string exeFilepath, string currentGameFilepath, string command)
        {
            var options = new Options { CurrentGameFilepath = currentGameFilepath};

            Debug.Assert(File.Exists(exeFilepath));

            var result = CommandExecutor.RunCommand(command, options, Console.Out);
            Console.ReadLine();

            return result;
        }

        private static bool StartSramComparerCmd(string exeFilepath, string currentGameFilepath, string? commands = null)
        {
            var arguments = currentGameFilepath;
            if (commands is not null)
                arguments += $@"--cmd ""{commands}""";

            Debug.Assert(File.Exists(exeFilepath));

            var process = new Process
            {
                StartInfo = new ProcessStartInfo(exeFilepath)
                {
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                }
            };

            process.OutputDataReceived += OutputHandler;
            process.ErrorDataReceived += ErrorHandler;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    if (input == "q") break;

                    process.StandardInput.WriteLine(input);
                }
                catch (IOException ex)
                {
                    ConsolePrinter.PrintError(ex.Message);
                }
                catch (Exception ex)
                {
                    ConsolePrinter.PrintError(ex);
                }
            }

            return true;

            static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
            {
                var data = outLine.Data;
                Console.WriteLine(data);
            }

            static void ErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
            {
                var data = outLine.Data;
                ConsolePrinter.PrintError(data!);
            }
        }
    }
}
