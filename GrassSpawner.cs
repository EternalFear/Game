using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
	public GameObject grass;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5000; i ++) {
        	Instantiate(grass, new Vector3(Random.Range(-149.5f, 149.66f),Random.Range(0.14f, 0.16f), Random.Range(-149.7f, 149.47f)), new Quaternion(0, 0, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
