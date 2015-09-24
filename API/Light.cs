using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class Light
{
    Color c;
    int x;
    int y;
    int fallOff50;
    int fallOff100;

    public Light(Color c, int x, int y, int fallOff50, int fallOff100)
    {
        this.c = c;
        this.x = x;
        this.y = y;
        this.fallOff50 = fallOff50;
        this.fallOff100 = fallOff100;
    }
    
    public static Texture2D createLightMap(Color ambient, LinkedList<Light> Lights, int camX, int camY, int w, int h)
    {
        //first make a light map that is the size of the whole screen.
        Color[] lightMap = new Color[w * h];

        //Make it all ambient.
        fillLightMap(lightMap,w,h,ambient);
 
        foreach (Light l in Lights)
        {
            for (int lit_x = l.x - (l.fallOff100); lit_x != l.x + (l.fallOff100); lit_x++)
            { 
                for (int lit_y = l.y - (l.fallOff100); lit_y != l.y + (l.fallOff100);lit_y++)
                { 
                    if (inCameraArray(lit_x,lit_y,camX,camY,w, h) && (Utils.dist(lit_x, l.x, lit_y, l.y) < l.fallOff100))
                    {
                        //TODO draw a diagram
                        if ((Utils.dist(lit_x, l.x, lit_y, l.y) < l.fallOff50))
                            lightMap[(lit_x - camX) + (w * (lit_y - camY))] = Utils.mix(l.fallOff50 * 2f, (float)Utils.dist(lit_x, l.x, lit_y, l.y), l.c, lightMap[(lit_x - camX) + (w * (lit_y - camY))]);
                        else
                            lightMap[(lit_x - camX) + (w * (lit_y - camY))] = Utils.mix((l.fallOff100 - l.fallOff50), (.5f* (l.fallOff100 - l.fallOff50)) + (.5f * (((float)Utils.dist(lit_x, l.x, lit_y, l.y)) - l.fallOff50)), l.c, lightMap[(lit_x - camX) + (w * (lit_y - camY))]);
                    }
                }
            }
        }

        Texture2D lightMapTexture = new Texture2D(Utils.gfx,w,h);
        lightMapTexture.SetData<Color>(lightMap);
        return lightMapTexture;
    }

    //TODO: Have a prepared lightmap I can clone
    private static void fillLightMap(Color[] lightMap, int w, int h, Color ambient)
    {
        for (int x = 0; x != w; x++)
        {
            for (int y = 0; y != h; y++)
            {
                lightMap[x + (w * y)] = ambient;
            }

        }
    }

    private static bool inCameraArray(int lit_x, int lit_y, int camX, int camY, int w, int h)
    {
        return 0 <= (lit_x - camX) && (lit_x - camX) < w && 0 <= (lit_y - camY) && (lit_y - camY) < h;
    }

}
