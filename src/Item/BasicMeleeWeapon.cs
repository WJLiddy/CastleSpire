using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class BasicMeleeWeapon : Item
{
    public int StrengthRequirement { get; private set; }
    public int DexterityRequirement { get; private set; }
    public int FrameTime { get; private set; }
    public int Damage { get; private set; }
    public Color Tint { get; private set; }

    //generate based on some level requirement
    private BasicMeleeWeapon(string pathToXML, string name, int strReq, int dexReq, int frameTime, int damage, Color c) : base(pathToXML,name)
    {
        StrengthRequirement = strReq;
        DexterityRequirement = dexReq;
        FrameTime = frameTime;
        Damage = damage;
        Tint = c;
    }

    public static BasicMeleeWeapon generateBasicWeapon(int level)
    {
        return generateBasicWeapon(level, Utils.RandomNumber());
    }


    public static BasicMeleeWeapon generateBasicWeapon(int level, double percentStrength)

    {
        //SWING SPEED (12) : SLOW = 5x
        //SWING SPEED (18) : SLOW = 3.3x
        //SWING SPEED (24) : SLOW = 2.5x
        //SWING SPEED (30) : SLOW = 2x
        //SWING SPEED (36) : SLOW = 1.6x
        
        double rarityNumber = generateRarity();

        Color color = generateRarityColor(rarityNumber);
        int str = (int)(level * percentStrength);
        int dex = (int)(level * (1 - percentStrength));

        BasicMeleeWeapon i = null;

        if (percentStrength < .2)
        {
            int damage = 1 + (int)((rarityNumber * percentStrength) / 5.0);
            i = new BasicMeleeWeapon(@"items\melee\knife.xml", "TEST KNIFE", str, dex, 2, damage, color);
        }
        else if (percentStrength < .4)
        {
            int damage = 1 + (int)((rarityNumber * percentStrength) / 3.3);
            i = new BasicMeleeWeapon(@"items\melee\saber.xml", "TEST SABER", str, dex, 3, damage, color);
        }
        else if (percentStrength < .6)
        {
            int damage = 1 + (int)((rarityNumber * percentStrength) / 2.5);
            i = new BasicMeleeWeapon(@"items\melee\sword.xml", "TEST SWORD", str, dex, 4, damage, color);
        }
        else if (percentStrength < .8)
        {
            int damage = 1 + (int)((rarityNumber * percentStrength) / 2.0);
            i = new BasicMeleeWeapon(@"items\melee\axe.xml", "TEST AXE", str, dex, 5, damage, color);
        }
        else if (percentStrength <= 1.0)
        {
            int damage = 1 + (int)((rarityNumber * percentStrength) / 1.7);
            //!!
            i = new BasicMeleeWeapon(@"items\melee\hammer.xml", "TEST HAMMER", str, dex, 6, damage, color);
        }
        return i;
    }

    public static double generateRarity()
    {
        // 1 / 20 weapons are light blue => x1.2
        // 1 / 100 weapons are blue => x1.4
        // 1 / 500 weapons are dark blue  => x1.6
        // 1 / 1000 weapons are light purple => x1.8
        // 1 / 5000 weapons are purple  => x2.0
        double rarityDice = Utils.RandomNumber();
        if (rarityDice < .0002)
            return 2.0;
        if (rarityDice < .001)
            return 1.8;
        if (rarityDice < .002)
            return 1.6;
        if (rarityDice < .01)
            return 1.4;
        if (rarityDice < .05)
            return 1.2;
        return 1.0;
    }

    public static Color generateRarityColor(double d)
    {
        if (d == 2.0)
            return new Color(50, 20, 90);
        if (d == 1.8)
            return new Color(100, 40, 150);
        if (d == 1.6)
            return Color.DarkBlue;
        if (d == 1.4)
            return Color.Blue;
        if (d == 1.2)
            return new Color(125, 125, 255);
        return Color.White;
    }


    public void SetCoords(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override void DrawOnFloor(AD2SpriteBatch sb, int camX, int camY)
    {
        sb.Draw(Texture, new Rectangle((-camX + X + -OffsetX), (-camY + Y + -OffsetY), Texture.Width / 4, Texture.Height), new Rectangle(16, 0, Texture.Width / 4, Texture.Height), Tint);
    }

    public override void DrawAlone(AD2SpriteBatch sb, int x, int y, int dir)
    {
        sb.Draw(Texture, new Rectangle(x, y, Texture.Width / 4, Texture.Height), new Rectangle(dir*16, 0, Texture.Width / 4, Texture.Height), Tint);
    }

    public override void DrawHUDDescription(PC p, AD2SpriteBatch sb, int tx, int ty)
    {
        int nameStartX = tx + ((HUD.PanelWidth / 2) - (Utils.DefaultFont.GetWidth(Name, false) / 2));
        Utils.DefaultFont.Draw(sb, Name, nameStartX, ty, Tint);
        Utils.DefaultFont.Draw(sb, "SWING: " + Damage, 1 + tx, 7 + ty, Color.Red);
        Utils.DefaultFont.Draw(sb, Name, nameStartX, ty, Tint);
    }

    public override bool CanUse(Creature c)
    {
        return true;
    }
}