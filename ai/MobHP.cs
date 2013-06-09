using UnityEngine;

public class MobHP : MonoBehaviour
{
	public float maxHP = 100; //�������� ��
	public float curHP = 100; //������� ��
	public Color MaxDamageColor = Color.red; //����� ��������� ��������
	public Color MinDamageColor = Color.blue; //� ������ ����
	public GameObject explosionPrefab;

	private void Awake()
	{
		GlobalVars.MobList.Add(gameObject); //��������� ���� � ����� ���� �����
		GlobalVars.MobCount++; //����������� ������� �����
		if (maxHP < 1) maxHP = 1; //���� ������������ �� ������ ����� ������� - ������ �������
	}

	public void ChangeHP(float adjust) //����� ������������� �� ����
	{
		if ((curHP + adjust) > maxHP) curHP = maxHP;//���� ����� �������� �� � adjust � ���������� �����, ��� ������������ �� - ������� �� ���������� ������ �������������
		else curHP += adjust; //����� ������ ��������� adjust
	}

	private void Update()
	{
		gameObject.renderer.material.color = Color.Lerp(MaxDamageColor, MinDamageColor, curHP / maxHP); //������ ���� ���� �� �������� � ������ ������. � �������: ������� - ��� ����� ��������� ����, ����� - �����.
		if (curHP <= 0) //���� �� ����� � ���� ��� ����
		{
			MobAI mai = gameObject.GetComponent<MobAI>(); //������������ � ���������� AI ����
			if (mai != null)
			{
				GlobalVars.PlayerMoney += mai.mobPrice; //���� �� ���������� - ��������� ����� ������ � ������� ���� �� ������ ����
			}

			if (explosionPrefab != null)
			{
				Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
			}

			Destroy(gameObject); //������� ����
		}
	}

	private void OnDestroy() //��� ��������
	{
		GlobalVars.MobList.Remove(gameObject); //������� ���� �� ����������� ������ �����
		GlobalVars.MobCount--; //��������� ���������� ������� ����� �� 1
	}
}