using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Fire : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;
    ParticleSystem muzzleFlash;
    PhotonView pv;    
    bool isMouseClick => Input.GetMouseButtonDown(0);
    void Start()
    {
        //포톤뷰 컴포넌트 연결
        pv = GetComponent<PhotonView>();
        //FirePos 하위에 있는 총구화염효과 연결
        muzzleFlash = firePos.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    }


    void Update()
    {
        //로컬유저 여부와 마우스 왼쪽버튼을 클릭했을 때 총알 발사
        if (pv.IsMine && isMouseClick)
        {
            FireBullet();
            //RPC 로 원격지에 있는 함수를 호출
            pv.RPC("FireBullet", RpcTarget.Others, null);
        }
    }
    [PunRPC]
    void FireBullet()
    {
        //총구화염효과가 실행중이 아닌 경우에 총구화염효과 실행
        if (!muzzleFlash.isPlaying) muzzleFlash.Play(true);

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
    }
}

