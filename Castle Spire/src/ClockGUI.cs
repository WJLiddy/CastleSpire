using Microsoft.Xna.Framework;

class ClockHUD
{
    public static int width = 57;

    public static void draw (Clock c)
    {

        Utils.drawRect(Color.Black, (CastleSpire.baseWidth / 2) - (width / 2), CastleSpire.baseHeight - 7, width, 7);

        Utils.drawString(c.monthDay(), (CastleSpire.baseWidth / 2) - (width / 2) + 1, CastleSpire.baseHeight - 7,Color.Orange,1);

        Utils.drawString(c.hourMin(), (CastleSpire.baseWidth / 2) - 1, CastleSpire.baseHeight - 7, Color.Orange, 1);

        Utils.drawString(c.AMPM(), (CastleSpire.baseWidth / 2) + 17, CastleSpire.baseHeight - 7, Color.Orange, 1);



    }

}
