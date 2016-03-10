using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


public class GS
{
    //Game States we can be in.
    public enum State { Title, CharSelect, InGame }
    static State GState;

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
    
    //Starts game at title
    public GS()
    {
        // Input for each player. This is read from an XML.
        Inputs = new Input[4];
        GState = State.Title;

        /** This is how to do a keyboard input.
        KeyboardInput k = new KeyboardInput();
        k.UpKey = Keys.Up;
        k.DownKey = Keys.Down;
        k.LeftKey = Keys.Left;
        k.RightKey = Keys.Right;
        k.InventoryLKey = Keys.Q;
        k.FireKey = Keys.K;
        */

        /** This is how to controller input..
        KeyboardInput k = new KeyboardInput();
        k.UpKey = Keys.Up;
        k.DownKey = Keys.Down;
        k.LeftKey = Keys.Left;
        k.RightKey = Keys.Right;
        k.InventoryLKey = Keys.Q;
        k.FireKey = Keys.K;
*/


        
        ControllerInput k = new ControllerInput();
        k.ControllerNumber = 0;
        k.UpKey = 2;
        Inputs[0] = k;
        

        Inputs[1] = null;
        Inputs[2] = null;
        Inputs[3] = null;
    
        ControllerManager = new ControllerManager();
        Title = new Title();
        Logo = Utils.TextureLoader(@"misc\logo.png");
    }

    //invoked from above. Updates all of the states.
    public static void Update(int ms, KeyboardState ks, SlimDX.DirectInput.JoystickState[] joyStates)
    { 

        for (int i = 0; i != 4; i++)
        {
            if (Inputs[i] is KeyboardInput)
                ((KeyboardInput)Inputs[i]).Update(ks);
            if (Inputs[i] is ControllerInput)
                ((ControllerInput)Inputs[i]).Update(joyStates);
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

