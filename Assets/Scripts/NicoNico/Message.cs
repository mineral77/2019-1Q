using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Scripts.NicoNico
{
    public class Message : MonoBehaviour, IPunInstantiateMagicCallback
    {

        PhotonView pView;

        /// <summary>
        /// 一秒間に動く距離
        /// </summary>
        float moveSpeed = -60;

        // Update is called once per frame
        void Update()
        {
            //他プレイヤーのクライアントが生成したオブジェクトの場合は実行しない
            if (this.pView.IsMine == false)
            {
                return;
            }

            //指定した座標地点まで移動
            if (this.gameObject.transform.localPosition.x >= -500)
            {
                this.transform.Translate(this.moveSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                //他プレイヤーにオブジェクトを破棄したを同期
                this.pView.RPC("RpcDestroy", RpcTarget.All);
            }
        }

        /// <summary>
        /// PhotonNetwork.Instantiateでインスタンスを生成したときに呼ばれる
        /// </summary>
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            this.pView = PhotonView.Get(this);
            //Messageオブジェクトの親(Canvas)設定の同期
            this.pView.RPC("RpcSetParentCanvas", RpcTarget.All);
        }

        //テキストのセット(InputController.csから呼び出される)
        public void SetText(string setText)
        {
            this.pView.RPC("RpcSetText", RpcTarget.All, setText);
        }

        //破棄を同期
        [PunRPC]
        void RpcDestroy()
        {
            Destroy(this.gameObject);
        }

        //親設定を同期
        [PunRPC]
        void RpcSetParentCanvas()
        {
            //タグからオブジェクトを取得
            Transform t = GameObject.FindWithTag("Canvas").transform;
            this.transform.SetParent(t);
        }

        //メッセージ内容を同期
        [PunRPC]
        void RpcSetText(string setText)
        {
            this.GetComponent<Text>().text = setText;
        }
    }
}
