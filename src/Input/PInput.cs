using Microsoft.Xna.Framework.Input;
using System;
/**

public class PInput : Input
{
    public static readonly float DEADZONE = (.5f * ControllerManager.AXIS_RANGE);
    public int controllerNo = 0;

    //Objects here are "Keys" enums from a keyboard.


    public void update(KeyboardState k, SlimDX.DirectInput.JoystickState[] joyStates)
    {
        if (useKeyboard)
        {
           
        }

        else if (controllerNo < joyStates.Length)
        {
           
            SlimDX.DirectInput.JoystickState controller = joyStates[controllerNo];

            pA = (!A && controller.IsPressed((int)cA));
            pB = (!B && controller.IsPressed((int)cB));
            p = (!X && controller.IsPressed((int)cX));
            pUSE = (!USE && controller.IsPressed((int)cUSE));

            pUP = (!UP && controller.Y < -DEADZONE);
            pRIGHT = (!RIGHT && controller.X > DEADZONE);
            pDOWN = (!DOWN && controller.Y > DEADZONE);
            pLEFT = (!LEFT && controller.X < -DEADZONE);

            pL = (!L && controller.IsPressed((int)cL));
            pR = (!R && controller.IsPressed((int)cR));
            pS = (!S && controller.IsPressed((int)cS));

            A = controller.IsPressed((int)cA);
            B = controller.IsPressed((int)cB);
            X = controller.IsPressed((int)cX);
            Y = controller.IsPressed((int)cY);

            UP = controller.Y < -DEADZONE;
            RIGHT = controller.X > DEADZONE;
            DOWN = controller.Y > DEADZONE;
            LEFT = controller.X < -DEADZONE;

            L = controller.IsPressed((int)cL);
            R = controller.IsPressed((int)cR);
            S = controller.IsPressed((int)cS);
 
        }
    }
}
    */
