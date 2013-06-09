using UnityEngine;

public class BaseHP : MonoBehaviour
{
	public float maxHP = 1000;
	public float curHP = 1000;
	public const float regenerationDelayConstant = 2.5f; //��������� �������� ����� ������������ �� ����
	public float regenarationDelayVariable = 2.5f; //���������� ��� �� ��������
	public float regenerationAmount = 10.0f; //���������� ������������������ �� ��� ����������� �� ���

	private void Awake()
	{
		if (maxHP < 1) maxHP = 1;
	}

	public void ChangeHP(float adjust)
	{
		if ((curHP + adjust) > maxHP) curHP = maxHP;
		else curHP += adjust;
		if (curHP > maxHP) curHP = maxHP; //just in case
	}

	private void Update()
	{
		if (curHP <= 0)
		{
			Destroy(gameObject);
		}
		else
		{
			if (regenarationDelayVariable > 0) regenarationDelayVariable -= Time.deltaTime; //���� ���������� �������� ����� ���� - �������� �� �� ������� � �������
			if (regenarationDelayVariable <= 0) //���� ��� ����� ������ ��� ����� ����
			{
				ChangeHP(regenerationAmount); //��������������� ����� ��������� ���������� ��
				regenarationDelayVariable = regenerationDelayConstant; //� ���������� ���� ���������� � � �������������� ��������
			}
		}
	}

	private void OnGUI()
	{
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position); //������� ������� ������� �� ������ ������������ ����
		Vector3 cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position); //�������� ��������� ������� �� ������
		if (cameraRelative.z > 0) //���� ������ ��������� ������� ������
		{
			//���������� ���������� ��� HP
			if (curHP > 0) GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, 200f, 20f), curHP.ToString());
		}
	}
}