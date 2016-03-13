using System.Xml;

public class StatSet
{
    public int StrXP { get; private set; }
    public int DexXP { get; private set; }
    public int AffXP { get; private set; }
    public int VitXP { get; private set; }
    public int SpdXP { get; private set; }

    public readonly int XPPerSkill = 50;


    public static readonly int HPPerStat = 10;
    public static readonly int MPPerStat = 10;
    public static readonly double BaseSpeed = 45;
    public static readonly double SkillSpeed = 1;

    // In Zombies
    // There are 50 waves.
    // I want to give 2 levels ~ 100 XP per wave.
    // So I will distribute 65 levels starting out.

    public StatSet(string pathToURL)
    {
        XmlReader reader = XmlReader.Create(pathToURL);

        reader.ReadToFollowing("str");
        StrXP = XPPerSkill * reader.ReadElementContentAsInt();

        reader.ReadToFollowing("dex");
        DexXP = XPPerSkill * reader.ReadElementContentAsInt();

        reader.ReadToFollowing("aff");
        AffXP = XPPerSkill * reader.ReadElementContentAsInt();

        reader.ReadToFollowing("vit");
        VitXP = XPPerSkill * reader.ReadElementContentAsInt();

        reader.ReadToFollowing("spd");
        SpdXP = XPPerSkill * reader.ReadElementContentAsInt();
    }

    public void AwardStrXP(int xp)
    {
        StrXP += xp;
    }

    public void AwardDexXP(int xp)
    {
        DexXP += xp;
    }

    public void AwardAffXP(int xp)
    {
        AffXP += xp;
    }

    public void AwardVitXP(int xp)
    {
        VitXP += xp;
    }

    public void AwardSpdXP(int xp)
    {
        SpdXP += xp;
    }

    public int Str()
    {
        return StrXP / XPPerSkill;
    }

    public int Dex()
    {
        return DexXP / XPPerSkill;
    }

    public int Aff()
    {
        return AffXP / XPPerSkill;
    }

    public int Vit()
    {
        return VitXP / XPPerSkill;
    }

    public int Spd()
    {
        return SpdXP / XPPerSkill;
    }
}

