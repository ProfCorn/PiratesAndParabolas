using System.Collections.Generic;
using UnityEngine;

public class EnemyPirateShipSink : MonoBehaviour
{
    [Header("Effect Settings")]
    public GameObject sinkEffectPrefab;
    public List<Transform> BubbleSpawners = new List<Transform>();

    [Header("Spawner Settings")]
    public int maxSpawners = 200;
    [Range(0f, 1f)] public float spawnerChance = 1f; // Chance each vertex creates a spawner (1 = 100%)

    [Header("Effect Chance")]
    [Range(0f, 1f)] public float bubbleEffectChance = 1f; // Chance each spawner will play effect

    private float sinkSpeed = 0.5f;
    private float destroyYPosition = -8f;
    public bool isSinking = false;

    private List<GameObject> activeSinkEffects = new List<GameObject>();
    private Mesh shipMesh;

    private void Start()
    {
        MeshFilter mf = GetComponentInChildren<MeshFilter>();
        if (mf != null)
        {
            shipMesh = mf.mesh;
            GenerateSpawnersFromMesh();
        }
        else
        {
            Debug.LogWarning("MeshFilter not found on ship!");
        }
    }

    private void Update()
    {
        if (isSinking)
        {
            transform.Translate(Vector3.back * sinkSpeed * Time.deltaTime);

            if (transform.position.y <= destroyYPosition)
            {
                DestroyShip();
            }

            // Spawn effects only if below threshold and passes random chance
            foreach (Transform spawner in BubbleSpawners)
            {
                if (spawner.position.y <= -3.1f && Random.value <= bubbleEffectChance)
                {
                    // Create the particle system at the cannonball's position
                    GameObject effect = Instantiate(sinkEffectPrefab, spawner.position, Quaternion.identity);

                    // Destroy the particle effect after its duration is over
                    Destroy(effect, effect.GetComponent<ParticleSystem>().main.duration);


                }
            }

            
        }
    }

    private void GenerateSpawnersFromMesh()
    {
        BubbleSpawners.Clear();

        if (shipMesh == null) return;

        Vector3[] vertices = shipMesh.vertices;
        int added = 0;

        for (int i = 0; i < vertices.Length && added < maxSpawners; i++)
        {
            if (Random.value > spawnerChance) continue; // Skip this one

            Vector3 localVertex = vertices[i];
            Vector3 worldPos = transform.TransformPoint(localVertex);

            GameObject spawner = new GameObject("Spawner_" + i);
            spawner.transform.position = worldPos;
            spawner.transform.parent = transform;

            BubbleSpawners.Add(spawner.transform);
            added++;
        }
    }

    public void StartSinking()
    {
        if (isSinking) return;
        isSinking = true;
    }

    private void DestroyShip()
    {
        foreach (GameObject effect in activeSinkEffects)
        {
            if (effect != null)
                Destroy(effect);
        }

        Destroy(gameObject);
    }
}
