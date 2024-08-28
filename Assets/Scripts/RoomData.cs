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
            //�� ���� ǥ��
            roomInfoText.text = $"{_roomInfo.Name}({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            //��ư Ŭ�� �̺�Ʈ�� �Լ� ����
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
        //������ ����
        photonManager.SetUserId();
        //�� ����
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
