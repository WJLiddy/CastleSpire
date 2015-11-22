using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Title
{
    Texture2D Splash;
    public Title()
    {
        Splash = Utils.TextureLoader(@"misc\splash.png");
        SoundManager.Play("welcome.ogg");
    }

    public GS.State Update(int ms, KeyboardState ks)
    {
        if (ks.IsKeyDown(Keys.Enter))
        {
            SoundManager.Stop();
            return GS.State.CharSelect;
        }
        else
            return GS.State.Title;
    }

    public void Draw(AD2SpriteBatch sb)
    {
        sb.Draw(Splash,new Rectangle(0,0,360,270),Color.White);
        //TODO: f.draw center.
        Utils.DefaultFont.Draw(sb, "PRESS ENTER", 100, 200, Color.White, 3, true);

    }
}

