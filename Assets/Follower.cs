using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] float maxShotCoolTime;
    [SerializeField] float curShotCoolTime;
    [SerializeField] ObjectManager OM;

    [SerializeField] Vector3 followPos;
    [SerializeField] int followDelay;
    [SerializeField] Transform parent;
    [SerializeField] Queue<Vector3> parentPos;

    private void Awake()
    {
        parentPos = new Queue<Vector3>();
    }
    private void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
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

        if (curShotCoolTime < maxShotCoolTime) return;

        GameObject bullet = OM.MakeObj("BulletFollower0");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);


        curShotCoolTime = 0;
    }
    void Reload()
    {
        curShotCoolTime += Time.deltaTime;
    }
}
