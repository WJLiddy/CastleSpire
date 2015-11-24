using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;

public class CastleSpire : AD2Game
{
    //our minimum game dimensions. Needed for setting resolution.
    public static readonly int BaseWidth = 360;
    public static readonly int BaseHeight = 270;

    public CastleSpire() : base (BaseWidth, BaseHeight, 100/6)
    {
        Renderer.Resolution = Renderer.ResolutionType.WindowedLarge;
    }
      
    protected override void AD2LoadContent()
    {
        //Init our gamestate with basically will handle everything.
        new GS();
    }
    
    protected override void AD2Logic(int ms, KeyboardState keyboardState, GamePadState[] gamePadState)
    {

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        // Check the device for Player One
        GamePadState[] gamepads = new GamePadState[4]{
            GamePad.GetState(PlayerIndex.One),
            GamePad.GetState(PlayerIndex.Two),
            GamePad.GetState(PlayerIndex.Three),
            GamePad.GetState(PlayerIndex.Four)
        };

        GS.Update(ms, Keyboard.GetState(),gamepads);
    }
    
    protected override void AD2Draw(AD2SpriteBatch sb)
    {
        GS.Draw(sb);
    }
}
