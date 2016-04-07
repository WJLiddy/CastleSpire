public abstract class Creature : Entity
{
    public static readonly int MaxFatigue = 100;
    public static readonly int FatigueRegenFrames = 12;
    public static readonly int InvSize = 7;

    //TODO Document. This is a tricky structure that keeps track of mouse inputs so that newpresses are detected in the right direction.
    protected int[] PrioritySet = new int[4]{ 0, 1, 2, 3 };

    public int HP { get; protected set; }
    public int MP { get; protected set; }
    public int FA { get; protected set; }
    protected int TimeToNextFatiguePoint = 0;
    public StatSet Stats { get; protected set; }
    public int InvIndex { get; protected set; } = 0;
    public Item[] Inventory { get; protected set; }


    public static readonly int DeltaScale = 1000;
    //In this game engine, creatures are graphically locked to a discrete grid which is the set of all whole numbers.
    //Collisions are done based on this as well.
    //Howeever, for movement purposes, there exists a delta-X which is based on a 1/1000 (delta-scale) scale. Think millipixels. That way characters can move at different 'speeds'


    public Creature()
    {
        Inventory = new Item[InvSize];
    }

    protected int getMilliPixelsToMove(int ms)
    {
        double pixelsPerSecond = StatSet.BaseSpeed + (StatSet.SkillSpeed * Stats.Spd());
        double pixelsToMove = pixelsPerSecond * ((double)ms / 1000);
        return (int)(DeltaScale * pixelsToMove);
    }

}