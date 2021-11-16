using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] float speed = 1f;
    
    bool moving;
    Vector3 target;
    float step;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        step =  speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        var direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);

        if (Input.GetButtonDown("Fire1")){
            Instantiate(_laserPrefab, transform.position, Quaternion.identity);
        }

        if(Input.GetButtonDown("Fire2") && moving == false){
            moving = true;
            target = Input.mousePosition;
        }

        if(moving == true){ 
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }

        if(Vector3.Distance(transform.position, target) < 0.001f && moving == true){
            moving = false;
        }
    }
}
