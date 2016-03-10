using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class CharSelect
{
    private AnimationSet Pirate;
    private AnimationSet Ninja;
    private AnimationSet Meximage;
    private AnimationSet Dragon;
    private Texture2D Template;

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
        
        Template = Utils.TextureLoader(@"misc\controller\template.png");
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

            /**
            if (CharacterSelect == 0)
                Pirate.Hold("swing", 0, 2);
            if (CharacterSelect == 1)
                Dragon.Hold("swing", 0, 2);
            if (CharacterSelect == 2)
                Meximage.Hold("swing", 0, 2);
            if (CharacterSelect == 3)
                Ninja.Hold("swing", 0, 2);
            */
        }

        return GS.Inputs[0].PressedFire ? GS.State.InGame : GS.State.CharSelect;
    }

    public void Draw(AD2SpriteBatch sb)
    {
        Renderer.GraphicsDevice.Clear(new Color (20,0,28));
        Utils.DrawRect(sb, CastleSpire.BaseWidth / 2, 0, 1, CastleSpire.BaseHeight, Color.White);
        Utils.DrawRect(sb, 0, CastleSpire.BaseHeight / 2, CastleSpire.BaseWidth, 1, Color.White);

        for (int playerID = 0; playerID != 4; playerID++)
        {
            int baseX = (CastleSpire.BaseWidth / 2) * (playerID % 2);
            int baseY = (CastleSpire.BaseHeight / 2) * (playerID / 2);
            Pirate.Draw(sb, 10 + baseX, 20 + baseY);
            Dragon.Draw(sb, 30 + baseX, 20 + baseY);
            Meximage.Draw(sb, 50 + baseX, 20 + baseY);
            Ninja.Draw(sb, 70 + baseX, 20 + baseY);
            sb.DrawTexture(Template, 70+ baseX, 70 + baseY);
        }
        
    }

    internal void Draw()
    {
        throw new NotImplementedException();
    }
}

