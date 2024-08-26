using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    //������ ����
    private readonly string version = "1.0";
   //������ �г���
    private string userId = "Zack";
    //�������� �Է��� ��ǲ�ʵ�
    //public TMP_InputField userIF;
    //�� �̸��� �Է��� ��ǲ�ʵ�
    //public TMP_InputField roomNameIF;
    private void Awake()
    {
        //������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        PhotonNetwork.AutomaticallySyncScene = true;
        //���� ���� ����
        PhotonNetwork.GameVersion = version;
        //���� ������ �г��� ����
        //PhotonNetwork.NickName = userId;

        //���� �������� �������� �ʴ� ���� Ƚ��
        Debug.Log(PhotonNetwork.SendRate);

        //���� ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        //�������� �����ϱ� ���� �ڵ� ������ �ּ�ó��(���߿�)
        PhotonNetwork.JoinRandomRoom();
    }
   //������ �� ������ �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Filed {returnCode} : {message}");

        //���� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20; //�뿡 ������ �� �ִ� �ִ� ������ ��
        ro.IsOpen = true;   //���� ���� ����
        ro.IsVisible = true;//�κ񿡼� �� ��Ͽ� �����ų�� ����

        PhotonNetwork.CreateRoom("My Room", ro);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Create Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName} , {player.Value.ActorNumber}");
        }
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
