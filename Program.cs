using System;
using System.Drawing;
using Common.Shared.Min.Extensions;
using SramComparer.Extensions;
using SramComparer.Helpers;
using SramComparer.Services;
using SramComparer.SoE.Services;

namespace SramComparer.SoE.WrapperApp
{
	internal class Program
	{
		private static IConsolePrinter ConsolePrinter => ServiceCollection.ConsolePrinter;
		
		private static void Main(string[] args)
		{
			var options = new CmdLineParserSoE().Parse(args[1..]);

			Initialize(options);

			PrintParams(args, options);

			while (true)
			{
				PrintHelp();

				var key = Console.ReadLine()!.ToLower();
				if (key == "q" || key == "quit")
					break;

				try
				{
					var result = key switch
					{
						"1" => SramComparerApiStarter.Start(args[1..]),
						"2" => SramComparerCmdStarter.Start(args[0], args[1..]),
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

		private static void PrintParams(string[] args, IOptions options)
		{
			ConsolePrinter.PrintSectionHeader("Arguments");
			ConsolePrinter.PrintConfigLine("Current file", "{0}", @$"""{options.CurrentFilePath}""");
			ConsolePrinter.PrintConfigLine("Other params for SRAM-Comparer", args[1..].Join(" "));
			ConsolePrinter.ResetColor();
		}

		private static void PrintHelp()
		{
			ConsolePrinter.PrintSectionHeader("Enter starting method");
			ConsolePrinter.PrintConfigLine("Quit [Q]", "Quit the app");
			ConsolePrinter.PrintConfigLine("1", "via .NET Api");
			ConsolePrinter.PrintConfigLine("2", "via Cmd");
			ConsolePrinter.PrintLine();
			ConsolePrinter.ResetColor();
		}

		private static void Initialize(IOptions options)
		{
			PaletteHelper.SetScreenColors(Color.White, Color.FromArgb(17, 17, 17));
			Console.Clear();

			if (options.UILanguage is not null)
				CultureHelper.TrySetCulture(options.UILanguage);
		}
	}
}
