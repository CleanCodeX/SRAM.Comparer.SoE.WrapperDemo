using System;
using System.Diagnostics;
using System.IO;
using SramComparer.Services;

namespace SramComparer.SoE.WrapperApp
{
	internal class SramComparerApiStarter
	{
		private static ICommandHandler CommandExecutor => ServiceCollection.CommandHandler!;

		internal static bool Start(string[] args)
		{ 
			var result = SoE.Program.Main(args);

			return result == 0;
		}

		internal static bool RunSingleSramComparerCommandNet(string exeFilePath, string currentFilePath, string command)
		{
			var options = new Options { CurrentFilePath = currentFilePath};

			Debug.Assert(File.Exists(exeFilePath));

			var result = CommandExecutor.RunCommand(command, options, Console.Out);
			Console.ReadLine();

			return result;
		}
	}
}