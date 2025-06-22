using System.Collections;
using UnityEngine;

public class AttackState : StateMain
{
    private bool attack;
    public override void EntreState(ZombieBase state)
    {
        var attack = Random.Range(0, 1);

        state.animator.CrossFade(attack == 0 ? "Attack_Right" : "Attack_Left", .2f);
        state.StartCoroutine(Attack(state, attack));

    }
    public override void UpdateState(ZombieBase state)
    {
        if (attack) return;

        //PreAttack(state);
        if (Vector3.Distance(state.target.position, state.transform.position) > state.distToAttack - .2f)
        {
            state.SwitchState(state.choseState);
        }

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
                else
                {
                    state.SwitchState(state.choseState);
                }
            }
        }
        else
        {
            state.SwitchState(state.choseState);
        }
    }

    public IEnumerator Attack(ZombieBase state, int i)
    {
        attack = true;
        
        yield return new WaitForSeconds(.3f);

        if (i == 0)
            state.colliderRight.enabled = true;
        else
            state.colliderLeft.enabled = true;

        yield return new WaitForSeconds(state.timeToAttack);

        if (i == 0)
            state.colliderRight.enabled = false;
        else
            state.colliderLeft.enabled = false;
        attack = false;
    }
}
