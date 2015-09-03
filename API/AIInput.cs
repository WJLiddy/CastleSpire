using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class AIInput : Input
{

    public void pressA() {A = true; }
    public void pressB() {B = true; }
    public void pressX() {X = true; }
    public void pressY() {Y = true; }
    public void pressUP() {UP = true; }
    public void pressDOWN() {DOWN = true; }
    public void pressLEFT() {LEFT = true; }
    public void pressRIGHT() {RIGHT = true; }
    public void pressL() {L = true; }
    public void pressR() {R = true; }
    public void pressS() {S = true; }

    public void clear()
    {
        A = false;
        B = false;
        X = false;
        Y = false;
        UP = false;
        DOWN = false;
        LEFT = false;
        RIGHT = false;
        L = false;
        R = false;
        S = false;
    }



}


