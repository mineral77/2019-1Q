using UnityEngine;
using Photon.Pun;

namespace Scripts.NicoNico
{
    public class Network : MonoBehaviourPunCallbacks
    {

        private string gameVersion = "1";

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            Connect();
        }

        private void Connect()
        {
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("マスターに接続");

            //ロビーに入る
            PhotonNetwork.JoinLobby();
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("ロビーに入りました");

            //ルームに入室する
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("ルームへ入室しました");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("ルームの入室に失敗しました。");

            // ルームがないと入室に失敗するため、ルームがない場合は自分で作る
            PhotonNetwork.CreateRoom("myRoomName");
        }

    }
}
