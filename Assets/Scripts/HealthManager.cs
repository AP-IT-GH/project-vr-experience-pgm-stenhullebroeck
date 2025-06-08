using UnityEngine;

public class HealthManager : MonoBehaviour
{
	[SerializeField]
	private int _health;
	public int health
	{
		set
		{
			_health = value;
		}
		get { return _health; }
	}
}
