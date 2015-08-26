using Assets.Scripts.Base_Scripts;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts
{
    public class PatrolScript : Character
    {

        public Transform[] waypoints;
        private float patrolSpeed = 2;
        private float runSpeed = 6;
        private bool loop = true;
        private float dampingLook = 6.0f;
        private float pauseDuration = 0;
        private float rotationSpeed = 3;

        private float curTime;
        private int currentWaypoint;
        private CharacterController character;
        private Animation anim;
        private Transform player;
        
        private float chaseThreshold = 10;
        private float giveUpThreshold = 20;
        private float attackThreshold = 3f;
        private float attackRepeatTime = 3;
        public float attackTime;


        void Start ()
        {
            character = GetComponent<CharacterController>();
            anim = GetComponent<Animation>();
            if (player == null && GameObject.FindWithTag("Player"))
                player = GameObject.FindWithTag("Player").transform;
        }
	
        void Update () {
            if (currentWaypoint < waypoints.Length)
            {
                CheckForAggro();
                if (!chasing)
                    Patrol();
            }
            else
            {
                if (loop)
                {
                    currentWaypoint = 0;
                }
            }
        }

        void Patrol()
        {
            Vector3 target = waypoints[currentWaypoint].position;
            target.y = transform.position.y;
            Vector3 moveDirection = target - transform.position;

            if (moveDirection.magnitude < 0.5)
            {
                if (curTime == 0)
                    curTime = Time.time;

                if ((Time.time - curTime) >= pauseDuration)
                {
                    currentWaypoint++;
                    curTime = 0;
                }
            }
            else
            {
                var rotation = Quaternion.LookRotation(target - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingLook);
                character.Move(moveDirection.normalized*patrolSpeed*Time.deltaTime);
                anim.Play("Allosaurus_Walk");
            }
        }

        void CheckForAggro()
        {
            var distance = (player.position - transform.position).magnitude;

            if (chasing)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotationSpeed*Time.deltaTime);
                if (distance > attackThreshold)
                {
                    transform.position += transform.forward*runSpeed*Time.deltaTime;
                    anim.Play("Allosaurus_Run");
                }

                if (distance > giveUpThreshold)
                {
                    chasing = false;
                }

                if (distance < attackThreshold && Time.time > attackTime)
                {
                    anim.Play("Allosaurus_Attack01");
                    player.SendMessage("ApplyDamage", new Attack { damage = damage, attacker = this});
                    attackTime = Time.time + attackRepeatTime;
                }

                if (distance < attackThreshold)
                {
                    anim.Play("Allosaurus_IdleAggressive");
                }
            }
            else
            {
                if (distance < chaseThreshold)
                    chasing = true;
            }

        }

        public override void ApplyDamage(Attack param)
        {
            if (hitPoints <= 0.0) return;

            hitPoints -= param.damage;
            if (hitPoints <= 0.0)
                Death(param.attacker);
        }

        public override void Death(Character attacker)
        {
            
        }
    }
}
