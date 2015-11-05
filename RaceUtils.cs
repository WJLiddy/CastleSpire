using Microsoft.Xna.Framework.Graphics;

public class RaceUtils
{ 
    public enum Race { Pirate, Dragon, Meximage, Ninja }
    public static Texture2D[] portraits;

    public static void load()
    {
        portraits = new Texture2D[4];
        portraits[(int)(Race.Pirate)] = Utils.TextureLoader("creatures/pc/pirate/icon.png");
        portraits[(int)(Race.Dragon)] = Utils.TextureLoader("creatures/pc/dragon/icon.png");
        portraits[(int)(Race.Meximage)] = Utils.TextureLoader("creatures/pc/meximage/icon.png");
        portraits[(int)(Race.Ninja)] = Utils.TextureLoader("creatures/pc/ninja/icon.png");
    }

    public static Texture2D getPotrait(Race r)
    {
        return portraits[(int)r];
    }

}
