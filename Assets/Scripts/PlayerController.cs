using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5.0f;
    private GameObject focalPoint;
    public GameObject powerupIndicator, smashPowerupIndicator, bullet;
    public bool hasPowerUp = false;
    public bool hasPowerUpSmash = false;
    public bool inTheAir = false;
    private float poweUpStrength = 15.0f;
    public GameObject[] enemy;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed *  forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        smashPowerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (Input.GetKeyDown(KeyCode.J) && hasPowerUpSmash)
        {
            StartCoroutine(ballJump());
        }
        
    }

    IEnumerator ballJump()
    {
        playerRb.AddForce(Vector3.up * poweUpStrength, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        inTheAir = true;
        playerRb.AddForce(Vector3.down * 80f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
        if (other.gameObject.CompareTag("secondPowerUp"))
        {
            Destroy(other.gameObject);
            for(int i = 0; i < enemy.Length; i++)
            {
                GameObject spawnedBullet = Instantiate(bullet, transform.position, bullet.transform.rotation);
                Vector3 fireDirection = (enemy[i].transform.position -  spawnedBullet.transform.position).normalized;
                spawnedBullet.GetComponent<Rigidbody>().AddForce(fireDirection * poweUpStrength, ForceMode.Impulse);
            }
        }
        if (other.gameObject.CompareTag("thirdPowerUp"))
        {
            hasPowerUpSmash = true;
            Destroy(other.gameObject);
            smashPowerupIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided with " + collision.gameObject.name + " with power up set to " + hasPowerUp);
            enemyRigidBody.AddForce(awayFromPlayer * poweUpStrength, ForceMode.Impulse);
        }
        if(collision.gameObject.name == "Island" && inTheAir)
        {
            foreach (GameObject e in enemy)
            {
                e.GetComponent<Enemy>().speed *= -5;
            }
            inTheAir = false;
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        hasPowerUpSmash = false;
        powerupIndicator.SetActive(false);
        smashPowerupIndicator.SetActive(false);
    }
}
