using UnityEngine;

public class MenuController : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Awake()
    {
        sceneLoader = Object.FindFirstObjectByType<SceneLoader>();
    }

    public void StartGame()
    {
        GameController.Instance.StateMachine.ChangeState(GameState.Playing);
        sceneLoader.LoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
