using Microsoft.Xna.Framework.Input;
using System;


public class PInput : Input
{

    //Objects here are "Keys" enums from a keyboard.

    public Object cA { private get; set; }
    public Object cB { private get; set; }
    public Object cX { private get; set; }
    public Object cY { private get; set; }

    public Object cUP { private get; set; }
    public Object cDOWN { private get; set; }
    public Object cLEFT { private get; set; }
    public Object cRIGHT { private get; set; }

    public Object cL { private get; set; }
    public Object cR { private get; set; }

    public Object cS { private get; set; }

    public void update(KeyboardState k)
    {
        //Applies no matter what?
        pA = (cA == null) ? false : (!A && k.IsKeyDown((Keys)cA));
        pB = (cB == null) ? false : (!B && k.IsKeyDown((Keys)cB));
        pX = (cX == null) ? false : (!X && k.IsKeyDown((Keys)cX));
        pY = (cY == null) ? false : (!Y && k.IsKeyDown((Keys)cY));

        pUP = (cUP == null) ? false : (!UP && k.IsKeyDown((Keys)cUP));
        pRIGHT = (cRIGHT == null) ? false : (!RIGHT && k.IsKeyDown((Keys)cRIGHT));
        pDOWN = (cDOWN == null) ? false : (!DOWN && k.IsKeyDown((Keys)cDOWN));
        pLEFT = (cLEFT == null) ? false :  (!LEFT && k.IsKeyDown((Keys)cLEFT));

        pL = (cL == null) ? false : (!L && k.IsKeyDown((Keys)cL));
        pR = (cR == null) ? false : (!R && k.IsKeyDown((Keys)cR));
        pS = (cS == null) ? false : (!S && k.IsKeyDown((Keys)cS));

        //TODO: Only have keyboard support
        A = (cA == null) ? false : k.IsKeyDown((Keys)cA);
        B = (cB == null) ? false : k.IsKeyDown((Keys)cB);
        X = (cX == null) ? false : k.IsKeyDown((Keys)cX);
        Y = (cY == null) ? false : k.IsKeyDown((Keys)cY);

        UP = (cUP == null) ? false : k.IsKeyDown((Keys)cUP);
        DOWN = (cDOWN == null) ? false : k.IsKeyDown((Keys)cDOWN);
        LEFT = (cLEFT == null) ? false : k.IsKeyDown((Keys)cLEFT);
        RIGHT = (cRIGHT == null) ? false : k.IsKeyDown((Keys)cRIGHT);

        L = (cL == null) ? false : k.IsKeyDown((Keys)cL);
        R = (cR == null) ? false : k.IsKeyDown((Keys)cR);

        S = (cS == null) ? false : k.IsKeyDown((Keys)cS);



    }

}
