using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomData : MonoBehaviour
{
    private RoomInfo _roomInfo;
    private Text roomInfoText;
    private PhotonManager photonManager;
    
    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            //룸 정보 표시
            roomInfoText.text = $"{_roomInfo.Name}({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            //버튼 클릭 이벤트에 함수 연결
            GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => OnEnterRoom(_roomInfo.Name));
        }
    }
    private void Awake()
    {
        roomInfoText = GetComponentInChildren<Text>();
        photonManager = GameObject.Find("PhotonManager").GetComponent<PhotonManager>();
    }
    void OnEnterRoom(string roomName)
    {
        //유저명 설정
        photonManager.SetUserId();
        //룸 접속
        PhotonNetwork.JoinRoom(roomName);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
