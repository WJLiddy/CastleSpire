
//GameState
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

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

    //Small font heavily used in game
    static ADFont spireFont;
    
    public static Texture2D logo;

    public static GameTime lastDelta;

    //A single input set.
    public static PInput input { get; private set; }

    //Need to initalize stuff specific to the game? Do it here!
    public GS()
    {
        state = State.Title;
        input = new PInput();
        input.cUP = Keys.W;
        input.cRIGHT = Keys.D;
        input.cDOWN = Keys.S;
        input.cLEFT = Keys.A;
        input.cA = Keys.J;
        spireFont = new ADFont(@"misc\spireFont.png");

        title = new Title();
        logo = Utils.TextureLoader(@"misc\logo.png");
    }


    public static void update(GameTime delta, KeyboardState ks)
    {

        lastDelta = delta;

        input.update(ks);

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

