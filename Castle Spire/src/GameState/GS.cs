
//GameState
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;

public class GS
{
    //Game States we can be in.
    enum State { Title, CharSelect, InGame }
    static State state;

    //A dictionary for all of our animations. Good if we need a one-off use of some animation.
    //NO GOOD because i can only have one given animation at a time. 

    //TODO: There must be a smarter texture2d copier function. We don't want to constantly read from disk. UTILS can help us here.
    //static Hashtable animTable;

    //Title screen draw/updater.
    static Title title;
    //Character Select draw/updater
    static CharSelect charsSlect;
    //Game draw/updater
    static InGame inGame;

    //Small font heavily used in game
    static ADFont spireFont;

    //Set of pirate animations, for testing only
    static AnimSet pirateAnimSet;

    //A single input set.
    static PInput input;

    //Need to initalize stuff specific to the game? Do it here!
    public GS()
    {
        state = State.Title;
        animDefs();
        input = new PInput();
        input.cUP = Keys.W;
        input.cRIGHT = Keys.D;
        input.cDOWN = Keys.S;
        input.cLEFT = Keys.A;
        spireFont = new ADFont(@"misc\spireFont.png");
    }

    public static void animDefs()
    {
        pirateAnimSet =  new AnimSet(@"creatures\pc\pirate\anim.xml");
    }

    public static void update(GameTime delta, KeyboardState ks)
    {
       input.update(ks);

        if (input.UP)
            pirateAnimSet.hold("idle", 0, 0);
        else if (input.RIGHT)
            pirateAnimSet.hold("idle", 0, 1);
        else if (input.DOWN)
            pirateAnimSet.hold("idle", 0, 2);
        else if (input.LEFT)
            pirateAnimSet.hold("idle", 0, 3);
    }

    public static void draw()
    {
        pirateAnimSet.draw(30, 30);
        pirateAnimSet.draw(130, 130, 50,30);

        spireFont.draw("AD Engine", 300, 5, Color.White, 1, true);
    }
}

