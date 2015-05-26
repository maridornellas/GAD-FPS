using UnityEngine;
using System.Collections;

public class Rifle : Weapon 
{
	public Transform muzzleTransform;

	public override void Update()
	{
		Transform cameraTransform = Camera.main.transform;

		var middleWidth = Screen.width * 0.5;
		var middleHeight = Screen.height * 0.5;
		var pos = new Vector3 ((float)middleWidth, (float)middleHeight);
		var ray = Camera.main.ScreenPointToRay(pos);

		Ray rayAnother = new Ray(cameraTransform.position, cameraTransform.forward);
		Debug.DrawRay (rayAnother.origin, rayAnother.direction, Color.red);
		Debug.DrawRay (transform.position, transform.forward,Color.white);
		Debug.DrawRay (ray.origin, cameraTransform.forward,Color.yellow);
	}

	public override void Fire()
	{
		Transform cameraTransform = Camera.main.transform;
		Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);

		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, range))
		{
			// It's a hit!
			Health health = hitInfo.collider.GetComponent<Health>();
			if (health != null)
			{
				Debug.Log("there is health");
				hitInfo.rigidbody.AddExplosionForce(100f, hitInfo.point, 1f);
				health.TakeDamage(damage);
				VFXManager.Instance.Spawn("BloodSplatter", hitInfo.point, Quaternion.identity);
			}
			else
			{
				Debug.Log("there is no health");
				Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);
				//VFXManager.Instance.Spawn ("Dust", hitInfo.point, rotation);
			}
		}
		Debug.DrawRay (ray.origin, ray.direction, Color.magenta, 5.0f);

		VFXManager.Instance.Spawn ("MuzzleFlare", muzzleTransform.position, muzzleTransform.rotation);
	}
}
















