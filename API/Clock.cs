using Microsoft.Xna.Framework;
using System;

public class Clock
{
    private TimeSpan time;
    private static readonly TimeSpan second = new TimeSpan(0, 0, 1);
    private double deltaSec;

    public Clock()
    {
        time = new TimeSpan(1, 7, 30, 0);
        deltaSec = 0;
    }

    public void tick(double dsec)
    {
        deltaSec += dsec;
        while (deltaSec >= 1)
        {
            time = time.Add(second);
            deltaSec = deltaSec - 1;
        }
    }

    public int seconds()
    {
        return time.Seconds;
    }

    public int minutes()
    {
        return time.Minutes;
    }

    public int hours()
    {
        return time.Hours;
    }

    public int days()
    {
        return time.Days;
    }

    public String monthDay()
    {
        return "OCT " + time.Days;
    }

    public String hourMin()
    {
        int hour = time.Hours;
        if (time.Hours == 0)
        {
            hour = 12;
        }
        if (time.Hours >= 13)
        {
            hour = time.Hours - 12;
        }
        return hour + ":" + time.Minutes.ToString("00");
    }

    public String AMPM()
    {
        return (time.Hours >= 12 ? "PM" : "AM");
    }
}


