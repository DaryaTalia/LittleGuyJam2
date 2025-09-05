using UnityEngine;

public interface IAction
{
    void AssignUnit(Unit unit, bool player);

    void Execute();

    bool CheckCanExecute();

    public bool FromPlayer {  get; }
}
