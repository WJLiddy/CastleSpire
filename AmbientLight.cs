using Microsoft.Xna.Framework;

class AmbientLight
{
    public static readonly Color Day = new Color(0f, 0f, 0f, 1f);
    public static readonly Color Twilight = new Color(150f / 255f, 100f / 255f, 0f, .5f);
    public static readonly Color TwilightNight = new Color(150f / 255f, 50f / 255f, 0f, .5f);
    public static readonly Color Night = new Color(0f, 0f, 0f, 0f);

    //sun peeks out at 7 
    public static readonly int HourDawn = 7;
    //sun is fully out at 8
    public static readonly int HourSunrise = 8;
    //normal daytime conditions start at 9
    public static readonly int HourDayStart = 7;
    //normal daytime conditions terminate at 19.
    public static readonly int HourDayEnd = 19;
    //golden color hits at 20
    public static readonly int HourSunset = 20;
    //cease any light at 21.
    public static readonly int HourDusk = 21;
        
    public static Color AmbientColor(Clock c)
    {
        
        if (c.Hours() < HourDawn || c.Hours() >= HourDusk)
        {
            return Night;
        }
        else if (c.Hours() == HourDawn)
        {
            return Utils.Mix(c.Minutes() + (c.Seconds() / 60f), Night, Twilight);
        }
        else if (c.Hours() == HourSunrise)
        {
            return Utils.Mix(c.Minutes() + (c.Seconds() / 60f), Twilight, Day);
        }
        
        else if (c.Hours() == HourDayEnd)
        {
            return Utils.Mix(c.Minutes() + (c.Seconds() / 60f), Day, TwilightNight);
        }
        else if (c.Hours() == HourSunset)
        {
            return Utils.Mix(c.Minutes() + (c.Seconds() / 60f), TwilightNight, Night);
        }
        else
    
            return Day;
    }
}
