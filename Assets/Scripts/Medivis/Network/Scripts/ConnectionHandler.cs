using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Medivis.Network
{
    using UnityEngine.Networking;
    public class ConnectionHandler : BaseConnectionHandler
    {

        [HideInInspector]
        public NetworkState networkState = NetworkState.Init;

        public GameObject gameSessionPrefab;
        public NetworkSession gameSession;

        public void Start()
        {
            networkState = NetworkState.Offline;

            ClientScene.RegisterPrefab(gameSessionPrefab);
        }

        public override void OnStartConnecting()
        {
            networkState = NetworkState.Connecting;
        }

        public override void OnStopConnecting()
        {
            networkState = NetworkState.Offline;
        }

        public override void OnServerCreated()
        {
            // Create game session
            // Spawn the game session when a new client is connected to the server
            NetworkSession oldSession = FindObjectOfType<NetworkSession>();
            if (oldSession == null)
            {
                GameObject serverSession = Instantiate(gameSessionPrefab);
                NetworkServer.Spawn(serverSession);
            }
            else
            {
                Debug.LogError("GameSession already exists!");
            }
        }

        public override void OnJoinedLobby()
        {
            networkState = NetworkState.Connected;

            gameSession = FindObjectOfType<NetworkSession>();
            if (gameSession)
            {
                gameSession.OnJoinedLobby();
            }
        }

        public override void OnLeftLobby()
        {
            networkState = NetworkState.Offline;

            gameSession.OnLeftLobby();
        }

        public override void OnCountdownStarted()
        {
            gameSession.OnCountdownStarted();
        }

        public override void OnCountdownCancelled()
        {
            gameSession.OnCountdownCancelled();
        }

        public override void OnStartGame(List<BaseNetworkPlayer> aStartingPlayers)
        {
            gameSession.OnStartGame(aStartingPlayers);
        }

        public override void OnAbortGame()
        {
            gameSession.OnAbortGame();
        }
    }
}