using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

class CharSelect
{
    private AnimationSet pirate;
    private AnimationSet ninja;
    private AnimationSet meximage;
    private AnimationSet dragon;
    private Texture2D arrow;

    public int charSelect { get; private set; } = 0;

    public CharSelect()
    {
        pirate = new AnimationSet(@"creatures\pc\pirate\anim.xml");
        ninja = new AnimationSet(@"creatures\pc\ninja\anim.xml");
        meximage = new AnimationSet(@"creatures\pc\meximage\anim.xml");
        dragon = new AnimationSet(@"creatures\pc\dragon\anim.xml");

        pirate.hold("swing", 0, 2);
        ninja.hold("idle", 0, 2);
        meximage.hold("idle", 0, 2);
        dragon.hold("idle", 0, 2);

        arrow = Utils.TextureLoader(@"misc\bigDownArrow.png");
        Utils.soundManager.play("origin.ogg", true);

    }

    //Load each of the characters.
    public GS.State update(GameTime delta)
    {
        if(GS.inputs[0].pressedRIGHT)
        {
            charSelect = (charSelect + 1) % 4;
        }

        if (GS.inputs[0].pressedLEFT)
        {
            charSelect = charSelect == 0 ? 3 : charSelect - 1;
        }

        if (GS.inputs[0].pressedRIGHT || GS.inputs[0].pressedLEFT)
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

        return GS.inputs[0].pressedFIRE ? GS.State.InGame : GS.State.CharSelect;
    }

    public void draw()
    {
        Utils.gfx.Clear(new Color (20,0,28));
        Utils.drawString("CHOOSE yOUR CHARACTER", 50, 50, Color.White, 3);
        pirate.draw(10 + 50, 10 + 100,24*2,32*2);
        dragon.draw(10 + 100, 10 + 100, 24 * 2, 32 * 2);
        meximage.draw(10 + 150, 10 + 100, 24 * 2, 32 * 2);
        ninja.draw(10 + 200, 10 + 100, 24 * 2, 32 * 2);

        Utils.drawTexture(arrow, 63 + (50 * charSelect), 91, Color.DarkOrange);
    }
}

