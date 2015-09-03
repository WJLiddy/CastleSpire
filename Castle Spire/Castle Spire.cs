using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

public class CastleSpire : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D logo;
    AnimSheet dragon;
    ADFont spireFont;

    //our minimum game dimensions.
    private readonly int baseWidth = 360;
    private readonly int baseHeight = 270;

    //Turn this on for fullscreen
    bool fullScreen = false;

    //If windowed, antialiasing will never happen. 
    //Else, it might. We can ignore it and deal with blurry edges, or turn this on and I'll try to correct it.
    bool fullScreenWithAlias = true;

    //important: needed to stop anti-aliasing by making the game window small in fullscreen.
    private int drawScaleX = 1;
    private int drawScaleY = 1;
    private int drawXOff = 0;
    private int drawYOff = 0;


    public CastleSpire()
    {
        graphics = new GraphicsDeviceManager(this);
        Utils.game = this;
        configureResolution();

     
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
        logo = Utils.TextureLoader(@"misc\logo.png");
        dragon = new AnimSheet(@"creatures\pc\pirate\walk.xml");
        spireFont = new ADFont(@"misc\spireFont.png");

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
    /// 
    protected override void Update(GameTime gameTime)
    {

        //Fetch player inputs/ clear AI inputs
        //Let AI decide inputs
        //do all update logic.
          

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        Matrix matrixScale = Matrix.Identity;
        matrixScale.M11 = drawScaleX; matrixScale.M22 = drawScaleY;
        matrixScale.Translation = new Vector3(drawXOff, drawYOff, 0);
        spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone,null,matrixScale);

        //dest, source
        //1024×768
        spriteBatch.Draw(logo, new Rectangle(0, 0, baseWidth, baseHeight), Color.White);

        dragon.draw(spriteBatch,2,1,70,150,24,32);

        spireFont.draw(spriteBatch, "AD Engine", 200, 5, Color.White,3,true);



        spriteBatch.End();


        base.Draw(gameTime);
    }

    private void configureResolution()
    {
        graphics.IsFullScreen = fullScreen;

        int resWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        int resHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;


        if (!fullScreen)
        {
            //Simply make the window as big as possible while remaining a multiple. Easy.
            drawScaleX = System.Math.Min(resHeight / baseHeight, resWidth / baseWidth);
            drawScaleY = drawScaleX;

            graphics.PreferredBackBufferHeight = baseHeight * drawScaleY;
            graphics.PreferredBackBufferWidth = baseWidth * drawScaleX;
        }

        else if (fullScreenWithAlias)
        {
            //Simply make the window as big as possible while remaining a multiple.
            drawScaleX = System.Math.Min(resHeight / baseHeight, resWidth / baseWidth);
            drawScaleY = drawScaleX;

            //now, we actually do want to set the dimentions correctly.
            graphics.PreferredBackBufferHeight = resHeight;
            graphics.PreferredBackBufferWidth = resWidth;

            //now, find offsets.
            drawXOff = (resWidth - (baseWidth * drawScaleY)) / 2;
            drawYOff = (resHeight - (baseHeight * drawScaleX)) / 2;

        }
        else
        {

            //full screen, no alias
            //boy this is gonna be ugly but you asked for it.
            drawScaleY = 1;
            drawScaleX = 1;
        }


        //if aliasing is false... your fate is in the hands of C# now. May Bill help you now.
        //  if (!alias)
        // {
        //     scale = 1;
        //    graphics.PreferredBackBufferHeight = 270;
        //   graphics.PreferredBackBufferWidth = 480;
        //}




        graphics.ApplyChanges();


    }

}
