using System;
using System.Diagnostics;
using System.IO;
using SramComparer.Enums;
using SramComparer.Services;

namespace SramComparer.SoE.WrapperApp
{
	internal static class SramComparerCmdStarter
	{
		private static IConsolePrinter ConsolePrinter => ServiceCollection.ConsolePrinter;

		public static bool Start(string exeFilePath, string currentFilePath, string? commands = null)
		{
			var arguments = $@"""{currentFilePath}""";
			if (commands is not null)
				arguments += $@" --batch-cmds ""{commands}""";

			Debug.Assert(File.Exists(exeFilePath));

			var process = new Process
			{
				StartInfo = new ProcessStartInfo(exeFilePath)
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
					if (input == nameof(Commands.Quit)) break;

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