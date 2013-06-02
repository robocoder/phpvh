using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;


namespace Components
{
    public static class IEnumerableExtension
    {
        [DebuggerStepThrough]
        public static void Iter<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<IEnumerable<T>> GroupEvery<T>(IEnumerable<T> source, int groupSize)
        {
            return Enumerable
                .Range(0, (int)Math.Ceiling((double)source.Count() / groupSize))
                .Select(x => source.Skip(x * groupSize).Take(groupSize));
        }

        [DebuggerStepThrough]
        public static void AsyncIter<T>(this IEnumerable<T> source, Action<T> action)
        {
            var resets = new List<ManualResetEvent>();

            foreach (var item in source)
            {
                var i = item;
                var reset = new ManualResetEvent(false);

                resets.Add(reset);

                ThreadPool.QueueUserWorkItem(x =>
                {
                    action(i);
                    reset.Set();
                });
            }

            foreach (var reset in resets)
            {
                reset.WaitOne();
            }
        }

        private static Random _random = new Random();

        [DebuggerStepThrough]
        public static T TakeRandom<T>(this IEnumerable<T> source)
        {
            lock (_random)
            {
                return source.ElementAt(_random.Next(0, source.Count()));
            }
        }
    }
}
