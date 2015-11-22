using Microsoft.Xna.Framework;

class ClockHUD
{
    public static int Width = 57;

    public static void Draw (AD2SpriteBatch sb, Clock c)
    {

        Utils.DrawRect(sb, (CastleSpire.BaseWidth / 2) - (Width / 2), CastleSpire.BaseHeight - 7, Width, 7, Color.Black);

        Utils.DefaultFont.Draw(sb, c.MonthDay(), (CastleSpire.BaseWidth / 2) - (Width / 2) + 1, CastleSpire.BaseHeight - 7,Color.Orange,1);

        Utils.DefaultFont.Draw(sb, c.HourMin(), (CastleSpire.BaseWidth / 2) - 1, CastleSpire.BaseHeight - 7, Color.Orange, 1);

        Utils.DefaultFont.Draw(sb, c.AMPM(), (CastleSpire.BaseWidth / 2) + 17, CastleSpire.BaseHeight - 7, Color.Orange, 1);

    }

}
