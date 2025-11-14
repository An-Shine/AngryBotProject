using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Player = Photon.Realtime.Player;

public class Damage : MonoBehaviour
{
    Renderer[] renderers;
    int initHp = 100;
    int currHp = 100;
    Animator anim;
    CharacterController cc;
    readonly int hashDie = Animator.StringToHash("Die");
    readonly int hashRespawn = Animator.StringToHash("Respawn");
    NewGameManager gameManager;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();    //Renderer 가 배열이므로 Components<<
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        currHp = initHp;

        gameManager = GameObject.Find("GamaManager").Getcomponenet<GameManager>();

    }
    void OnCollisionEnter(Collision collision)
    {
        if (currHp > 0 && collision.collider.CompareTag("BULLET"))
        {
            currHp -= 20;
            if (currHp <= 0)
            {
                //자신의 PhotonView 일때만 메세지 출력
                if(PhotonView.IsMine)
                {
                    //총알의ActorNumber 추출
                    var actorNo = coll.collider.GetComponent<Bullet>().actorNumber;
                    //ActorNumber 로 현재룸에 입장한 플레이어를 추출
                    Player lastShootPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNo);

                    //메세지 출력을 위한 문자열 포맷
                    string msg = string.Format("\n<color=#00ff00>{0}</color> is kiled by <color=#ff0000>{1}</color>", photonView.Owner.NickName, lastShootPlayer.NickName);
                    photonView.RPC("KillMessage", RpcTarget.AllBufferedViaServer, msg);
                }
                StartCoroutine(PlayerDie());
            }
        }
    }

    [PunRPC]
    void KillMessage()
    {
        //메세지 출력
        gameManager.msgList.text == msg;
    }
    IEnumerator PlayerDie()
    {
        cc.enabled = false;
        anim.SetBool(hashRespawn, false);
        anim.SetTrigger(hashDie);
        yield return new WaitForSeconds(3.0f);

        anim.SetBool(hashRespawn, true);
        SetPlayerVisible(false);
        yield return new WaitForSeconds(1.5f);

        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();

        int idx = Random.Range(1, points.Length);
        transform.position = points[idx].position;
        currHp = initHp;
        SetPlayerVisible(true);
        cc.enabled = true;

    }

    void SetPlayerVisible(bool isVisible)
    {
        for(int i = 0; i<renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;            
        }
    }
}
