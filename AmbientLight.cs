using Microsoft.Xna.Framework;

class AmbientLight
{
    public static readonly Color day = new Color(0f, 0f, 0f, 1f);
    public static readonly Color twilight = new Color(150f / 255f, 100f / 255f, 0f, .5f);
    public static readonly Color twilightNight = new Color(150f / 255f, 50f / 255f, 0f, .5f);
    public static readonly Color night = new Color(0f, 0f, 0f, 0f);

    //sun peeks out at 7 
    public static readonly int hourDawn = 7;
    //sun is fully out at 8
    public static readonly int hourSunrise = 8;
    //normal daytime conditions start at 9
    public static readonly int hourDayStart = 9;
    //normal daytime conditions terminate at 19.
    public static readonly int hourDayEnd = 19;
    //golden color hits at 20
    public static readonly int hourSunset = 20;
    //cease any light at 21.
    public static readonly int hourDusk = 21;
        
    public static Color ambientColor(Clock c)
    {
        
        if (c.hours() < hourDawn || c.hours() >= hourDusk)
        {
            return night;
        }
        else if (c.hours() == hourDawn)
        {
            return Utils.mix(60f,c.minutes() + (c.seconds() / 60f), night, twilight);
        }
        else if (c.hours() == hourSunrise)
        {
            return Utils.mix(60f ,c.minutes() + (c.seconds() / 60f), twilight, day);
        }
        
        else if (c.hours() == hourDayEnd)
        {
            return Utils.mix(60f, c.minutes() + (c.seconds() / 60f), day, twilightNight);
        }
        else if (c.hours() == hourSunset)
        {
            return Utils.mix(60f, c.minutes() + (c.seconds() / 60f), twilightNight, night);
        }
        else
    
            return day;
    }
}
