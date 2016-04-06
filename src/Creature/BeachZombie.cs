using CastleUtils;
using System;
using System.Collections.Generic;

class BeachZombie : NPC
{
    private AnimationSet Anim;
    private Stack<AllDir> Plan;

    public BeachZombie()
    {
        Anim = new AnimationSet(@"creatures\npc\zombie\anim.xml");
        Stats = new StatSet(@"creatures\npc\zombie\stat.xml");
    }

    public override void Draw(AD2SpriteBatch sb, int cameraX, int cameraY, int floor)
    {
        throw new NotImplementedException();
    }

    public override void Update(int ms)
    {
        //calcluate our speed, then follow directions on the stack as best we can until we run out of speed. Pop along the way.
    }

    public override void UpdatePlan()
    {
        Plan = new Stack<AllDir>();
        PC target = InGame.PlayerList.First.Value;
        PixelSet ps = new PixelSet();
        ps.Add(target.X, target.Y);

        //HARDOCDING LOL
        Plan = PathFinding.PixelPath(InGame.Map, X, Y, target.X, target.Y, ps, Size, 100);
        if(Plan == null)
        {
            Plan = LongPath.LongPathEstimation(InGame.Map, X, Y, target.X, target.Y, InGame.MapMesh, Size);
        }
    }
}