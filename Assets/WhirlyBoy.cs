using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlyBoy : MonoBehaviour
{
    public float fireRate;
    public WhirlyBall ballPrefab;
    public Transform projectileSpawn;
    float timeSinceLastFire;
    private bool m_FacingRight = true;
    

    public GameObject target = null;
    public float ballSpeed;




    // Start is called before the first frame update
    void Start()
    {
        if (!target)
            target = GameObject.FindWithTag("Player");

        ballSpeed = 2.0f;
        fireRate = 1.0f;
        shootDirectionCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            if(Time.time > timeSinceLastFire + fireRate)
            {
                fireProjectile();

                timeSinceLastFire = Time.time;

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            if (Time.time > timeSinceLastFire + fireRate)
            {
                fireProjectile();

                timeSinceLastFire = Time.time;

            }
        }
    }


    void shootDirectionCheck()
    {


        if (target.transform.position.x < transform.position.x && m_FacingRight == false)
        {
            Flip();
        }
           
        else if (target.transform.position.x > transform.position.x && m_FacingRight == true)
        {
            Flip();
        }
            


    }


    void fireProjectile()
    {
        shootDirectionCheck();

        WhirlyBall temp = Instantiate(ballPrefab, projectileSpawn.position, projectileSpawn.rotation);

        if (!m_FacingRight)
        {
            temp.speed = -ballSpeed;
            temp.GetComponent<SpriteRenderer>().flipX = false;
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (m_FacingRight)
        {
            temp.speed = ballSpeed;
            temp.GetComponent<SpriteRenderer>().flipX = true;
        }

    }




    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }





}
