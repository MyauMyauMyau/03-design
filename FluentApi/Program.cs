using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading;

namespace FluentTask
{
    internal class Program
    {
        private static void Main()
        {


            var behaviour = new Behavior()
                .Say("Привет мир!")
                .UntilKeyPressed(b => b
                    .Say("Ля-ля-ля!")
                    .Say("Тру-лю-лю"))
                .Jump(JumpHeight.High)
                .UntilKeyPressed(b => b
                    .Say("Aa-a-a-a-aaaaaa!!!")
                    .Say("[набирает воздух в легкие]"))
                .Say("Ой!")
                .Delay(TimeSpan.FromSeconds(1))
                .Say("Кто здесь?!")
                .Delay(TimeSpan.FromMilliseconds(2000))
                .Run(TimeSpan.FromSeconds(2));

            behaviour.Execute();
            //behaviour.Execute(); // повторное выполнение сценария


        }
    }

    public class Behavior
    {
        public List<Action> Tasks;
        public Behavior()
        {
            Tasks = new List<Action>();
        }

        public Behavior Say(string text)
        {
            Tasks.Add(() => Console.WriteLine(text));
            return this;
        }

        public Behavior UntilKeyPressed(Action<Behavior> toDo)
        {
            var copy = new Behavior();
            toDo(copy);
            Tasks.Add(() =>
            {
                while (!Console.KeyAvailable)
                {
                    copy.Execute();
                    Thread.Sleep(500);
                }
                Console.ReadKey(false);
            });

            return this;
        }

        public void Execute()
        {
            foreach (var task in Tasks)
            {
                task();
            }
        }

        public Behavior Jump(JumpHeight height)
        {
            if (height == JumpHeight.High)
                Tasks.Add(() => Console.WriteLine("jumped high"));
            else
            {
                Tasks.Add(() => Console.WriteLine("jumped low"));
            }
            return this;
        }

        public Behavior Delay(TimeSpan delay)
        {
            Tasks.Add(() => Thread.Sleep(delay));
            return this;
        }
    }

    public static class BehaviorExtensions
    {
        public static Behavior Run(this Behavior behavior, TimeSpan timeToRun)
        {
            behavior.Tasks.Add(() =>
            {
                var timer = new Stopwatch();
                timer.Start();
                while (timer.Elapsed < timeToRun)
                {
                    Console.WriteLine("I'm running...");
                    Thread.Sleep(100);
                }
                timer.Stop();
            });
            return behavior;
        }
    }
}