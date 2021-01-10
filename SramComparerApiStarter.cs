using System;
using System.Diagnostics;
using System.IO;
using SramComparer.Services;

namespace SramComparer.SoE.WrapperApp
{
	public class SramComparerApiStarter
	{
		private static ICommandHandler CommandExecutor => ServiceCollection.CommandHandler!;

		public static bool Start(string[] args) => SoE.Program.Main(args) == 0;

		public static bool RunCommand(string exeFilePath, string currentFilePath, string command, TextWriter? output = null)
		{
			var options = new Options { CurrentFilePath = currentFilePath};

			Debug.Assert(File.Exists(exeFilePath));

			var result = CommandExecutor.RunCommand(command, options, output ?? Console.Out);
			Console.ReadLine();

			return result;
		}
	}
}