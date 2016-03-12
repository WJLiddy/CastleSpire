using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        Dictionary<string,LinkedList<string>> control = Utils.GetXMLEntriesHash("config/controls.xml");

        for (int i = 1; i != 5; i++)
        {
            if (!control.ContainsKey("P" + i + "Input"))
                continue;
            if (control["P" + i + "Input"].First.Value.Equals("keyboard"))
            {
                Utils.Log("Player " + i + " is using a keyboard");

                KeyboardInput kinput = new KeyboardInput();

                kinput.UpKey = findKey(control["P" + i + "Up"].First.Value);
                kinput.DownKey = findKey(control["P" + i + "Down"].First.Value);
                kinput.LeftKey = findKey(control["P" + i + "Left"].First.Value);
                kinput.RightKey = findKey(control["P" + i + "Right"].First.Value);
                kinput.FireKey = findKey(control["P" + i + "Fight"].First.Value);
                kinput.BlockKey = findKey(control["P" + i + "Block"].First.Value);
                Inputs[i - 1] = kinput;

            }
            if (control["P" + i + "Input"].First.Value.Contains("controller"))
            {
                string controllerNo = control["P" + i + "Input"].First.Value;
                int number = Int32.Parse(Regex.Match(controllerNo, @"\d+").Value);
                Utils.Log("Player " + i + " is using controller " + number);
                //controller stuff.
                ControllerInput cinput = new ControllerInput();
                cinput.FireKey = Int32.Parse(control["P" + i + "Fight"].First.Value);
                cinput.BlockKey = Int32.Parse(control["P" + i + "Block"].First.Value);
                Inputs[i - 1] = cinput;
            }
        }

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
                    InGame = new InGame(CharSelect.CharacterSelect[0]);
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


    private Keys findKey(string key)
    {
        foreach(Keys k in Enum.GetValues(typeof(Keys)))
        {
            if (key.Equals(k.ToString()))
                return k;
        }
        Utils.Log("Warning: key " + key + " could not be found on the keyboard, so it was not bound.");
        return Keys.None;
    }
}

