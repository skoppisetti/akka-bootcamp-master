﻿using Akka.Actor;

namespace ChartApp.Actors
{
    #region Reporting
    /// <summary>
    ///  Signal used to indicate that its time to sample all counters
    /// </summary>
    /// 
    public class GatherMetrics { }

    /// <summary>
    ///  Metric data at the time of sample
    /// </summary>
    /// 
    public class Metric
    {
        public Metric(string series, float counterValue)
        {
            CounterValue = counterValue;
            Series = series;
        }

        public float CounterValue { get; private set; }
        public string Series { get; private set; }
    }
    #endregion

    #region Performance Counter Management
    /// <summary>
    /// All types of counters supported by this example
    /// </summary>
    /// 
    public enum CounterType
    {
        Cpu,
        Memory,
        Disk
    }

    /// <summary>
    /// Enables a counter and begind publishing values to <see cref="Subscriber"/>
    /// </summary>
    /// 
    public class SubscriberCounter
    {
        public SubscriberCounter(CounterType counter, IActorRef subscriber)
        {
            Subscriber = subscriber;
            Counter = counter;
        }

        public IActorRef Subscriber { get; private set; }
        public CounterType Counter { get; private set; }
    }

    /// <summary>
    /// Unscribers <see cref="Subscriber"/> from receiving updates
    /// </summary>
    public class UnsubscribeCounter
    {
        public UnsubscribeCounter(CounterType counter, IActorRef subscriber)
        {
            Subscriber = subscriber;
            Counter = counter;
        }

        public IActorRef Subscriber { get; private set; }
        public CounterType Counter { get; private set; }
    }
    #endregion
}
