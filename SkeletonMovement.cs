using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkeletonMovement : MonoBehaviour
{
    public GameObject player;
    private Rigidbody RB;
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
     RB = GetComponent <Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
    	Vector3 t = new Vector3(player.transform.position.x, 0.0064f, player.transform.position.z);
     transform.LookAt(t);
     if (!GetComponent<SkeletonAttack>().Attack){

     
     transform.Translate(transform.forward * -speed * Time.deltaTime);
    }
}
}