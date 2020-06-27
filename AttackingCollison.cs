using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingCollison : MonoBehaviour
{

	private bool GetHit;
	private Animator anim;
	private int Health;
    // Start is called before the first frame update
    void Start()
    {
    	Health = 100;
    	GetHit = false;
    	anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    	if (GetHit) {
    		anim.CrossFade("GetHit", 0.1f);
    		anim.SetBool("GetHit", true);
    	}
     	
    }
    void OnCollisionEnter(Collision c) {
        Debug.Log(c.gameObject);
    	if (c.gameObject.tag == "Weapon") {
    		Health -= 50;
    		GetHit = true;
    	}
    }
}
