using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroesServer
{
    public class Character
    {
        public int Id { get; set; }
        public short BaseId { get; set; }
        public int Health { get; set; } = 100;
        public int MaxHealth { get; set; } = 100;
        public bool IsDead { get; set; }
        public Vector3 Position { get; set; }
    }
}
