using UnityEngine;

public abstract class StateMain
{
    public abstract void EntreState(ZombieBase state);
    public abstract void UpdateState(ZombieBase state);
    public abstract void ExitState(ZombieBase state);
    public abstract void PhysicsState(ZombieBase state); 
    public abstract void OnCollisionEnter(ZombieBase state);
}
