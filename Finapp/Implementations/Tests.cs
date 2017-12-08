using Finapp.ICreateDatabase;
using Finapp.Interfaces;
using Finapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Finapp.Implementations
{
    public class Tests : ITests
    {
        private readonly ICreator _creator;
        private readonly IAlgorithms _algorithms;

        public Tests(ICreator creator, IAlgorithms algorithms)
        {
            _creator = creator;
            _algorithms = algorithms;
        }

        public TestsViewModel TestFor1000()
        {
            Stopwatch stopWatch = new Stopwatch();
            Stopwatch stopWatchDB = new Stopwatch();

            stopWatchDB.Start();
            _creator.ClearDB();
            _creator.CreateDB(500, 500);
            stopWatchDB.Stop();

            stopWatch.Start();
            _algorithms.Associating();

            TimeSpan time = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            time.Hours, time.Minutes, time.Seconds,
            time.Milliseconds / 10);

            TimeSpan timeDB = stopWatchDB.Elapsed;
            string elapsedTimeDB = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            timeDB.Hours, timeDB.Minutes, timeDB.Seconds,
            timeDB.Milliseconds / 10);

            return new TestsViewModel
            {
                TimeFor1000 = elapsedTime,
                CreateDBTime1000 = elapsedTimeDB
            };
        }
    }
}