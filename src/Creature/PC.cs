using System;
using System.Collections.Generic;

public class PC : Creature
{
    public RaceUtils.Race Race { get; private set; }
    AnimationSet Anim;
    enum Dir { Up,Right,Down,Left};
    Dir Direction = Dir.Down;
    public string Name { get; private set; }

    private int AttackFramesLeft = 0;
    private int TimeLeftOnFrame = 0;

    int[,,] WalkingHandPositions;
    // Game runs at 60 FPS.
    // An attack has frames: Wind-up, Diagonal, Horizontal, Delivery, Recover I, Recover II.
    //                             2   3   4  5   6 
    // Weapon speeds can then be: 12, 18, 24, 30, 36 frames.


    public PC(int racei)
    {
        //TODO Clean
        X = 225;
        Y = 200;
        Race = (RaceUtils.Race)racei;
        switch (Race)
        {
            case RaceUtils.Race.Pirate:
                Anim = new AnimationSet(@"creatures\pc\pirate\anim.xml");
                Stats = new StatSet(@"creatures\pc\pirate\stat.xml");
                FillWalkingHandPositions(@"creatures\pc\pirate\hands.xml");
                Size = 12;
                Name = "NANCY";
                break;
            case RaceUtils.Race.Dragon:
                Anim = new AnimationSet(@"creatures\pc\dragon\anim.xml");
                Stats = new StatSet(@"creatures\pc\dragon\stat.xml");
                FillWalkingHandPositions(@"creatures\pc\dragon\hands.xml");
                Size = 16;
                Name = "ALESSIA";
                break;
            case RaceUtils.Race.Meximage:
                Anim = new AnimationSet(@"creatures\pc\meximage\anim.xml");
                Stats = new StatSet(@"creatures\pc\meximage\stat.xml");
                Size = 14;
                Name = "JESUS";
                break;
            case RaceUtils.Race.Ninja:
                Anim = new AnimationSet(@"creatures\pc\ninja\anim.xml");
                Stats = new StatSet(@"creatures\pc\ninja\stat.xml");
                Size = 12;
                Name = "BIP";
                break;
        }

        //fix!
        Anim.Speed = 9;
        HP = StatSet.MPPerStat * Stats.Vit();
        MP = StatSet.MPPerStat * Stats.Aff();
        FA = MaxFatigue;
    }

    public void Update(Input i, int ms)
    {
        //physically move character.
        Move(i,ms);
        AnimateMove(i);

        if (i.PressedUse)
            Use();
        if (i.PressedFire)
            Fire();

    
        Anim.Update();
    }

    //Consider a camera
    public void Draw(AD2SpriteBatch sb, int cameraX, int cameraY )
    {
        if(Direction == Dir.Down || Direction == Dir.Right)
            Anim.Draw(sb, X + - cameraX, Y + - cameraY);
        if (Inventory[0] != null)
        {
            if (Anim.CurrentAnimationName.Equals("walk"))
            {
                // First we center up on the top left corner of the character by subtracting the XOffset. 
                // Then we add the hand offset. Then we subtract the item's hand offset. Then the camera.
            
                //should center item on top left corner of sprite
                Utils.Log("" + Inventory[0].HandX);

                int XHandPosition = X + -Anim.CurrentAnimation.XOffset + WalkingHandPositions[Anim.XFrame,(int)Direction, 0] +- Inventory[0].HandX + -cameraX;
                int YHandPosition = Y + -Anim.CurrentAnimation.YOffset + WalkingHandPositions[Anim.XFrame,(int)Direction, 1] + -Inventory[0].HandY + -cameraY;


                Inventory[0].DrawAlone(sb, XHandPosition, YHandPosition, (int)Direction);
            }
        }
        if (Direction == Dir.Left || Direction == Dir.Up)
            Anim.Draw(sb, X + -cameraX, Y + -cameraY);
    }
    
    //Put the priority to move "dir" direction first.
    private void Prioritize(int dir)
    {
        int oldDirIndex = 0;

        for(int i = 0; i != PrioritySet.Length; i++)
        {
            //Find where the old direction was in the set.
            if(PrioritySet[i] == dir)
            {
                oldDirIndex = i;
                break;
            }
        }

        //Shift everything else down.
        for (int i = oldDirIndex; i != 0; i--)
        {
            PrioritySet[i] = PrioritySet[i - 1];
        }

        //Put this priority first.
        PrioritySet[0] = dir;
    }

    //push this to entity
    private bool CanMove(int dir)
    {
        switch (dir)
        {
            case 0:
                for (int top = X; top != X + Size; top++)
                {
                    if (InGame.Map.Collide(top,Y - 1))
                        return false;
                }
                return true;
            case 2:
                for (int bottom = X; bottom != X + Size; bottom++)
                {
                    if (InGame.Map.Collide(bottom, Y + Size))
                        return false;
                }
                return true;

            case 1:
                for (int right = Y; right!= Y + Size; right++)
                {
                    if (InGame.Map.Collide(X+Size, right))
                        return false;
                }
                return true;

            case 3:
                for (int left = Y; left != Y + Size; left++)
                {
                    if (InGame.Map.Collide(X - 1,left))
                        return false;
                }
                return true;

            default:
                return false;
        }
    }
    
