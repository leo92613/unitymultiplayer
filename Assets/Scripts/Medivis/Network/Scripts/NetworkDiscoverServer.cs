using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Medivis.Network
{
    using UnityEngine.Networking;
    public class NetworkDiscoverServer : NetworkDiscovery
    {

        public MedivisNetworkManager networkManager;
        public BroadcastData broadcastDataObject;
        public bool isOpen { get { return broadcastDataObject.isOpen; } set { broadcastDataObject.isOpen = value; } }
        public int numPlayers { get { return broadcastDataObject.numPlayers; } set { broadcastDataObject.numPlayers = value; } }
        public int serverScore { get { return broadcastDataObject.serverScore; } set { broadcastDataObject.serverScore = value; } }
        public string privateTeamKey { get { return broadcastDataObject.privateTeamKey; } set { broadcastDataObject.privateTeamKey = value; } }
        void Start()
        {
            showGUI = false;
            useNetworkManager = false;
        }
        public void Setup(MedivisNetworkManager aNetworkManager)
        {
            networkManager = aNetworkManager;
            broadcastKey = Mathf.Abs(aNetworkManager.broadcastIdentifier.Hash()); // Make sure broadcastKey matches client
            Debug.Log("Set-up hash code in Server: " + broadcastKey.ToString());
            isOpen = false;
            numPlayers = 0;
            broadcastDataObject = new BroadcastData();
            broadcastDataObject.Name = networkManager.RoomName;
            broadcastDataObject.peerId = networkManager.peerId;
            UpdateBroadcastData();
        }
        public void UpdateBroadcastData()
        {
            broadcastData = broadcastDataObject.ToString();
        }
        public void Reset()
        {
            isOpen = false;
            numPlayers = 0;
            UpdateBroadcastData();
        }
        public void RestartBroadcast()
        {
            if (running)
            {
                StopBroadcast();
            }
            // Delay briefly to let things settle down
            CancelInvoke("RestartBroadcastInternal");
            Invoke("RestartBroadcastInternal", 0.5f);
        }
        private void RestartBroadcastInternal()
        {
            UpdateBroadcastData();
            if (networkManager.verboseLogging)
            {
                Debug.Log("#CaptainsMess# Restarting server with data: " + broadcastData);
            }
            // You can't update broadcastData while the server is running so I have to reinitialize and restart it
            // I think Unity is fixing this
            if (!Initialize())
            {
                Debug.LogError("#CaptainsMess# Network port is unavailable!");
            }
            if (!StartAsServer())
            {
                Debug.LogError("#CaptainsMess# Unable to broadcast!");
                // Clean up some data that Unity seems not to
                if (hostId != -1)
                {
                    if (isServer)
                    {
                        NetworkTransport.StopBroadcastDiscovery();
                    }
                    NetworkTransport.RemoveHost(hostId);
                    hostId = -1;
                }
            }
        }
    }
}
