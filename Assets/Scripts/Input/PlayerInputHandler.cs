using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private CubeSpawner spawner;
    [SerializeField] private float horizontalLimit = 4f;
    [SerializeField] private float launchForce = 15f;
    [SerializeField] private float spawnDelay = 0.3f;

    private Cube activeCube;
    private bool isHolding;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        SpawnNewCube();
    }

    private void Update()
    {
        if (IsGameStopped())
            return;

        HandleInput();
    }

    private bool IsGameStopped()
    {
        if (ScoreManager.Instance != null && ScoreManager.Instance.IsGameOver())
            return true;

        if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver())
            return true;

        return false;
    }

    private void SpawnNewCube()
    {
        if (IsGameStopped()) return;
        activeCube = spawner.Spawn();
    }

    private void HandleInput()
    {
        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            isHolding = true;
        }

        if (Pointer.current.press.isPressed && isHolding)
        {
            MoveCube(Pointer.current.position.ReadValue());
        }

        if (Pointer.current.press.wasReleasedThisFrame && isHolding)
        {
            isHolding = false;
            LaunchCube();
        }
    }

    private void MoveCube(Vector2 screenPosition)
    {
        if (activeCube == null) return;

        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            float clampedX = Mathf.Clamp(worldPoint.x, -horizontalLimit, horizontalLimit);
            activeCube.MoveHorizontal(clampedX);
        }
    }

    private void LaunchCube()
    {
        if (activeCube == null) return;
        if (IsGameStopped()) return;

        activeCube.Launch(launchForce);
        activeCube = null;

        Invoke(nameof(SpawnNewCube), spawnDelay);
    }
}
