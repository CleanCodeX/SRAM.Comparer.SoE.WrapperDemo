using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common.Shared.Min.Extensions;
using SramComparer.Enums;
using SramComparer.Services;

namespace SramComparer.SoE.WrapperApp
{
	internal static class SramComparerCmdStarter
	{
		private static IConsolePrinter ConsolePrinter => ServiceCollection.ConsolePrinter;

		public static bool Start(string exeFilePath, string[] args)
		{
			Debug.Assert(File.Exists(exeFilePath));

			var process = new Process
			{
				StartInfo = new ProcessStartInfo(exeFilePath)
				{
					Arguments = args.Select(e => $@"""{e}""").Join(" "),
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