using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Follower : MonoBehaviour
{
    public float maxShotCoolTime;
    [SerializeField] float curShotCoolTime;
    public int bulletType;

    public ObjectPooler OP;

    [SerializeField] Vector3 followPos;
    [SerializeField] int followDelay;
    [SerializeField] Transform parent;
    [SerializeField] Queue<Vector3> parentPos;

    public Player player;
    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }


    private void Update()
    {
        Watch();
        Follow();
        Fire();
        curShotCoolTime += Time.deltaTime;
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
        if (!Input.GetButton("Fire1")) return;

        if (!player.GetComponent<PhotonView>().IsMine) return;

        if (curShotCoolTime < maxShotCoolTime) return;

        switch (bulletType)
        {
            case 1:
                GameObject bullet = OP.PoolInstantiate("FollowerBullet1", transform.position,Quaternion.identity);
                //bullet.transform.position = transform.position;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bullet2 = OP.PoolInstantiate("FollowerBullet2", transform.position, Quaternion.identity);
                //bullet2.transform.position = transform.position;

                Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
                rigid2.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
        }

        curShotCoolTime = 0;
    }
}
