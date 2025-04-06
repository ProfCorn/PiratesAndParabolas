using UnityEngine;

public class Cannonball : MonoBehaviour
{
    private Vector3 initialVelocity;
    private float gravity = 9.81f;
    public GameObject enemyPirateShip;  // Reference to the enemy pirate ship (optional)

    // Reference to the explosion particle effect prefab
    public GameObject explosionPrefab;

    private void Start()
    {
        // Set the initial velocity (this should be set by LaunchController)
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = initialVelocity;
    }

    public void SetInitialVelocity(Vector3 velocity)
    {
        initialVelocity = velocity;
    }

    // Update is called once per frame to check if the cannonball goes below y = -3
    void Update()
    {
        if (transform.position.y < -5f)
        {
            DestroyCannonball();
        }
    }

    // This method is called when the cannonball collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the cannonball hits the enemy pirate ship
        if (collision.gameObject.CompareTag("EnemyPirateShip"))
        {
            DestroyCannonball();
            DestroyEnemyPirateShip(collision);
        }
    }

    // Destroy the cannonball (either by falling below y = -3 or colliding with the pirate ship)
    private void DestroyCannonball()
    {
        // Instantiate the explosion particle effect at the cannonball's position
        if (explosionPrefab != null)
        {
            // Create the particle system at the cannonball's position
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Destroy the particle effect after its duration is over
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }

        // Destroy the cannonball object
        Destroy(gameObject);
    }

    // Destroy the cannonball (either by falling below y = -3 or colliding with the pirate ship)
    private void DestroyEnemyPirateShip(Collision collision)
    {
        // Instantiate the explosion particle effect at the cannonball's position
        if (explosionPrefab != null)
        {
            // Create the particle system at the cannonball's position
            GameObject explosion = Instantiate(explosionPrefab, collision.gameObject.transform.position, Quaternion.identity);

            // Destroy the particle effect after its duration is over
            Destroy(explosion, explosion.GetComponent<ParticleSystem>().main.duration);
        }

        // Destroy the cannonball object
        Destroy(collision.gameObject);
    }
}
