using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

public class CastleSpire : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D logo;

    public CastleSpire()
    {
        graphics = new GraphicsDeviceManager(this);
    }

        
    protected override void Initialize()
    {
        base.Initialize();
    }

      
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);


        // Load a SoundEffect from a file 
        // (NOTE: This is NOT in the current version of MonoGame)
        //  stream = TitleContainer.OpenStream("Content/My Sound.wav");
        // mySound = SoundEffect.FromStream(stream);

        //TODO: Link on non-release builds. 
        Directory.SetCurrentDirectory(@"C:\Users\JACK\Desktop\Projects\ad2-engine\Castle Spire\assets\");
       logo = Utils.TextureLoader("test", graphics.GraphicsDevice);
    }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
          

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin();

        //dest, source
        spriteBatch.Draw(logo, new Rectangle(0, 0, 800, 480), Color.White);

        spriteBatch.End();


        base.Draw(gameTime);
    }

}
