using BeeState;
using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;



public class Bee : MonoBehaviour
{

    public enum State { Idle, Trace, Return, Attack, Patrol, Size }

    [SerializeField] public float detectRange;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float AttackRange;
    [SerializeField] public float lastAttackTime;
    [SerializeField] public Transform[] patrolPoints;

    private StateBase[] states;
    private State curState;

    private Transform player;
    private Vector3 returnPosition;
    private int patrolIndex = 0;

    private void Awake()
    {
        states = 
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Patrol] = new PatrolState(this);

    }
    private void Start()
    {
        curState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }

    private void Update()
    {
        states[(int)curState].Update();
    }


    private void OnDrawGizmos()
    {

    }
}


namespace BeeState
{
    public class IdleState : StateBase
    {
        private Bee bee;

        public IdleState(Bee bee)
        {
            this.bee = bee;
        }
    }
    public class TraceState : StateBase 
    {
        private Bee bee;

        public TraceState(Bee bee)
        {
            this.bee = bee;
        }

    }
    public class AttackState : StateBase
    {
        private Bee bee;
        public AttackState(Bee bee)
        {
            this.bee = bee;
        }
    }
    public class ReturnState : StateBase
    {
        private Bee bee;
        public ReturnState(Bee bee)
        {
            this.bee = bee;
        }
    }
    public class PatrolState : StateBase
    {
        private Bee bee;
        public PatrolState(Bee bee)
        {
            this.bee = bee;
        }
    }
}
