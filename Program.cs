using System;
using Common.Shared.Min.Extensions;
using SramComparer.Services;

namespace SramComparer.SoE.WrapperApp
{
	internal class Program
	{
		private static IConsolePrinter ConsolePrinter => ServiceCollection.ConsolePrinter;
		
		private static void Main(string[] args)
		{
			Console.WriteLine("Arguments:");
			Console.WriteLine($"{{0}}: {args[0]}");

			if(args.Length > 1)
				Console.WriteLine($"CMD: {args[1..].Join(" ")}");

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
						"1" => SramComparerApiStarter.Start(args[1..]),
						"2" => SramComparerCmdStarter.Start(args[0], args[1..]),
						"quit" => false,
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
