using UnityEngine;

public class MobAI : MonoBehaviour
{
	public GameObject Target; //������� ����

	public float mobPrice = 5.0f; //���� �� �������� ����
	public float mobMinSpeed = 0.5f; //����������� �������� ����
	public float mobMaxSpeed = 2.0f; //������������ �������� ����
	public float mobRotationSpeed = 2.5f; //�������� �������� ����
	public float attackDistance = 5.0f; //��������� �����
	public float damage = 5; //����, ��������� �����
	public float attackTimer = 0.0f; //���������� ������� �������� ����� �������
	public const float coolDown = 2.0f; //���������, ������������ ��� ������ ������� ����� � ��������� ��������

	public float MobCurrentSpeed; //�������� ����, �������������� �����
	private Transform mob; //���������� ��� ���������� ����

	private void Awake()
	{
		mob = transform; //����������� ��������� ���� � ���������� (�������� ������������������)
		MobCurrentSpeed = Random.Range(mobMinSpeed, mobMaxSpeed); //����������� ������� �������� �������� ����� ���������� � ����������� ���������
	}

	private void Update()
	{
		if (Target != null) //���� � ��� ���� ����
		{
			mob.rotation = Quaternion.Lerp(mob.rotation, Quaternion.LookRotation(new Vector3(Target.transform.position.x, 0.0f, Target.transform.position.z) - new Vector3(mob.position.x, 0.0f, mob.position.z)), mobRotationSpeed); //�������-�������, ��������� � ����� �������!
			mob.position += mob.forward * MobCurrentSpeed * Time.deltaTime; //������� � �������, ���� ������� ���
			float squaredDistance = (Target.transform.position - mob.position).sqrMagnitude; //������ ��������� �� ����
			Vector3 structDirection = (Target.transform.position - mob.position).normalized; //�������� ������ �����������
			float attackDirection = Vector3.Dot(structDirection, mob.forward); //�������� ������ �����
			if (squaredDistance < attackDistance * attackDistance && attackDirection > 0) //���� �� �� ��������� ����� � ���� ����� ����
			{
				if (attackTimer > 0) attackTimer -= Time.deltaTime; //���� ������ ����� ������ 0 - �������� ���
				if (attackTimer <= 0) //���� �� �� ���� ������ ���� ��� ����� ���
				{
					BaseHP bhp = Target.GetComponent<BaseHP>(); //������������ � ���������� �� ����
					if (bhp != null) bhp.ChangeHP(-damage); //���� ���� ��� �����, ������� ���� (�� ����� �� ���� ���� �� ����, ������ �������� ����������)
					attackTimer = coolDown; //���������� ������ � �������� ���������
					MobHP mhp = GetComponent<MobHP>();
					mhp.curHP = 0;
				}
			}
		}
		else
		{
			GameObject baza = GameObject.FindGameObjectWithTag("Base"); //������� ��� ������ � �����, �� ����� ����
			if (baza != null) Target = baza;
		}
	}
}