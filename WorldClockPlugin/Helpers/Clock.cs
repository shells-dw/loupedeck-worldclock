namespace Loupedeck.WorldClockPlugin.Helpers
{
    using Loupedeck.WorldClockPlugin.Helpers.Interfaces;
    using NodaTime.TimeZones;
    using NodaTime;

        public class ClockService : IClockService
        {
            private readonly IClock _clock;

            public DateTimeZone TimeZone { get; private set; }

            public ClockService()
                : this(SystemClock.Instance)
            {
            }

            public ClockService(IClock clock)
            {
                _clock = clock;

                // NOTE: Get the current users timezone here instead of hard coding it...
                TimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Europe/Zurich");
            }

            public Instant Now
                => _clock.GetCurrentInstant();

            public LocalDateTime LocalNow
                => Now.InZone(TimeZone).LocalDateTime;

            public Instant? ToInstant(LocalDateTime? local)
                => local?.InZone(TimeZone, Resolvers.LenientResolver).ToInstant();

            public LocalDateTime? ToLocal(Instant? instant)
                => instant?.InZone(TimeZone).LocalDateTime;
        public Instant ToInstant(LocalDateTime local) => throw new System.NotImplementedException();
        public LocalDateTime ToLocal(Instant instant) => throw new System.NotImplementedException();
    }
    }
