using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
namespace Medivis.Network
{
    public class ExampleMessage : MessageBase
    {
        public byte[] byteBuffer;
    }



    public class NetworkPlayer : BaseNetworkPlayer
    {
        //public Image image;
        //public Text nameField;
        //public Text readyField;
        //public Text rollResultField;
        //public Text totalPointsField;

        [SyncVar]
        public Color myColour;

        // Simple game states for a dice-rolling game

        [SyncVar]
        public int rollResult;

        [SyncVar]
        public int totalPoints;

        [SyncVar]
        public bool locationSynced;
        [SyncVar]
        public Vector3 SyncPos;
        public GameObject spherePrefab;

        private byte[] savedBytes;

        private bool locationSent;


        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();


            // Send custom player info
            // This is an example of sending additional information to the server that might be needed in the lobby (eg. colour, player image, personal settings, etc.)

            myColour = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
            CmdSetCustomPlayerInfo(myColour);
            locationSent = false;
        }




        [Command]
        public void CmdSetLocationSynced()
        {
            locationSynced = true;
        }


        [Command]
        public void CmdSetCustomPlayerInfo(Color aColour)
        {
            myColour = aColour;
        }

        [Command]
        public void CmdRollDie()
        {
            rollResult = UnityEngine.Random.Range(1, 7);
        }

        [Command]
        public void CmdMakeSphere(Vector3 position, Quaternion rotation)
        {

            var sphere = (GameObject)Instantiate(spherePrefab, position, rotation);
            NetworkServer.Spawn(sphere);
            RpcSetSphereColor(sphere, myColour.r, myColour.g, myColour.b);
        }


        [Command]
        public void CmdPlayAgain()
        {
           NetworkSession.instance.PlayAgain();
        }
        [Command]
        public void CmdSyncPos(Vector3 pos)
        {
            if (isLocalPlayer)
            {
                return;
            }
            SyncPos = pos;
            if (isServer)
            {
                transform.localPosition = pos;
            }

        }

        public void OnSyncPosChanged()
        {
            transform.localPosition = SyncPos;
        }

        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            // Brief delay to let SyncVars propagate
            Invoke("ShowPlayer", 0.5f);
        }


        public override void OnClientReady(bool readyState)
        {
            //if (readyState)
            //{
            //    readyField.text = "READY!";
            //    readyField.color = Color.green;
            //}
            //else
            //{
            //    readyField.text = "not ready";
            //    readyField.color = Color.red;
            //}
        }

        void ShowPlayer()
        {
            //transform.SetParent(GameObject.Find("Canvas/PlayerContainer").transform, false);

            //image.color = myColour;
            //nameField.text = deviceName;
            //readyField.gameObject.SetActive(true);

            //rollResultField.gameObject.SetActive(false);
            //totalPointsField.gameObject.SetActive(false);

            OnClientReady(IsReady());
        }

        public void Update()
        {
            string synced = locationSynced ? "SYNC" : "NO";
            //totalPointsField.text = "Points: " + totalPoints.ToString() + synced;
            //if (rollResult > 0)
            //{
            //    rollResultField.text = "Roll: " + rollResult.ToString();
            //}
            //else
            //{
            //    rollResultField.text = "";
            //}
            if (isLocalPlayer)
            {
                CmdSyncPos(transform.localPosition);
            }
        }


        [ClientRpc]
        public void RpcSetSphereColor(GameObject sphere, float r, float g, float b)
        {
            sphere.GetComponent<Renderer>().material.color = new Color(r, g, b);
        }


        [ClientRpc]
        public void RpcOnStartedGame()
        {
            //readyField.gameObject.SetActive(false);

            //rollResultField.gameObject.SetActive(true);
            //totalPointsField.gameObject.SetActive(true);
        }

    }
}