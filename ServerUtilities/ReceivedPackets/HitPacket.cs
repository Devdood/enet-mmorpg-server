using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroesServer.ReceivedPackets
{
    public class HitPacket : PacketBase
    {
        public HitPacket() : base()
        {
        }

        public override void Read(Client client)
        {
            ServerUtilities.CombatManager.HitData data = new ServerUtilities.CombatManager.HitData();
            data.attacker = client.CharacterInfo;

            byte count = reader.ReadByte();
            for (int i = 0; i < count; i++)
            {
                int id = reader.ReadInt32();

                if(CharactersManager.Instance.GetCharacterInfo(id, out CharacterInfo character))
                {
                    data.targets.Add(character);
                }
            }

            CombatManager.HitTarget(data);
        }

        public override void Write()
        {
        }
    }
}
