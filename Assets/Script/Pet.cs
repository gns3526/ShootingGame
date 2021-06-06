using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pet : MonoBehaviourPun, IPunObservable
{
    public float maxShotCoolTime;
    [SerializeField] float curShotCoolTime;
    public int bulletType;

    PetManager petManager;
    public ObjectPooler objectPooler;
    JobManager jobManager;
    PlayerState ps;

    [SerializeField] Vector3 followPos;
    [SerializeField] int followDelay;
    public Transform followTarget;
    [SerializeField] Queue<Vector3> followTargetPos;

    [SerializeField] PhotonView pv;

    public Player player;

    Vector3 curPosPv;
    private void Awake()
    {
        petManager = FindObjectOfType<PetManager>();

        if (pv.IsMine)
        {
            objectPooler = petManager.OP;
            jobManager = petManager.JM;
            ps = petManager.ps;
            followTargetPos = new Queue<Vector3>();

            transform.parent = petManager.gameObject.transform;

            for (int i = 0; i < petManager.petGroup.Length; i++)
            {
                if(petManager.petGroup[i] == null)
                {
                    petManager.petGroup[i] = gameObject;
                    return;
                }
            }
        }
    }


    private void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            if (!jobManager.skillBOn)
            {
                Watch();
                Follow();
            }
            Fire();
            curShotCoolTime += Time.deltaTime * (ps.petAttackSpeedPer / 100) * (jobManager.skillBOn == true ? 1.5f : 1);

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, curPosPv, Time.deltaTime * 15);
        }
    }


    void Watch()
    {
        if (!followTargetPos.Contains(followTarget.position))//부모가 움직이지 않으면
        {
            followTargetPos.Enqueue(followTarget.position);//먼저 기억하기(위치값을 넣는다)
        }
        if(followTargetPos.Count > followDelay)//데이터 갯수가 체워지면
        {
            followPos = followTargetPos.Dequeue();//빼서 대입한다
        }
        else if(followTargetPos.Count < followDelay)//안체워지면
        {
            followPos = followTarget.position;//부모위치로 텔포
        }
    }
    void Follow()
    {
        transform.position = followPos;
    }

    void Fire()
    {
        if (player.isDie) return;

        if (!player.GetComponent<PhotonView>().IsMine) return;

        if (curShotCoolTime < maxShotCoolTime) return;

        switch (bulletType)
        {
            case 1:
                if (jobManager.skillBOn)
                {
                    float angle = Mathf.Atan2(jobManager.skillBPoint.transform.position.y - gameObject.transform.position.y, jobManager.skillBPoint.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
                    GameObject bullet = objectPooler.PoolInstantiate("BulletBasic", transform.position, Quaternion.Euler(0, 0, angle - 90), 2, -1, 5, 0, true);
                    bullet.GetComponent<BulletScript>().dmgPer = 0;
                    bullet.GetComponent<BulletScript>().ispetAttack = true;
                } 
                else if (player.isFire)
                {
                    GameObject bullet1 = objectPooler.PoolInstantiate("BulletBasic", transform.position, Quaternion.identity, 2, -1, 5, 0, true);
                    bullet1.GetComponent<BulletScript>().dmgPer = 0;
                    bullet1.GetComponent<BulletScript>().ispetAttack = true;
                }
                    
                break;
            case 2:
                if (jobManager.skillBOn)
                {
                    float angle = Mathf.Atan2(jobManager.skillBPoint.transform.position.y - gameObject.transform.position.y, jobManager.skillBPoint.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
                    GameObject bullet2 = objectPooler.PoolInstantiate("BulletBasic", transform.position, Quaternion.Euler(0, 0, angle - 90), 3, -1, 5, 0, true);
                    bullet2.GetComponent<BulletScript>().dmgPer = 300;
                    bullet2.GetComponent<BulletScript>().dmg = 5;
                    bullet2.GetComponent<BulletScript>().ispetAttack = true;
                }
                else if (player.isFire)
                {
                    GameObject bullet3 = objectPooler.PoolInstantiate("BulletBasic", transform.position, Quaternion.identity, 3, -1, 5, 0, true);
                    bullet3.GetComponent<BulletScript>().dmgPer = 300;
                    bullet3.GetComponent<BulletScript>().dmg = 5;
                    bullet3.GetComponent<BulletScript>().ispetAttack = true;
                }
                   
                break;
        }
        curShotCoolTime = 0;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//isMine = true
        {
            stream.SendNext(followPos);
        }
        else
        {
            curPosPv = (Vector3)stream.ReceiveNext();
        }
    }
}
