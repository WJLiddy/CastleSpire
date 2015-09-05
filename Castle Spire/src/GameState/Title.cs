using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Title
{
    Texture2D splash;
    ADFont f;
    public Title()
    {
        splash = Utils.TextureLoader(@"misc\splash.png");
        f = new ADFont(@"misc\spireFont.png");
    }

    public GS.State update(GameTime gs, KeyboardState ks)
    {
        if (ks.IsKeyDown(Keys.Enter))
            return GS.State.CharSelect;
        else
            return GS.State.Title;
    }

    public void draw()
    {
        Utils.sb.Draw(splash,new Rectangle(0,0,360,270),Color.White);
        //TODO: f.draw center.
        f.draw("Press ENTER", 100, 200, Color.White, 3, true);
    }
}

