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
    private Texture2D TemplateFire;

    public int[] CharacterSelect { get; private set; } = { 0, 0, 0, 0 };

    public CharSelect()
    {

        Pirate = new AnimationSet(@"creatures\pc\pirate\anim.xml");
        Ninja = new AnimationSet(@"creatures\pc\ninja\anim.xml");
        Meximage = new AnimationSet(@"creatures\pc\meximage\anim.xml");
        Dragon = new AnimationSet(@"creatures\pc\dragon\anim.xml");

        Pirate.Hold("idle", 0, 2);
        Ninja.Hold("idle", 0, 2);
        Meximage.Hold("idle", 0, 2);
        Dragon.Hold("idle", 0, 2);
        
        Template = Utils.TextureLoader(@"misc\controller\template.png");
        TemplateFire = Utils.TextureLoader(@"misc\controller\fight.png");
        SoundManager.Play("originlong.ogg");

    }

    //Load each of the characters.
    public GS.State Update(int ms)
    {
        for(int players = 0; players != 4; players++)
        { 
            if(GS.Inputs[players].PressedRight)
            {
                CharacterSelect[players] = (CharacterSelect[players] + 1) % 4;
            }

            if (GS.Inputs[players].PressedLeft)
            {
                CharacterSelect[players] = (CharacterSelect[players] + 3) % 4;
            }
        }

        // return GS.Inputs[0].PressedFire ? GS.State.InGame : GS.State.CharSelect;

         return GS.State.CharSelect;
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

            // rig the characters to line up. If one is selected, raise his/her hand.

            Pirate.Hold("idle", 0, 2);
            Ninja.Hold("idle", 0, 2);
            Meximage.Hold("idle", 0, 2);
            Dragon.Hold("idle", 0, 2);

            switch(CharacterSelect[playerID])
            {
                case 0:
                    Pirate.Hold("swing", 0, 2);
                    Pirate.Draw(sb, 10 + baseX, 20 + baseY + 5);
                    Dragon.Draw(sb, 30 + baseX, 20 + baseY + -4,Color.DarkGray);
                    Meximage.Draw(sb, 50 + baseX, 20 + baseY + -2, Color.DarkGray);
                    Ninja.Draw(sb, 70 + baseX, 20 + baseY, Color.DarkGray);

                    Utils.DefaultFont.Draw(sb, "THE PIRATE", 70 + baseX, 40 + baseY, Color.White,2);
                    Utils.DefaultFont.Draw(sb, "A GOOD ALL AROUND FIGHTER", 70 + baseX, 55 + baseY,Color.White);
                    break;
                case 1:
                    Dragon.Hold("swing", 0, 2);
                    Pirate.Draw(sb, 10 + baseX, 20 + baseY, Color.DarkGray);
                    Dragon.Draw(sb, 30 + baseX, 20 + baseY + -4 + 5);
                    Meximage.Draw(sb, 50 + baseX, 20 + baseY + -2, Color.DarkGray);
                    Ninja.Draw(sb, 70 + baseX, 20 + baseY, Color.DarkGray);

                    Utils.DefaultFont.Draw(sb, "THE DRAGON", 70 + baseX, 40 + baseY, Color.White, 2);
                    Utils.DefaultFont.Draw(sb, "STRONG BUT SLOW PALADIN", 70 + baseX, 55 + baseY, Color.White);
                    break;
                case 2:
                    Meximage.Hold("swing", 0, 2);
                    Pirate.Draw(sb, 10 + baseX, 20 + baseY, Color.DarkGray);
                    Dragon.Draw(sb, 30 + baseX, 20 + baseY + -4, Color.DarkGray);
                    Meximage.Draw(sb, 50 + baseX, 20 + baseY + -2 + 5);
                    Ninja.Draw(sb, 70 + baseX, 20 + baseY, Color.DarkGray);
                    Utils.DefaultFont.Draw(sb, "THE MEXIMAGE", 70 + baseX, 40 + baseY, Color.White, 2);
                    Utils.DefaultFont.Draw(sb, "A MAGE WHO CAN TAKE A HIT", 70 + baseX, 55 + baseY, Color.White);
                    break;
                case 3:
                    Ninja.Hold("swing", 0, 2);
                    Pirate.Draw(sb, 10 + baseX, 20 + baseY, Color.DarkGray);
                    Dragon.Draw(sb, 30 + baseX, 20 + baseY + -4, Color.DarkGray);
                    Meximage.Draw(sb, 50 + baseX, 20 + baseY + -2, Color.DarkGray);
                    Ninja.Draw(sb, 70 + baseX, 20 + baseY + 5);
                    Utils.DefaultFont.Draw(sb, "THE NINJA", 70 + baseX, 40 + baseY, Color.White, 2);
                    Utils.DefaultFont.Draw(sb, "FAST BUT FRAGILE ROGUE", 70 + baseX, 55 + baseY, Color.White);
                    break;
            }

            sb.DrawTexture(Template, 70 + baseX, 70 + baseY);
            if (GS.Inputs[playerID] != null && GS.Inputs[playerID].Fire)
                sb.DrawTexture(TemplateFire, 70 + baseX, 70 + baseY);
        }
        
    }
}

