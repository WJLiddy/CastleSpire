using Microsoft.Xna.Framework;

class InGame
{
    
    PC player;
    Clock clock;


    public static Map map;
    HUD h,h1,h2,h3;


    public InGame(int race)
    {
        player = new PC(race);

        h = new HUD(player, HUD.Corner.TOPLEFT);
        h1 = new HUD(player, HUD.Corner.TOPRIGHT);
        h2 = new HUD(player, HUD.Corner.BOTTOMLEFT);
        h3 = new HUD(player, HUD.Corner.BOTTOMRIGHT);
        map = new Map(@"maps\testmap.xml");
        clock = new Clock();
    }

    //Load each of the characters.
    public GS.State update(GameTime delta)
    {
        player.update(GS.input,delta);

        // day -> 1 hour
        // 24 hours -> 60 minutes
        // hour 75 seconds

        // minute 75 / 60 seconds

        //75.0 / (60.0 * 60.0)

        //How many sec per minue
        clock.tick((60.0 / 75.0));
        return GS.State.InGame;

    }

    public void draw()
    {

        Utils.gfx.Clear(Color.White);

        //figure out camera stuff
        int cameraX = player.x - (CastleSpire.baseWidth / 2);
        int cameraY = player.y - (CastleSpire.baseHeight / 2);

        //Map drawing outside it's limits.
        map.drawBase(cameraX, cameraY, CastleSpire.baseWidth, CastleSpire.baseHeight);

       for (int y = 0; y != CastleSpire.baseHeight; y++)
        {
            map.drawObjectLine(cameraX, cameraY, CastleSpire.baseWidth, CastleSpire.baseHeight,y);

            //THIS WILL WORK when drawObjectLine correctly does
            //Keep in mind draw object line should look for a low wall then rise up.
            //What this is doing is over-drawing all of the walls. 
            if ((cameraY + y) == (player.y))
              player.draw(cameraX, cameraY);
        }

        Utils.drawRect(clock.color(), 0, 0, CastleSpire.baseWidth, CastleSpire.baseHeight);

        //Nancies();

    

        




        
      //  h.draw();
        h1.draw();
        h2.draw();
        h3.draw();
        ClockHUD.draw(clock);

        

    }


    public void Nancies(int cameraX, int cameraY)
    {
        
    System.Random r = new System.Random();

    for (int i = 0; i != 1000; i++)
    {
        player.draw(cameraX + (int)(r.NextDouble()*400.0) - 200, cameraY + (int)( r.NextDouble() * 400) - 200 );

    }

    Utils.drawString("TOO MANY NANCIIIEEES", 20, 50, new Color((float)(r.NextDouble() ), (float)(r.NextDouble()), (float)(r.NextDouble() )), 3, true);

    }
}
