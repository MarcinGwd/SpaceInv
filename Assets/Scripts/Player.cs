using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameObject player;
    [SerializeField]int lives = 3;
    [SerializeField]float speed = 0.2f;
    float mapBoundary = 6;
    float fireRate = 0.45f;
    private float nextFire;
    [SerializeField] GameObject projectile;
    [SerializeField] GameObject[] livesSprite;
    [SerializeField] GameObject explosion;
    bool offensivePowerUp;
    public GameObject shield;
    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        MovePlayer();
        FireCheck();
        BoundaryCheck();
    }
  
    void MovePlayer()
    {
        //Check for Horizontal input and translate ship position
        player.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed, 0f, 0f));
    }
    void FireCheck()
    {
        //Firing a projectile when Fire1 is pressed and fireRate time has passed
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            audio.Play();
            nextFire = Time.time + fireRate;
            Vector3 projectilePosition = this.transform.position;
            //if offensivePowerUp is active shoot with 3 projectiles
            if(offensivePowerUp)
            {
                Instantiate(projectile, projectilePosition, Quaternion.identity);
                Instantiate(projectile, new Vector3(projectilePosition.x-0.5f,projectilePosition.y), Quaternion.identity);
                Instantiate(projectile, new Vector3(projectilePosition.x + 0.5f, projectilePosition.y), Quaternion.identity);
            }    
            else
            Instantiate(projectile, projectilePosition, Quaternion.identity);
            /* //Object pooling from pool of projectiles
            nextFire = Time.time + fireRate;
            GameObject bullet = ObjectPool.SharedInstance.GetPooledObject(); 
            if (bullet != null) {
            bullet.transform.position = this.transform.position;
            bullet.transform.rotation = this.transform.rotation;
            bullet.SetActive(true);
            }*/

        }
    }
    void BoundaryCheck()
    {
        //If player position is out of map set it to position within map boundaries.
        if (player.transform.position.x > mapBoundary)
        {
            player.transform.position = new Vector3(mapBoundary, player.transform.position.y);
        }
        if (player.transform.position.x < -mapBoundary)
        {
            player.transform.position = new Vector3(-mapBoundary, player.transform.position.y);
        }
    }

    void Die()
    {
        //Explosion
        //Check if player has lives. If true decrease number of lives left
        if(lives>1)
        {
            lives--;
        }
        else
        {
            //call gameover event
            GameManager.Instance.GameOver();
        }
        UpdateLivesSprite(lives);
        
    }
    
    void ShieldPowerUp()
    {
        //Activates shield that can take 5 hits
        if (shield.active)
        {
            shield.SetActive(false);
            shield.SetActive(true);
        }
        else
            shield.SetActive(true);
    }
    void AddLives()
    {
        //checks if player has all lives if not adds him a live and calls a funtion to update GUI
        if (lives < 3)
        {
            lives++;
            UpdateLivesSprite(lives);
        }
    }
        void OffensivePowerUp()
    {
        offensivePowerUp = true;
        StartCoroutine(offensivePowerUpTimer());
    }
    IEnumerator offensivePowerUpTimer()
    {
        yield return new WaitForSeconds(25);
        offensivePowerUp = false;
    }    
    void UpdateLivesSprite(int lives)
    {
        //Updates the amount of remaining lives on the UI
        switch (lives)
        {
            case 1:
                livesSprite[0].SetActive(true);
                livesSprite[1].SetActive(false);
                livesSprite[2].SetActive(false);
                break;
            case 2:
                livesSprite[0].SetActive(true);
                livesSprite[1].SetActive(true);
                livesSprite[2].SetActive(false);
                break;
            case 3:
                livesSprite[0].SetActive(true);
                livesSprite[1].SetActive(true);
                livesSprite[2].SetActive(true);
                break;
            default:
                livesSprite[0].SetActive(true);
                livesSprite[1].SetActive(true);
                livesSprite[2].SetActive(true);
                break;
        }
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        //If player collided with enemy projectile call Die funtion.
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Die();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //If the enemy has touched the player the game is over

            //call gameover event
            GameManager.Instance.GameOver();
        }
        if (collision.gameObject.CompareTag("ShieldPowerUp"))
        {
            //Call ShieldPowerUp if player took the powerup
            ShieldPowerUp();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Heart"))
        {
            //Call AddLives if player took the Heart
            AddLives();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("OffensivePowerUp"))
        {
            //Call offensivePowerUp if player took the powerup and destroy the powerup gameobject
            OffensivePowerUp();
            Destroy(collision.gameObject);

        }
        if (collision.tag == "Enemy")
        {
            GameManager.Instance.GameOver();
        }
    }
   
}
