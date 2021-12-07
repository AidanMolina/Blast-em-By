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
    private GameObject[] below25;
    private GameObject[] above25;
    private GameObject[] above75;

    private bool above75HP;
    private bool above50HP;
    private bool above25HP;
    private bool below25HP;

    private int pointer;

    bool hitByLaser;
    float laserTimer = 1.0f;

    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", 2f, 2f);

        above75 = new GameObject[]{point1, point2, point3, point4};
        above50 = new GameObject[]{point1, point3, point4, point2};
        above25 = new GameObject[]{point4, point3, point2, point1};
        below25 = new GameObject[]{point4, point1, point3, point2};
        
        pointer = 0;
        hitByLaser = false;

        above75HP = true;
        above50HP = false;
        above25HP = false;
        below25HP = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null){
            var direction = new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        

            if(health <= 0){
                Destroy(gameObject);
            }

            if(health >= 75){
                Move(above75[pointer].transform.position);
            }
            else if(health >= 50){
                Move(above50[pointer].transform.position);
                if(above75HP){
                    above50HP = true;
                    above75HP = false;
                }
            }
            else if(health >= 25){
                Move(above25[pointer].transform.position);
                if(above50HP){
                    above25HP = true;
                    above50HP = false;
                }
            }
            else{
                Move(below25[pointer].transform.position);
                if(above25HP){
                    below25HP = true;
                    above25HP = false;
                }
            }

            if(hitByLaser == true){
                laserTimer -= Time.deltaTime;
                if(laserTimer <= 0.0f){
                    laserTimer = 1.0f;
                    hitByLaser = false;
                }
            }

            if(above50HP){
                Target();
            }

            if(above25HP){
                Target();
                Bomb();
            }

            if(below25HP){
                Target();
                Bomb();
                Spray();
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

    void Target(){

    }

    void Bomb(){

    }

    void Spray(){

    }
}
