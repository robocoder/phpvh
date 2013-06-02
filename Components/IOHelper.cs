using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Components
{
    public static class IOHelper
    {
        public const int DefaultMaxTries = 8192;

        public const int DefaultSleepTime = 10;

        public static void TryAction(Action IOAction)
        {
            TryAction(IOAction, DefaultMaxTries, DefaultSleepTime);
        }

        public static void TryAction(Action IOAction, int MaxTries, int SleepTime)
        {
            int tries = 0;

            while (true)
            {
                try
                {
                    IOAction();

                    return;
                }
                catch (IOException) { Thread.Sleep(SleepTime); }
                catch (AccessViolationException) { Thread.Sleep(SleepTime); }
                catch (UnauthorizedAccessException) { Thread.Sleep(SleepTime); }

                tries++;

                if (tries > MaxTries)
                {
                    throw new IOException(string.Format("Failed after exceeding {0} tries.", MaxTries));
                }
                else if (SleepTime > 0)
                {
                    Thread.Sleep(SleepTime);
                }
            }
        }
    }
}
