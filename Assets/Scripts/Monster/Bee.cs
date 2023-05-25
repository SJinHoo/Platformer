using BeeState;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Bee : MonoBehaviour
{
    private TMP_Text text;
    public float detectRange;
    public float moveSpeed;
    public float AttackRange;
    public float lastAttackTime;
    public Transform[] patrolPoints;

    private StateBase[] states;
    private State curState;
    

    public Transform player;
    public Vector3 returnPosition;
    public int patrolIndex = 0;

    private void Awake()
    {
        states = new StateBase[(int)State.Size];
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

    public void ChangeState(State state)
    {
        curState = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }


}

namespace BeeState
{
    public enum State { Idle, Trace, Return, Attack, Patrol, Size }
    public class IdleState : StateBase
    {
        private Bee bee;
        private float idleTime;

        public IdleState(Bee bee)
        {
            this.bee = bee;
        }
        public override void Update()
        {
            // Nothing Action
            idleTime += Time.deltaTime;

            if (idleTime > 2)
            {
                idleTime = 0;
                bee.patrolIndex = (bee.patrolIndex + 1) % bee.patrolPoints.Length;
                bee.ChangeState(State.Patrol);
            }


            // detectRange 안에 들어올 경우 State.Trace 상태로 변경
            // 플레이어가 가까워졌을때
            if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.detectRange)
            {
                bee.ChangeState(State.Trace);
            }
        }
    }

    public class TraceState : StateBase
    {
        private Bee bee;
        public TraceState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Update()
        {
            // Trace player
            Vector2 dir = (bee.player.position - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(bee.player.position, bee.transform.position) > bee.detectRange)
            {
                bee.ChangeState(State.Return);
            }
            // 공격 범위 안에 있으면 트레이스
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.AttackRange)
            {
                bee.ChangeState(State.Trace);
            }
        }
    }
    public class ReturnState : StateBase
    {
        private Bee bee;
        public ReturnState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Update()
        {
            // 원래 자리로 돌아가기
            Vector2 dir = (bee.returnPosition - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            // 원래 자리에 도착했으면
            if (Vector2.Distance(bee.transform.position, bee.returnPosition) < 0.02f)
            {
                bee.ChangeState(State.Idle);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.detectRange)
            {
                bee.ChangeState(State.Trace);
            }
        }
    }

    public class AttackState : StateBase
    {
        private Bee bee;
        public AttackState(Bee bee)
        {
            this.bee = bee;
        }

        public override void Update()
        {
            if (bee.lastAttackTime > 3)
            {
                Debug.Log("공격");
                bee.lastAttackTime = 0;
            }
            bee.lastAttackTime += Time.deltaTime;

            if (Vector2.Distance(bee.player.position, bee.transform.position) > bee.AttackRange)
            {
                bee.ChangeState(State.Trace);
            }
        }
    }

    public class PatrolState : StateBase
    {
        private Bee bee;
        public PatrolState(Bee bee)
        {
            this.bee = bee;
        }
        public override void Update()
        {
            Vector2 dir = (bee.patrolPoints[bee.patrolIndex].position - bee.transform.position).normalized;
            bee.transform.Translate(dir * bee.moveSpeed * Time.deltaTime);

            if (Vector2.Distance(bee.transform.position, bee.patrolPoints[bee.patrolIndex].position) < 0.02f)
            {
                bee.ChangeState(State.Idle);
            }
            else if (Vector2.Distance(bee.player.position, bee.transform.position) < bee.detectRange)
            {
                bee.ChangeState(State.Trace);
            }
        }
    }
}

