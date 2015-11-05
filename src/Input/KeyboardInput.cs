using Microsoft.Xna.Framework.Input;

class KeyboardInput : Input
{
    public Keys FIREkey;
    public Keys BLOCKkey;
    public Keys SPECIALkey;
    public Keys USEkey;

    public Keys UPkey;
    public Keys DOWNkey;
    public Keys LEFTkey;
    public Keys RIGHTkey;

    public Keys INVRkey;
    public Keys INVLkey;
    public Keys STARTkey;
    
    public void update(KeyboardState k)
    {

        pressedFIRE = isPressed(k, FIRE, FIREkey);
        pressedBLOCK = isPressed(k, BLOCK, BLOCKkey);
        pressedSPECIAL = isPressed(k, SPECIAL, SPECIALkey);
        pressedUSE = isPressed(k, USE, USEkey);

        pressedUP = isPressed(k, UP, USEkey);
        pressedLEFT = isPressed(k, LEFT, LEFTkey);
        pressedRIGHT = isPressed(k, RIGHT, RIGHTkey);
        pressedDOWN = isPressed(k, DOWN, DOWNkey);

        pressedINVR = isPressed(k, INVR, INVRkey);
        pressedINVL = isPressed(k, INVL, INVLkey);
        pressedSTART = isPressed(k, START, STARTkey);


        FIRE = isHeld(k, FIRE, FIREkey);
        BLOCK = isHeld(k, BLOCK, BLOCKkey);
        SPECIAL = isHeld(k, SPECIAL, SPECIALkey);
        USE = isHeld(k, USE, USEkey);

        UP = isHeld(k, UP, UPkey);
        LEFT = isHeld(k, LEFT, LEFTkey);
        RIGHT = isHeld(k, RIGHT, RIGHTkey);
        DOWN = isHeld(k, DOWN, DOWNkey);

        INVR = isHeld(k, INVR, INVRkey);
        INVL = isHeld(k, INVL, INVLkey);
        START = isHeld(k, START, STARTkey);
    }


    private bool isPressed(KeyboardState state, bool command, Keys key)
    {
        return (!command && state.IsKeyDown(key));
    }

    private bool isHeld(KeyboardState state, bool command, Keys key)
    {
        return (state.IsKeyDown(key));
    }
}

