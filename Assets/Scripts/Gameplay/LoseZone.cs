using UnityEngine;

public class LoseZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        if (cube == null) return;

        GameOverManager.Instance.CubeEnteredZone(cube);
    }

    private void OnTriggerExit(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        if (cube == null) return;

        GameOverManager.Instance.CubeExitedZone(cube);
    }
}
