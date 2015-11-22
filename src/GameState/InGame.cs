using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

class InGame
{
    //TODO: Proper data struct
    PC[] Players = new PC[4];
    private LinkedList<PC> PlayerList = new LinkedList<PC>();

    HUD[] HUDs = new HUD[4];
    Color SoulCol = new Color(0, 0, 0.1f, 0.5f);
    Clock Clock;
    double TickTime = (60.0 / 75.0);
    LightMap Lightmap;
    Light L;
    LinkedList<Item> FloorItems;


    Color[] LosTemp;
    Texture2D[] PlayerLosOverlay;
    RenderTarget2D TotalLosOverlay;

    public static ObliqueMap Map;
    int LOSturn = 0;

    public InGame(int race)
    {
        SoundManager.Stop();
        Players[0] = new PC(race);
        PlayerList.AddFirst(Players[0]);
        HUDs[0] = new HUD(Players[0], HUD.Corner.TOPLEFT);

        Map = new ObliqueMap(@"maps\testmap.xml", CastleSpire.BaseWidth, CastleSpire.BaseHeight);

        Lightmap = new LightMap(CastleSpire.BaseWidth, CastleSpire.BaseHeight);
        FloorItems = new LinkedList<Item>();

        TotalLosOverlay = new RenderTarget2D(Renderer.GraphicsDevice, CastleSpire.BaseWidth, CastleSpire.BaseHeight);
        LosTemp = new Color[CastleSpire.BaseWidth * CastleSpire.BaseHeight];
        PlayerLosOverlay = new Texture2D[4];

        for (int i = 0; i != 4; i++)
        {
           PlayerLosOverlay[i] = new Texture2D(Renderer.GraphicsDevice, CastleSpire.BaseWidth, CastleSpire.BaseHeight);
        }

        L = new Light(.1f, .1f, .2f, 50, 50, 50);
        Lightmap.AddLight(L);
       
        Lightmap.AddLight(new Light(.1f, .0f, .0f, 300, 100, 100));
        Lightmap.AddLight(new Light(.0f, .0f, .1f, 400, 200, 100));
        Lightmap.AddLight(new Light(.0f, .1f, .0f, 500, 300, 100));
        Lightmap.AddLight(new Light(.3f, .0f, .3f, 100, 100, 50));
        Lightmap.AddLight(new Light(.3f, .3f, .0f, 200, 150, 50));
        Lightmap.AddLight(new Light(.0f, .3f, .3f, 300, 200, 50));
        Lightmap.AddLight(new Light(.5f, .5f, .5f, 300, 600, 70));
        Clock = new Clock();

        FloorItems.AddFirst(new Item(@"items\melee\axe.xml",300,300));

        SoundManager.Play("night.ogg", true);
    }

    //Load each of the characters.
    public GS.State Update(int ms)
    {
        Join();

        UpdatePlayersAndHud(ms);

        Clock.Tick(TickTime);
        return GS.State.InGame;
    }

    public void Draw(AD2SpriteBatch sb)
    {


        Renderer.GraphicsDevice.Clear(Color.White);

        //figure out camera stuff
        int cameraX = 0; 
        int cameraY = 0; 

        foreach(PC p in allPlayers())
        {
            cameraX += p.X - (CastleSpire.BaseWidth / 2);
            cameraY += p.Y - (CastleSpire.BaseHeight / 2);
        }

        cameraX /= allPlayers().Count;
        cameraY /= allPlayers().Count;

        //no soul lighitng
       // l.center_x = p.x + player.size/2;
        //l.center_y = player.y + player.size/2;

        DrawWorldNoLighting(sb, cameraX,cameraY);
        Lightmap.RenderLightMap(sb, AmbientLight.AmbientColor(Clock), cameraX, cameraY, CastleSpire.BaseWidth, CastleSpire.BaseHeight);
        

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
            if(HUDs[i] != null)
                HUDs[i].Draw(sb);
        }

