
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlimDX;
using SlimDX.DirectInput;


public class GS
{
    //Game States we can be in.
    public enum State { Title, CharSelect, InGame }
    static State GState;

    //TODO: There must be a smarter texture2d copier function. We don't want to constantly read from disk. UTILS can help us here.

    //Title screen draw/updater.
    static Title Title;
    //Character Select draw/updater
    static CharSelect CharSelect;
    //Game draw/updater
    static InGame InGame;
    
    public static Texture2D Logo;

    //A single input set.
    public static Input[] Inputs { get; private set; }

    public static ControllerManager ControllerManager;

    //Need to initalize stuff specific to the game? Do it here!
    public GS()
    {
        Inputs = new Input[4];
        GState = State.Title;

        KeyboardInput k = new KeyboardInput();
        k.UpKey = Keys.Up;
        k.DownKey = Keys.Down;
        k.LeftKey = Keys.Left;
        k.RightKey = Keys.Right;
        k.InventoryLKey = Keys.Q;
        k.FireKey = Keys.K;

        Inputs[0] = k;
        KeyboardInput k2 = new KeyboardInput();

        Inputs[1] = k2;
        Inputs[2] = k2;
        Inputs[3] = k2;
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
    
        ControllerManager = new ControllerManager();
        Title = new Title();
        Logo = Utils.TextureLoader(@"misc\logo.png");
    }


    public static void Update(int ms, Microsoft.Xna.Framework.Input.KeyboardState ks, GamePadState[] gs)
    {

        SlimDX.DirectInput.JoystickState[] joyStates = ControllerManager.GetState();    

        for (int i = 0; i != 4; i++)
        {
            if (Inputs[i] is KeyboardInput)
                ((KeyboardInput)Inputs[i]).Update(ks);
        }

        State newState;
        switch (GState)
        {
            case State.Title:
                newState = Title.Update(ms,ks);
                if (newState == State.CharSelect)
                {
                    GState = State.CharSelect;
                    CharSelect = new CharSelect();
                }
                break;

            case State.CharSelect:

                newState = CharSelect.Update(ms);
               if (newState == State.InGame)
               {
                    GState = State.InGame;
                    InGame = new InGame(CharSelect.CharacterSelect);
               }
                break;


            case State.InGame:

                newState = InGame.Update(ms);
                if (newState == State.Title)
                {
                }
                break;
        }
    }

    public static void Draw(AD2SpriteBatch sb)
    {

      switch (GState)
        {
            case State.Title:
                Title.Draw(sb);
                break;
            case State.CharSelect:
                CharSelect.Draw(sb);
                break;
            case State.InGame:
                InGame.Draw(sb);
                break;
        }

        //Utils.DefaultFont.Draw(sb, LastDelta.IsRunningSlowly ? "SLOW!" : "", 100, 1, Color.BlanchedAlmond, 1, true);
    }
}

