using UnityEngine;

public class HealthManager : MonoBehaviour
{
	public bool TrainMode = false;
	[SerializeField]
	private int _health;
	public int health
	{
		set
		{
			_health = value;
			if (!TrainMode && value <= 0)
				GameManager.Instance.OnStopPressed();
		}
		get { return _health; }
	}
}