        ClockHUD.Draw(sb, Clock);

    }

    private void DrawWorldNoLighting(AD2SpriteBatch sb, int cameraX, int cameraY)
    {
        Map.drawBase(sb, cameraX, cameraY, CastleSpire.BaseWidth, CastleSpire.BaseHeight);

        Item.DrawGlowingItems(sb, allPlayers(), FloorItems, cameraX, cameraY);
        foreach( Item i in FloorItems)
        {
            i.Draw(sb, cameraX,cameraY);
        }

        for (int y = 0; y != CastleSpire.BaseHeight + 120; y++)
        {
            Map.drawObjectLine(sb, cameraX, cameraY, CastleSpire.BaseWidth, CastleSpire.BaseHeight, y);

            //THIS WILL WORK when drawObjectLine correctly does
            //Keep in mind draw object line should look for a low wall then rise up.
            //What this is doing is over-drawing all of the walls. 
            foreach (PC p in allPlayers())
            {
                if ((cameraY + y) == (p.Y + (p.Size - 1)))
                    p.Draw(sb, cameraX, cameraY);
            }
        }

        Map.drawAlways(sb, cameraX, cameraY, CastleSpire.BaseWidth, CastleSpire.BaseHeight);
    }

    private void Join()
    {
        for (int i = 1; i != 4; i++)
        {
            //have people join. 
            if (Players[i] == null)
            {
                if (GS.Inputs[i].PressedUp)
                    Players[i] = new PC((int)RaceUtils.Race.Pirate);
                else if (GS.Inputs[i].PressedRight)
                    Players[i] = new PC((int)RaceUtils.Race.Dragon);
                else if (GS.Inputs[i].PressedDown)
                    Players[i] = new PC((int)RaceUtils.Race.Meximage);
                else if (GS.Inputs[i].PressedLeft)
                    Players[i] = new PC((int)RaceUtils.Race.Ninja);

                if (Players[i] != null)
                {
                    HUDs[i] = new HUD(Players[i], (HUD.Corner)i);
                    PlayerList.AddFirst(Players[i]);
                }
            }
        }
    }


    public void UpdatePlayersAndHud(int ms)
    {
        for (int i = 0; i != 4; i++)
        {
            if (Players[i] != null)
            {
                Players[i].Update(GS.Inputs[i], ms);
                HUDs[i].Update(GS.Inputs[i], ms);
            }
        }
    }

    private Texture2D GetLOS(int cameraX, int cameraY)
    {
        LOSturn = ++LOSturn % PlayerList.Count;

        //100% opaque
        Color opaque = new Color(0,0,0,1f);
        
        //faster using GPU
        for (int i = 0; i != CastleSpire.BaseHeight * CastleSpire.BaseWidth; i++)
        {
            LosTemp[i] = opaque;
        }

        //Now, when we raycast, we update the the associated player's LOS if it is their turn.
        Raycast(Players[LOSturn],LosTemp,cameraX,cameraY);

        PlayerLosOverlay[LOSturn].SetData<Color>(LosTemp);
        //now we have updated this character's LOS.

        Renderer.GraphicsDevice.SetRenderTarget(TotalLosOverlay);
        Renderer.GraphicsDevice.Clear(opaque);

        //TODO : use this on LIGHTS!!!!
        BlendState zeroOr = new BlendState();
        zeroOr.AlphaBlendFunction = BlendFunction.Min;
        zeroOr.AlphaDestinationBlend = Blend.One;
        zeroOr.AlphaSourceBlend = Blend.One;

        SpriteBatch b = new SpriteBatch(Renderer.GraphicsDevice);

        b.Begin(SpriteSortMode.Deferred,zeroOr,null,null,null);

        for (int i = 0; i != PlayerList.Count; i++)
        {
            b.Draw(PlayerLosOverlay[i],new Rectangle(0,0,CastleSpire.BaseWidth,CastleSpire.BaseHeight), Color.White);
        }
        b.End();
        Renderer.GraphicsDevice.SetRenderTarget(null);
        
        //We need to OR all of the lines of sight.

        return TotalLosOverlay;
    }

    private void Raycast(PC p, Color[] losData, int cameraX, int cameraY)
    {
        int playerX = p.X + (p.Size / 2);
        int playerY = p.Y + (p.Size / 2);
        //for top
        int destY = cameraY;

        for (int ddestX = cameraX; ddestX != cameraX + CastleSpire.BaseWidth; ddestX++)
        {
            RaycastLinear(playerX, playerY, ddestX, destY, cameraX, cameraY, losData);       
        }

        //for bottom
        destY = cameraY + CastleSpire.BaseHeight;
        for (int ddestX = cameraX; ddestX != cameraX + CastleSpire.BaseWidth; ddestX++)
        {
            RaycastLinear(playerX, playerY, ddestX, destY, cameraX, cameraY, losData);
        }

        //for left
        int destX = cameraX;
        for (int ddestY = cameraY; ddestY != cameraY + CastleSpire.BaseHeight; ddestY++)
        {
            RaycastLinear(playerX, playerY, destX, ddestY, cameraX, cameraY, losData);
        }

        //for right
        destX = cameraX + CastleSpire.BaseWidth;
        for (int ddestY = cameraY; ddestY != cameraY + CastleSpire.BaseHeight; ddestY++)
        {
            RaycastLinear(playerX, playerY, destX, ddestY, cameraX, cameraY, losData);
        }
    }

    private void RaycastLinear(int startX, int startY, int destX, int destY, int cameraX, int cameraY, Color[] losData)
    {
        int distToDest = (int)(1 + Utils.Dist(startX, destX, startY, destY));

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


            int index = ((x - cameraX) + (CastleSpire.BaseWidth * (y - cameraY)));

            if (Map.wall[x, y])
                return;
            else if (index >= 0 && index < CastleSpire.BaseWidth * CastleSpire.BaseHeight)
                losData[index] = Color.Transparent;

        }


    }

    public LinkedList<PC> allPlayers()
    {
        return PlayerList;
    }
}
