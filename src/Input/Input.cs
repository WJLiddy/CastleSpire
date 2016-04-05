public abstract class Input
{
    //Main commands
    public bool Fire {get; protected set;}
    public bool Block {get; protected set;}
    public bool Special { get; protected set; }
    public bool Use { get; protected set; }

    public bool PressedFire { get; protected set; }
    public bool PressedBlock { get; protected set; }
    public bool PressedSpecial { get; protected set; }
    public bool PressedUse { get; protected set; }

    //Movement Commands
    public bool Up { get; protected set; }
    public bool Down { get; protected set; }
    public bool Left { get; protected set; }
    public bool Right { get; protected set; }

    public bool PressedUp { get; protected set; }
    public bool PressedDown { get; protected set; }
    public bool PressedLeft { get; protected set; }
    public bool PressedRight { get; protected set; }

    //Shift inventory left or right
    public bool InventoryL { get; protected set; }
    public bool InventoryR { get; protected set; }

    public bool PressedInventoryL { get; protected set; }
    public bool PressedInventoryR { get; protected set; }

    //Pauses 
    public bool Start { get; protected set; }
    public bool PressedStart { get; protected set; }
    
}
