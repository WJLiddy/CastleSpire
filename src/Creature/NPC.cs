using System;
using System.Collections.Generic;
using CastleUtils;

public abstract class NPC : Creature
{
    CardinalDir Direction = CardinalDir.South;

    public abstract void UpdatePlan();

    public abstract void Update(int ms);

    public abstract void Draw(AD2SpriteBatch sb, int cameraX, int cameraY, int floor);
}