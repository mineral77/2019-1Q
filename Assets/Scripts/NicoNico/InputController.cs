using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Scripts.NicoNico
{
    public class InputController : MonoBehaviour
    {
        public InputField inputField;

        /// <summary>入力完了時にコール</summary>
        public void onEndEdit()
        {
            GameObject obj = PhotonNetwork.Instantiate("Message", Vector3.zero, Quaternion.identity, 0);

            //初期位置を適当に設定
            float x = 380f;
            float y = Random.Range(-150f, 150f);
            Vector3 pos = new Vector3(x, y, 0);
            obj.GetComponent<RectTransform>().localPosition = pos;

            //入力されているテキストをセット
            obj.GetComponent<Message>().SetText(this.inputField.textComponent.text);
            //入力内容を空にする
            this.inputField.text = "";

            Debug.Log("入力完了");
        }
    }
}
