using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] GameObject area;
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        target = area.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step =  speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if(Vector3.Distance(transform.position, target) < 0.001f){
            Destroy(gameObject);
        }
    }
}
