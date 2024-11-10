using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fallingObjectPrefab;
    [SerializeField] private Transform swordTransform;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float xSpawnRange = 8f;
    [SerializeField] private Color[] spawnColors = { Color.red, Color.blue, Color.green };


    private const float InitialDelay = 1f;

    private void Start()
    {
        StartCoroutine(SpawnObjectsRoutine());
    }

    private IEnumerator SpawnObjectsRoutine()
    {
        yield return new WaitForSeconds(InitialDelay);

        while (true)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameActive)
            {
                SpawnObject();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }


    private void SpawnObject()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newObject = Instantiate(fallingObjectPrefab, spawnPosition, Quaternion.identity);

        Color randomColor = GetRandomColor();
        FallingObject fallingObject = newObject.GetComponent<FallingObject>();
        if (fallingObject != null)
        {
            fallingObject.ObjectColor = randomColor;
        }

        newObject.GetComponent<SliceManager>().sword = swordTransform;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = transform.position.x + Random.Range(-xSpawnRange, xSpawnRange);
        return new Vector3(randomX, transform.position.y, transform.position.z);
    }

    private Color GetRandomColor()
    {
        int randomIndex = Random.Range(0, spawnColors.Length);
        return spawnColors[randomIndex];
    }
}
