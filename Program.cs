using System;
using App.Commons.Extensions;
using SramComparer.Services;

namespace SramComparer.SoE.WrapperApp
{
	internal class Program
	{
		private static IConsolePrinter ConsolePrinter => ServiceCollection.ConsolePrinter;
		
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
				Console.WriteLine("1: via .NET Api");
				Console.WriteLine("2: via Cmd");
				Console.WriteLine("q: quit");

				var key = Console.ReadLine();

				try
				{
					result = key switch
					{
						"1" => SramComparerApiStarter.Start(currentGameFilepath),
						"2" => SramComparerCmdStarter.Start(exeFilepath, currentGameFilepath),
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
	}
}
