public abstract class Entity
{
    //change
    public int x { get; protected set; }
    public int y { get; protected set; }
    public int size { get; protected set; }

    public bool collide(Entity e)
    {
        return !(
        (y + (size - 1) < e.y) ||
        (y > e.y + (e.size - 1)) ||
        (x > e.x + (e.size - 1) ||
        (x + (size - 1) < e.x)));

    }
}
