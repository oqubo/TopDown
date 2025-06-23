using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected FSMController controller;

    public virtual void OnEnterState(FSMController controller)
    {
        this.controller = controller;
    }
    public abstract void OnUpdateState();
    public abstract void OnExitState();
}
