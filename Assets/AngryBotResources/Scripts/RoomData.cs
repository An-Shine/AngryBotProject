using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class RoomData : MonoBehaviour
{
    RoomInfo _roomInfo;
    TMP_Text roomInfoText;
    PhotonManager photonManager;

    public RoomInfo RoomInfo
    {
        get{ return _roomInfo;}
        set
        {
            _roomInfo = value; 
            roomInfoText.text = $"{_roomInfo.Name} ({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            //버튼클릭 이벤트 함수 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(()=> OnEnterRoom(_roomInfo.Name));

        }
    }

    void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }

    void OnEnterRoom(string roomName)
    {
        //유저명 설정
        photonManager.SetUserId();

        //룸의 속성 정의
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;     //룸에 입장가능한 최대인원수
        ro.IsOpen = true;       //룸의 오픈 여부
        ro.IsVisible = true;    //로비에서 룸목록에 노출시킬지 여부
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }
}
