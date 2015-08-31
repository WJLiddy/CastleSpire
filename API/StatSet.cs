public class StatSet
{
    public int strXP { get; private set; }
    public int dexXP { get; private set; }
    public int affXP { get; private set; }
    public int vitXP { get; private set; }

    public readonly int XPperSkill;

    public StatSet(int strlvl, int dexlvl, int afflvl, int vitlvl)
    {
        this.strXP = 0;
        this.dexXP = 0;
        this.affXP = 0;
        this.vitXP = 0;
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

    public int getStrSkill()
    {
        return strXP / XPperSkill;
    }

    public int getDexSkill()
    {
        return dexXP / XPperSkill;
    }

    public int getAffSkill()
    {
        return affXP / XPperSkill;
    }

    public int getVitSkill()
    {
        return vitXP / XPperSkill;
    }

}

