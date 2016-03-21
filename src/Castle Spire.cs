using Microsoft.Xna.Framework;

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
        new GS();
    }
    
    protected override void AD2Logic(int ms, Microsoft.Xna.Framework.Input.KeyboardState keyboardState, SlimDX.DirectInput.JoystickState[] gamePadState)
    {

        if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            Exit();
      
        GS.Update(ms, Microsoft.Xna.Framework.Input.Keyboard.GetState(),gamePadState);
    }
    
    protected override void AD2Draw(AD2SpriteBatch sb)
    {
        GS.Draw(sb);

        if (IsRunningSlowly())
        {
            Utils.DefaultFont.Draw(sb, "SLOW", 100, 5, Color.White,2,true);
        }
    }
}
