using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer
{
    public class ClientsManager
    {
        public static ClientsManager Instance { get; private set; }

        public Dictionary<uint, Client> clients = new Dictionary<uint, Client>();

        public ClientsManager()
        {
            Instance = this;
        }
    }
}
