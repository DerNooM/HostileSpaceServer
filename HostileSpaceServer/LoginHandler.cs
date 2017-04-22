using System;
using HostileSpaceNetLib;
using HostileSpaceNetLib.Packets;
using SFML.System;

namespace HostileSpaceServer
{
    class LoginHandler : ServerComponent
    {
        public LoginHandler(HostileSpaceServer Server)
            : base(Server)
        {

        }

        public override void Update(Time Elapsed)
        {
        }

        public override void HandlePacket(Client Client, IPacket Packet)
        {
            LoginRequest request = (LoginRequest)Client.Packet;
            LoginResponse response = new LoginResponse();

            if (Server.PlayerData.AccountData.Rows.Contains(request.Username))
            {
                PlayerData.AccountDataRow tmp = Server.PlayerData.AccountData.FindByName(request.Username);

                if (ComparePasswords(tmp.Password, request.Password))
                {
                    lock(Server.Clients)
                    {
                        foreach(GameClient cli in Server.Clients)
                        {
                            if (cli.AccountData == tmp)
                            {
                                response.Response = LoginResponse.Responses.AllreadyLoggedIn;
                                Client.BeginSend(response);
                                Console.WriteLine("client allready logged in");
                                return;
                            }
                        }
                    }
                    response.Response = LoginResponse.Responses.Accepted;

                    GameClient client = new GameClient(Server, Client);
                    client.AccountData = tmp;

                    lock (Server.Clients)
                        Server.Clients.Add(client);
                }
                else
                {
                    response.Response = LoginResponse.Responses.InvalidPassword;
                }
            }
            else
            {
                PlayerData.AccountDataRow tmp = Server.PlayerData.AccountData.NewAccountDataRow();
                tmp.Name = request.Username;
                tmp.Password = request.Password;
                tmp.IsGM = false;
                tmp.ID = Guid.NewGuid();

                Server.PlayerData.AccountData.AddAccountDataRow(tmp);
                Server.SaveDB();

                response.Response = LoginResponse.Responses.AccountCreated;
                Console.WriteLine("new account created: " + tmp.Name);
            }


            Client.BeginSend(response);
        }

        Boolean ComparePasswords(Byte[] A, Byte[] B)
        {
            if (A.Length != B.Length)
                return false;

            for (int i = 0; i < A.Length; i++)
            {
                if (A[i] != B[i])
                    return false;
            }
            return true;
        }


    }
}
