using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Networking.Types;

namespace creXa.GameBase.UNet
{
    [AddComponentMenu("creXa/Network/Lobby")]
    public abstract class ZLobbyCtrl : NetworkLobbyManager
    {
        public static ZLobbyCtrl Singleton;

        public bool DebugMessage = false;

        [SerializeField]
        bool _AllowMatchMaking = false;
        public bool AllowMatchMaking
        {
            set { _AllowMatchMaking = value; if(value) StartMatchMaker(); else StopMatchMaker(); }
            get { return _AllowMatchMaking;  }
        }

        #region RunTime Info

        MatchInfo _currentMatchInfo;
        MatchInfoSnapshot _currentMatchInfoSnapshot;
        string roomname = "";

        #endregion

        #region RunTime PeerType

        [SerializeField]
        bool _isMatchmaking = false;
        public bool isMatchmaking { get { return _isMatchmaking; } }

        [SerializeField]
        bool _isHost = false;
        public bool isHost { get { return _isHost; } }

        [SerializeField]
        bool _isServer = false;
        public bool isServer { get { return _isServer; } }

        [SerializeField]
        bool _isClient = false;
        public bool isClient { get { return _isClient; } }

        [SerializeField]
        bool _isDisconnectServer = false;
        public bool isDisconnectServer { get { return _isDisconnectServer; } }

        #endregion

        void Start()
        {
            Singleton = this;
            StartRun();
        }
        public virtual void StartRun() { }

        #region Client Connection

        public override void OnClientConnect(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log(conn!=null? "ID: " + conn.connectionId + " IP: " + conn.address : "Connection Failed.");
            base.OnClientConnect(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnClientDisconnect(conn);
        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnClientSceneChanged(conn);
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address + " ErrorCode: " + errorCode);
            base.OnClientError(conn, errorCode);
        }

        public override void OnClientNotReady(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnClientNotReady(conn);
        }

        #endregion

        #region Matching

        public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (DebugMessage) ZMsg.Log("Success: " + success + " NetworkID: " + matchInfo.networkId + " IP: " + matchInfo.address + " Port: " + matchInfo.port);
            if (success)
            {
                _isMatchmaking = true;
                _currentMatchInfo = matchInfo;
            } 
            base.OnMatchCreate(success, extendedInfo, matchInfo);
        }
        
        public override void OnSetMatchAttributes(bool success, string extendedInfo)
        {
            if (DebugMessage) ZMsg.Log("Success: " + success);
            base.OnSetMatchAttributes(success, extendedInfo);
        }
        
        public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
        {
            if (DebugMessage) ZMsg.Log("Success: " + success);
            base.OnMatchList(success, extendedInfo, matchList);
        }

