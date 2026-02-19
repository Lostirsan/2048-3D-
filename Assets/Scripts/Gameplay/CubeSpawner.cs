using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube cubePrefab;
    [SerializeField] private Transform spawnPoint;

    public Cube Spawn()
    {
        int value = Random.value < 0.75f ? 2 : 4;

        Cube cube = Instantiate(cubePrefab, spawnPoint.position, Quaternion.identity);
        cube.Initialize(value);

        return cube;
    }
}
