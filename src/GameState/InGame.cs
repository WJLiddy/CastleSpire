using CastleUtils;
using System;
using System.Collections.Generic;

class InGame
{
    private static PC[] Players = new PC[4];
    public static LinkedList<PC> PlayerList { get; private set; } = new LinkedList<PC>(); 
    private static LinkedList<NPC> NPCList = new LinkedList<NPC>();
    private static int NPCPlanIndex = 0;

    private static HUD[] HUDs = new HUD[4];
    public static LinkedList<Item> FloorItems { get; private set; }

    public static ObjectMap Map { get; private set; }
    public static PathFindingMesh MapMesh { get; private set; }

    private static int UpdateCounter = 0;
    
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
        MapMesh = new PathFindingMesh(@"maps\mesh.xml",Map.BaseMap.Width,Map.BaseMap.Height);
        
        FloorItems = new LinkedList<Item>();
        for (int i = 0; i != 100; i++)
        {
            //knives
            BasicMeleeWeapon b = BasicMeleeWeapon.generateBasicWeapon(5,0.1);
            b.SetCoords((int)(70 + Utils.RandomNumber() * 20), (int)(70 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
            //sabers
            b = BasicMeleeWeapon.generateBasicWeapon(5,0.3);
            b.SetCoords((int)(85 + Utils.RandomNumber() * 20), (int)(105 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
            //swords
            b = BasicMeleeWeapon.generateBasicWeapon(5,0.5);
            b.SetCoords((int)(85 + Utils.RandomNumber() * 20), (int)(175 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
            //axes
            b = BasicMeleeWeapon.generateBasicWeapon(5,0.7);
            b.SetCoords((int)(100 + Utils.RandomNumber() * 20), (int)(210 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
            //hammers
            b = BasicMeleeWeapon.generateBasicWeapon(5,0.9);
            b.SetCoords((int)(130+ Utils.RandomNumber() * 20), (int)(180 + Utils.RandomNumber() * 20));
            FloorItems.AddFirst(b);
        }

        for (int i = 0; i != 50; i++)
        {
            BeachZombie bz = new BeachZombie(250, 230);
            bz.Stats.AwardSpdXP((bz.Stats.XPPerSkill/2) * -i);
       //     NPCList.AddFirst(bz);

        }
        SoundManager.Play("night.ogg", true);
    }
    
    //Load each of the characters.
    public GS.State Update(int ms)
    {
        UpdateCounter++;
        //Do garbage collection every second, but avoid AI.
        if (UpdateCounter % 60 == 0)
        {
            GC.Collect();
        }
        UpdatePlayersAndHud(ms);
        UpdateNPCs(ms,UpdateCounter, UpdateCounter % 60 != 0);
        return GS.State.InGame;
    }

    private void UpdateNPCs(int ms, int updateCounter, bool execAI)
    {
        PixelSet s = new PixelSet();
        s.Add(allPlayers().First.Value.X, allPlayers().First.Value.Y);
        int index = 0;
        bool updateNext = false;
        foreach (NPC npc in NPCList)
        {
            if (execAI && (updateNext ||  index == (NPCPlanIndex % NPCList.Count)))
            {
                npc.UpdatePlan();
                updateNext = !updateNext;
            }
            npc.Update(ms);
            index++;
        }

        if (execAI)
            NPCPlanIndex = NPCPlanIndex + 2 ;
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

        /**
        foreach(PathFindingMesh.MeshRegion p in pfm.allRegions)
        {
            Utils.DefaultFont.Draw(sb, "" + p.ID, p.centerX + -(Utils.DefaultFont.GetWidth(""+p.ID,false)/2) + -cameraX, p.centerY +- 3 + -cameraY, Color.Black, 1);
        }
        */

        Item.DrawGlowingItems(sb, allPlayers(), FloorItems, cameraX, cameraY);

        //draw floor items
        foreach (Item i in FloorItems)
        {
            i.DrawOnFloor(sb, cameraX, cameraY);
        }

        //draw y ordered items
        for (int floor = 0; floor != Map.BaseMap.Height; floor++)
        {
            Map.DrawObjects(sb, cameraX, cameraY, floor);
            foreach (PC p in allPlayers())
            {
                p.Draw(sb, cameraX, cameraY,floor);
            }

            foreach (NPC p in NPCList)
            {
                p.Draw(sb, cameraX, cameraY, floor);
            }
        }   
        // draw the always map.
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
