using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Light
{
    public Texture2D texture { get; private set; }
    public int center_x;
    public int center_y;
    private float r;
    private float g;
    private float b;
    private float strength;
    private int fallOff100;

    public Light(float r, float g, float b, int x, int y,int fallOff100)
    {
        normalize(r, g, b);
        this.center_x = x;
        this.center_y = y;
        this.fallOff100 = fallOff100;
        this.texture = generateLightTexture();
    }

    //generates linear fall-off lightsource.
    private Texture2D generateLightTexture()
    {
        Color[] lightMap = new Color[fallOff100 * fallOff100 * 4];

        fill(lightMap, fallOff100*2, fallOff100*2, new Color(0,0,0,0f));

        for (int lit_x = 0; lit_x != (fallOff100*2); lit_x++)
        {
            for (int lit_y = 0; lit_y != (fallOff100*2); lit_y++)
            {
                if ((Utils.dist(lit_x, fallOff100, lit_y, fallOff100) < fallOff100))
                {
                    float delta = (float)(fallOff100 - Utils.dist(lit_x, fallOff100, lit_y, fallOff100)) / fallOff100;
                    lightMap[lit_x + ((fallOff100 * 2) * (lit_y))] = new Color(delta*r, delta*g, delta*b, strength*delta);
                }
            }
        }

        Texture2D map = new Texture2D(Utils.gfx, fallOff100 * 2, fallOff100 * 2);
        map.SetData<Color>(lightMap);
        return map;
    }
    
    public static void fill(Color[] lightMap, int w, int h, Color f)
    {
        for (int dx = 0; dx != w; dx++)
        {
            for (int dy = 0; dy != h; dy++)
            {
                lightMap[dx + (w * dy)] = f;
            }
        }
    }

    public int getX()
    {
        return center_x - fallOff100;
    }

    public int getY()
    {
        return center_y - fallOff100;
    }

    private void normalize(float r, float g, float b)
    {
        this.strength = Math.Max(Math.Max(r, g), b);
        float min = Math.Min(Math.Min((float)r, (float)g), (float)b);
        this.r = r - min;
        this.g = g - min;
        this.b = b - min;
    }
}
