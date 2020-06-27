using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator Anim;
    public GameObject player;
    public bool Attack;
    private int AttackNum; 
    public int AttackRange;

    void Start()
    {
    	Anim = GetComponent<Animator>(); 
        Attack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < AttackRange&&!Attack)
        {
            AttackNum = Mathf.FloorToInt(Random.Range(1,2));
        	Anim.SetTrigger("Attack" + AttackNum);
            Attack = true;
        }
        else if (Vector3.Distance(player.transform.position, transform.position) >= AttackRange&&Attack){
            Anim.SetTrigger("Attack" + AttackNum);
            Attack = false;
        }
    }
}