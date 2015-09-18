using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;


public class Utils
{
    public static GraphicsDevice gfx;
    public static SpriteBatch sb;
    public static string pathToAssets;
    public static Texture2D rect;
    public static ADFont f;

    public static Texture2D TextureLoader(String pathToTexture)
    {
        System.IO.Stream stream = File.Open(Utils.pathToAssets + pathToTexture, FileMode.Open);
        Texture2D t =  Texture2D.FromStream(gfx, stream);
        stream.Close();
        return t;
    }

    public static void load()
    {
        rect = Utils.TextureLoader(@"misc/rect.png");
        f = new ADFont(@"misc/spireFont.png");
    }

    public static void drawTexture(Texture2D t, int x, int y)
    {
        sb.Draw(t, new Rectangle(x, y, t.Bounds.Width, t.Bounds.Height), Color.White);   
    }

    public static void drawTexture(Texture2D t, int x, int y, Color c)
    {
        sb.Draw(t, new Rectangle(x, y, t.Bounds.Width, t.Bounds.Height), c);
    }

    public static void drawTexture(Texture2D t, int x, int y, int w, int h)
    {
        sb.Draw(t, new Rectangle(x, y, w, h), Color.White);
    }

    public static void drawTexture(Texture2D t, int x, int y, int w, int h, Color c)
    {
        sb.Draw(t, new Rectangle(x, y, w, h), c);
    }

    public static void drawTexture(object logo, int x, int y, int size1, int size2)
    {
        throw new NotImplementedException();
    }

    public static void drawRect(Color c, int x, int y, int w, int h)
    {
        Utils.drawTexture(rect, x, y, w, h, c);
    }

    public static void drawString(string s, int x, int y, Color c, int scale = 1, bool outline = false)
    {
        f.draw(s, x,y, c, scale, outline);
    }
}


