public abstract class Entity
{
    //change
    public int X { get; protected set; }
    public int Y { get; protected set; }
    public int Size { get; protected set; }

    public bool collide(Entity e)
    {
        return !(
        (Y + (Size - 1) < e.Y) ||
        (Y > e.Y + (e.Size - 1)) ||
        (X > e.X + (e.Size - 1) ||
        (X + (Size - 1) < e.X)));

    }
}
