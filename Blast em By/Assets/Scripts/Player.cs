using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject bulletParent;
    [SerializeField] float speed = 1f;
    [SerializeField] GameObject canvas;
    
    bool moving;
    Vector3 target;
    float step;
    
    bool justHit;
    float invincibilityTimer = 1.0f;

    bool shield;
    bool shieldOnCD;
    float shieldCD = 10.0f;

    bool spreadOnCD;
    float spreadCD = 3.0f;

    bool laserActive;
    bool laserOnCD;
    float laserCD = 20.0f;
    float laserActiveTime = 3.0f;

    public int ammo;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        ammo = 8;       
        justHit = false;

        shield = false;
        shieldOnCD = false;

        spreadOnCD = false;

        laserActive = false;
        laserOnCD = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Looking at cursor
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

        //Firing bullets
        if (Input.GetButtonDown("Fire1") && ammo > 0){
            bulletParent.transform.GetChild(ammo-1).gameObject.SetActive(true);
            GameObject bullet = bulletParent.transform.GetChild(ammo-1).gameObject;
            Laser laserObject = bullet.GetComponent<Laser>();
            laserObject.updateTarget(gameObject.transform.GetChild(1).transform.position);
            bullet.transform.position = gameObject.transform.position;
            
            ammo -= 1;
            TakeBullets();
        }

        //Movement
        if(Input.GetButtonDown("Fire2") && moving == false){
            moving = true;
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ammo = 8;
            AddBullets();
        }

        if(moving == true){ 
            step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        if(Vector3.Distance(transform.position, target) < 0.001f && moving == true){
            moving = false;
            Vector3 temp = new Vector3(0f, 0f, 10f);
            transform.position += temp;
        }

        //Shield
        if(Input.GetButtonDown("Jump") && shield == false && shieldOnCD == false && ammo >= 4){
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
            shield = true;
            ammo -= 4;
        }

        if(shieldOnCD == true){
            shieldCD -= Time.deltaTime;
            if(shieldCD <= 0.0f){
                shieldOnCD = false;
                shieldCD = 10.0f;
                CheckShield();
            }
        }

        //Spread Shot
        if(Input.GetButtonDown("Fire3") && ammo >= 3 && spreadOnCD == false){
            GameObject bullet1 = bulletParent.transform.GetChild(ammo-1).gameObject;
            GameObject bullet2 = bulletParent.transform.GetChild(ammo-2).gameObject;
            GameObject bullet3 = bulletParent.transform.GetChild(ammo-3).gameObject;

            bullet1.SetActive(true);
            bullet2.SetActive(true);
            bullet3.SetActive(true);

            Laser laserObject1 = bullet1.GetComponent<Laser>();
            Laser laserObject2 = bullet2.GetComponent<Laser>();
            Laser laserObject3 = bullet3.GetComponent<Laser>();

            laserObject1.updateTarget(gameObject.transform.GetChild(1).transform.position);
            laserObject2.updateTarget(gameObject.transform.GetChild(3).transform.position);
            laserObject3.updateTarget(gameObject.transform.GetChild(4).transform.position);

            bullet1.transform.position = gameObject.transform.position;
            bullet2.transform.position = gameObject.transform.position;
            bullet3.transform.position = gameObject.transform.position;

            ammo -= 3;
            spreadOnCD = true;
            CheckSpread();
            TakeBullets();
        }

        if(spreadOnCD == true){
            spreadCD -= Time.deltaTime;
            if(spreadCD <= 0.0f){
                spreadOnCD = false;
                spreadCD = 3.0f;
                CheckSpread();
            }
        }

        //Laser
        if(Input.GetKeyDown(KeyCode.X) && laserActive == false && laserOnCD == false && ammo == 8){
            gameObject.transform.GetChild(5).gameObject.SetActive(true);
            laserActive = true;
            ammo = 0;
            laserOnCD = true;
            CheckLaser();
            TakeBullets();
        }

        if(laserOnCD == true){
            laserCD -= Time.deltaTime;
            laserActiveTime -= Time.deltaTime;

            if(laserActiveTime <= 0.0f){
                gameObject.transform.GetChild(5).gameObject.SetActive(false);
                laserActive = false;
            }
            if(laserCD <= 0.0f){
                laserOnCD = false;
                laserCD = 20.0f;
                laserActiveTime = 3.0f;
                CheckLaser();
            }
        }

        //Invincibility Frames
        if(justHit == true){
            invincibilityTimer -= Time.deltaTime;

            GameObject sprite = gameObject.transform.GetChild(0).gameObject;
            Color temp = sprite.GetComponent<SpriteRenderer>().color;
            temp.a = 0.4f;
            sprite.GetComponent<SpriteRenderer>().color = temp;

            if(invincibilityTimer <= 0.0f){
                justHit = false;
                temp.a = 1f;
                invincibilityTimer = 1.0f;
                sprite.GetComponent<SpriteRenderer>().color = temp;
            }
        }

        //Death
        if(health <= 0){
            Destroy(gameObject);
            canvas.transform.GetChild(17).gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("EnemyBullet")){
            collider.gameObject.SetActive(false);
            if(shield == true){
                shield = false;
                shieldOnCD = true;
                CheckShield();
                gameObject.transform.GetChild(2).gameObject.SetActive(false);
            }
            else if(justHit == false){
                health -= 1;
                justHit = true;
                CheckHealth();
            }
        }
    }

    void CheckHealth(){
        for(int i = 0; i < 3; i ++){
            if(i > health - 1){
                canvas.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void TakeBullets(){
        for(int i = 0; i < 8; i ++){
            if(i > ammo - 1){
                canvas.transform.GetChild(i+3).gameObject.SetActive(false);
            }
        }
    }

    void AddBullets(){
        for(int i = 3; i < 11; i++){
            canvas.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void CheckShield(){
        if(shieldOnCD == true){
            canvas.transform.GetChild(11).gameObject.SetActive(false);
        }
        else{
            canvas.transform.GetChild(11).gameObject.SetActive(true);
        }
    }

    void CheckSpread(){
        if(spreadOnCD == true){
            canvas.transform.GetChild(12).gameObject.SetActive(false);
            canvas.transform.GetChild(13).gameObject.SetActive(false);
            canvas.transform.GetChild(14).gameObject.SetActive(false);
        }
        else{
            canvas.transform.GetChild(12).gameObject.SetActive(true);
            canvas.transform.GetChild(13).gameObject.SetActive(true);
            canvas.transform.GetChild(14).gameObject.SetActive(true);
        }
    }

    void CheckLaser(){
        if(laserOnCD == true){
            canvas.transform.GetChild(15).gameObject.SetActive(false);
        }
        else{
            canvas.transform.GetChild(15).gameObject.SetActive(true);
        }
    }
}
