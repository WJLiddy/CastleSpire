using Microsoft.Xna.Framework;
using System;

public class Clock
{
    public TimeSpan time;
    public TimeSpan second;

    public Color day;
    public Color twilight;
    public Color night;

    public int hourDawn;
    public int hourSunrise;
    public int hourDayStart;
    public int hourDayEnd;
    public int hourSunset;
    public int hourDusk;

    public double deltaSec;

    public Clock()
    {
        time = new TimeSpan(1, 6, 30, 0);
        second = new TimeSpan(0, 0, 1);

        day = new Color(0f, 0f, 0f, 0f);

        twilight = new Color(40f / 255f, 25f / 255f, 0f, 0.5f);
        night = new Color(0f, 0f, 0f, 1f);

        hourDawn = 7;
        hourSunrise = 8;
        hourDayStart = 9;
        hourDayEnd = 18;
        hourSunset = 19;
        hourDusk = 21;

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
        bool PM = false;

        if (time.Hours >= 12)
        {
            PM = true;
        }

        return (PM ? "PM" : "AM");
    }

    public Color color()
    {
        if (hours() < hourDawn)
        {
            return night;
        }
        else if (hours() < hourSunrise)
        {
            return Utils.mix(60f * (hourSunrise - hourDawn), ((60f * (hours() - hourDawn)) + minutes()) + (seconds() / 60f), night, twilight);
        }
        else if (hours() < hourDayStart)
        {
            return Utils.mix(60f * (hourDayStart - hourSunrise), ((60f * (hours() - hourSunrise)) + minutes()) + (seconds() / 60f), twilight, day);
        }
        else
            return day;
    }
}


