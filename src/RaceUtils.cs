using Microsoft.Xna.Framework.Graphics;

public class RaceUtils
{ 
    public enum Race { Pirate, Dragon, Meximage, Ninja }
    public static Texture2D[] Portraits;

    public static void Load()
    {
        Portraits = new Texture2D[4];
        Portraits[(int)(Race.Pirate)] = Utils.TextureLoader("creatures/pc/pirate/icon.png");
        Portraits[(int)(Race.Dragon)] = Utils.TextureLoader("creatures/pc/dragon/icon.png");
        Portraits[(int)(Race.Meximage)] = Utils.TextureLoader("creatures/pc/meximage/icon.png");
        Portraits[(int)(Race.Ninja)] = Utils.TextureLoader("creatures/pc/ninja/icon.png");
    }

    public static Texture2D GetPotrait(Race r)
    {
        if(Portraits == null)
            Load();
        return Portraits[(int)r];
    }

}
