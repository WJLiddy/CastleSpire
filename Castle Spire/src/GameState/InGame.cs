using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class InGame
{
    
    ADFont f;
    PC player;
    

    public InGame(int race)
    {
        player = new PC(race);
        f = new ADFont(@"misc\spireFont.png");

    }

    //Load each of the characters.
    public GS.State update(GameTime delta)
    {
        player.update(GS.input,delta);
        return GS.State.InGame;
    }

    public void draw()
    {
        Utils.gfx.Clear(Color.NavajoWhite);
        player.draw();
    }


    /**
           input.update(ks);

        if (input.UP)
            pirateAnimSet.hold("idle", 0, 0);
        else if (input.RIGHT)
            pirateAnimSet.hold("idle", 0, 1);
        else if (input.DOWN)
            pirateAnimSet.hold("idle", 0, 2);
        else if (input.LEFT)
            pirateAnimSet.hold("idle", 0, 3);
    */
}
