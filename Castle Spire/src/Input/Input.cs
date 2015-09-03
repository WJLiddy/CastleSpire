using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

abstract class Input
{
    public bool A { get; protected set; }
    public bool B { get; protected set; }
    public bool X { get; protected set; }
    public bool Y { get; protected set; }

    public bool UP { get; protected set; }
    public bool DOWN { get; protected set; }
    public bool LEFT { get; protected set; }
    public bool RIGHT { get; protected set; }

    public bool L { get; protected set; }
    public bool R { get; protected set; }

    public bool S { get; protected set; }
}
