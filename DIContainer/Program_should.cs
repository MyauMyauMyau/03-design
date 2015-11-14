using System;
using System.IO;
using FakeItEasy;
using Ninject;
using NUnit.Framework;

namespace DIContainer
{
    [TestFixture]
    public class Program_should
    {
        [Test]
        public void run_command_by_name()
        {
            var command = A.Fake<ICommand>();
            A.CallTo(() => command.Name).Returns("cmd");
            var program = new Program(new CommandLineArgs("CMD"), command);

            program.Run(new StreamWriter(Console.OpenStandardOutput()));

            A.CallTo(() => command.Execute()).MustHaveHappened();
        }
    }
}