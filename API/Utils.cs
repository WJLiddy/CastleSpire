using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class Utils
{
    public static Texture2D TextureLoader(String pathToTexture, GraphicsDevice gd)
    {
        System.IO.Stream stream = File.Open(pathToTexture, FileMode.Open);
        return Texture2D.FromStream(gd, stream);
    }

}