        public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (DebugMessage) ZMsg.Log("Success: " + success + " NetworkID: " + matchInfo.networkId + " IP: " + matchInfo.address + " Port: " + matchInfo.port);
            if (success)
            {
                _currentMatchInfo = matchInfo;
            }
            base.OnMatchJoined(success, extendedInfo, matchInfo);
        }
        
        public override void OnDestroyMatch(bool success, string extendedInfo)
        {
            if (DebugMessage) ZMsg.Log("Success: " + success);
            if (success && _isDisconnectServer)
            {
                _isDisconnectServer = false;
                _currentMatchInfo = null;
                _isMatchmaking = false;
                StopMatchMaker();
                StopHost();
            }
            base.OnDestroyMatch(success, extendedInfo);
        }
        
        public override void OnDropConnection(bool success, string extendedInfo)
        {
            if (DebugMessage) ZMsg.Log("Success: " + success);
            base.OnDropConnection(success, extendedInfo);
        }
        
        #endregion

        #region Server Connection

        public override void OnServerConnect(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnServerConnect(conn);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnServerDisconnect(conn);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address + " PlayerCtrlID: " + playerControllerId);
            base.OnServerAddPlayer(conn, playerControllerId);
        }
        
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address + " PlayerCtrlID: " + playerControllerId);
            base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
        }
        
        public override void OnServerError(NetworkConnection conn, int errorCode)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnServerError(conn, errorCode);
        }
        
        public override void OnServerReady(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnServerReady(conn);
        }
        
        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address + " PlayerCtrlID: " + player.playerControllerId);
            base.OnServerRemovePlayer(conn, player);
        }
        
        public override void OnServerSceneChanged(string sceneName)
        {
            if (DebugMessage) ZMsg.Log("SceneName: " + sceneName);
            base.OnServerSceneChanged(sceneName);
        }
        
        #endregion

        #region Utility

        public override void OnStartHost()
        {
            if (DebugMessage) ZMsg.Log();
            _isHost = true;
            base.OnStartHost();
        }

        public override void OnStopHost()
        {
            if (DebugMessage) ZMsg.Log();
            _isHost = false;
            base.OnStopHost();
        }

        public override void OnStartClient(NetworkClient client)
        {
            if (DebugMessage) ZMsg.Log(client!=null && client.connection != null? "HostID: " + client.connection.hostId + " IP: " + client.connection.address + " ConnectionID: " + client.connection.connectionId : "Connection Failed.");
            _isClient = true;
            base.OnStartClient(client);
        }

        public override void OnStopClient()
        {
            if (DebugMessage) ZMsg.Log();
            _isClient = false;
            base.OnStopClient();
        }

        public override void OnStartServer()
        {
            if (DebugMessage) ZMsg.Log();
            _isServer = true;
            base.OnStartServer();
        }

        public override void OnStopServer()
        {
            if (DebugMessage) ZMsg.Log();
            _isServer = false;
            base.OnStopServer();
        }

        #endregion

        #region Lobby

        public override void OnLobbyClientAddPlayerFailed()
        {
            if (DebugMessage) ZMsg.Log();
            base.OnLobbyClientAddPlayerFailed();
        }

        public override void OnLobbyClientConnect(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log(conn!=null? "ID: " + conn.connectionId + " IP: " + conn.address : "Connection Failed.");
            base.OnLobbyClientConnect(conn);
        }

        public override void OnLobbyClientDisconnect(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnLobbyClientDisconnect(conn);
        }

        public override void OnLobbyClientEnter()
        {
            if (DebugMessage) ZMsg.Log();
            base.OnLobbyClientEnter();
        }

        public override void OnLobbyClientExit()
        {
            if (DebugMessage) ZMsg.Log();
            base.OnLobbyClientExit();
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnLobbyClientSceneChanged(conn);
        }

        public override void OnLobbyServerConnect(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnLobbyServerConnect(conn);
        }

        public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address + " PlayerCtrlID: " + playerControllerId);
            return base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
        }

        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address + " PlayerCtrlID: " + playerControllerId);
            return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
        }
        
        public override void OnLobbyServerDisconnect(NetworkConnection conn)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address);
            base.OnLobbyServerDisconnect(conn);
        }
        
        public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
        {
            if (DebugMessage) ZMsg.Log("ID: " + conn.connectionId + " IP: " + conn.address + " PlayerCtrlID: " + playerControllerId);
            base.OnLobbyServerPlayerRemoved(conn, playerControllerId);
        }
        
        public override void OnLobbyServerPlayersReady()
        {
            /* Default: Automatic Start When All Players Are Ready */
            if (DebugMessage) ZMsg.Log();
            base.OnLobbyServerPlayersReady();
        }
        
        public override void OnLobbyServerSceneChanged(string sceneName)
        {
            if (DebugMessage) ZMsg.Log("SceneName: " + sceneName);
            base.OnLobbyServerSceneChanged(sceneName);
        }
        
        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            if (DebugMessage) ZMsg.Log("LobbyPlayer: " + lobbyPlayer + " GamePlayer: " + gamePlayer);
            return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
        }
        
        public override void OnLobbyStartClient(NetworkClient lobbyClient)
        {
            if (DebugMessage) ZMsg.Log(lobbyClient != null && lobbyClient.connection != null? "IP: " + lobbyClient.connection.address + " ConnectionID: " + lobbyClient.connection.connectionId : "Connection Failed.");
            base.OnLobbyStartClient(lobbyClient);
        }
        
        public override void OnLobbyStartHost()
        {
            if (DebugMessage) ZMsg.Log();
            base.OnLobbyStartHost();
        }
        
        public override void OnLobbyStartServer()
        {
            if (DebugMessage) ZMsg.Log();
            base.OnLobbyStartServer();
        }
        
        public override void OnLobbyStopClient()
        {
            if (DebugMessage) ZMsg.Log();
            base.OnLobbyStopClient();
        }
        
        public override void OnLobbyStopHost()
        {
            if (DebugMessage) ZMsg.Log();
            base.OnLobbyStopHost();
        }
        
        #endregion

        public int NowPlayers
        {
            get {
                int rtn = 0;
                for (int i = 0; i < lobbySlots.Length; i++)
                    if (lobbySlots[i])
                        rtn++;
                return rtn;
            }
        }

        public string GameRoomName
        {
            get {
                if (_isHost && roomname != "") return roomname;
                if (_isClient && _currentMatchInfoSnapshot != null)
                    return _currentMatchInfoSnapshot.name;
                return "";
            }
        }

        public bool isAllLobbyPlayersReady
        {
            get
            {
                for (int i = 0; i < lobbySlots.Length; ++i)
                {
                    if (lobbySlots[i] != null)
                        if (!lobbySlots[i].readyToBegin)
                            return false;
                }
                return true;
            }
        }

        public virtual void StartGame()
        {
            ServerChangeScene(playScene);
            
        }

        public string LocalIP
        {
            get
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        return ip.ToString();
                return "";
            }
        }
        
        public virtual void PlayAndHost()
        {
            StartHost();
        }

        public virtual void ConnectToServer(string ip)
        {
            networkAddress = ip;
            StartClient();
        }

        public virtual void StartDedicatedServer()
        {
            StartServer();
        }

        public virtual void CreateGameRoom(string roomName, int maxPlayers, string password = "")
        {
            if (!AllowMatchMaking) return;
            StartMatchMaker();
            roomname = roomName;
            matchMaker.CreateMatch(roomName, (uint)maxPlayers,
                true, password, "", "", 0, 0, OnMatchCreate);
            _isMatchmaking = true;
        }

        public virtual void JoinGameRoom(MatchInfoSnapshot info, string password = "")
        {
            _isMatchmaking = true;
            _currentMatchInfoSnapshot = info;
            matchMaker.JoinMatch(info.networkId, password, "", "", 0, 0, OnMatchJoined);
        }

        public virtual void CloseRoom()
        {
            if(_currentMatchInfo != null)
            {
                matchMaker.DestroyMatch(_currentMatchInfo.networkId, 0, OnDestroyMatch);
                _isDisconnectServer = true;
            }
            _isMatchmaking = false;
        }

        public virtual void RefreshRoomList(string filter = "")
        {
            if (!AllowMatchMaking) return;
            StartMatchMaker();
            matchMaker.ListMatches(0, 20, filter, false, 0, 0, OnMatchList);
        }

        public virtual void Disconnect()
        {
            if (!isNetworkActive) return;
            if (_isMatchmaking && _isHost) { CloseRoom(); return; }
            _isMatchmaking = false;
            if (_isHost) { StopHost(); return; }
            if (_isClient) { StopClient(); _currentMatchInfoSnapshot = null; return; }
            if (_isServer) { StopServer(); return; }
        }

        void OnApplicationQuit()
        {
            Disconnect();
        }

    }
}
