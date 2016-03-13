using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class HUD
{
    public enum Corner { TOPLEFT, TOPRIGHT, BOTTOMLEFT, BOTTOMRIGHT}

    Texture2D BackPanel;
    Texture2D ItemFrame;
    Texture2D ItemFrameBig;
    Texture2D StatBar;

    //The entire panel area for the HUD.
    private static readonly int PanelWidth = 52;
    private static readonly int PanelHeight = 86;


    private static readonly int PortraitHeight = 29;
    private static readonly int PortraitWidth = 30;

    //Width, height of stats bar.
    private static readonly int BarHeight = 31;
    private static readonly int BarWidth = 22;

    private static readonly int ItemBarWidth = 117;


    //Height of the Name.
    private static readonly int NameSpaceY = 9;

    //Location of the name.
    private static readonly int NameX = 2;
    private static readonly int NameY = 1;

    //Height of name frmae
    private static readonly int ItemFrameHeight = 21;

    //Height of name frmae
    private static readonly int ItemFrameWidth = 117;

    //The extend factor. 0 to 1.
    private double Extend = 0;

    //if we should extend teh GUI out then true.
    private bool Extendify = false;

    //Timer to wait before shrinking the gui.
    private double IdleTimer;
    private double IdleTimerMax = 1.0;

    private class DrawParams
    {
        public int HUDX = 0;
        public int HUDY = 0;
        public int NameX = 0;
        public int NameY = 0;
        public bool ReflectX = false;
        public bool ReflectY = false;
    }

    
    private class Coord
    {
        public int X = 0;
        public int Y = 0;
    }


    PC Player;
    Corner Ccorner;

    public HUD(PC player, Corner corner)
    {
        //TODO: again, a textureloader.
        //load up all the things.
        BackPanel = Utils.TextureLoader("hud/backPanel.png");
        ItemFrame = Utils.TextureLoader("hud/itemFrame.png");
        ItemFrameBig = Utils.TextureLoader("hud/itemFrameBig.png");
        StatBar = Utils.TextureLoader("hud/statBar.png");
        Player = player;
        Ccorner = corner;
    }

    public void Draw(AD2SpriteBatch sb)
    {
        DrawBars(sb);
    }

    public void Update(Input i, int ms)
    {
        if (!Extendify)
        {
            if (IdleTimer > 0)
                IdleTimer -= (double)ms/1000;
            else if (Extend > 0)
            {
                Extend -= (double)ms / 1000;
            }
        }

        if ((i.InventoryL || i.InventoryR))
            Extendify = true;

        if (Extendify)
        {
            IdleTimer = IdleTimerMax;
            Extend += (double)ms / 1000;
            if (Extend >= 1)
            {
                Extend = 1;
                Extendify = false;
            }
        }

    }



    //Corner args
    public void DrawBars(AD2SpriteBatch sb)
    {
        DrawParams d = generateDrawParams();
        //name
        Utils.DefaultFont.Draw(sb, Player.Name, d.NameX, d.NameY, Color.White, 1, true);
        Coord bar = findBarPosition(d);
        Coord item = findItemPosition(d, bar);
        //Extended GUI.
        drawExtendedHUD(sb, d,item);


        sb.DrawTexture(StatBar, d.HUDX + bar.X, d.HUDY + bar.Y);
       
        sb.DrawTexture(ItemFrame, d.HUDX + item.X, d.HUDY  + item.Y);

        if (Player.Inventory[Player.InvIndex] != null)
        {
            Player.Inventory[Player.InvIndex].DrawAlone(sb, d.HUDX + item.X, d.HUDY + item.Y + 2,1);
        }


        Utils.DrawRect(sb, d.HUDX + 1 + bar.X, d.HUDY + 1 + bar.Y, 20, 9, Color.Red);
        Utils.DrawRect(sb, d.HUDX + 1 + bar.X, d.HUDY + 11 + bar.Y, 20, 9, Color.Blue);
        Utils.DrawRect(sb, d.HUDX + 1 + bar.X, d.HUDY + 21 + bar.Y, 20, 9, Color.Green);

        Utils.DefaultFont.Draw(sb, Player.HP.ToString(), d.HUDX + 4 + bar.X, d.HUDY + 2 + bar.Y, Color.White, 1, true);
        Utils.DefaultFont.Draw(sb, Player.MP.ToString(), d.HUDX + 4 + bar.X, d.HUDY + 12 + bar.Y, Color.White, 1, true);
        Utils.DefaultFont.Draw(sb, Player.FA.ToString(), d.HUDX + 4 + bar.X, d.HUDY + 22 + bar.Y, Color.White, 1, true);

  
    }
    
    private DrawParams generateDrawParams()
    {
        DrawParams d = new DrawParams(); 

        switch (Ccorner)
        {
            case Corner.TOPLEFT:
                d.NameX = NameX;
                d.NameY = NameY;
                d.HUDX = 0;
                d.HUDY = NameSpaceY;
                d.ReflectX = false;
                d.ReflectY = false;
                break;

            case Corner.TOPRIGHT:
                //fake
                d.NameX = CastleSpire.BaseWidth - Utils.DefaultFont.GetWidth(Player.Name, true);
                d.NameY = NameY;
                d.HUDX = CastleSpire.BaseWidth - PanelWidth;
                d.HUDY = NameSpaceY;
                d.ReflectX = true;
                d.ReflectY = false;
                break;

            case Corner.BOTTOMLEFT:
                d.NameX = NameX;
                d.NameY = CastleSpire.BaseHeight - NameSpaceY + 1;
                d.HUDX = 0;
                d.HUDY = CastleSpire.BaseHeight + -NameSpaceY + -PanelHeight;
                d.ReflectX = false;
                d.ReflectY = true;
                break;

            case Corner.BOTTOMRIGHT:
                d.NameX = CastleSpire.BaseWidth - Utils.DefaultFont.GetWidth(Player.Name, true);
                d.NameY = CastleSpire.BaseHeight + -NameSpaceY + 1;
                d.HUDX = CastleSpire.BaseWidth - PanelWidth;
                d.HUDY = CastleSpire.BaseHeight + -NameSpaceY + -PanelHeight;
                d.ReflectX = true;
                d.ReflectY = true;
                break;
        }
        return d;
    }

    private void drawExtendedHUD(AD2SpriteBatch sb, DrawParams d, Coord item)
    {
        //back panel
        int hideOffset = (d.ReflectX ? (PanelWidth + -(int)(Extend * PanelWidth)) : (-PanelWidth + (int)(Extend * PanelWidth)));

        sb.DrawTexture(BackPanel, d.HUDX + hideOffset, d.HUDY);

        Coord portrait = findPortraitPosition(d, hideOffset);

        int itemHideOffset = (d.ReflectX ? (ItemFrameWidth + -(int)(Extend * ItemFrameWidth)) : (-ItemFrameWidth + (int)(Extend * ItemFrameWidth)));
        Coord itemBar = findItemBarPosition(d, item, itemHideOffset);


        //portrait
        if (d.ReflectX)
        {
            sb.DrawTextureHFlip(RaceUtils.GetPotrait(Player.Race), d.HUDX + portrait.X, d.HUDY + portrait.Y);
            sb.DrawTextureHFlip(ItemFrameBig, d.HUDX + itemBar.X, d.HUDY + itemBar.Y);
        }
        else
        {
            sb.DrawTexture(RaceUtils.GetPotrait(Player.Race), d.HUDX + portrait.X, d.HUDY + portrait.Y);
            sb.DrawTexture(ItemFrameBig, d.HUDX + itemBar.X, d.HUDY + itemBar.Y);
        }
        
    }

    private Coord findBarPosition(DrawParams d)
    {
        Coord bar = new Coord();

        if (d.ReflectY)
            bar.Y = PanelHeight - BarHeight;

        if (d.ReflectX)
            bar.X = PanelWidth - BarWidth;

        return bar;
    }

    private Coord findItemPosition(DrawParams d, Coord bar)
    {
       Coord item = new Coord();
       item.Y = bar.Y + BarHeight + (-1);
       item.X = 0;

        if (d.ReflectY)
            item.Y = PanelHeight + -BarHeight + -ItemFrameHeight + (1);

        if (d.ReflectX)
            item.X = PanelWidth + -ItemFrameHeight;

        return item;
    }

    private Coord findPortraitPosition(DrawParams d, int hideOffset)
    {
        Coord portrait = new Coord();
        portrait.Y = 1;
        portrait.X = PanelWidth +- (PortraitWidth) + -1 + hideOffset;

        if (d.ReflectY)
            portrait.Y = PanelHeight + -PortraitHeight  + -1;

        if (d.ReflectX)
            portrait.X = 1 + hideOffset;

        return portrait;
    }

    private Coord findItemBarPosition(DrawParams d, Coord item, int itemOffset)
    {
        Coord itemBar = new Coord();
        itemBar.Y = item.Y;
        itemBar.X = itemOffset;

        if (d.ReflectX)
            itemBar.X = -ItemBarWidth + PanelWidth + itemOffset;

        return itemBar;
    }
}

