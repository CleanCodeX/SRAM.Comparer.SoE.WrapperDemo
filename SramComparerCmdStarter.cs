using System;
using System.Diagnostics;
using System.IO;
using SramComparer.Services;
using SramComparer.SoE.Enums;

namespace SramComparer.SoE.WrapperApp
{
	internal static class SramComparerCmdStarter
	{
		private static IConsolePrinter ConsolePrinter => ServiceCollection.ConsolePrinter;

		public static bool Start(string exeFilepath, string currentGameFilepath, string? commands = null)
		{
			var arguments = $@"""{currentGameFilepath}""";
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
					if (input == nameof(Commands.q)) break;

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