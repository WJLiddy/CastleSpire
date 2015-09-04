using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

public class CastleSpire : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Texture2D logo;
    ADFont spireFont;
    AnimSet pirateAnimSet;
    PInput input;

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
        Directory.SetCurrentDirectory(@"..\..\..\assets\");
        Utils.pathToAssets = Directory.GetCurrentDirectory();
        input = new PInput();
        input.cUP = Keys.Up;
        input.cDOWN = Keys.Down;
        input.cLEFT = Keys.Left;
        input.cRIGHT = Keys.Right;

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
        logo = Utils.TextureLoader(@"misc\logo.png");
        spireFont = new ADFont(@"misc\spireFont.png");
        pirateAnimSet = new AnimSet(@"creatures\pc\pirate\anim.xml");
        pirateAnimSet.hold("idle", 0, 0);

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
        input.update(Keyboard.GetState());
    
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

        //specify a default draw, problably idle, if not idle, whatever is first in the list.


        if (input.UP)
            pirateAnimSet.hold("idle", 0, 0);
        else if (input.RIGHT)
            pirateAnimSet.hold("idle", 0, 1);
        else if (input.DOWN)
            pirateAnimSet.hold("idle", 0, 2);
        else if (input.LEFT)
            pirateAnimSet.hold("idle", 0, 3);


        pirateAnimSet.draw(spriteBatch, 30, 30);

        pirateAnimSet.draw(spriteBatch, 130, 130, 50,30);

        spireFont.draw(spriteBatch, "AD Engine", 200, 5, Color.White,3,true);

        Random r = new Random();

        spireFont.draw(spriteBatch, "BIP WUZ HERE", 0, 100, new Color ( ( (float)(r.NextDouble()) ), (float)((r.NextDouble()) ), (float)((r.NextDouble()) )), 5, true);



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
