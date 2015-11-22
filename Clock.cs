using Microsoft.Xna.Framework;
using System;

public class Clock
{
    private TimeSpan Time;
    private static readonly TimeSpan Second = new TimeSpan(0, 0, 1);
    private double DeltaSec;

    public Clock()
    {
        Time = new TimeSpan(1, 9, 30, 0);
        DeltaSec = 0;
    }

    public void Tick(double dsec)
    {
        DeltaSec += dsec;
        while (DeltaSec >= 1)
        {
            Time = Time.Add(Second);
            DeltaSec = DeltaSec - 1;
        }
    }

    public int Seconds()
    {
        return Time.Seconds;
    }

    public int Minutes()
    {
        return Time.Minutes;
    }

    public int Hours()
    {
        return Time.Hours;
    }

    public int Days()
    {
        return Time.Days;
    }

    public string MonthDay()
    {
        return "OCT " + Time.Days;
    }

    public string HourMin()
    {
        int hour = Time.Hours;
        if (Time.Hours == 0)
        {
            hour = 12;
        }
        if (Time.Hours >= 13)
        {
            hour = Time.Hours - 12;
        }
        return hour + ":" + Time.Minutes.ToString("00");
    }

    public string AMPM()
    {
        return (Time.Hours >= 12 ? "PM" : "AM");
    }
}


