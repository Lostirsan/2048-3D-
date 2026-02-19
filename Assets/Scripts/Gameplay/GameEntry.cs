using UnityEngine;

public class GameEntry : MonoBehaviour
{
    [SerializeField] private CubeSpawner spawner;

    private void Start()
    {
        spawner.Spawn();
    }
}
