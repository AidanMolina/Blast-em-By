using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    Vector3 mainTarget;
    float step;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        step =  speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        Move(mainTarget);
    }

    void Move(Vector3 target){
        if(gameObject.activeSelf){
            transform.position = Vector3.MoveTowards(transform.position, target, step);
            if(Vector3.Distance(transform.position, target) < 0.001f){
                gameObject.SetActive(false);
            }
        }
    }

    public void updateTarget(Vector3 target){
        mainTarget = target;
    }
}
