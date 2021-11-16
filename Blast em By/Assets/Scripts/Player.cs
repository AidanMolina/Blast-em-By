using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject bulletParent;
    [SerializeField] float speed = 1f;
    
    bool moving;
    Vector3 target;
    float step;

    public int ammo;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        step =  speed * Time.deltaTime;
        ammo = 8;
    }

    // Update is called once per frame
    void Update()
    {
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

        if (Input.GetButtonDown("Fire1") && ammo > 0){
            bulletParent.transform.GetChild(ammo-1).gameObject.SetActive(true);
            GameObject bullet = bulletParent.transform.GetChild(ammo-1).gameObject;
            Laser laserObject = bullet.GetComponent<Laser>();
            laserObject.updateTarget(gameObject.transform.GetChild(1).transform.position);
            bullet.transform.position = gameObject.transform.position;
            
            ammo -= 1;
        }

        if(Input.GetButtonDown("Fire2") && moving == false){
            moving = true;
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ammo = 8;
        }

        if(moving == true){ 
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        if(Vector3.Distance(transform.position, target) < 0.001f && moving == true){
            moving = false;
            Vector3 temp = new Vector3(0f, 0f, 10f);
            transform.position += temp;
        }
    }
}
