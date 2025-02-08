using System;
using System.Collections.Generic;

namespace OpenAdhanForWindowsX
{
    public class TaskScheduler
    {
        // https://stackoverflow.com/questions/3243348/how-to-call-a-method-daily-at-specific-time-in-c

        private static TaskScheduler _instance;
        private List<System.Threading.Timer> timers = new List<System.Threading.Timer>();

        private TaskScheduler() { }

        public static TaskScheduler Instance => _instance ?? (_instance = new TaskScheduler());

        public void ScheduleTask(int hour, int min, double intervalInHour, Action task)
        {
            DateTime now = DateTime.Now;
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            var timer = new System.Threading.Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromHours(intervalInHour));

            timers.Add(timer);
        }
        public void ClearTimers()
        {
            for (int i = 0; i < this.timers.Count; i++)
            {
                this.timers[i].Dispose();
            }
            this.timers.RemoveRange(0, this.timers.Count);
        }
    }
}
