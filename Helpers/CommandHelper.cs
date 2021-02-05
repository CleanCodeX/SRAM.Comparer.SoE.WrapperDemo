using System;
using SRAM.Comparison.Enums;
using SRAM.Comparison.Services;

namespace SRAM.Comparison.SoE.WrapperDemo.Helpers
{
	public class CommandHelper
	{
		private static ICommandHandler CommandHandler => ComparisonServices.CommandHandler!;

		public static bool Start(string[] args) => SoE.Program.Main(args) == 0;

		public static void Compare(IOptions options) => RunCommand(nameof(Commands.Compare), options);

		public static bool RunCommand(string command, IOptions options) => CommandHandler.RunCommand(command, options, Console.Out);
	}
}