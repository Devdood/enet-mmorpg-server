using HeroesServer;
using HeroesServer.ReceivedPackets;
using HeroesServer.SendPackets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeroesServer
{
    public class PacketsManager
    {
        private static Dictionary<byte, Type> packets = new Dictionary<byte, Type>();

        public static void Initialize()
        {
            packets = new Dictionary<byte, Type>()
            {
                { 1, typeof(JoinGamePacket) },
                { 2, typeof(PlayerStatePacket) },
                { 3, typeof(HitPacket) },
            };

            Console.WriteLine("Packets manager initialized.");
        }

        public static void ReadBytes(Client client, byte[] buffer, int length)
        {
            MemoryStream stream = new MemoryStream(buffer);
            BinaryReader reader = new BinaryReader(stream);

            while (reader.BaseStream.Position != length)
            {
                byte packetId = reader.ReadByte();
                if (packets.ContainsKey(packetId))
                {
                    try
                    {
                        PacketBase packet = (PacketBase)Activator.CreateInstance(packets[packetId]);
                        packet.SetReader(reader, stream);
                        packet.Read(client);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("No packet with id: " + packetId);
                }
            }
        }
    }
}
