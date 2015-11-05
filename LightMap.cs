using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class LightMap
{
    private LinkedList<Light> lights;

    private RenderTarget2D lightMap;
    private SpriteBatch lightBatch;

    private RenderTarget2D transMap;
    private SpriteBatch transBatch;

    private BlendState alphacomp;

    public LightMap(int w, int h)
    {
        // Create a new render target
        lightMap = new RenderTarget2D(Utils.gfx, w, h);
        lights = new LinkedList<Light>();
        lightBatch = new SpriteBatch(Utils.gfx);
        transMap = new RenderTarget2D(Utils.gfx,w, h);
        transBatch = new SpriteBatch(Utils.gfx);

        alphacomp = new BlendState();

        //Pass alpha as strength of light. It is additive.
        alphacomp.AlphaDestinationBlend = Blend.One;
        alphacomp.AlphaSourceBlend = Blend.One;

        //Not sold on color yet.
        alphacomp.ColorDestinationBlend = Blend.One;
        alphacomp.ColorSourceBlend = Blend.One;
    }

    public void addLight(Light l)
    {
        lights.AddFirst(l);
    }

    public void renderLightMap(Color ambient, int camX, int camY, int w, int h)
    {

          Utils.gfx.SetRenderTarget(lightMap);
          clearToAmbient(ambient);

         lightBatch.Begin(SpriteSortMode.Deferred, alphacomp, null, null, null);

        foreach (Light l in lights)
        {
            drawLight(l,camX,camY);
        }

         lightBatch.End();

        Utils.gfx.SetRenderTarget(null);

        toTransparent();
        Utils.drawTexture(transMap,0,0);

    }


    private static bool inCameraArray(int lit_x, int lit_y, int camX, int camY, int w, int h)
    {
        return 0 <= (lit_x - camX) && (lit_x - camX) < w && 0 <= (lit_y - camY) && (lit_y - camY) < h;
    }

    private void drawLight(Light l,int camX, int camY)
    {
        lightBatch.Draw(l.texture, new Rectangle(l.getX() - camX, l.getY() - camY,l.texture.Width,l.texture.Height),Color.White);
    }

    private void clearToAmbient(Color c)
    {
        Utils.gfx.Clear(c);
    } 

    private void toTransparent()
    {
        Utils.gfx.SetRenderTarget(transMap);
        Utils.gfx.Clear(new Color(0, 0, 0, 1f));

        BlendState b = new BlendState();

        //decode alpha by taking it's inverse. Zero means 
        b.AlphaDestinationBlend = Blend.InverseSourceAlpha;
        b.ColorDestinationBlend = Blend.Zero;
        b.AlphaSourceBlend = Blend.Zero;
        b.ColorSourceBlend = Blend.One;
       
        transBatch.Begin(SpriteSortMode.Deferred,b, null, null, null);
        transBatch.Draw(lightMap, new Rectangle(0,0,transMap.Width,transMap.Height), Color.White);

        transBatch.End();
        Utils.gfx.SetRenderTarget(null);
       
    }
}

