using System.Xml;

public class StatSet
{
    public int strXP { get; private set; }
    public int dexXP { get; private set; }
    public int affXP { get; private set; }
    public int vitXP { get; private set; }
    public int spdXP { get; private set; }

    public readonly int XPperSkill = 50;

    public static readonly double baseSpeed = 45;
    public static readonly double skillSpeed = .07;

    public StatSet(string pathToURL)
    {
        XmlReader reader = XmlReader.Create(pathToURL);

        reader.ReadToFollowing("str");
        strXP = XPperSkill * reader.ReadElementContentAsInt();

        reader.ReadToFollowing("dex");
        dexXP = XPperSkill * reader.ReadElementContentAsInt();

        reader.ReadToFollowing("aff");
        affXP = XPperSkill * reader.ReadElementContentAsInt();

        reader.ReadToFollowing("vit");
        vitXP = XPperSkill * reader.ReadElementContentAsInt();

        reader.ReadToFollowing("spd");
        spdXP = XPperSkill * reader.ReadElementContentAsInt();
    }

    public void awardStrXP(int xp)
    {
        strXP += xp;
    }

    public void awardDexXP(int xp)
    {
        dexXP += xp;
    }

    public void awardAffXP(int xp)
    {
        affXP += xp;
    }

    public void awardVitXP(int xp)
    {
        vitXP += xp;
    }

    public void awardSpdXP(int xp)
    {
        spdXP += xp;
    }

    public int str()
    {
        return strXP / XPperSkill;
    }

    public int dex()
    {
        return dexXP / XPperSkill;
    }

    public int aff()
    {
        return affXP / XPperSkill;
    }

    public int vit()
    {
        return vitXP / XPperSkill;
    }

    public int spd()
    {
        return spdXP / XPperSkill;
    }
}

