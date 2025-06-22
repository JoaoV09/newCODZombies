using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieBase : MonoBehaviour, TakeDamager
{
    [SerializeField] private StateMain currentState;
    public ChoseState choseState = new ChoseState();
    public AttackState attackState = new AttackState();

    [Header("States")]
    [SerializeField] private int life;
    
    [Header("Moviments Settings")]
    public float currentSpeed;
    public float speedWalk = 1;
    public float speedRun = 2;

    [Space(10)]
    public float speedRotate;

    [Space(10)]
    public float distToAttack;
    public float angleToAttack;

    [Space(10)]
    public float timeToAttack;
    public Collider colliderRight;
    public Collider colliderLeft;


    [Space(10)]
    public Transform checkPosition;
    public float radius;
    public LayerMask maks;
    public Vector3 velocity;

    [Header("References")]
    public NavMeshAgent agent;
    public CharacterController cc;
    public Animator animator;
    public Transform target;
    public RuntimeAnimatorController[] controller;

    [Space(10)]
    public List<Rigidbody> rbRigdoll;
    public List<Collider> collidersRigdoll;

    public bool isGrounded;

    [Header("Event")]
    public Action OnHit; 
    public Action OnDead;
    private void Start()
    {
        animator.runtimeAnimatorController = controller[UnityEngine.Random.Range(0, controller.Length)];

        target = GameObject.FindGameObjectWithTag("Player").transform;

        foreach (var rb in rbRigdoll)
        {
            rb.isKinematic = true;
        }

        currentState = choseState;
        currentState.EntreState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }
    private void FixedUpdate()
    {
        currentState.PhysicsState(this);
    }

    public void AplayGravity()
    {
        isGrounded = CheckGround();

        if (isGrounded && velocity.y < 0)
            velocity.y = -2;

        velocity.y += -9.8f * Time.deltaTime;
        
        cc.Move(velocity * Time.fixedDeltaTime);

    }

    public void SwitchState(StateMain NextState)
    {
        currentState?.ExitState(this);
        currentState = NextState;
        NextState?.EntreState(this);

    }
    public bool CheckGround()
    {
        return Physics.CheckSphere(checkPosition.position, radius, maks);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = CheckGround() ? Color.green : Color.red;
        Gizmos.DrawSphere(checkPosition.position, radius);
    }

    public void Takedamager(int damager)
    {
        int num = UnityEngine.Random.Range(0, 2);
        animator.CrossFade(num == 0 ? "hit_1" : num == 1 ? "hit_2" : "hit_3", .2f);
        life = Mathf.Clamp(life - damager, 0, 100);
        
        if (life <= 0)
            Dei();
    }
    public void Dei()
    {
        enabled = false;
        cc.enabled = false;
        agent.enabled = false;
        animator.enabled = false;

        Destroy(gameObject, 5);
    }
}
