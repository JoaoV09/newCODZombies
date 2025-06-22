using UnityEngine;

public class ChoseState : StateMain
{
    public override void EntreState(ZombieBase state)
    {
        state.agent.updatePosition = false;
        state.agent.updateRotation = false;
    }
    public override void UpdateState(ZombieBase state)
    {
        state.AplayGravity();
        Moviment(state);
        PreAttack(state);
        
    }

    public override void ExitState(ZombieBase state)
    {
       
    }

    public override void OnCollisionEnter(ZombieBase state)
    {
       
    }

    public override void PhysicsState(ZombieBase state)
    {
    }

    public void Moviment(ZombieBase state)
    {
        var target = state.target == null ? state.transform.position + new Vector3(Random.Range(1, 5), 0, Random.Range(1, 5)) : state.target.position;

        state.animator.SetFloat("Speed", state.currentSpeed, .2f, Time.deltaTime);

        if (state.agent.isOnNavMesh)
        {
            state.agent.nextPosition = state.transform.position;
        }

        state.currentSpeed = state.speedWalk;

        var mov = (state.agent.steeringTarget - state.transform.position).normalized;

        mov.y = 0f;
        //state.cc.Move(mov * state.currentSpeed * Time.deltaTime);
        state.transform.rotation = Quaternion.Lerp(state.transform.rotation, Quaternion.LookRotation(mov), state.speedRotate * Time.deltaTime);

        state.agent.SetDestination(target);
    }
    public void PreAttack(ZombieBase state)
    {
        var pos = state.target.position - state.transform.position;
        var dist = Vector3.Distance(state.target.position, state.transform.position);
        var angle = Vector3.Angle(state.transform.forward, pos);

        if (dist <= state.distToAttack && angle <= state.angleToAttack)
        {
            if (Physics.Raycast(state.transform.position, pos, out RaycastHit hit))
            {
                if (hit.collider.tag == "Player")
                {
                    state.SwitchState(state.attackState);
                }
            }
        }
    }
}
