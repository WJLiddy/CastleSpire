using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class HUD
{
    public enum Corner { TOPLEFT, TOPRIGHT, BOTTOMLEFT, BOTTOMRIGHT}

    Texture2D backPanel;
    Texture2D itemFrame;
    Texture2D itemFrameBig;
    Texture2D portrait;
    Texture2D statBar;

    private readonly int PANELWIDTH = 53;
    private readonly int PANELHEIGHT = 57;
    private readonly int BARHEIGHT = 31;
    private readonly int BARWIDTH = 22;
    private readonly int NAMESPACEY = 9;
    private readonly int NAMEX = 2;
    private readonly int NAMEY = 1;

    PC player;
    Corner corner;

    public HUD(PC player, Corner corner)
    {
        //TODO: again, a textureloader.
        //load up all the things.
        backPanel = Utils.TextureLoader("hud/backPanel.png");
        itemFrame = Utils.TextureLoader("hud/itemFrame.png");
        itemFrameBig = Utils.TextureLoader("hud/itemFrameBig.png");
        portrait = Utils.TextureLoader("hud/portrait.png");
        statBar = Utils.TextureLoader("hud/statBar.png");
        this.player = player;
        this.corner = corner;
    }

    public void draw()
    {
        drawBars();

    }

    //Corner args
    public void drawBars()
    {
        int x_HUD = 0;
        int y_HUD = 0;
        int x_NAME = 0;
        int y_NAME = 0;
        bool reflectX = false;
        bool reflectY = false;

        switch (corner)
        {
            case Corner.TOPLEFT:
                x_NAME = NAMEX;
                y_NAME = NAMEY;
                x_HUD = 0;
                y_HUD = NAMESPACEY;
                reflectX = false;
                reflectY = false;
                break;

            case Corner.TOPRIGHT:
                //fake
                x_NAME = CastleSpire.baseWidth - ADFont.getScaleWidth(player.name, true);
                y_NAME = NAMEY;
                x_HUD = CastleSpire.baseWidth - PANELWIDTH;
                y_HUD = NAMESPACEY;
                reflectX = true;
                reflectY = false;
                break;

            case Corner.BOTTOMLEFT:
                x_NAME = NAMEX;
                y_NAME = CastleSpire.baseHeight - NAMESPACEY + 1;
                x_HUD = 0;
                y_HUD = CastleSpire.baseHeight +- NAMESPACEY +- PANELHEIGHT;
                reflectX = false;
                reflectY = true;
                break;

            case Corner.BOTTOMRIGHT:
                x_NAME = CastleSpire.baseWidth - ADFont.getScaleWidth(player.name,true);
                y_NAME = CastleSpire.baseHeight + -NAMESPACEY + 1;
                x_HUD = CastleSpire.baseWidth - PANELWIDTH;
                y_HUD = CastleSpire.baseHeight + -NAMESPACEY + -PANELHEIGHT;
                reflectX = true;
                reflectY = true;
                break;
        }


        //Utils.drawTexture(backPanel, x_HUD, y_HUD);
        Utils.drawString(player.name, x_NAME, y_NAME, Color.White, 1, true);

        int additionalYOffset = 0;
        int additionalXOffset = 0;

        if (reflectY)
            additionalYOffset = PANELHEIGHT - BARHEIGHT;

        if (reflectX)
            additionalXOffset = PANELWIDTH - BARWIDTH;


        Utils.drawTexture(statBar, x_HUD + additionalXOffset, y_HUD + additionalYOffset);

        Utils.drawRect(Color.Red, x_HUD + 1 + additionalXOffset, y_HUD + 1 + additionalYOffset, 20, 9);
        Utils.drawRect(Color.Blue, x_HUD + 1 + additionalXOffset, y_HUD + 11 + additionalYOffset, 20, 9);
        Utils.drawRect(Color.Green, x_HUD + 1 + additionalXOffset, y_HUD + 21 + additionalYOffset, 20, 9);

        Utils.drawString(player.HP.ToString(), x_HUD + 4 + additionalXOffset, y_HUD + 2 + additionalYOffset, Color.White, 1, true);
        Utils.drawString(player.MP.ToString(), x_HUD + 4 + additionalXOffset, y_HUD + 12 + additionalYOffset, Color.White, 1, true);
        Utils.drawString(player.FA.ToString(), x_HUD + 4 + additionalXOffset, y_HUD + 22 + additionalYOffset, Color.White, 1, true);
        
    }
    

}
