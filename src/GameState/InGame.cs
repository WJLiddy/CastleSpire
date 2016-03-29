using CastleUtils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

//Singletonize.
class InGame
{

    private static PC[] Players = new PC[4];
    private static LinkedList<PC> PlayerList = new LinkedList<PC>();

    private static HUD[] HUDs = new HUD[4];
    public static LinkedList<Item> FloorItems { get; private set; }
    public static ObjectMap Map { get; private set; }

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
        Map = new ObjectMap(@"maps\zombieBase.xml", CastleSpire.BaseWidth, CastleSpire.BaseHeight);

        // knives
        FloorItems = new LinkedList<Item>();
        for (int i = 0; i != 100; i++)
        {
            //knives
            BasicMeleeWeapon b = BasicMeleeWeapon.generateBasicWeapon(30,0.1);
            b.SetCoords((int)(70 + Utils.RandomNumber() * 20), (int)(70 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
            //sabers
            b = BasicMeleeWeapon.generateBasicWeapon(30,0.3);
            b.SetCoords((int)(85 + Utils.RandomNumber() * 20), (int)(105 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
            //swords
            b = BasicMeleeWeapon.generateBasicWeapon(30,0.5);
            b.SetCoords((int)(85 + Utils.RandomNumber() * 20), (int)(175 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
            //axes
            b = BasicMeleeWeapon.generateBasicWeapon(30,0.7);
            b.SetCoords((int)(100 + Utils.RandomNumber() * 20), (int)(210 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
            //hammers
            b = BasicMeleeWeapon.generateBasicWeapon(30,0.9);
            b.SetCoords((int)(130+ Utils.RandomNumber() * 20), (int)(180 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
        }


        SoundManager.Play("night.ogg", true);
    }

    //TEMP!
    Stack<AllDir> DijTest;
    int DijX;
    int DijY;

    //Load each of the characters.
    public GS.State Update(int ms)
    {
        //Fake dijikstra stress
        DijX = 172 + -10 + (int)(20 * Utils.RandomNumber());
        DijY = 123 + -10 + (int)(20 * Utils.RandomNumber());
        DijTest = PathFinding.DijikstraPath(Map, DijX, DijY, 172, 123);

        UpdatePlayersAndHud(ms);
        return GS.State.InGame;
    }

    public void Draw(AD2SpriteBatch sb)
    { 
        //figure out camera stuff
        int cameraX = 0; 
        int cameraY = 0;

        foreach (PC p in allPlayers())
        {
            cameraX += p.X - (CastleSpire.BaseWidth / 2);
            cameraY += p.Y - (CastleSpire.BaseHeight / 2);
        }
        cameraX /= allPlayers().Count;
        cameraY /= allPlayers().Count;

        DrawWorld(sb, cameraX, cameraY);


        Utils.DrawRect(sb, -cameraX + DijX, -cameraY + DijY, 1, 1, Color.Purple);
        /** Dijtest Temp */
        while (DijTest != null && DijTest.Count > 0)
        {
            AllDir d = DijTest.Pop();
            DijX += DirectionUtils.getDeltaX(d);
            DijY += DirectionUtils.getDeltaY(d);
            Utils.DrawRect(sb, -cameraX + DijX, -cameraY + DijY, 1, 1, Color.Red);

        }
        

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
        foreach (Item i in FloorItems)
        {
            i.DrawOnFloor(sb, cameraX, cameraY);
        }

        for (int floor = 0; floor != Map.BaseMap.Height; floor++)
        {
            Map.DrawObjects(sb, cameraX, cameraY, floor);
            foreach (PC p in allPlayers())
            {
                p.Draw(sb, cameraX, cameraY,floor);
            }
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
