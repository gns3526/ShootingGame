using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemys;
    [SerializeField] Transform[] enemySpawnPoint;

    [SerializeField] float maxSpawnDelay;
    [SerializeField] float curSpawnDelay;

    [SerializeField] GameObject player;

    [SerializeField] Text scoreText;
    [SerializeField] Image[] lifeImage;
    [SerializeField] Image[] boomImage;
    [SerializeField] GameObject retryPanel;
    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            
            curSpawnDelay = 0;
        }
        Player playerScript = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}",playerScript.score);
    }

    void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, 3);
        int randomPoint = Random.Range(0, 8);
        GameObject enemy = Instantiate(enemys[randomEnemy], enemySpawnPoint[randomPoint].position, enemySpawnPoint[randomPoint].rotation);
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
        enemyScript.player = player;

        if(randomPoint == 5 || randomPoint == 8)
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyScript.speed * (-1), -1);
        }
        else if (randomPoint == 6 || randomPoint == 7)
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyScript.speed, -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyScript.speed * (-1));
        }
    }
    public void UpdateBoomIcon(int boom)
    {
        for (int i = 0; i < 3; i++)//끄기
        {
            boomImage[i].color = new Color(1, 1, 1, 0);
        }
        for (int i = 0; i < boom; i++)
        {
            boomImage[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateLifeIcon(int life)
    {
        for (int i = 0; i < 3; i++)//끄기
        {
            lifeImage[i].color = new Color(1, 1, 1, 0);
        }
        for (int i = 0; i < life; i++)
        {
            lifeImage[i].color = new Color(1, 1, 1, 1);
        }
    }

    public void ReSpawnM()
    {
        Invoke("ReSpawn", 2);
    }
    void ReSpawn()
    {
        player.transform.position = new Vector3(0, -3.5f, 0);
        player.SetActive(true);

        player.GetComponent<Player>().canHit = true;
    }
    public void GameOver()
    {
        retryPanel.SetActive(true);
    }
    public void ReStartGame()
    {
        SceneManager.LoadScene(0);
    }
}
