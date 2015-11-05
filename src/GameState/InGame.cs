using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

class InGame
{
    //TODO: Proper data struct
    PC[] players = new PC[4];
    private LinkedList<PC> playerList = new LinkedList<PC>();

    HUD[] huds = new HUD[4];
    Color soulcol = new Color(0, 0, 0.1f, 0.5f);
    Clock clock;
    double TICK_TIME = (60.0 / 75.0);
    LightMap lightmap;
    Light l;
    LinkedList<Item> floorItems;


    Color[] LosTemp;
    Texture2D[] playerLosOverlay;
    RenderTarget2D totalLosOverlay;

    public static Map map;
    int LOSturn = 0;

    public InGame(int race)
    {
        Utils.soundManager.stop();
        players[0] = new PC(race);
        playerList.AddFirst(players[0]);
        huds[0] = new HUD(players[0], HUD.Corner.TOPLEFT);

        map = new Map(@"maps\testmap.xml", CastleSpire.baseWidth, CastleSpire.baseHeight);
        lightmap = new LightMap(CastleSpire.baseWidth, CastleSpire.baseHeight);
        floorItems = new LinkedList<Item>();

        totalLosOverlay = new RenderTarget2D(Utils.gfx, CastleSpire.baseWidth, CastleSpire.baseHeight);
        LosTemp = new Color[CastleSpire.baseWidth * CastleSpire.baseHeight];
        playerLosOverlay = new Texture2D[4];

        for (int i = 0; i != 4; i++)
        {
           playerLosOverlay[i] = new Texture2D(Utils.gfx, CastleSpire.baseWidth, CastleSpire.baseHeight);
        }

        l = new Light(.1f, .1f, .2f, 50, 50, 50);
        lightmap.addLight(l);
       
        lightmap.addLight(new Light(.1f, .0f, .0f, 300, 100, 100));
        lightmap.addLight(new Light(.0f, .0f, .1f, 400, 200, 100));
        lightmap.addLight(new Light(.0f, .1f, .0f, 500, 300, 100));
        lightmap.addLight(new Light(.3f, .0f, .3f, 100, 100, 50));
        lightmap.addLight(new Light(.3f, .3f, .0f, 200, 150, 50));
        lightmap.addLight(new Light(.0f, .3f, .3f, 300, 200, 50));
        lightmap.addLight(new Light(.5f, .5f, .5f, 300, 600, 70));
        clock = new Clock();

        floorItems.AddFirst(new Item(@"items\melee\axe.xml",300,300));

        Utils.soundManager.play("night.ogg", true);
    }

    //Load each of the characters.
    public GS.State update(GameTime delta)
    {
        join();

        updatePlayersAndHud(delta);

        clock.tick(TICK_TIME);
        return GS.State.InGame;
    }

    public void draw()
    {


        Utils.gfx.Clear(Color.White);

        //figure out camera stuff
        int cameraX = 0; 
        int cameraY = 0; 

        foreach(PC p in allPlayers())
        {
            cameraX += p.x - (CastleSpire.baseWidth / 2);
            cameraY += p.y - (CastleSpire.baseHeight / 2);
        }

        cameraX /= allPlayers().Count;
        cameraY /= allPlayers().Count;

        //no soul lighitng
       // l.center_x = p.x + player.size/2;
        //l.center_y = player.y + player.size/2;

        drawWorldNoLighting(cameraX,cameraY);
        lightmap.renderLightMap(AmbientLight.ambientColor(clock), cameraX, cameraY, CastleSpire.baseWidth, CastleSpire.baseHeight);
        

        /*** LIGHTING STUFF    
          /**
          //Before dawn, draw a lightsource at the player spot. That way he/she can see a little bit.
          if (clock.hours() < AmbientLight.hourDawn || clock.hours() >= AmbientLight.hourDusk)
          {
              soul = new Light(soulcol, player.x + (player.size / 2), player.y + (player.size / 2), 7, 40);
              lights.AddFirst(soul);
          }
          if (clock.hours() == AmbientLight.hourDawn && (clock.minutes() < 20))
          {
              soul = new Light(Utils.mix(20 * 60,(60 * clock.minutes()) + clock.seconds(),soulcol, AmbientLight.ambientColor(clock)), player.x + (player.size / 2), player.y + (player.size / 2), 7,40);
              lights.AddFirst(soul);
          }
         if (clock.hours() == AmbientLight.hourSunset && (clock.minutes() > 40))
         {
             soul = new Light(Utils.mix(20 * 60, (60 * (clock.minutes() - 40 ) ) + clock.seconds(), AmbientLight.ambientColor(clock), soulcol) , player.x + (player.size / 2), player.y + (player.size / 2), 7, 40);
             lights.AddFirst(soul);
         }
         //Nancies();
         */
        for (int i = 0; i != 4; i++)
        {
            if(huds[i] != null)
                huds[i].draw();
        }

        ClockHUD.draw(clock);

    }

    private void drawWorldNoLighting(int cameraX, int cameraY)
    {
        map.drawBase(cameraX, cameraY, CastleSpire.baseWidth, CastleSpire.baseHeight);

        Item.drawGlowingItems(allPlayers(), floorItems, cameraX, cameraY);
        foreach( Item i in floorItems)
        {
            i.draw(cameraX,cameraY);
        }

        for (int y = 0; y != CastleSpire.baseHeight + Map.MAX_OBJECT_HEIGHT; y++)
        {
            map.drawObjectLine(cameraX, cameraY, CastleSpire.baseWidth, CastleSpire.baseHeight, y);

            //THIS WILL WORK when drawObjectLine correctly does
            //Keep in mind draw object line should look for a low wall then rise up.
            //What this is doing is over-drawing all of the walls. 
            foreach (PC p in allPlayers())
            {
                if ((cameraY + y) == (p.y + (p.size - 1)))
                    p.draw(cameraX, cameraY);
            }
        }

        map.renderRoofs(getLOS(cameraX, cameraY), cameraX, cameraY, CastleSpire.baseWidth, CastleSpire.baseHeight);

        map.drawAlways(cameraX, cameraY, CastleSpire.baseWidth, CastleSpire.baseHeight);
    }

