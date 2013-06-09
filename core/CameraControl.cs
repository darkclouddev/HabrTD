using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public float CameraSpeed = 100.0f; //�������� �������� ������
	public float CameraSpeedBoostMultiplier = 2.0f; //��������� ��������� �������� ������ ��� ������� Shift

	//����� ������� �� ��������� ��� ������, ����� ������� ��� - ������� ��� ���� ;)
	public float DefaultCameraPosX = 888.0f;
	public float DefaultCameraPosY = 50.0f;
	public float DefaultCameraPosZ = 1414.0f;

	private void Awake()
	{
		//����� ������� �� ��������� ��� ������, ��������� ����� ��������� ����������
		transform.position = new Vector3(DefaultCameraPosX, DefaultCameraPosY, DefaultCameraPosZ);
	}

	private void Update()
	{
		float smoothCamSpeed = CameraSpeed * Time.smoothDeltaTime; //������ �������� ����������� ������ �� ���������� ������ Time.deltaTime

		//��� ������� �����-���� �� ������ �� WASD ���������� ����������� � ��������������� �������, ������� ����� ���� ������ ����� �������������� (WA ����� ������� ������ ����� � �����), ������� Shift ��� ���� �������� ������������.
		if (Input.GetKey(KeyCode.W)) transform.position += Input.GetKey(KeyCode.LeftShift) ? new Vector3(0.0f, 0.0f, smoothCamSpeed * CameraSpeedBoostMultiplier) : new Vector3(0.0f, 0.0f, smoothCamSpeed); //�����
		if (Input.GetKey(KeyCode.A)) transform.position += Input.GetKey(KeyCode.LeftShift) ? new Vector3(-smoothCamSpeed * CameraSpeedBoostMultiplier, 0.0f, 0.0f) : new Vector3(-smoothCamSpeed, 0.0f, 0.0f); //������
		if (Input.GetKey(KeyCode.S)) transform.position += Input.GetKey(KeyCode.LeftShift) ? new Vector3(0.0f, 0.0f, -smoothCamSpeed * CameraSpeedBoostMultiplier) : new Vector3(0.0f, 0.0f, -smoothCamSpeed); //����
		if (Input.GetKey(KeyCode.D)) transform.position += Input.GetKey(KeyCode.LeftShift) ? new Vector3(smoothCamSpeed * CameraSpeedBoostMultiplier, 0.0f, 0.0f) : new Vector3(smoothCamSpeed, 0.0f, 0.0f); //�������
	}
}