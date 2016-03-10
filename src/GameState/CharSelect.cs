using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class CharSelect
{
    private AnimationSet Pirate;
    private AnimationSet Ninja;
    private AnimationSet Meximage;
    private AnimationSet Dragon;
    private Texture2D Arrow;

    public int CharacterSelect { get; private set; } = 0;

    public CharSelect()
    {
        Pirate = new AnimationSet(@"creatures\pc\pirate\anim.xml");
        Ninja = new AnimationSet(@"creatures\pc\ninja\anim.xml");
        Meximage = new AnimationSet(@"creatures\pc\meximage\anim.xml");
        Dragon = new AnimationSet(@"creatures\pc\dragon\anim.xml");

        Pirate.Hold("swing", 0, 2);
        Ninja.Hold("idle", 0, 2);
        Meximage.Hold("idle", 0, 2);
        Dragon.Hold("idle", 0, 2);

        Arrow = Utils.TextureLoader(@"misc\bigDownArrow.png");
        SoundManager.Play("originlong.ogg");

    }

    //Load each of the characters.
    public GS.State Update(int ms)
    {
        if(GS.Inputs[0].PressedRight)
        {
            CharacterSelect = (CharacterSelect + 1) % 4;
        }

        if (GS.Inputs[0].PressedLeft)
        {
            CharacterSelect = CharacterSelect == 0 ? 3 : CharacterSelect - 1;
        }

        if (GS.Inputs[0].PressedRight || GS.Inputs[0].PressedLeft)
        {

            Pirate.Hold("idle", 0, 2);
            Ninja.Hold("idle", 0, 2);
            Meximage.Hold("idle", 0, 2);
            Dragon.Hold("idle", 0, 2);

            if (CharacterSelect == 0)
                Pirate.Hold("swing", 0, 2);
            if (CharacterSelect == 1)
                Dragon.Hold("swing", 0, 2);
            if (CharacterSelect == 2)
                Meximage.Hold("swing", 0, 2);
            if (CharacterSelect == 3)
                Ninja.Hold("swing", 0, 2);
        }

        return GS.Inputs[0].PressedFire ? GS.State.InGame : GS.State.CharSelect;
    }

    public void Draw(AD2SpriteBatch sb)
    {
        Renderer.GraphicsDevice.Clear(new Color (20,0,28));
        Utils.DefaultFont.Draw(sb, "CHOOSE YOUR CHARACTER", 50, 50, Color.White, 3);
        Pirate.Draw(sb, 10 + 50, 10 + 100,24*2,32*2);
        Dragon.Draw(sb, 10 + 100, 10 + 100, 24 * 2, 32 * 2);
        Meximage.Draw(sb, 10 + 150, 10 + 100, 24 * 2, 32 * 2);
        Ninja.Draw(sb, 10 + 200, 10 + 100, 24 * 2, 32 * 2);

        sb.DrawTexture(Arrow, 63 + (50 * CharacterSelect), 91, Color.DarkOrange);
    }

    internal void Draw()
    {
        throw new NotImplementedException();
    }
}