    private void join()
    {
        for (int i = 1; i != 4; i++)
        {
            //have people join. 
            if (players[i] == null)
            {
                if (GS.inputs[i].pressedUP)
                    players[i] = new PC((int)RaceUtils.Race.Pirate);
                else if (GS.inputs[i].pressedRIGHT)
                    players[i] = new PC((int)RaceUtils.Race.Dragon);
                else if (GS.inputs[i].pressedDOWN)
                    players[i] = new PC((int)RaceUtils.Race.Meximage);
                else if (GS.inputs[i].pressedLEFT)
                    players[i] = new PC((int)RaceUtils.Race.Ninja);

                if (players[i] != null)
                {
                    huds[i] = new HUD(players[i], (HUD.Corner)i);
                    playerList.AddFirst(players[i]);
                }
            }
        }
    }


    public void updatePlayersAndHud(GameTime delta)
    {
        for (int i = 0; i != 4; i++)
        {
            if (players[i] != null)
            {
                players[i].update(GS.inputs[i], delta);
                huds[i].update(GS.inputs[i], delta);
            }
        }
    }

    private Texture2D getLOS(int cameraX, int cameraY)
    {
        LOSturn = ++LOSturn % playerList.Count;

        //100% opaque
        Color opaque = new Color(0,0,0,1f);
        
        //faster using GPU
        for (int i = 0; i != CastleSpire.baseHeight * CastleSpire.baseWidth; i++)
        {
            LosTemp[i] = opaque;
        }

        //Now, when we raycast, we update the the associated player's LOS if it is their turn.
        raycast(players[LOSturn],LosTemp,cameraX,cameraY);

        playerLosOverlay[LOSturn].SetData<Color>(LosTemp);
        //now we have updated this character's LOS.

        Utils.gfx.SetRenderTarget(totalLosOverlay);
        Utils.gfx.Clear(opaque);

        //TODO : use this on LIGHTS!!!!
        BlendState zeroOr = new BlendState();
        zeroOr.AlphaBlendFunction = BlendFunction.Min;
        zeroOr.AlphaDestinationBlend = Blend.One;
        zeroOr.AlphaSourceBlend = Blend.One;

        SpriteBatch b = new SpriteBatch(Utils.gfx);

        b.Begin(SpriteSortMode.Deferred,zeroOr,null,null,null);

        for (int i = 0; i != playerList.Count; i++)
        {
            b.Draw(playerLosOverlay[i],new Rectangle(0,0,CastleSpire.baseWidth,CastleSpire.baseHeight), Color.White);
        }
        b.End();
        Utils.gfx.SetRenderTarget(null);
        
        //We need to OR all of the lines of sight.

        return totalLosOverlay;
    }

    private void raycast(PC p, Color[] losData, int cameraX, int cameraY)
    {
        int playerX = p.x + (p.size / 2);
        int playerY = p.y + (p.size / 2);
        //for top
        int destY = cameraY;

        for (int ddestX = cameraX; ddestX != cameraX + CastleSpire.baseWidth; ddestX++)
        {
            raycastLinear(playerX, playerY, ddestX, destY, cameraX, cameraY, losData);       
        }

        //for bottom
        destY = cameraY + CastleSpire.baseHeight;
        for (int ddestX = cameraX; ddestX != cameraX + CastleSpire.baseWidth; ddestX++)
        {
            raycastLinear(playerX, playerY, ddestX, destY, cameraX, cameraY, losData);
        }

        //for left
        int destX = cameraX;
        for (int ddestY = cameraY; ddestY != cameraY + CastleSpire.baseHeight; ddestY++)
        {
            raycastLinear(playerX, playerY, destX, ddestY, cameraX, cameraY, losData);
        }

        //for right
        destX = cameraX + CastleSpire.baseWidth;
        for (int ddestY = cameraY; ddestY != cameraY + CastleSpire.baseHeight; ddestY++)
        {
            raycastLinear(playerX, playerY, destX, ddestY, cameraX, cameraY, losData);
        }
    }

    private void raycastLinear(int startX, int startY, int destX, int destY, int cameraX, int cameraY, Color[] losData)
    {
        int distToDest = (int)(1 + Utils.dist(startX, destX, startY, destY));

        double dx = destX - startX;
        double dy = destY - startY;


        double xDouble = startX;
        double yDouble = startY;  
        double deltaSize = .5;
        double incX = deltaSize * (dx / distToDest);
        double incY = deltaSize * (dy / distToDest);

        for (double delta = 0; delta < distToDest; delta += deltaSize)
        {
            xDouble += incX;
            yDouble += incY;
            int x = (int)xDouble;
            int y = (int)yDouble;


            int index = ((x - cameraX) + (CastleSpire.baseWidth * (y - cameraY)));

            if (map.wall[x, y])
                return;
            else if (index >= 0 && index < CastleSpire.baseWidth * CastleSpire.baseHeight)
                losData[index] = Color.Transparent;

        }


    }

    public LinkedList<PC> allPlayers()
    {
        return playerList;
    }
}
