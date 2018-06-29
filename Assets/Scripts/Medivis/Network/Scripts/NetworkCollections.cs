namespace Medivis.Network
{
    using System;
    public enum NetworkState
    {
        Init,
        Offline,
        Connecting,
        Connected,
        Disrupted
    }

    public enum GameState
    {
        Offline,
        Connecting,
        Lobby,
        Countdown,
        SendLocationSync,
        WaitForLocationSync,
        WaitingForRolls,
        Scoring,
        GameOver
    }
    public class DiscoveredServer : BroadcastData
    {
        public string rawData;
        public string ipAddress;
        public float timestamp;

        public DiscoveredServer(BroadcastData aData)
        {
            version = aData.version;
            peerId = aData.peerId;
            isOpen = aData.isOpen;
            numPlayers = aData.numPlayers;
            serverScore = aData.serverScore;
            privateTeamKey = aData.privateTeamKey;
            Name = aData.Name;
        }
    }

    [System.Serializable]
    public class BroadcastData
    {
        public static int VERSION = 1;

        public int version = VERSION;
        public string Name;
        public string peerId;
        public bool isOpen;
        public int numPlayers;
        public int serverScore;
        public string privateTeamKey = "";

        public override string ToString()
        {
            // IMPORTANT: I’m adding a token at the end of this string (in this case two colons “::“) so I can tell when the
            // data ends because Unity doesn’t seem to clear the broadcastData buffer when changing it. This led to a weird
            // bug where the final element would get overwritten. eg. Previously, before adding the token, if the final
            // value was 999 but then changed to 1, it would be received as 199 because Unity would write the new value over
            // the previous value without clearing it. This way, the garbage data at the end will just get ignored.
            //
            // TODO: Find a better way of dealing with this!
            return String.Format("{0}:{1}:{2}:{3}:{4}:{5}:{6}::", version, peerId, isOpen ? 1 : 0, numPlayers, serverScore, privateTeamKey,Name);
        }

        public void FromString(string aString)
        {
            var items = aString.Split(':');
            version = Convert.ToInt32(items[0]);
            peerId = items[1];
            isOpen = (Convert.ToInt32(items[2]) != 0);
            numPlayers = Convert.ToInt32(items[3]);

            if (items.Length > 4)
            {
                serverScore = Convert.ToInt32(items[4]);
            }
            else
            {
                serverScore = 1;
            }

            if (items.Length > 5)
            {
                privateTeamKey = items[5];
            }
            if (items.Length > 6)
            {
                Name = items[6];
            }
        }
    }
}
