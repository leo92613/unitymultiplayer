using System;
using System.Collections.Generic;
using UnityEngine;

namespace Medivis.Network
{
    public class NetworkCenter : MonoBehaviour
    {
        public string broadcastIdentifier = "CM";
        public int minPlayers = 2;
        public int maxPlayers = 4;
        public NetworkPlayer playerPrefab;
        public GameObject networkManagerPrefab;
        public float countdownDuration = 3; // Wait for this many seconds after people are ready before starting the game
        public ConnectionHandler handler;
        public bool verboseLogging = false;
        //public bool useDebugGUI = true;
        public bool forceServer = false;
        private MedivisNetworkManager networkManager;
        public void Awake()
        {
            ValidateConfig();
            // Create network manager
            networkManager = (Instantiate(networkManagerPrefab) as GameObject).GetComponent<MedivisNetworkManager>();
            if (networkManager != null)
            {
                //networkManager.logLevel = 0;
                networkManager.name = "CaptainsMessNetworkManager";
                networkManager.RoomName = "WCU Test";
                networkManager.runInBackground = false; // runInBackground is not recommended on iOS
                networkManager.broadcastIdentifier = broadcastIdentifier;
                networkManager.minPlayers = minPlayers;
                networkManager.SetMaxPlayers(maxPlayers); //Setting maxPlayers and maxConnections
                networkManager.allReadyCountdownDuration = countdownDuration;
                networkManager.forceServer = forceServer;
                // I'm just using a single scene for everything
                networkManager.offlineScene = "";
                networkManager.onlineScene = "";
                networkManager.playerPrefab = playerPrefab.gameObject;
                networkManager.handler = handler;
                networkManager.verboseLogging = verboseLogging;
            }
            else
            {
                Debug.LogError("#CaptainsMess# Error creating network manager");
            }
        }
        public void ValidateConfig()
        {
            if (broadcastIdentifier == "Spaceteam" /*&& !Application.bundleIdentifier.Contains("com.sleepingbeastgames")*/)
            {
                Debug.LogError("#CaptainsMess# You should pick a unique Broadcast Identifier for your game", this);
            }
            if (playerPrefab == null)
            {
                Debug.LogError("#CaptainsMess# Please pick a Player prefab", this);
            }
            if (handler == null)
            {
                Debug.LogError("#CaptainsMess# Please set a Listener object", this);
            }
        }
        public void Update()
        {
            if (networkManager == null)
            {
                networkManager = FindObjectOfType(typeof(MedivisNetworkManager)) as MedivisNetworkManager;
                networkManager.handler = handler;
                if (networkManager.verboseLogging)
                {
                    Debug.Log("#CaptainsMess# !! RECONNECTING !!");
                }
            }
        }
        public List<BaseNetworkPlayer> Players()
        {
            return networkManager.LobbyPlayers() as List<BaseNetworkPlayer>;
        }
        public NetworkPlayer LocalPlayer()
        {
            return networkManager.localPlayer as NetworkPlayer;
        }
        public void AutoConnect()
        {
            networkManager.InitNetworkTransport();
            networkManager.minPlayers = minPlayers;
            networkManager.AutoConnect();
        }
        [ContextMenu("Start Host")]
        public void StartHosting()
        {
            networkManager.InitNetworkTransport();
            networkManager.minPlayers = minPlayers;
            networkManager.StartHosting();
        }
        public void StartJoining()
        {
            networkManager.InitNetworkTransport();
            networkManager.minPlayers = minPlayers;
            networkManager.StartJoining();
        }
        public void Cancel()
        {
            networkManager.Cancel();
            networkManager.ShutdownNetworkTransport();
        }
        public bool AreAllPlayersReady()
        {
            return networkManager.AreAllPlayersReady();
        }
        public float CountdownTimer()
        {
            return networkManager.allReadyCountdown;
        }
        public void StartLocalGameForDebugging()
        {
            networkManager.InitNetworkTransport();
            networkManager.minPlayers = 1;
            networkManager.StartLocalGameForDebugging();
        }
        public bool IsConnected()
        {
            return networkManager.IsConnected();
        }
        public bool IsHost()
        {
            return networkManager.IsHost();
        }
        public void FinishGame()
        {
            networkManager.FinishGame();
        }
        public void SetForceServer(bool fs)
        {
            forceServer = fs;
            networkManager.forceServer = fs;
        }
        public void SetPrivateTeamKey(string key)
        {
            networkManager.SetPrivateTeamKey(key);
        }
        public int HighestConnectedVersion()
        {
            return networkManager.HighestConnectedVersion();
        }
    }
}