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

    public ObjectPooler OP;
    [SerializeField] JobManager JM;

    [SerializeField] Vector3 followPos;
    [SerializeField] int followDelay;
    [SerializeField] Transform parent;
    [SerializeField] Queue<Vector3> parentPos;

    

    public Player player;
    [SerializeField] Rigidbody2D rigid;

    Vector3 curPosPv;
    private void Awake()
    {
        JM = FindObjectOfType<JobManager>();
        parentPos = new Queue<Vector3>();
        rigid.Sleep();
    }


    private void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            if (!JM.skillBOn)
            {
                Watch();
                Follow();
            }
            Fire();
            curShotCoolTime += Time.deltaTime * (player.petAttackSpeedPer / 100) * (JM.skillBOn == true ? 1.5f : 1);

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, curPosPv, Time.deltaTime * 15);
        }
    }


    void Watch()
    {
        if (!parentPos.Contains(parent.position))//부모가 움직이지 않으면
        {
            parentPos.Enqueue(parent.position);//먼저 기억하기(위치값을 넣는다)
        }
        if(parentPos.Count > followDelay)//데이터 갯수가 체워지면
        {
            followPos = parentPos.Dequeue();//빼서 대입한다
        }
        else if(parentPos.Count < followDelay)//안체워지면
        {
            followPos = parent.position;//부모위치로 텔포
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
                if (JM.skillBOn)
                {
                    float angle = Mathf.Atan2(JM.skillBPoint.transform.position.y - gameObject.transform.position.y, JM.skillBPoint.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
                    GameObject bullet = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.Euler(0, 0, angle - 90), 2, -1, 5, true);
                    bullet.GetComponent<BulletScript>().dmgPer = 100;
                } 
                else if (player.isFire)
                {
                    GameObject bullet1 = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.identity, 2, -1, 5, true);
                    bullet1.GetComponent<BulletScript>().dmgPer = 100;
                }
                    
                break;
            case 2:
                if (JM.skillBOn)
                {
                    float angle = Mathf.Atan2(JM.skillBPoint.transform.position.y - gameObject.transform.position.y, JM.skillBPoint.transform.position.x - gameObject.transform.position.x) * Mathf.Rad2Deg;
                    GameObject bullet2 = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.Euler(0, 0, angle - 90), 2, -1, 5, true);
                    bullet2.GetComponent<BulletScript>().dmgPer = 100;
                }
                else if (player.isFire)
                {
                    GameObject bullet3 = OP.PoolInstantiate("BulletBasic", transform.position, Quaternion.identity, 2, -1, 5, true);
                    bullet3.GetComponent<BulletScript>().dmgPer = 100;
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
