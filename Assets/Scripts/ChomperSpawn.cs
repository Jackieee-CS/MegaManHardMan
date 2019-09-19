using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperSpawn : MonoBehaviour
{
    public GameObject chompEnemy;

    public float lifetime;
    public Transform chompSpawn;

    public BoxCollider2D chompBox;
    private bool spawnSnake = false;


    // Start is called before the first frame update
    void Start()
    {
        lifetime = 2.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {

            Invoke("spawnChomp", 1);
            Invoke("doneSpawn", 1.1f);
        }

    }


   void spawnChomp()
    {
        if (spawnSnake == false)
        {
            spawnSnake = true;
            GameObject temp = Instantiate(chompEnemy, chompSpawn.position, chompSpawn.rotation);
        }

    }


    void doneSpawn()
    {
        spawnSnake = false;
    }


}
