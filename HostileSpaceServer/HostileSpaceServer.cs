using System;
using System.Collections.Generic;
using SFML.System;
using HostileSpaceNetLib;
using HostileSpaceNetLib.Packets;


namespace HostileSpaceServer
{
    class HostileSpaceServer
    {
        Server server = new Server();
        readonly object syncRoot = new object();

        PlayerData playerData = new PlayerData();


        List<GameClient> clients = new List<GameClient>();


        LoginHandler loginHandler;

        public HostileSpaceServer()
        {
            LoadDB();
            loginHandler = new LoginHandler(this);

            server.PacketReceieved += Server_PacketReceieved;

            server.Start();
        }


        private void Server_PacketReceieved(object sender, EventArgs e)
        {
            Client client = (Client)sender;

            if (client.Packet.ID == PacketID.LoginRequest)
            {
                loginHandler.HandlePacket(client, client.Packet);
            }
        }




        public void Update(Time Elapsed)
        {
            List<GameClient> tmp = new List<GameClient>();


            lock (clients)
            {
                foreach (GameClient client in clients)
                {
                    if (client.Client.Connected)
                    {
                        client.Update(Elapsed);
                    }
                    else
                    {
                        tmp.Add(client);
                    }
                }
            }

            foreach(GameClient client in tmp)
            {
                lock(clients)
                {
                    clients.Remove(client);
                }
            }
            
        }


        public void SaveDB()
        {
            playerData.WriteXml("PlayerData.xml");
            Console.WriteLine("database saved");
        }

        public void LoadDB()
        {
            playerData.ReadXml("PlayerData.xml");
            Console.WriteLine("database loaded");
        }
        



        public Server Server
        {
            get { return server; }
        }

        public PlayerData PlayerData
        {
            get { return playerData; }
        }

        public List<GameClient> Clients
        {
            get { return clients; }
        }
        

        static void Main(string[] args)
        {
            HostileSpaceServer server = new HostileSpaceServer();

            Clock clock = new Clock();
            Time elapsed = Time.Zero;

            while (true)
            {
                server.Update(elapsed);
                elapsed = clock.Restart();

                while(clock.ElapsedTime.AsMilliseconds() < 20)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
        }


    }
}
