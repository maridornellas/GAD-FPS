using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour 
{
	public float attackRange;
	public float attackDamage;

	Animator animator;
	NavMeshAgent agent;
	GameObject target;

	enum State
	{
		Idle,
		Chase,
		Attack,
		Damage,
		Dead
	};
	State state;

	void Start()
	{
		animator = GetComponentInChildren<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update()
	{
		switch (state)
		{
		case State.Idle:
			UpdateIdle();
			break;
		case State.Chase:
			UpdateChase();
			break;
		case State.Attack:
			UpdateAttack();
			break;
		case State.Damage:
			UpdateDamage();
			break;
		case State.Dead:
			UpdateDead();
			break;
		}
 	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			target = other.gameObject;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag ("Player")) 
		{
			target = null;
		}
	}


	void UpdateIdle()
	{
		if (target != null)
		{
			state =  State.Chase;
			animator.SetBool("TargetSpotted" , true);
		}
	}
	void UpdateChase()
	{
		if (target == null)
		{
			state = State.Idle;
			animator.SetBool("TargetSpotted" , false);
		}
		else
		{
	
			float distance = Vector3.Distance (transform.position, target.transform.position);
			if (distance <= attackRange)
			{
				state = State.Attack;
				animator.SetTrigger("AttackTrigger");
				agent.Stop();
			}
			else
			{
				agent.SetDestination(target.transform.position);
			}
		}
	}
	void UpdateAttack()
	{
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		if (info.IsName("Main.Idle"))
		{
			state = State.Idle;
			animator.SetBool("TargetSpotted", false);
		}
	}
	void UpdateDamage()
	{
	}
	void UpdateDead()
	{
	}

	public void OnAttackEvent()
	{
		if (target != null) 
		{
			float distance = Vector3.Distance(transform.position, target.transform.position);
			if (distance <= attackRange)
			{
				target.SendMessage("TakeDamage", attackDamage);
				DamageHUD.Instance.OnPlayerHit();
			}
		}
	}
}