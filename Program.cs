using System;
using SramComparer.Services;
using SramComparer.SoE.Services;
using SramComparer.SoE.WrapperApp.Helpers;

namespace SramComparer.SoE.WrapperApp
{
	internal class Program
	{
		private static IConsolePrinter ConsolePrinter => ServiceCollection.ConsolePrinter;
		
		private static void Main(string[] args)
		{
			var options = new CmdLineParserSoE().Parse(args[1..]);

			ConsoleHelper.Initialize(options);
			ConsoleHelper.PrintParams(args, options);
			ConsoleHelper.PrintHelp();

			while (true)
			{
				var key = Console.ReadLine()!.ToLower();
				if (key == "q" || key == "quit")
					break;

				if (key == "??")
				{
					ConsoleHelper.PrintHelp();
					continue;
				}

				try
				{
					var result = key switch
					{
						"1" => CommandHelper.Start(args[1..]),
						"2" => CmdLineStarter.Start(args[0], args[1..]),
						// ReSharper disable once UnreachableSwitchArmDueToIntegerAnalysis
						_ => true
					};

					if (!result)
						break;
				}
				catch (Exception ex)
				{
					ConsolePrinter.PrintFatalError(ex.Message);
				}
			}
		}
	}
}
