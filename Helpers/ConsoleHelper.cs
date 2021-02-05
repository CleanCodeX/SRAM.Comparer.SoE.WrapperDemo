using System;
using System.Drawing;
using System.Linq;
using Common.Shared.Min.Extensions.Specialized;
using SRAM.Comparison.Extensions;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.Services;

namespace SRAM.Comparison.SoE.WrapperDemo.Helpers
{
	public static class ConsoleHelper
	{
		private static IConsolePrinter ConsolePrinter => ComparisonServices.ConsolePrinter;

		public static void Initialize(IOptions options)
		{
			PaletteHelper.SetScreenColors(Color.White, Color.FromArgb(17, 17, 17));
			Console.Clear();

			if (options.UILanguage is not null)
				CultureHelper.TrySetCulture(options.UILanguage);
		}

		public static void PrintParams(string[] args, IOptions options)
		{
			ConsolePrinter.PrintSectionHeader("Arguments");
			ConsolePrinter.PrintConfigLine("Current file", "{0}", @$"""{options.CurrentFilePath}""");
			ConsolePrinter.PrintConfigLine("Other params for SRAM-Comparer", args[1..].Join(" "));
			ConsolePrinter.ResetColor();
		}

		public static void PrintHelp()
		{
			ConsolePrinter.PrintSectionHeader("Commands");
			ConsolePrinter.PrintConfigLine("?", "This help");
			ConsolePrinter.PrintConfigLine("Quit [Q]", "Quit the app");
			ConsolePrinter.PrintConfigLine("1", "via .NET Api");
			ConsolePrinter.PrintConfigLine("2", "via Cmd Line");
			ConsolePrinter.PrintLine();
			ConsolePrinter.ResetColor();
		}
	}
}
