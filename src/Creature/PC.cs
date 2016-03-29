using System;
using System.Collections.Generic;
using CastleUtils;

public class PC : Creature
{
    public static readonly bool HandDebug = false;
    public static readonly double AttackMovementPercent = 0.5;
    public static readonly int PunchFrames = 60;
    public enum PlayerState { IDLE, WALKING, USING };

    public RaceUtils.Race Race { get; private set; }
    private AnimationSet Anim;
    public PlayerState State { get; private set; } = PlayerState.IDLE;
    CardinalDir Direction = CardinalDir.South;
    public string Name { get; private set; }

    private int UseFramesLeft = 0;
    private int TimeLeftOnUseFrame = 0;

    int[,] IdleHandPositions;
    int[,,] WalkingHandPositions;
    int[,,] SwingingHandPositions;
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
                FillHandPositions(@"creatures\pc\pirate\hands.xml");
                Size = 12;
                Name = "NANCY";
                break;
            case RaceUtils.Race.Dragon:
                Anim = new AnimationSet(@"creatures\pc\dragon\anim.xml");
                Stats = new StatSet(@"creatures\pc\dragon\stat.xml");
                FillHandPositions(@"creatures\pc\dragon\hands.xml");
                Size = 16;
                Name = "ALESSIA";
                break;
            case RaceUtils.Race.Meximage:
                Anim = new AnimationSet(@"creatures\pc\meximage\anim.xml");
                Stats = new StatSet(@"creatures\pc\meximage\stat.xml");
                FillHandPositions(@"creatures\pc\meximage\hands.xml");
                Size = 14;
                Name = "JESUS";
                break;
            case RaceUtils.Race.Ninja:
                Anim = new AnimationSet(@"creatures\pc\ninja\anim.xml");
                Stats = new StatSet(@"creatures\pc\ninja\stat.xml");
                FillHandPositions(@"creatures\pc\ninja\hands.xml");
                Size = 12;
                Name = "BIP";
                break;
        }

        //fix!
        Anim.Speed = 9;
        //Fix this too
        Anim.Hold("idle", 0, (int)Direction);
        HP = StatSet.MPPerStat * Stats.Vit();
        MP = StatSet.MPPerStat * Stats.Aff();
        FA = MaxFatigue;
    }

    public void Update(Input i, int ms)
    {
        
        if (State != PlayerState.USING)
        {
            TimeToNextFatiguePoint--;
            if(TimeToNextFatiguePoint < 0)
            {
                if (FA < MaxFatigue)
                    FA++;
                TimeToNextFatiguePoint = FatigueRegenFrames;
            }
            Move(i, ms);
            AnimateMove(i);
            if (i.PressedInventoryL)
                ShiftItemsLeft();
            if (i.PressedInventoryR)
                ShiftItemsRight();
            if (i.PressedUse)
                Use();
            if (i.PressedFire)
                Fire();
        }

        if(State == PlayerState.USING)
        {
            UseLogic(ms);
        }

        ConvertDXDYToMovement();
        Anim.Update();
    }

    private void ShiftItemsLeft()
    {
        InvIndex--;
        if(InvIndex < 0)
            InvIndex = Inventory.Length - 1;
    }

    private void ShiftItemsRight()
    {
        InvIndex++;
        if (InvIndex >= Inventory.Length)
            InvIndex = 0;
    }

    //Consider a camera
    public void Draw(AD2SpriteBatch sb, int cameraX, int cameraY, int floor )
    {
        if(Y + (Size - 1) != floor)
            return;
        //If facing down or right, the weapon draws over player.
        if(Direction == CardinalDir.South || Direction == CardinalDir.East)
            Anim.Draw(sb, X + - cameraX, Y + - cameraY);
        if (Inventory[InvIndex] != null)
        {
            int handPositionX = 0;
            int handPositionY = 0;
            switch(Anim.CurrentAnimationName)
            {
                case "idle":
                    handPositionX = IdleHandPositions[(int)Direction, 0];
                    handPositionY = IdleHandPositions[(int)Direction, 1];
                    break;
                case "walk":
                    handPositionX = WalkingHandPositions[(int)Direction, Anim.XFrame, 0];
                    handPositionY = WalkingHandPositions[(int)Direction, Anim.XFrame, 1];
                    ///
                    break;
                case "swing":
                    handPositionX = SwingingHandPositions[(int)Direction, Anim.XFrame, 0];
                    handPositionY = SwingingHandPositions[(int)Direction, Anim.XFrame, 1];
                    break;
            }

                Utils.Log(handPositionY +"");
                int XHandPosition = X + -Anim.CurrentAnimation.XOffset + handPositionX +- Inventory[InvIndex].HandX + -cameraX;
                int YHandPosition = Y + -Anim.CurrentAnimation.YOffset + handPositionY + -Inventory[InvIndex].HandY + -cameraY;

                if(HandDebug)
                {
                    Utils.DrawRect(sb, XHandPosition, YHandPosition, 1, 1, new Microsoft.Xna.Framework.Color(255, 0, 255));
                }

                Inventory[InvIndex].DrawAlone(sb, XHandPosition, YHandPosition, (int)Direction);
        }

        // If facing left or up, the weapon draws under player.
        if (Direction == CardinalDir.West || Direction == CardinalDir.North)
            Anim.Draw(sb, X + -cameraX, Y + -cameraY);

        if (HandDebug)
        {
            //f'nize
            int handPositionX = 0;
            int handPositionY = 0;
            switch (Anim.CurrentAnimationName)
            {
                case "idle":
                    handPositionX = IdleHandPositions[(int)Direction, 0];
                    handPositionY = IdleHandPositions[(int)Direction, 1];
                    break;
                case "walk":
                    handPositionX = WalkingHandPositions[(int)Direction, Anim.XFrame, 0];
                    handPositionY = WalkingHandPositions[(int)Direction, Anim.XFrame, 1];
                    ///
                    break;
                case "swing":
                    handPositionX = SwingingHandPositions[(int)Direction, Anim.XFrame, 0];
                    handPositionY = SwingingHandPositions[(int)Direction, Anim.XFrame, 1];
                    break;
                default:
                    //error!
                    break;
            }
            Utils.DrawRect(sb, X + -Anim.CurrentAnimation.XOffset + handPositionX + + -cameraX, Y + -Anim.CurrentAnimation.YOffset + handPositionY + -cameraY, 1, 1, new Microsoft.Xna.Framework.Color(255, 0, 255));
        }
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
            DX = DX - (int)(milliPixelsToMove * Util.Rad2Over2);
            DY = DY - (int)(milliPixelsToMove * Util.Rad2Over2);
            Direction = CardinalDir.West;
        }
        else if (xy[0] == 3 && xy[1] == 2)
        {
            DX = DX - (int)(milliPixelsToMove * Util.Rad2Over2);
            DY = DY + (int)(milliPixelsToMove * Util.Rad2Over2);
            Direction = CardinalDir.West;
        }
        else if (xy[0] == 1 && xy[1] == 0)
        {
            DX = DX + (int)(milliPixelsToMove * Util.Rad2Over2);
            DY = DY - (int)(milliPixelsToMove * Util.Rad2Over2);
            Direction = CardinalDir.East;
        }
        else if (xy[0] == 1 && xy[1] == 2)
        {
            DX = DX + (int)(milliPixelsToMove * Util.Rad2Over2);
            DY = DY + (int)(milliPixelsToMove * Util.Rad2Over2);
            Direction = CardinalDir.East;
        }

        else if (xy[0] == 1)
        {
            DX = DX + milliPixelsToMove;
            Direction = CardinalDir.East;
        }
        else if (xy[0] == 3)
        {
            DX = DX - milliPixelsToMove;
            Direction = CardinalDir.West;
        }
        else if (xy[1] == 0)
        {
            DY = DY - milliPixelsToMove;
            Direction = CardinalDir.North;
        }
        else if (xy[1] == 2)
        {
            DY = DY + milliPixelsToMove;
            Direction = CardinalDir.South;
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

    }

    private void ConvertDXDYToMovement()
    {
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
        if(Inventory[InvIndex] != null && Inventory[InvIndex] is BasicMeleeWeapon)
        {
            BasicMeleeWeapon w = ((BasicMeleeWeapon)Inventory[InvIndex]);
            if (w.CanUse(this) && FA >= w.FatigueCost())
            {
                UseFramesLeft = 6;
                TimeLeftOnUseFrame = ((BasicMeleeWeapon)Inventory[InvIndex]).FrameTime;
                FA -= w.FatigueCost();
            }
            else
                return;
        } else
        {
            //fisticuffs
            TimeLeftOnUseFrame = PunchFrames;
            UseFramesLeft = 6;
        }
        Anim.Speed = TimeLeftOnUseFrame;
        State = PlayerState.USING;
        Anim.AutoAnimateOnce("swing", (int)Direction);
    }

    private void UseLogic(int ms)
    {
        //Functionize
        double pixelsPerSecond = StatSet.BaseSpeed + (StatSet.SkillSpeed * Stats.Spd());
        double pixelsToMove = pixelsPerSecond * ((double)ms / 1000);
        int milliPixelsToMove = (int)(DeltaScale * pixelsToMove * AttackMovementPercent);

        if (CanMove((int)Direction))
        {
            switch(Direction)
            {
                case CardinalDir.South:
                    DY += milliPixelsToMove;
                    break;
                case CardinalDir.North:
                    DY -= milliPixelsToMove;
                    break;
                case CardinalDir.West:
                    DX -= milliPixelsToMove;
                    break;
                case CardinalDir.East:
                    DX += milliPixelsToMove;
                    break;
            }
        }
        TimeLeftOnUseFrame--;
        if (TimeLeftOnUseFrame == 0)
        {
            UseFramesLeft--;
            TimeLeftOnUseFrame = ((BasicMeleeWeapon)Inventory[InvIndex]) != null? ((BasicMeleeWeapon)Inventory[InvIndex]).FrameTime : PunchFrames;
        }
        if (UseFramesLeft == 0)
        {
            State = PlayerState.IDLE;
            Anim.Speed = 9;
            Anim.Hold("idle", 0,  (int)Direction);
        }
     }

    private void FillHandPositions(string xml)
    {
        Dictionary<string,LinkedList<string>> hands = Utils.GetXMLEntriesHash(xml);
        
        WalkingHandPositions = new int[4, 4 , 2];
        IdleHandPositions = new int[4, 2];
        SwingingHandPositions = new int[4, 6, 2];

        for (int dir = 0; dir != 4; dir++)
        {
            IdleHandPositions[dir,0] = retrieveHandCoords(hands, "idle", dir, 0, true) % 24;
            IdleHandPositions[dir,1] = retrieveHandCoords(hands, "idle", dir, 0, false) % 32;

            for (int frame = 0; frame != 4; frame++)
            {
                WalkingHandPositions[dir, frame, 0] = retrieveHandCoords(hands,"walk",dir,frame,true) % 24;
                WalkingHandPositions[dir, frame, 1] = retrieveHandCoords(hands, "walk", dir, frame, false) % 32;
            }

            for (int frame = 0; frame != 6; frame++)
            {
                SwingingHandPositions[dir, frame, 0] = retrieveHandCoords(hands, "swing", dir, frame, true) % 24;
                SwingingHandPositions[dir, frame, 1] = retrieveHandCoords(hands, "swing", dir, frame, false) % 32;
            }

        }
    }

    private int retrieveHandCoords(Dictionary<string, LinkedList<string>> hands, string name, int dir, int frame,bool getX)
    {
        try
        {
            string walkcoords = hands[name + frame + dir].First.Value;
            return Int32.Parse(walkcoords.Split(',')[getX? 0 : 1]);
        }
        catch (Exception)
        {
            Utils.Log("Failed to get hand coords for : " + name + dir + frame);
            return 0;
        }
    }
}