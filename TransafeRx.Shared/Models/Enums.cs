using System.ComponentModel;

namespace TransafeRx.Shared.Models
{
    public enum ShapeType
    {
        [Description("Circle")]
        circle = 0,
        [Description("Rectangle")]
        rect = 1,
        [Description("Polygon")]
        poly = 2
    }


    #region schedulingEnums
    public enum FrequencyType
    {
        [Description("Once")]
        Once = 1,
        [Description("Daily")]
        Daily = 2,
        [Description("Weekly")]
        Weekly = 3,
        [Description("Monthly")]
        Monthly = 4,
        [Description("Monthly (Relative)")]
        MonthlyRelative = 5
    }

    public enum FrequencySubDayType
    {
        [Description("Specified Time")]
        SpecifiedTime = 1,
        [Description("Second(s)")]
        Seconds = 2,
        [Description("Minute(s)")]
        Minutes = 3,
        [Description("Hour(s)")]
        Hours = 4
    }

    public enum FrequencyRelativeInterval
    {
        [Description("First")]
        First = 1,
        [Description("Second")]
        Second = 2,
        [Description("Third")]
        Third = 3,
        [Description("Fourth")]
        Fourth = 4,
        [Description("Last")]
        Last = 5
    }

    public enum FrequencyIntervalWeekly
    {
        [Description("Sunday")]
        Sunday = 1,
        [Description("Monday")]
        Monday = 2,
        [Description("Tuesday")]
        Tuesday = 3,
        [Description("Wednesday")]
        Wednesday = 4,
        [Description("Thursday")]
        Thursday = 5,
        [Description("Friday")]
        Friday = 6,
        [Description("Saturday")]
        Saturday = 7
    }

    public enum FrequencyIntervalMonthlyRelative
    {
        [Description("Sunday")]
        Sunday = 1,
        [Description("Monday")]
        Monday = 2,
        [Description("Tuesday")]
        Tuesday = 3,
        [Description("Wednesday")]
        Wednesday = 4,
        [Description("Thursday")]
        Thursday = 5,
        [Description("Friday")]
        Friday = 6,
        [Description("Saturday")]
        Saturday = 7,
        [Description("Day")]
        Day = 8,
        [Description("Weekday")]
        Weekday = 9,
        [Description("Weekend Day")]
        WeekendDay = 10
    }

    #endregion

    public enum FileType
    {
        [Description("Audio")]
        Audio = 0,
        [Description("Video")]
        Video = 1
    }
}