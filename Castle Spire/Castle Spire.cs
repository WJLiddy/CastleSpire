using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

public class CastleSpire : Game
{

    //required to load in any graphics
    GraphicsDeviceManager graphics;
    //Everything to draw
    SpriteBatch spriteBatch;

    //our minimum game dimensions. Needed for setting resolution.
    public static readonly int baseWidth = 360;
    public static readonly int baseHeight = 270;

    //Turn this on for fullscreen.
    bool fullScreen = false;

    //If windowed, antialiasing will never happen, so that's good.
    //Else, it might. If we do not want antialiasing, that's good, I'll just draw boxes. Else, fill the whole screen.
    bool fullScreenWithAntiAlias = true;

    //How to scale the resolution.
    private int drawScaleX = 1;
    private int drawScaleY = 1;
    private int drawXOff = 0;
    private int drawYOff = 0;
    Matrix matrixScale;

    public CastleSpire()
    {
        //Here we want to init as much as we can that isn't graphics.

        //Set the current directory that points to where to look for the assets folder.
        Directory.SetCurrentDirectory(@"..\..\..\assets\");

        //Save the path for fast access.
        Utils.pathToAssets = Directory.GetCurrentDirectory() + @"\";

        //init the graphics manager.
        graphics = new GraphicsDeviceManager(this);

        //figure out what resolution to run the game at.
        configureResolution();

        //now that we know the resolution, make the matrix for it for scaling later.
        matrixScale = Matrix.Identity;
        matrixScale.M11 = drawScaleX; matrixScale.M22 = drawScaleY;
        matrixScale.Translation = new Vector3(drawXOff, drawYOff, 0);

    }


    protected override void Initialize()
    {
        base.Initialize();
    }

      
    protected override void LoadContent()
    {
        //Init graphics stuff and save to UTIL library
        spriteBatch = new SpriteBatch(GraphicsDevice);
        Utils.gfx = graphics.GraphicsDevice;
        graphics.SynchronizeWithVerticalRetrace = true;
        Utils.sb = spriteBatch;

        //Init our gamestate with basically will handle everything.
        new GS();
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

        GS.update(gameTime, Keyboard.GetState());

        base.Update(gameTime);
    }

    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        //nuke old graphics.
        GraphicsDevice.Clear(Color.Black);
         
        //set the spritebatch to start.
        spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone,null,matrixScale);

        GS.draw();

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

        else if (!fullScreenWithAntiAlias)
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

        graphics.ApplyChanges();
    }

}
