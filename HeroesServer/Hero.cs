using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer
{
    public class Hero : Character
    {
        public Nature Nature { get; set; }
    }

    public enum Nature
    {
        FIRE = 1,
        LIGHTNING = 2,
        EARTH = 3,
        WOOD = 4
    }
}
