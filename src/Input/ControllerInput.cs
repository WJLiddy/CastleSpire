using SlimDX.DirectInput;

class ControllerInput : Input
{
    public int ControllerNumber;
    public int FireKey;
    public int BlockKey;
    public int SpecialKey;
    public int UseKey;

    public int UpKey;
    public int DownKey;
    public int LeftKey;
    public int RightKey;

    public int InventoryRKey;
    public int InventoryLKey;
    public int StartKey;

    public void Update(JoystickState[] allStates)
    {
        JoystickState k = allStates[ControllerNumber];

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


    private bool IsPressed(JoystickState state, bool command, int key)
    {
        return (!command && state.IsPressed(key));
    }

    private bool IsHeld(JoystickState state, bool command, int key)
    {
        return (state.IsPressed(key));
    }
}

