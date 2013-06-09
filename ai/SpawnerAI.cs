using UnityEngine;

public class SpawnerAI : MonoBehaviour
{
	public int waveAmount = 5; //���������� ����� �� 1 ����� �� ������ ����� ������
	public int waveNumber = 0; //���������� ������� �����
	public float waveDelayTimer = 30.0F; //���������� ������� ������ �����
	public float waveCooldown = 20.0F; //���������� (�� ��������� ���!) ��� ������ ������� ����, �� � ����� ��������������
	public int maximumWaves = 500; //������������ ���������� ����� � ����
	public Transform Mob; //���������� ��� �������� ������� � Unity
	public GameObject[] SpawnPoints; //������ ����� ������

	private void Awake()
	{
		SpawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint"); //�������� ��� ����� ������ � ������
	}

	private void Update()
	{
		if (waveDelayTimer > 0) //���� �����h ������ ����� ������ ����
		{
			if (GlobalVars.MobCount == 0) waveDelayTimer = 0; //���� ����� �� ����� ��� - ������������� ��� � ����
			else waveDelayTimer -= Time.deltaTime; //����� �������� ������
		}
		if (waveDelayTimer <= 0) //���� ������ ����� ��� ����� ����
		{
			if (SpawnPoints != null && waveNumber < maximumWaves) //���� ������� ����� ������ � ��� �� ��������� ������ ���������� ����
			{
				foreach (GameObject spawnPoint in SpawnPoints) //�� ������ ����� ������
				{
					for (int i = 0; i < waveAmount; i++) //���������� i ��� ����������� ��� ������, ����� ���� �� ���� � ���� ���� � �����
					{
						Instantiate(Mob, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z + i * 10), Quaternion.identity); //������� ����
					}

					if (waveCooldown > 5.0f) //���� �������� ������ ����� 5 ������
					{
						waveCooldown -= 0.1f; //��������� �� 0.1 �������
						waveDelayTimer = waveCooldown; //����� ����� ������
					}
					else //�����
					{
						waveCooldown = 5.0f; //�������� ������� �� ����� ����� 5 ������
						waveDelayTimer = waveCooldown;
					}

					if (waveNumber >= 50) //����� 50 �����
					{
						waveAmount = 10; //����� �������� �� 10 ����� �� ������ �����
					}
				}
				waveNumber++; //����������� ����� �����
			}
		}
	}
}