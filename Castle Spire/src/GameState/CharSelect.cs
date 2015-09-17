using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

class CharSelect
{
    private AnimSet pirate;
    private AnimSet ninja;
    private AnimSet meximage;
    private AnimSet dragon;
    private Texture2D arrow;

    public int charSelect { get; private set; } = 0;

    public CharSelect()
    {
        pirate = new AnimSet(@"creatures\pc\pirate\anim.xml");
        ninja = new AnimSet(@"creatures\pc\ninja\anim.xml");
        meximage = new AnimSet(@"creatures\pc\meximage\anim.xml");
        dragon = new AnimSet(@"creatures\pc\dragon\anim.xml");

        pirate.hold("swing", 0, 2);
        ninja.hold("idle", 0, 2);
        meximage.hold("idle", 0, 2);
        dragon.hold("idle", 0, 2);

        arrow = Utils.TextureLoader(@"misc\bigDownArrow.png");

    }

    //Load each of the characters.
    public GS.State update(GameTime delta)
    {
        if(GS.input.pRIGHT)
        {
            charSelect = (charSelect + 1) % 4;
        }

        if (GS.input.pLEFT)
        {
            charSelect = charSelect == 0 ? 3 : charSelect - 1;
        }

        if (GS.input.pRIGHT || GS.input.pLEFT)
        {

            pirate.hold("idle", 0, 2);
            ninja.hold("idle", 0, 2);
            meximage.hold("idle", 0, 2);
            dragon.hold("idle", 0, 2);

            if (charSelect == 0)
                pirate.hold("swing", 0, 2);
            if (charSelect == 1)
                dragon.hold("swing", 0, 2);
            if (charSelect == 2)
                meximage.hold("swing", 0, 2);
            if (charSelect == 3)
                ninja.hold("swing", 0, 2);
        }

        return GS.input.pA ? GS.State.InGame : GS.State.CharSelect;
    }

    public void draw()
    {
        Utils.gfx.Clear(new Color (20,0,28));
        Utils.drawString("CHOOSE yOUR CHARACTER", 50, 50, Color.White, 3);
        pirate.draw(50, 100,24*2,32*2);
        dragon.draw(100, 100, 24 * 2, 32 * 2);
        meximage.draw(150, 100, 24 * 2, 32 * 2);
        ninja.draw(200, 100, 24 * 2, 32 * 2);

        Utils.drawTexture(arrow, 63 + (50 * charSelect), 91, Color.DarkOrange);
    }
}

