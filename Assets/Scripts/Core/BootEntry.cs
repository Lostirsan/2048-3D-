using UnityEngine;

public class BootEntry : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

    private void Start()
    {
        GameController.Instance.StateMachine.ChangeState(GameState.MainMenu);
        sceneLoader.LoadMainMenu();
    }
}
