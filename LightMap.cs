using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class LightMap
{
    private LinkedList<Light> Lights;

    private RenderTarget2D LightMapR;
    private SpriteBatch LightBatch;

    private RenderTarget2D TransMap;
    private SpriteBatch TransBatch;

    private BlendState Alphacomp;

    public LightMap(int w, int h)
    {
        // Create a new render target
        LightMapR = new RenderTarget2D(Renderer.GraphicsDevice, w, h);
        Lights = new LinkedList<Light>();
        LightBatch = new SpriteBatch(Renderer.GraphicsDevice);
        TransMap = new RenderTarget2D(Renderer.GraphicsDevice, w, h);
        TransBatch = new SpriteBatch(Renderer.GraphicsDevice);

        Alphacomp = new BlendState();

        //Pass alpha as strength of light. It is additive.
        Alphacomp.AlphaDestinationBlend = Blend.One;
        Alphacomp.AlphaSourceBlend = Blend.One;

        //Not sold on color yet.
        Alphacomp.ColorDestinationBlend = Blend.One;
        Alphacomp.ColorSourceBlend = Blend.One;
    }

    public void AddLight(Light l)
    {
        Lights.AddFirst(l);
    }

    public void RenderLightMap(AD2SpriteBatch sb, Color ambient, int camX, int camY, int w, int h)
    {

        Renderer.GraphicsDevice.SetRenderTarget(LightMapR);
        ClearToAmbient(ambient);

        LightBatch.Begin(SpriteSortMode.Deferred, Alphacomp, null, null, null);

        foreach (Light l in Lights)
        {
            DrawLight(l,camX,camY);
        }

         LightBatch.End();

        Renderer.GraphicsDevice.SetRenderTarget(null);

        ToTransparent();
        sb.DrawTexture(TransMap,0,0);

    }


    private static bool InCameraArray(int lit_x, int lit_y, int camX, int camY, int w, int h)
    {
        return 0 <= (lit_x - camX) && (lit_x - camX) < w && 0 <= (lit_y - camY) && (lit_y - camY) < h;
    }

    private void DrawLight(Light l,int camX, int camY)
    {
        LightBatch.Draw(l.Texture, new Rectangle(l.GetX() - camX, l.GetY() - camY,l.Texture.Width,l.Texture.Height),Color.White);
    }

    private void ClearToAmbient(Color c)
    {
        Renderer.GraphicsDevice.Clear(c);
    } 

    private void ToTransparent()
    {
        Renderer.GraphicsDevice.SetRenderTarget(TransMap);
        Renderer.GraphicsDevice.Clear(new Color(0, 0, 0, 1f));

        BlendState b = new BlendState();

        //decode alpha by taking it's inverse. Zero means 
        b.AlphaDestinationBlend = Blend.InverseSourceAlpha;
        b.ColorDestinationBlend = Blend.Zero;
        b.AlphaSourceBlend = Blend.Zero;
        b.ColorSourceBlend = Blend.One;
       
        TransBatch.Begin(SpriteSortMode.Deferred,b, null, null, null);
        TransBatch.Draw(LightMapR, new Rectangle(0,0,TransMap.Width,TransMap.Height), Color.White);

        TransBatch.End();
        Renderer.GraphicsDevice.SetRenderTarget(null);
       
    }
}

