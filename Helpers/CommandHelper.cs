using System;
using SramComparer.Enums;
using SramComparer.Services;

namespace SramComparer.SoE.WrapperApp.Helpers
{
	public class CommandHelper
	{
		private static ICommandHandler CommandHandler => ServiceCollection.CommandHandler!;

		public static bool Start(string[] args) => SoE.Program.Main(args) == 0;

		public static void Compare(IOptions options) => RunCommand(nameof(Commands.Compare), options);

		public static bool RunCommand(string command, IOptions options) => CommandHandler.RunCommand(command, options, Console.Out);
	}
}