    private void Move(Input i, int ms)
    {
        double pixelsPerSecond = StatSet.BaseSpeed + (StatSet.SkillSpeed * Stats.Spd());
        double pixelsToMove = pixelsPerSecond * ((double)ms / 1000);
        int milliPixelsToMove = (int)(DeltaScale * pixelsToMove);

        //look for fresh new inputs.
        if (i.PressedUp)
        {
            Prioritize(0);
        }
        if (i.PressedRight)
        {
            Prioritize(1);
        }
        if (i.PressedDown)
        {
            Prioritize(2);
        }
        if (i.PressedLeft)
        {
            Prioritize(3);
        }

        //we need to decide what is actually being held down.

        //first things first: get rid of opposing direction
        int[] xy = new int[2] { -1, -1 };

        for (int x = 0; x != PrioritySet.Length; x++)
        {
            if ((PrioritySet[x] == 1 && i.Right) || (PrioritySet[x] == 3 && i.Left))
            {
                xy[0] = PrioritySet[x];
                break;
            }
        }

        for (int y = 0; y != PrioritySet.Length; y++)
        {
            if ((PrioritySet[y] == 0 && i.Up) || (PrioritySet[y] == 2 && i.Down))
            {
                xy[1] = PrioritySet[y];
                break;
            }
        }

        if (xy[0] == 3 && xy[1] == 0)
        {
            DX = DX - (int)(milliPixelsToMove * Rad2Over2);
            DY = DY - (int)(milliPixelsToMove * Rad2Over2);
            Direction = Dir.Left;
        }
        else if (xy[0] == 3 && xy[1] == 2)
        {
            DX = DX - (int)(milliPixelsToMove * Rad2Over2);
            DY = DY + (int)(milliPixelsToMove * Rad2Over2);
            Direction = Dir.Left;
        }
        else if (xy[0] == 1 && xy[1] == 0)
        {
            DX = DX + (int)(milliPixelsToMove * Rad2Over2);
            DY = DY - (int)(milliPixelsToMove * Rad2Over2);
            Direction = Dir.Right;
        }
        else if (xy[0] == 1 && xy[1] == 2)
        {
            DX = DX + (int)(milliPixelsToMove * Rad2Over2);
            DY = DY + (int)(milliPixelsToMove * Rad2Over2);
            Direction = Dir.Right;
        }

        else if (xy[0] == 1)
        {
            DX = DX + milliPixelsToMove;
            Direction = Dir.Right;
        }
        else if (xy[0] == 3)
        {
            DX = DX - milliPixelsToMove;
            Direction = Dir.Left;
        }
        else if (xy[1] == 0)
        {
            DY = DY - milliPixelsToMove;
            Direction = Dir.Up;
        }
        else if (xy[1] == 2)
        {
            DY = DY + milliPixelsToMove;
            Direction = Dir.Down;
        }

    }

    private Item GetItemBelow(LinkedList<Item> floorItems)
    {
        foreach(Item i in floorItems)
        {
            if (this.collide(i))
                return i;
        }
        return null;
    }

    private void AnimateMove(Input i)
    {
        if (i.Left || i.Right || i.Up || i.Down)
        {
            if (Anim.CurrentAnimationName.Equals("idle") || Anim.YFrame != (int)Direction)
                Anim.AutoAnimate("walk", (int)Direction);
        }

        else
        {
            Anim.Hold("idle", 0, (int)Direction);    
        }


        //check to see if my dx would cause a collision.
        if (DX < 0 && !CanMove(3)) DX = 0;
        if (DX >= DeltaScale && !CanMove(1)) DX = DeltaScale - 1;
        if (DY < 0 && !CanMove(0)) DY = 0;
        if (DY >= DeltaScale && !CanMove(2)) DY = DeltaScale - 1;

        while (DX >= DeltaScale) { DX = DX - DeltaScale; X++; } //3 , 1100 -> 4, 100
        while (DY >= DeltaScale) { DY = DY - DeltaScale; Y++; }
        while (DX < 0) { DX = DX + DeltaScale; X--; } //3, -100 -> 2, 900
        while (DY < 0) { DY = DY + DeltaScale; Y--; }

    }

    private void Use()
    {
        //get ground pick-uppable items and grab them all
        LinkedList<Item> removeList = new LinkedList<Item>();
        foreach (Item item in InGame.FloorItems)
        {
            if (item.collide(this))
            {
                for (int i = 0; i != Inventory.Length; i++)
                {
                    if(Inventory[i] == null)
                    {
                        Inventory[i] = item;
                        removeList.AddFirst(item);
                        break;
                    }
                }
            }
        }

        foreach (Item item in removeList)
        {
            InGame.FloorItems.Remove(item);
        }
    }

    private void Fire()
    {
        
    }

    private void FillWalkingHandPositions(string xml)
    {
        Dictionary<string,LinkedList<string>> hands = Utils.GetXMLEntriesHash(xml);
        
        WalkingHandPositions = new int[4, 4 , 2];
        for(int x = 0; x != 4; x++)
        {
            for(int y = 0; y != 4; y++)
            {
                String coords = hands["walk" + x + y].First.Value;
                String[] coordsSplit = coords.Split(',');
                // X COORD : 24
                WalkingHandPositions[x, y, 0] = Int32.Parse(coordsSplit[0]) % 24;
                // Y COORD : 32
                WalkingHandPositions[x, y, 1] = Int32.Parse(coordsSplit[1]) % 32;
            }
        }
    }

}
