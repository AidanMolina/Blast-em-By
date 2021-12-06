using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemyBulletParent;
    [SerializeField] float speed = 1f;

    public float health;
    public float maxHealth;
    
    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;
    [SerializeField] GameObject point3;
    [SerializeField] GameObject point4;
    private GameObject[] above50;
    private GameObject[] below50;

    private int pointer;

    bool hitByLaser;
    float laserTimer = 1.0f;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 2f, 2f);

        above50 = new GameObject[]{point1, point2, point3, point4};
        below50 = new GameObject[]{point4, point3, point2, point1};
        pointer = 0;
        hitByLaser = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0){
            Destroy(gameObject);
        }

        if(health > maxHealth/2){
            Move(above50[pointer].transform.position);
        }

        if(health <= maxHealth/2){
            Move(below50[pointer].transform.position);
        }

        if(hitByLaser == true){
            laserTimer -= Time.deltaTime;
            if(laserTimer <= 0.0f){
                laserTimer = 1.0f;
                hitByLaser = false;
            }
        }

    }

    void Shoot(){
        if(player != null){
            enemyBulletParent.transform.GetChild(0).gameObject.SetActive(true);
            GameObject bullet = enemyBulletParent.transform.GetChild(0).gameObject;
            EnemyBullet bulletObject = bullet.GetComponent<EnemyBullet>();
            bulletObject.updateTarget(player.transform.position);
            bullet.transform.position = gameObject.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("PlayerBullet")){
            collider.gameObject.SetActive(false);
            health -= 1;
            slider.value = health/maxHealth;
        }
    }

    void OnTriggerStay2D(Collider2D collider){
        if(collider.gameObject.CompareTag("PlayerLaser") && hitByLaser == false){
            health -= 5;
            hitByLaser = true;
        }
    }

    void Move(Vector3 target){
        if(gameObject.activeSelf){
            float step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if(Vector3.Distance(transform.position, target) < 0.001f){
                if(pointer < 3){
                    pointer += 1;
                }
                else{
                    pointer = 0;
                }
            }
        }
    }
}
