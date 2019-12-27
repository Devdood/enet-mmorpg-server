using ENet;
using ServerUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeroesServer
{
    class Server
    {
        public Server()
        {
            ENet.Library.Initialize();
            ClientsManager clients = new ClientsManager();

            using (Host server = new Host())
            {
                Console.WriteLine("Game Server started...");
                Address address = new Address();
                address.SetIP("127.0.0.1");
                address.Port = 2601;
                server.Create(address, 100);

                Event netEvent;

                while (!Console.KeyAvailable)
                {
                    bool polled = false;

                    while (!polled)
                    {
                        if (server.CheckEvents(out netEvent) <= 0)
                        {
                            if (server.Service(15, out netEvent) <= 0)
                                break;

                            polled = true;
                        }

                        switch (netEvent.Type)
                        {
                            case EventType.None:
                                break;

                            case EventType.Connect:
                                Console.WriteLine("Client connected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);

                                CharacterInfo info = CharactersManager.Instance.AddCharacter(new CharacterInfo()
                                {
                                    Character = new Character()
                                    {
                                        Position = new System.Numerics.Vector3(127, 14, 100)
                                    },
                                    ZoneId = 0
                                });

                                Client c = new Client()
                                {
                                    Peer = netEvent.Peer,
                                    CharacterInfo = info
                                };

                                c.CharacterInfo.Client = c;
                                ClientsManager.Instance.clients.Add(netEvent.Peer.ID, c);
                                break;

                            case EventType.Disconnect:
                                Console.WriteLine("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);

                                Client clientToRemove = ClientsManager.Instance.clients[netEvent.Peer.ID];
                                ClientsManager.Instance.clients.Remove(netEvent.Peer.ID);
                                CharactersManager.Instance.RemoveCharacter(clientToRemove.CharacterInfo);
                                break;

                            case EventType.Timeout:
                                Console.WriteLine("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                                break;

                            case EventType.Receive:
                                //Console.WriteLine("Packet received from - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP + ", Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                                byte[] buffer = new byte[1024];
                                netEvent.Packet.CopyTo(buffer);

                                PacketsManager.ReadBytes(ClientsManager.Instance.clients[netEvent.Peer.ID], buffer, netEvent.Packet.Length);

                                netEvent.Packet.Dispose();
                                break;
                        }
                    }
                }

                server.Flush();
            }
        }
    }
}
