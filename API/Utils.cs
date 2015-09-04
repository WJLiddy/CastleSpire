﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;


public class Utils
{
    public static Game game;
    public static String pathToAssets;
    public static Texture2D TextureLoader(String pathToTexture)
    {
        System.IO.Stream stream = File.Open(pathToTexture, FileMode.Open);
        return Texture2D.FromStream(game.GraphicsDevice, stream);
    }

}


