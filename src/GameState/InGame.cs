using CastleUtils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

class InGame
{
    private static PC[] Players = new PC[4];
    private static LinkedList<PC> PlayerList = new LinkedList<PC>();

    private static HUD[] HUDs = new HUD[4];
    public static LinkedList<Item> FloorItems { get; private set; }
    public static ObjectMap Map { get; private set; }

    private static int updateCounter = 0;

    PathFindingMesh pfm;

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

        //TEST!
         pfm = new PathFindingMesh(@"maps\mesh.xml",Map.BaseMap.Width,Map.BaseMap.Height);

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

    int ptrX = 12;
    int ptrY = 200;
    
    //Load each of the characters.
    public GS.State Update(int ms)
    {
        updateCounter++;

        PixelSet s = new PixelSet();
        s.Add(allPlayers().First.Value.X, allPlayers().First.Value.Y);
        Stack<AllDir> path;
        //hardcoded 16!
        path = PathFinding.PixelPath(Map, ptrX, ptrY, allPlayers().First.Value.X, allPlayers().First.Value.Y, s, 16, 50);

        if(path == null)
            path = PathFinding.LongPathEstimation(Map, ptrX, ptrY, allPlayers().First.Value.X, allPlayers().First.Value.Y, pfm, 16);

        if (updateCounter % 60 == 0)
        {
            GC.Collect();
        }

        if (((updateCounter + 1) % 6 == 0) && path != null && path.Count > 0)
        {
            ptrX += DirectionUtils.getDeltaX(path.Peek());
            ptrY += DirectionUtils.getDeltaY(path.Peek());
        }

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

        for (int i = 0; i != 4; i++)
        {
            if(HUDs[i] != null)
                HUDs[i].Draw(sb);
        }

    }

    private void DrawWorld(AD2SpriteBatch sb, int cameraX, int cameraY)
    {
       
        Map.DrawBase(sb, cameraX, cameraY);

        //PFM test
        /**
        for (int x = 0; x != Map.BaseMap.Width; x++)
        {
            for (int y = 0; y != Map.BaseMap.Height; y++)
            {
                  if (pfm.pixelToRegion[x, y] != null)
                  {
                        Utils.DrawRect(sb, x + -cameraX, y + -cameraY, 1, 1, new Color((pfm.pixelToRegion[x, y].ID * 197) % 255, (pfm.pixelToRegion[x, y].ID * 257) % 255, (pfm.pixelToRegion[x, y].ID * 379) % 255));
                  }
            }
        }
        */
        foreach(PathFindingMesh.MeshRegion p in pfm.allRegions)
        {
            Utils.DefaultFont.Draw(sb, "" + p.ID, p.centerX + -(Utils.DefaultFont.GetWidth(""+p.ID,false)/2) + -cameraX, p.centerY +- 3 + -cameraY, Color.Black, 1);
        }

        Utils.DefaultFont.Draw(sb, "X", ptrX + -cameraX, ptrY  + -cameraY, Color.Black, 1);


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
