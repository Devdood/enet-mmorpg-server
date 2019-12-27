using HeroesServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerUtilities
{
    public class CombatManager
    {
        public class HitData
        {
            public CharacterInfo attacker;
            public List<CharacterInfo> targets = new List<CharacterInfo>();
        }

        public static void HitTarget(HitData data)
        {
            foreach (var item in data.targets)
            {
                if(item.Character.IsDead)
                {
                    continue;
                }

                item.Character.Health -= 10;

                if(item.Character.Health <= 0)
                {
                    item.Character.IsDead = true;
                }

                if(item.Character.IsDead)
                {
                    CharactersManager.Instance.RemoveCharacter(item);
                }
            }
        }
    }
}
