using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class HUD
{
    public enum Corner { TOPLEFT, TOPRIGHT, BOTTOMLEFT, BOTTOMRIGHT}

    Texture2D backPanel;
    Texture2D itemFrame;
    Texture2D itemFrameBig;
    Texture2D statBar;

    //The entire panel area for the HUD.
    private static readonly int PANELWIDTH = 52;
    private static readonly int PANELHEIGHT = 86;


    private static readonly int PORTRAITHEIGHT = 29;
    private static readonly int PORTRAITWIDTH = 30;

    //Width, height of stats bar.
    private static readonly int BARHEIGHT = 31;
    private static readonly int BARWIDTH = 22;

    private static readonly int ITEMBARWIDTH = 117;


    //Height of the Name.
    private static readonly int NAMESPACEY = 9;

    //Location of the name.
    private static readonly int NAMEX = 2;
    private static readonly int NAMEY = 1;

    //Height of name frmae
    private static readonly int ITEMFRAMEHEIGHT = 21;

    //Height of name frmae
    private static readonly int ITEMFRAMEBIGWIDTH = 117;

    //The extend factor. 0 to 1.
    private  double extend = 0;

    //if we should extend teh GUI out then true.
    private bool extendify = false;

    //Timer to wait before shrinking the gui.
    private double idleTimer;
    private double idleTimerMax = 1.0;

    private class DrawParams
    {
        public int x_HUD = 0;
        public int y_HUD = 0;
        public int x_NAME = 0;
        public int y_NAME = 0;
        public bool reflectX = false;
        public bool reflectY = false;
    }

    
    private class Coord
    {
        public int x = 0;
        public int y = 0;
    }


    PC player;
    Corner corner;

    public HUD(PC player, Corner corner)
    {
        //TODO: again, a textureloader.
        //load up all the things.
        backPanel = Utils.TextureLoader("hud/backPanel.png");
        itemFrame = Utils.TextureLoader("hud/itemFrame.png");
        itemFrameBig = Utils.TextureLoader("hud/itemFrameBig.png");
        statBar = Utils.TextureLoader("hud/statBar.png");
        this.player = player;
        this.corner = corner;
    }

    public void draw()
    {
        drawBars();
    }

    public void update(Input i, GameTime dsec)
    {
        if (!extendify)
        {
            if (idleTimer > 0)
                idleTimer -= dsec.ElapsedGameTime.TotalSeconds;
            else if (extend > 0)
            {
                extend -= dsec.ElapsedGameTime.TotalSeconds;
            }
        }

        if ((i.INVL || i.INVR))
            extendify = true;

        if (extendify)
        {
            idleTimer = idleTimerMax;
            extend += dsec.ElapsedGameTime.TotalSeconds;
            if (extend >= 1)
            {
                extend = 1;
                extendify = false;
            }
        }

    }



    //Corner args
    public void drawBars()
    {
        DrawParams d = generateDrawParams();
        //name
        Utils.drawString(player.name, d.x_NAME, d.y_NAME, Color.White, 1, true);
        Coord bar = findBarPosition(d);
        Coord item = findItemPosition(d, bar);
        //Extended GUI.
        drawExtendedHUD(d,item);


        Utils.drawTexture(statBar, d.x_HUD + bar.x, d.y_HUD + bar.y);

        Utils.drawTexture(itemFrame, d.x_HUD + item.x, d.y_HUD  + item.y);       

        Utils.drawRect(Color.Red, d.x_HUD + 1 + bar.x, d.y_HUD + 1 + bar.y, 20, 9);
        Utils.drawRect(Color.Blue, d.x_HUD + 1 + bar.x, d.y_HUD + 11 + bar.y, 20, 9);
        Utils.drawRect(Color.Green, d.x_HUD + 1 + bar.x, d.y_HUD + 21 + bar.y, 20, 9);

        Utils.drawString(player.HP.ToString(), d.x_HUD + 4 + bar.x, d.y_HUD + 2 + bar.y, Color.White, 1, true);
        Utils.drawString(player.MP.ToString(), d.x_HUD + 4 + bar.x, d.y_HUD + 12 + bar.y, Color.White, 1, true);
        Utils.drawString(player.FA.ToString(), d.x_HUD + 4 + bar.x, d.y_HUD + 22 + bar.y, Color.White, 1, true);

  
    }
    
    private DrawParams generateDrawParams()
    {
        DrawParams d = new DrawParams(); 

        switch (corner)
        {
            case Corner.TOPLEFT:
                d.x_NAME = NAMEX;
                d.y_NAME = NAMEY;
                d.x_HUD = 0;
                d.y_HUD = NAMESPACEY;
                d.reflectX = false;
                d.reflectY = false;
                break;

            case Corner.TOPRIGHT:
                //fake
                d.x_NAME = CastleSpire.baseWidth - Utils.defaultFont.getWidth(player.name, true);
                d.y_NAME = NAMEY;
                d.x_HUD = CastleSpire.baseWidth - PANELWIDTH;
                d.y_HUD = NAMESPACEY;
                d.reflectX = true;
                d.reflectY = false;
                break;

            case Corner.BOTTOMLEFT:
                d.x_NAME = NAMEX;
                d.y_NAME = CastleSpire.baseHeight - NAMESPACEY + 1;
                d.x_HUD = 0;
                d.y_HUD = CastleSpire.baseHeight + -NAMESPACEY + -PANELHEIGHT;
                d.reflectX = false;
                d.reflectY = true;
                break;

            case Corner.BOTTOMRIGHT:
                d.x_NAME = CastleSpire.baseWidth - Utils.defaultFont.getWidth(player.name, true);
                d.y_NAME = CastleSpire.baseHeight + -NAMESPACEY + 1;
                d.x_HUD = CastleSpire.baseWidth - PANELWIDTH;
                d.y_HUD = CastleSpire.baseHeight + -NAMESPACEY + -PANELHEIGHT;
                d.reflectX = true;
                d.reflectY = true;
                break;
        }
        return d;
    }

    private void drawExtendedHUD(DrawParams d, Coord item)
    {
        //back panel
        int hideOffset = (d.reflectX ? (PANELWIDTH + -(int)(extend * PANELWIDTH)) : (-PANELWIDTH + (int)(extend * PANELWIDTH)));

        Utils.drawTexture(backPanel, d.x_HUD + hideOffset, d.y_HUD);

        Coord portrait = findPortraitPosition(d, hideOffset);

        int itemHideOffset = (d.reflectX ? (ITEMFRAMEBIGWIDTH + -(int)(extend * ITEMFRAMEBIGWIDTH)) : (-ITEMFRAMEBIGWIDTH + (int)(extend * ITEMFRAMEBIGWIDTH)));
        Coord itemBar = findItemBarPosition(d, item, itemHideOffset);


        //portrait
        if (d.reflectX)
        {
            Utils.drawTextureHFlip(RaceUtils.getPotrait(player.race), d.x_HUD + portrait.x, d.y_HUD + portrait.y);
            Utils.drawTextureHFlip(itemFrameBig, d.x_HUD + itemBar.x, d.y_HUD + itemBar.y);
        }
        else
        {
            Utils.drawTexture(RaceUtils.getPotrait(player.race), d.x_HUD + portrait.x, d.y_HUD + portrait.y);
            Utils.drawTexture(itemFrameBig, d.x_HUD + itemBar.x, d.y_HUD + itemBar.y);
        }
        
    }

    private Coord findBarPosition(DrawParams d)
    {
        Coord bar = new Coord();

        if (d.reflectY)
            bar.y = PANELHEIGHT - BARHEIGHT;

        if (d.reflectX)
            bar.x = PANELWIDTH - BARWIDTH;

        return bar;
    }

    private Coord findItemPosition(DrawParams d, Coord bar)
    {
       Coord item = new Coord();
       item.y = bar.y + BARHEIGHT + (-1);
       item.x = 0;

        if (d.reflectY)
            item.y = PANELHEIGHT + -BARHEIGHT + -ITEMFRAMEHEIGHT + (1);

        if (d.reflectX)
            item.x = PANELWIDTH + -ITEMFRAMEHEIGHT;

        return item;
    }

    private Coord findPortraitPosition(DrawParams d, int hideOffset)
    {
        Coord portrait = new Coord();
        portrait.y = 1;
        portrait.x = PANELWIDTH +- (PORTRAITWIDTH) + -1 + hideOffset;

        if (d.reflectY)
            portrait.y = PANELHEIGHT + -PORTRAITHEIGHT  + -1;

        if (d.reflectX)
            portrait.x = 1 + hideOffset;

        return portrait;
    }

    private Coord findItemBarPosition(DrawParams d, Coord item, int itemOffset)
    {
        Coord itemBar = new Coord();
        itemBar.y = item.y;
        itemBar.x = itemOffset;

        if (d.reflectX)
            itemBar.x = -ITEMBARWIDTH + PANELWIDTH + itemOffset;

        return itemBar;
    }
}

