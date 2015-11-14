﻿using System;
using System.IO;
using System.Linq;
using DIContainer.Commands;
using Ninject;
namespace DIContainer
{
    public class Program
    {
        private readonly CommandLineArgs arguments;
        private readonly ICommand[] commands;

        public Program(CommandLineArgs arguments, params ICommand[] commands)
        {
            this.arguments = arguments;
            this.commands = commands;
        
        }

        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Bind<ICommand>().To<TimerCommand>();
            kernel.Bind<ICommand>().To<PrintTimeCommand>();
            kernel.Bind<ICommand>().To<HelpCommand>();
            kernel.Bind<CommandLineArgs>().To<CommandLineArgs>().WithConstructorArgument(args);
            kernel.Bind<TextWriter>().To<StreamWriter>().WithConstructorArgument(Console.OpenStandardOutput());
            kernel.Get<Program>().Run(kernel.Get<TextWriter>());
            Console.ReadKey();
        }

        public void Run(TextWriter textWriter)
        {
            if (arguments.Command == null)
            {
                textWriter.WriteLine("Please specify <command> as the first command line argument");
                return;
            }
            var command = commands.FirstOrDefault(c => c.Name.Equals(arguments.Command, StringComparison.InvariantCultureIgnoreCase));
            if (command == null)
                textWriter.WriteLine("Sorry. Unknown command {0}", arguments.Command);
            else
                command.Execute();
        }
    }
}
