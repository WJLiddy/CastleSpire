using Microsoft.Xna.Framework;

class InGame
{
    
    ADFont f;
    PC player;
    public static Map map;


    public InGame(int race)
    {
        player = new PC(race);
        f = new ADFont(@"misc\spireFont.png");
        map = new Map(@"maps\testmap.xml");
    }

    //Load each of the characters.
    public GS.State update(GameTime delta)
    {
        player.update(GS.input,delta);
        return GS.State.InGame;
    }

    public void draw()
    {
        
        Utils.gfx.Clear(Color.White);

        //figure out camera stuff
        int cameraX = player.x - (CastleSpire.baseWidth / 2);
        int cameraY = player.y - (CastleSpire.baseHeight / 2);

        //Map drawing outside it's limits.
        map.draw(cameraX , cameraY, CastleSpire.baseWidth, CastleSpire.baseHeight);

        player.draw(cameraX, cameraY);
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
