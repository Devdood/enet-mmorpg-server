using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroesServer.SendPackets
{
    public class SetPositionsPacket : PacketBase
    {
        private bool local = false;

        public SetPositionsPacket(List<CharacterSnapshot> snapshots) : base()
        {
            writer.Write((byte)4);
            writer.Write((byte)snapshots.Count);
            foreach (var item in snapshots)
            {
                writer.Write(item.id);
                writer.Write(item.pos.X);
                writer.Write(item.pos.Y);
                writer.Write(item.pos.Z);
            }
        }

        public override void Read(Client client)
        {
        }

        public override void Write()
        {

        }

        public struct CharacterSnapshot
        {
            public int id;
            public Vector3 pos;
        }
    }
}
