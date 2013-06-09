using UnityEngine;

public class TurretHP : MonoBehaviour
{
	public float maxHP = 100; //�������� ��
	public float curHP = 100; //������� ��

	private void Awake()
	{
		GlobalVars.TurretList.Add(gameObject);
		GlobalVars.TurretCount++;
		if (maxHP < 1) maxHP = 1;
	}

	public void ChangeHP(float adjust)
	{
		if ((curHP + adjust) > maxHP) curHP = maxHP;
		else curHP += adjust;
		if (curHP > maxHP) curHP = maxHP;
	}

	private void Update()
	{
		if (curHP <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		GlobalVars.TurretList.Remove(gameObject);
		GlobalVars.TurretCount--;
	}
}