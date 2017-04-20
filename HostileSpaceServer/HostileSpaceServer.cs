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

        Dictionary<Client, Game> games = new Dictionary<Client, Game>();

        public HostileSpaceServer()
        {
            server.PacketReceieved += Server_PacketReceieved;

            server.Start();
        }
     
        private void Server_PacketReceieved(object sender, EventArgs e)
        {
            Client client = (Client)sender;

            if(client.Packet.ID == PacketID.LoginRequest)
            {
                LoginRequest request = new LoginRequest(client.Packet);
                LoginResponse response = new LoginResponse(LoginResponse.Responses.Unknown);

                if (playerData.AccountData.Rows.Contains(request.Username))
                {
                    PlayerData.AccountDataRow tmp = playerData.AccountData.FindByAccountName(request.Username);

                    if (ComparePasswords(tmp.Password, request.Password))
                    {
                        response = new LoginResponse(LoginResponse.Responses.Accepted);
                        client.LoggedIn = true;

                        lock (syncRoot)
                        {
                            games.Add(client, new Game(this, client));
                        }

                        client.Disconnected += Client_Disconnected;
                    }
                    else
                    {
                        response = new LoginResponse(LoginResponse.Responses.InvalidPassword);
                    }
                }
                else
                {
                    PlayerData.AccountDataRow tmp = playerData.AccountData.NewAccountDataRow();
                    tmp.AccountName = request.Username;
                    tmp.Password = request.Password;
                    tmp.IsGM = false;

                    playerData.AccountData.AddAccountDataRow(tmp);

                    response = new LoginResponse(LoginResponse.Responses.AccountCreated);
                }

                client.BeginSend(response.Packet);
            }
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            lock(syncRoot)
            {
                games.Remove((Client)sender);
                Console.WriteLine("game removec. count: " + games.Count);
            }
        }

        public void Update(Time Elapsed)
        {
            lock (syncRoot)
            {
                foreach (Game game in games.Values)
                {
                    game.Update(Elapsed);
                }
            }
        }


        bool ComparePasswords(Byte[] a1, Byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }


        public Dictionary<Client, Game> Games
        {
            get { return games; }
        }

        public Server Server
        {
            get { return server; }
        }

        public PlayerData PlayerData
        {
            get { return playerData; }
        }


        static void Main(string[] args)
        {
            HostileSpaceServer server = new HostileSpaceServer();


            Console.WriteLine("write exit to exit server");

            Clock clock = new Clock();
            Time elapsed = Time.Zero;


            while (true)
            {
                server.Update(elapsed);
                Console.Title = "elapsed: " + elapsed.AsMilliseconds();
                elapsed = clock.Restart();

                while(clock.ElapsedTime.AsMilliseconds() < 20)
                {
                    System.Threading.Thread.Sleep(1);
                }
            }
        }


    }
}
