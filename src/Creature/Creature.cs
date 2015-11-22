using System.Collections;

public abstract class Creature : Entity
{
    public static readonly double Rad2Over2 = .7071067812;
    public static readonly int MaxFatigue = 100;
    public static readonly int FatigueRegenSec = 5;
    public static readonly int InvSize = 7;

    //TODO Document
    protected int[] PrioritySet = new int[4]{ 0, 1, 2, 3 };

    public int HP { get; protected set; }
    public int MP { get; protected set; }
    public int FA { get; protected set; }

    public StatSet Stats { get; protected set; }


    public int InvIndex { get; protected set; } = 0;
    public Item[] Inventory { get; protected set; }

    //In this game engine, creatures are graphically locked to a discrete grid which is the set of all whole numbers.
    //Collisions are done based on this as well.
    //Howeever, for movement purposes, there exists a delta-X which is based on a 1/1000 (delta-scale) scale. Think millipixels. That way characters can move at different 'speeds'
    //This accumulator is really only used for movement.

    //for movement/speed purposes
    public static readonly int DeltaScale = 1000;
    protected int DX { get; set; }
    protected int DY { get; set; }
    
    //size of boundingbox of character


    public Creature()
    {
        Inventory = new Item[InvSize];

    }

}

