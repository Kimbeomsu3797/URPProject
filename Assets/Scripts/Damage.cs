using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Player = Photon.Realtime.Player;

public class Damage : MonoBehaviour
{
    // ��� �� ���� ó���� ���� MeshRenderer ������Ʈ�� �迭
    private Renderer[] renderers;

    //ĳ������ �ʱ� ����
    private int initHp = 100;
    //ĳ������ ���� ����
    public int currHp = 100;

    private Animator anim;
    private CharacterController cc;
    private PhotonView photonView;
    // �ִϸ����� �信 ������ �Ķ������ �ؽð� ����
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashRespawn = Animator.StringToHash("Respawn");

    private GameManager gameManager;
    private void Awake()
    {
        // ĳ���� ���� ��� Renderer ������Ʈ�� ������ �� �迭�� �Ҵ�
        renderers = GetComponentsInChildren<Renderer>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        // ���� ����ġ�� �ʱ� ����ġ�� �ʱ갪 ����
        currHp = initHp;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        // ���� ��ġ�� 0���� ũ�� �浹ü�� �±װ� Bullet�� ��쿡 ���� ��ġ�� ����
        if (currHp > 0 && coll.collider.CompareTag("BULLET"))
        {
            currHp -= 20;
            if(currHp <= 0)
            {
                if (photonView.IsMine)
                {
                    //�Ѿ��� ActorNumber�� ����
                    var actorNo = coll.collider.GetComponent<Bullet>().actorNumber;
                    //ActorNumber�� ���� �뿡 ������ �÷��̾ ����
                    Player lastShootPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNo);

                    //�޼��� ����� ���� ���ڿ� ����
                    string msg = string.Format("|n<color=#00ff00>{0}<|color> is killed by <color =#ff0000>{1}<|color>", photonView.Owner.NickName, lastShootPlayer.NickName);
                    photonView.RPC("KillMesseage", RpcTarget.AllBufferedViaServer, msg);
                }
                StartCoroutine(PlayerDie());
            }
        }
    }
    [PunRPC]
    void KillMessage(string msg)
    {
        gameManager.msgList.text += msg;
    }
    IEnumerator PlayerDie()
    {
        // CharacterController ������Ʈ ��Ȱ��ȭ
        cc.enabled = false;
        // ������ ��Ȱ��ȭ
        anim.SetBool(hashRespawn, false);
        // ĳ���� ��� �ִϸ��̼� ����
        anim.SetTrigger(hashDie);

        yield return new WaitForSeconds(3.0f);

        // ������ Ȱ��ȭ
        anim.SetBool(hashRespawn, true);

        // ĳ���� ���� ó��
        SetPlayerVisible(false);

        yield return new WaitForSeconds(1.5f);

        // ���� ��ġ�� ������
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length); // 0���� �θ�
        transform.position = points[idx].position;

        // ������ �� ���� �ʱ갪 ����
        currHp = 100;
        // ĳ���͸� �ٽ� ���̰� ó��
        SetPlayerVisible(true);
        // CharacterController ������Ʈ Ȱ��ȭ
        cc.enabled = true;
    }

    void SetPlayerVisible(bool isVisible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
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
