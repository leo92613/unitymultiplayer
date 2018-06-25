using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Medivis.Network
{
    public class BaseConnectionHandler : MonoBehaviour
    {
        [HideInInspector]
        public NetworkCenter center;

        public void Awake()
        {
            center = FindObjectOfType(typeof(NetworkCenter)) as NetworkCenter;
        }

        public virtual void OnStartConnecting()
        {
            // Override
        }

        public virtual void OnStopConnecting()
        {
            // Override
        }

        public virtual void OnServerCreated()
        {
            // Override
        }

        public virtual void OnReceivedBroadcast(string aFromAddress, string aData)
        {
            // Override
        }

        public virtual void OnDiscoveredServer(DiscoveredServer aServer)
        {
            // Override
        }

        public virtual void OnJoinedLobby()
        {
            // Override
        }

        public virtual void OnLeftLobby()
        {
            // Override
        }

        public virtual void OnCountdownStarted()
        {
            // Override
        }

        public virtual void OnCountdownCancelled()
        {
            // Override
        }

        public virtual void OnStartGame(List<BaseNetworkPlayer> aStartingPlayers)
        {
            // Override
        }

        public virtual void OnAbortGame()
        {
            // Override
        }
    }
}