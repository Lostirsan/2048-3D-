using System;
public class GameStateMachine
{
    public GameState CurrentState { get; private set; } = GameState.Boot;
    public event Action<GameState> StateChanged;
    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;
        CurrentState = newState;
        StateChanged?.Invoke(newState);
    }
}
