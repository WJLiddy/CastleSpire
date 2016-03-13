using System.Collections.Generic;

//Singletonize.
class InGame
{

    private static PC[] Players = new PC[4];
    private static LinkedList<PC> PlayerList = new LinkedList<PC>();

    private static HUD[] HUDs = new HUD[4];
    public static LinkedList<Item> FloorItems { get; private set; }
    public static CollisionMap Map { get; private set; }

    public InGame(int[] race, bool[] ready)
    {
        SoundManager.Stop();
        for (int i = 0; i != 4; i++)
        {
            if (ready[i])
            {
                Players[i] = new PC(race[i]);
                PlayerList.AddFirst(Players[i]);
                if (i == 0)
                    HUDs[i] = new HUD(Players[i], HUD.Corner.TOPLEFT);
                if (i == 1)
                    HUDs[i] = new HUD(Players[i], HUD.Corner.TOPRIGHT);
                if (i == 2)
                    HUDs[i] = new HUD(Players[i], HUD.Corner.BOTTOMLEFT);
                if (i == 3)
                    HUDs[i] = new HUD(Players[i], HUD.Corner.BOTTOMRIGHT);
            }
        }

        // Just using a collisionmap for now.
        Map = new CollisionMap(@"maps\zombieBase.xml", CastleSpire.BaseWidth, CastleSpire.BaseHeight);

        // And a single item.
        FloorItems = new LinkedList<Item>();
        for (int i = 0; i != 100; i++)
        {
            BasicMeleeWeapon b = BasicMeleeWeapon.generateBasicWeapon(30);
            b.SetCoords((int)(20 + Utils.RandomNumber() * 300), (int)(20 + Utils.RandomNumber() * 300));
            FloorItems.AddFirst(b);
        }

        SoundManager.Play("night.ogg", true);
    }

    //Load each of the characters.
    public GS.State Update(int ms)
    {
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
            i.DrawOnFloor(sb, cameraX,cameraY);
        }

        foreach (PC p in allPlayers())
        {
            p.Draw(sb, cameraX, cameraY);
        }
       
        Map.DrawAlways(sb, cameraX, cameraY);
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
