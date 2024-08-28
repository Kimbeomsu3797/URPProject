using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // ������ ����
    private readonly string version = "1.0";
    // ������ �г���
    private string userId = "Zack";
    //�������� �Է��� ��ǲ�ʵ�
    //public InputField userIF;
    public TMP_InputField userIF;
    //�� �̸��� �Է��� ��ǲ�ʵ�
    //public InputField roomNameIF;
    public TMP_InputField roomNameIF;

    //�� ��Ͽ� ���� �����͸� �����ϱ� ���� ��ųʸ� �ڷ���
    private Dictionary<string, GameObject> rooms = new Dictionary<string, GameObject>();
    //�� ����� ǥ���� ������
    private GameObject roomItemPrefab;
    //RoomItem �������� �߰��� ScrollContent
    public Transform scrollContent;
    private void Awake()
    {
        // ������ Ŭ���̾�Ʈ�� �� �ڵ� ����ȭ �ɼ�
        PhotonNetwork.AutomaticallySyncScene = true;
        // ���� ���� ����
        PhotonNetwork.GameVersion = version;
        // ���� ������ �г��� ����
        //PhotonNetwork.NickName = userId;

        // ���� �������� �������� �ʴ� ���� Ƚ��
        Debug.Log(PhotonNetwork.SendRate);


        // ���� ���� ����
        PhotonNetwork.ConnectUsingSettings();

        //RoomItem ������ �ε�
        roomItemPrefab = Resources.Load<GameObject>("RoomItem");

        //���� ���� ����
        if(PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        
    }

    // ���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master!");
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby();
    }

    // �κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork.InLobby = {PhotonNetwork.InLobby}");
        // �������� �����ϱ� ���� �ڵ� ������ �ּ�ó��
       // PhotonNetwork.JoinRandomRoom();
    }

    // ������ �� ������ �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"JoinRandom Failed {returnCode}:{message}");
        //���� �����ϴ� �Լ� ��god
        OnMakeRoomClick();
        // ���� �Ӽ� ����
        //RoomOptions ro = new RoomOptions();
        //ro.MaxPlayers = 20;     // �뿡 ������ �� �ִ� �ִ� ������ ��
        //ro.IsOpen = true;       // ���� ���� ����
        //ro.IsVisible = true;    // �κ񿡼� �� ��Ͽ� �����ų ����

        // �� ����
        //PhotonNetwork.CreateRoom("My Room", ro);
    }

    private void OnMakeRoomClick()
    {
        throw new System.NotImplementedException();
    }

    // �� ������ �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room");
        Debug.Log($"Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    // �뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"{player.Value.NickName}, {player.Value.ActorNumber}");
        }

        //Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //int idx = Random.Range(1, points.Length);

        //PhotonNetwork.Instantiate("Player", points[idx].position, points[idx].rotation, 0);

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //����� ���� �� �ε�
        userId = PlayerPrefs.GetString("USER_ID", $"USER_ {Random.Range(1,21):00}");
        userIF.text = userId;
        //���� ������ �г��� ���
        PhotonNetwork.NickName = userId;
    }
    public void SetUserId()
    {
        if (string.IsNullOrEmpty(userIF.text))
        {
            userId = $"USER_{Random.Range(1, 21):00}";
        }
        else
        {
            userId = userIF.text;
        }
        //������ ����
        PlayerPrefs.SetString("USER_ID", userId);
        //���� ������ �г��� ���
        PhotonNetwork.NickName = userId;
    }
    string SetRoomName()
    {
        if (string.IsNullOrEmpty(roomNameIF.text))
        {
            roomNameIF.text = $"ROOM_{Random.Range(1, 101):000}";
        }
        return roomNameIF.text;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //������ RoomItem �������� ������ �ӽ� ����
        GameObject tempRoom = null;

        foreach(var roomInfo in roomList)
        {
            //���� ������ ���
            if(roomInfo.RemovedFromList == true)
            {
                //��ųʸ����� �� �̸����� �˻��� ����� RoomItem �������� ����
                rooms.TryGetValue(roomInfo.Name, out tempRoom);

                //RoomItem������ ����
                Destroy(tempRoom);

                //��ųʸ����� �ش� �� �̸��� �����͸� ����
                rooms.Remove(roomInfo.Name);
            }
            else // �� ������ ����� ���
            {
                //�� �̸��� ��ųʸ��� ���� ��� ���� �߰�
                if(rooms.ContainsKey(roomInfo.Name) == false)
                {
                    //RoomInfo �������� scrollContent ������ ����
                    GameObject roomPrefab = Instantiate(roomItemPrefab, scrollContent);
                    //�� ������ ǥ���ϱ� ���� RoomInfo ���� ����
                    roomPrefab.GetComponent<RoomData>().RoomInfo = roomInfo;

                    //��ųʸ� �ڷ����� ������ �߰�
                    rooms.Add(roomInfo.Name, roomPrefab);
                }
                else //�� �̸��� ��ųʸ��� ���� ��쿡 �� ������ ����
                {
                    rooms.TryGetValue(roomInfo.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = roomInfo;
                }
            }
            Debug.Log($"Room={roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})");
        }
        
    }
    #region UI_BUTTON_EVENT
    public void OnLoginClick()
    {
        //��ǲ�ʵ忡 ���� �̸��� ����� �г����̶��
        //�α��� ����
        /*if(userId == userIF.text)
        {
            //�α��� ����
        }
        else
        {
            SetUserId();
            //�α��� ����
        }*/
        //��ǲ�ʵ忡 ���� �̸��� ����������� �г����̶��
        //���̵� ���θ���� �α��� ����
        SetUserId();

        PhotonNetwork.JoinRandomRoom();
    }
    public void OnMakeRoom()
    {
        SetUserId();

        // ���� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;     // �뿡 ������ �� �ִ� �ִ� ������ ��
        ro.IsOpen = true;       // ���� ���� ����
        ro.IsVisible = true;    // �κ񿡼� �� ��Ͽ� �����ų ����

        // �� ����
        PhotonNetwork.CreateRoom(SetRoomName(), ro);
    }
    #endregion
}
