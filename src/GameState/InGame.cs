using System.Collections.Generic;

//Singletonize.
class InGame
{
    //TODO: Proper data struct
    PC[] Players = new PC[4];
    private LinkedList<PC> PlayerList = new LinkedList<PC>();

    HUD[] HUDs = new HUD[4];
    LinkedList<Item> FloorItems;
  
    public static CollisionMap Map;

    public InGame(int race)
    {
        SoundManager.Stop();
        Players[0] = new PC(race);
        PlayerList.AddFirst(Players[0]);
        HUDs[0] = new HUD(Players[0], HUD.Corner.TOPLEFT);

        Map = new CollisionMap(@"maps\zombieBase.xml", CastleSpire.BaseWidth, CastleSpire.BaseHeight);

        FloorItems = new LinkedList<Item>();
        FloorItems.AddFirst(new Item(@"items\melee\axe.xml",200,200));

        SoundManager.Play("night.ogg", true);
    }

    //Load each of the characters.
    public GS.State Update(int ms)
    {
        Join();
        UpdatePlayersAndHud(ms);
        return GS.State.InGame;
    }

    public void Draw(AD2SpriteBatch sb)
    { 
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

        DrawWorld(sb, cameraX, cameraY);
        

        for (int i = 0; i != 4; i++)
        {
            if(HUDs[i] != null)
                HUDs[i].Draw(sb);
        }
    }

    private void DrawWorld(AD2SpriteBatch sb, int cameraX, int cameraY)
    {
        Map.DrawBase(sb, cameraX, cameraY);

        Item.DrawGlowingItems(sb, allPlayers(), FloorItems, cameraX, cameraY);

        foreach( Item i in FloorItems)
        {
            i.Draw(sb, cameraX,cameraY);
        }

        foreach (PC p in allPlayers())
        {
            p.Draw(sb, cameraX, cameraY);
        }
       
        Map.DrawAlways(sb, cameraX, cameraY);
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
   
    public LinkedList<PC> allPlayers()
    {
        return PlayerList;
    }

    public LinkedList<int[]> PlayerCoordsLinkedList()
    {
        LinkedList<int[]> coords = new LinkedList<int[]>();
        foreach(PC p in allPlayers())
        {
            coords.AddLast(new int[2] { p.X, p.Y });
        }
        return coords;

    }
}
