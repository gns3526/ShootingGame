using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ObjectPooler : MonoBehaviourPun
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPooler OP;
    [SerializeField] PhotonView pv;
    [SerializeField] GameManager gameManager;
    [SerializeField] NetworkManager networkManager;

    void Awake() => OP = this;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    GameObject curSpawnedOb;

    public void PrePoolInstantiate()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                curSpawnedOb = PhotonNetwork.Instantiate(pool.tag, new Vector3(16, 16, 0), Quaternion.identity);

                if (curSpawnedOb.tag == "Enemy")
                {
                    EnemyBasicScript enemyBasic = curSpawnedOb.GetComponent<EnemyBasicScript>();

                    enemyBasic.GM = gameManager;
                    enemyBasic.OP = OP;
                }
                
                else if (curSpawnedOb.tag == "Bullet")
                {
                    BulletScript bulletScript = curSpawnedOb.GetComponent<BulletScript>();

                    bulletScript.GM = gameManager;
                    bulletScript.OP = OP;
                }

                else if (curSpawnedOb.tag == "Barrier")
                {
                    BarrierScript barrier = curSpawnedOb.GetComponent<BarrierScript>();

                    barrier.gm = gameManager;
                    barrier.op = OP;
                    barrier.myPlayerScript = networkManager.myPlayer.GetComponent<Player>();

                    Debug.Log("방벽");
                }

                curSpawnedOb.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, false, -2, -1, 0, true);
                objectPool.Enqueue(curSpawnedOb);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    public GameObject PoolInstantiate(string tag, Vector3 position, Quaternion rotation, int bulletIndex, int bulletAniCode, int bulletSpeedIndex, bool isPlayerAttack)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't excist");
            return null;
        }

        curSpawnedOb = poolDictionary[tag].Dequeue();//Take out First as Num

        curSpawnedOb.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, true, bulletIndex, bulletAniCode, bulletSpeedIndex, isPlayerAttack);
        curSpawnedOb.transform.position = position;
        curSpawnedOb.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(curSpawnedOb);

        return curSpawnedOb;
    }

    public GameObject DamagePoolInstantiate(string tag, Vector3 position, Quaternion rotation, int damageAmount, int damageColorCode, int damageSkinCode, bool isPlus)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't excist");
            return null;
        }

        curSpawnedOb = poolDictionary[tag].Dequeue();//Take out First as Num

        curSpawnedOb.GetComponent<PhotonView>().RPC("DamagePoolRPC", RpcTarget.All, true, damageAmount, damageColorCode, damageSkinCode, isPlus);
        curSpawnedOb.transform.position = position;
        curSpawnedOb.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(curSpawnedOb);

        return curSpawnedOb;
    }

    public void PoolDestroy(GameObject obj)
    {
        obj.GetComponent<PhotonView>().RPC("SetActiveRPC", RpcTarget.All, false, -2, -1, 0, true);
    }
}
