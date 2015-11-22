using Microsoft.Xna.Framework.Input;

class KeyboardInput : Input
{
    public Keys FireKey;
    public Keys BlockKey;
    public Keys SpecialKey;
    public Keys UseKey;

    public Keys UpKey;
    public Keys DownKey;
    public Keys LeftKey;
    public Keys RightKey;

    public Keys InventoryRKey;
    public Keys InventoryLKey;
    public Keys StartKey;
    
    public void Update(KeyboardState k)
    {

        PressedFire = IsPressed(k, Fire, FireKey);
        PressedBlock = IsPressed(k, Block, BlockKey);
        PressedSpecial = IsPressed(k, Special, SpecialKey);
        PressedUse = IsPressed(k, Use, UseKey);

        PressedUp = IsPressed(k, Up, UseKey);
        PressedLeft = IsPressed(k, Left, LeftKey);
        PressedRight = IsPressed(k, Right, RightKey);
        PressedDown = IsPressed(k, Down, DownKey);

        PressedInventoryR = IsPressed(k, InventoryR, InventoryRKey);
        PressedInventoryL = IsPressed(k, InventoryL, InventoryLKey);
        PressedStart = IsPressed(k, Start, StartKey);


        Fire = IsHeld(k, Fire, FireKey);
        Block = IsHeld(k, Block, BlockKey);
        Special = IsHeld(k, Special, SpecialKey);
        Use = IsHeld(k, Use, UseKey);

        Up = IsHeld(k, Up, UpKey);
        Left = IsHeld(k, Left, LeftKey);
        Right = IsHeld(k, Right, RightKey);
        Down = IsHeld(k, Down, DownKey);

        InventoryR = IsHeld(k, InventoryR, InventoryRKey);
        InventoryL = IsHeld(k, InventoryL, InventoryLKey);
        Start = IsHeld(k, Start, StartKey);
    }


    private bool IsPressed(KeyboardState state, bool command, Keys key)
    {
        return (!command && state.IsKeyDown(key));
    }

    private bool IsHeld(KeyboardState state, bool command, Keys key)
    {
        return (state.IsKeyDown(key));
    }
}

