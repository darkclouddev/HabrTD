using UnityEngine;

public class PlasmaTurretAI : MonoBehaviour
{
	public GameObject curTarget;
	public float towerPrice = 100.0f;
	public float attackMaximumDistance = 50.0f; //��������� �����
	public float attackMinimumDistance = 5.0f;
	public float attackDamage = 10.0f; //����
	public float reloadTimer = 2.5f; //�������� ����� ����������, ���������� ��������
	public float reloadCooldown = 2.5f; //�������� ����� ����������, ���������
	public float rotationSpeed = 1.5f; //��������� �������� �������� �����
	public int FiringOrder = 1; //����������� �������� ��� ������� (� ��� �� �� 2)
	public int upgradeLevel = 0;
	public int ammoAmount = 64;
	public int ammoAmountConst = 64;
	public float ammoReloadTimer = 5.0f;
	public float ammoReloadConst = 5.0f;
	public LayerMask turretLayerMask;

	public Transform turretHead;

	//���������� ���� ����� ��� �������������
	private void Start()
	{
		turretHead = transform.Find("pushka"); //������� ����� � �������� ������ ������
	}

	//� ���� ����� ���������� ������ �����
	private void Update()
	{
		if (curTarget != null) //���� ���������� ������� ���� �� ������
		{
			float squaredDistance = (turretHead.position - curTarget.transform.position).sqrMagnitude; //������ ��������� �� ���
			if (Mathf.Pow(attackMinimumDistance, 2) < squaredDistance && squaredDistance < Mathf.Pow(attackMaximumDistance, 2)) //���� ��������� ������ ������� ���� � ������ ��������� ��������� �����
			{
				turretHead.rotation = Quaternion.Lerp(turretHead.rotation, Quaternion.LookRotation(curTarget.transform.position - turretHead.position), rotationSpeed * Time.deltaTime); //������� ����� � ������� ����
				if (reloadTimer > 0) reloadTimer -= Time.deltaTime; //���� ������ ����������� ������ ���� - �������� ���
				if (reloadTimer <= 0)
				{
					if (ammoAmount > 0) //���� ���� ����� � ������������
					{
						MobHP mhp = curTarget.GetComponent<MobHP>();
						switch (FiringOrder) //�������, �� ������ ������ ��������
						{
							case 1:
								if (mhp != null) mhp.ChangeHP(-attackDamage); //������� ���� ����
								FiringOrder++; //����������� �����
								ammoAmount--; //����� ������
								break;
							case 2:
								if (mhp != null) mhp.ChangeHP(-attackDamage);
								FiringOrder = 1;
								ammoAmount--;
								break;
						}
						reloadTimer = reloadCooldown; //���������� ���������� ������� ����������� � �������������� �������� �� "���������"
					}
					else
					{
						if (ammoReloadTimer > 0) ammoReloadTimer -= Time.deltaTime;
						if (ammoReloadTimer <= 0)
						{
							ammoAmount = ammoAmountConst;
							ammoReloadTimer = ammoReloadConst;
						}
					}
				}
				if (squaredDistance < Mathf.Pow(attackMinimumDistance, 2)) curTarget = null;//���������� � ������� ������� ����, ���� ��� ��� ������� �����
			}
		}
		else
		{
			curTarget = SortTargets(); //��������� ���� � �������� �����
		}
	}

	//���������������� �������� ������ ��������� ����
	private GameObject SortTargets()
	{
		float closestMobSquaredDistance = 0; //���������� ��� �������� �������� ���������� ���������� ����
		GameObject nearestmob = null; //������������� ���������� ���������� ����
		Collider[] mobColliders = Physics.OverlapSphere(transform.position, attackMaximumDistance, turretLayerMask.value); //������� ���������� ���� ����� � ������� ������������ ��������� ����� � ������ ������ ��� ����������

		foreach (var mobCollider in mobColliders) //��� ������� ���������� � �������
		{
			float distance = (mobCollider.transform.position - turretHead.position).sqrMagnitude;
			//���� ��������� �� ���� ������, ��� closestMobDistance ��� ����� ����
			if (distance < closestMobSquaredDistance && (distance > Mathf.Pow(attackMinimumDistance, 2)) || closestMobSquaredDistance == 0)
			{
				closestMobSquaredDistance = distance; //���������� � � ����������
				nearestmob = mobCollider.gameObject;//������������� ���� ��� ����������
			}
		}
		return nearestmob; // � ���������� ���
	}

	private void OnGUI()
	{
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position); //������� ������� ������� �� ������ ������������ ����
		Vector3 cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position); //�������� ��������� ������� �� ������
		if (cameraRelative.z > 0) //���� ������ ��������� ������� ������
		{
			string ammoString;
			if (ammoAmount > 0)
			{
				ammoString = ammoAmount + "/" + ammoAmountConst;
			}
			else
			{
				ammoString = "Reloading: " + (int)ammoReloadTimer + " s left";
			}
			GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, 250f, 20f), ammoString);
		}
	}
}