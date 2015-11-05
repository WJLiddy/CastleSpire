
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlimDX;
using SlimDX.DirectInput;


public class GS
{
    //Game States we can be in.
    public enum State { Title, CharSelect, InGame }
    static State state;

    //TODO: There must be a smarter texture2d copier function. We don't want to constantly read from disk. UTILS can help us here.

    //Title screen draw/updater.
    static Title title;
    //Character Select draw/updater
    static CharSelect charSelect;
    //Game draw/updater
    static InGame inGame;
    
    public static Texture2D logo;

    public static GameTime lastDelta;

    //A single input set.
    public static Input[] inputs { get; private set; }

    public static ControllerManager controllerManager;

    //Need to initalize stuff specific to the game? Do it here!
    public GS()
    {
        inputs = new Input[4];
        state = State.Title;

        KeyboardInput k = new KeyboardInput();
        k.UPkey = Keys.Up;
        k.DOWNkey = Keys.Down;
        k.LEFTkey = Keys.Left;
        k.RIGHTkey = Keys.Right;
        k.INVLkey = Keys.Q;
        k.FIREkey = Keys.K;

        inputs[0] = k;
        KeyboardInput k2 = new KeyboardInput();

        inputs[1] = k2;
        inputs[2] = k2;
        inputs[3] = k2;
        /**
        for (int i = 1; i != 4; i++)
        {
            inputs[i] = new PInput();
            inputs[i].useKeyboard = false;
            inputs[i].controllerNo = 0;
            inputs[i].cA = 1;
            inputs[i].cB = 2;
            inputs[i].cL = 9;
            inputs[i].cR = 10;
            inputs[i].cS = 0;
            inputs[i].cX = 0;
            inputs[i].cY = 0;
        }
    */
    
        controllerManager = new ControllerManager();
        title = new Title();
        logo = Utils.TextureLoader(@"misc\logo.png");
    }


    public static void update(GameTime delta, Microsoft.Xna.Framework.Input.KeyboardState ks, GamePadState[] gs)
    {
        lastDelta = delta;

        SlimDX.DirectInput.JoystickState[] joyStates = controllerManager.getState();    

        for (int i = 0; i != 4; i++)
        {
            if (inputs[i] is KeyboardInput)
                ((KeyboardInput)inputs[i]).update(ks);
        }

        State newState;
        switch (state)
        {
            case State.Title:
                newState = title.update(delta,ks);
                if (newState == State.CharSelect)
                {
                    state = State.CharSelect;
                    charSelect = new CharSelect();
                }
                break;

            case State.CharSelect:

                newState = charSelect.update(delta);
               if (newState == State.InGame)
               {
                    state = State.InGame;
                    inGame = new InGame(charSelect.charSelect);
               }
                break;


            case State.InGame:

                newState = inGame.update(delta);
                if (newState == State.Title)
                {
                }
                break;
        }
    }

    public static void draw()
    {

      switch (state)
        {
            case State.Title:
                title.draw();
                break;
            case State.CharSelect:
                charSelect.draw();
                break;
            case State.InGame:
                inGame.draw();
                break;
        }

        Utils.drawString(lastDelta.IsRunningSlowly ? "SLOW!" : "", 100, 1, Color.BlanchedAlmond, 1, true);
    }
}

