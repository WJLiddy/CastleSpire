using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class Utils
{
    public static GraphicsDevice gfx;
    public static SpriteBatch sb;
    public static string pathToAssets;
    public static Texture2D rect;
    public static ADFont f;
    public static Random random;

    public struct Mix
    {
        public float delta;
        public Color last;
        public Color next;

        public Mix(float d, Color l, Color n)
        {
            delta = d;
            last = l;
            next = n;
        }

        public override bool Equals(Object m)
        {
            return ((Mix)m).delta == delta &&
            ((Mix)m).last.Equals(last) &&
            ((Mix)m).next.Equals(next);
        }

        public override int GetHashCode()
        {
            return ((int)(Int32.MaxValue * delta)/2) + (int)((last.PackedValue / 4)) + (int)((next.PackedValue / 4));
        }
    }


    public static Dictionary<Mix,Color> colorHash;

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
        random = new Random();
        colorHash = new Dictionary<Mix, Color>(1024*1024*32);
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

    /** Hash Edition 
    public static Color mix(float minDuration, float position, Color last, Color next)
    {
        //implement a hash in case we've seen this argument before
        float delta = position / minDuration;

        Mix m = new Mix(delta, last, next);
        if (colorHash.ContainsKey(m))
            return colorHash[m];
        else
        {


            float R = ((last.R / 255f) * (1f - delta)) + (delta * (next.R / 255f));
            float G = ((last.G / 255f) * (1f - delta)) + (delta * (next.G / 255f));
            float B = ((last.B / 255f) * (1f - delta)) + (delta * (next.B / 255f));
            float A = ((last.A / 255f) * (1f - delta)) + (delta * (next.A / 255f));

            colorHash[m] = new Color(R, G, B, A);
            return colorHash[m];
        }
    }
       */

 
    public static Color mix(float minDuration, float position, Color last, Color next)
    {
        float delta = position / minDuration;

        float R = ((last.R / 255f) * (1f - delta)) + (delta * (next.R / 255f));
        float G = ((last.G / 255f) * (1f - delta)) + (delta * (next.G / 255f));
        float B = ((last.B / 255f) * (1f - delta)) + (delta * (next.B / 255f));
        float A = ((last.A / 255f) * (1f - delta)) + (delta * (next.A / 255f));
        return new Color(R, G, B, A);
    }



    public static double dist(int x1, int x2, int y1, int y2)
    {
        return (Math.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2))));
    }

}


