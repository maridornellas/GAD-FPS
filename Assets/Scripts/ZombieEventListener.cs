using UnityEngine;
using System.Collections;

public class ZombieEventListener : MonoBehaviour {

	public void OnAttack()
	{
		SendMessageUpwards ("OnAttackEvent");
	}
}
