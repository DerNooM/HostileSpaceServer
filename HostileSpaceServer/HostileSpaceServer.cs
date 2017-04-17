using System;
using HostileSpaceNetLib;
using HostileSpaceNetLib.Packets;


namespace HostileSpaceServer
{
    class HostileSpaceServer
    {
        Server server = new Server();

        GameData gameData = new GameData();

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

                if (gameData.Users.Rows.Contains(request.Username))
                {
                    GameData.UsersRow tmp = gameData.Users.FindByName(request.Username);

                    if (ComparePasswords(tmp.Password, request.Password))
                    {
                        response = new LoginResponse(LoginResponse.Responses.Accepted);
                        client.LoggedIn = true;
                    }
                    else
                    {
                        response = new LoginResponse(LoginResponse.Responses.InvalidPassword);
                    }
                }
                else
                {
                    GameData.UsersRow tmp = gameData.Users.NewUsersRow();
                    tmp.Name = request.Username;
                    tmp.Password = request.Password;
                    tmp.GM = false;

                    gameData.Users.AddUsersRow(tmp);

                    response = new LoginResponse(LoginResponse.Responses.AccountCreated);
                }

                client.BeginSend(response.Packet);
            }

            if (!client.LoggedIn)
                return;

            switch (client.Packet.ID)
            {
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


        static void Main(string[] args)
        {
            HostileSpaceServer server = new HostileSpaceServer();


            Console.WriteLine("write exit to exit server");
            while(Console.ReadLine() != "exit")
            {
                ;
            }
        }


    }
}
