using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common.Shared.Min.Extensions;
using SramComparer.Services;

namespace SramComparer.SoE.WrapperApp
{
	public static class SramComparerCmdStarter
	{
		private static IConsolePrinter ConsolePrinter => ServiceCollection.ConsolePrinter;

		public static bool Start(string exeFilePath, string[] args, TextWriter? output = null)
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

			using (new TemporaryOutputSetter(output))
			{
				while (true)
				{
					try
					{
						var key = Console.ReadLine()!.ToLower();
						if (key == "q" || key == "quit")
							break;

						process.StandardInput.WriteLine(key);
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

		private class TemporaryOutputSetter : IDisposable
		{
			private readonly TextWriter _oldOut = Console.Out;

			public TemporaryOutputSetter(TextWriter? output)
			{
				if (output is null) return;

				Console.SetOut(output);
			}

			public void Dispose() => Console.SetOut(_oldOut);
		}
	}
}