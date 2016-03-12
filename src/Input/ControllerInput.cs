using SlimDX.DirectInput;

class ControllerInput : Input
{
    public int ControllerNumber;
    public int FireKey;
    public int BlockKey;
    public int SpecialKey;
    public int UseKey;

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

        PressedUp = AxisIsPressed(k, Up, k.Y,false);
        PressedLeft = AxisIsPressed(k, Up, k.X, true);
        PressedRight = AxisIsPressed(k, Up, k.X, false);
        PressedDown = AxisIsPressed(k, Up, k.Y, true);

        PressedInventoryR = IsPressed(k, InventoryR, InventoryRKey);
        PressedInventoryL = IsPressed(k, InventoryL, InventoryLKey);
        PressedStart = IsPressed(k, Start, StartKey);


        Fire = IsHeld(k, Fire, FireKey);
        Block = IsHeld(k, Block, BlockKey);
        Special = IsHeld(k, Special, SpecialKey);
        Use = IsHeld(k, Use, UseKey);

        Up = AxisIsHeld(k, Up, k.Y, false);
        Left = AxisIsHeld(k, Up, k.X, true);
        Right = AxisIsHeld(k, Up, k.X, false);
        Down = AxisIsHeld(k, Up, k.Y, true);

        InventoryR = IsHeld(k, InventoryR, InventoryRKey);
        InventoryL = IsHeld(k, InventoryL, InventoryLKey);
        Start = IsHeld(k, Start, StartKey);
    }


    // command refers to the if the key was held down last time.
    private bool IsPressed(JoystickState state, bool command, int key)
    {
        return (!command && state.IsPressed(key));
    }

    private bool IsHeld(JoystickState state, bool command, int key)
    {
        return (state.IsPressed(key));
    }

    private bool AxisIsPressed(JoystickState state, bool command, int axisValue, bool negative)
    {
        return (!command && negative ? axisValue < 0 : axisValue > 0);
    }

    private bool AxisIsHeld(JoystickState state, bool command, int axisValue, bool negative)
    {
        return negative ? axisValue < 0 : axisValue > 0;
    }
}

