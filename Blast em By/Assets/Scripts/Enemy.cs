using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemyBulletParent;

    public int health;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0){
            Destroy(gameObject);
        }
    }

    void Shoot(){
            enemyBulletParent.transform.GetChild(0).gameObject.SetActive(true);
            GameObject bullet = enemyBulletParent.transform.GetChild(0).gameObject;
            EnemyBullet bulletObject = bullet.GetComponent<EnemyBullet>();
            bulletObject.updateTarget(player.transform.position);
            bullet.transform.position = gameObject.transform.position;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("PlayerBullet")){
            collider.gameObject.SetActive(false);
            health -= 1;
        }
    }
}
