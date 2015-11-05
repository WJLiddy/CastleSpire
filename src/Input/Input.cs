public class Input
{
    //Main commands
    public bool FIRE {get; protected set;}
    public bool BLOCK {get; protected set;}
    public bool SPECIAL { get; protected set; }
    public bool USE { get; protected set; }

    public bool pressedFIRE { get; protected set; }
    public bool pressedBLOCK { get; protected set; }
    public bool pressedSPECIAL { get; protected set; }
    public bool pressedUSE { get; protected set; }

    //Movement Commands
    public bool UP { get; protected set; }
    public bool DOWN { get; protected set; }
    public bool LEFT { get; protected set; }
    public bool RIGHT { get; protected set; }

    public bool pressedUP { get; protected set; }
    public bool pressedDOWN { get; protected set; }
    public bool pressedLEFT { get; protected set; }
    public bool pressedRIGHT { get; protected set; }

    //Shift inventory left or right
    public bool INVL { get; protected set; }
    public bool INVR { get; protected set; }

    public bool pressedINVL { get; protected set; }
    public bool pressedINVR { get; protected set; }

    //Pauses 
    public bool START { get; protected set; }
    public bool pressedSTART { get; protected set; }
    
}
