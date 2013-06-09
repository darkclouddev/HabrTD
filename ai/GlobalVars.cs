using System.Collections.Generic;
using UnityEngine;

public static class GlobalVars
{
	public static List<GameObject> MobList = new List<GameObject>(); //������ ����� � ����
	public static int MobCount = 0; //������� ����� � ����

	public static List<GameObject> TurretList = new List<GameObject>(); //������ ����� � ����
	public static int TurretCount = 0; //������� ����� � ����

	public static float PlayerMoney = 200.0f; //��� ������ ����, ���� ���� ���������� ������ ��� ������ ������ - �� ���������� 200$, ����� ����������� �� ������

	public static ClickState mau5tate = ClickState.Default; //��������� ��������� �������

	public enum ClickState //������������ ���� ��������� �������
	{
		Default, //�������
		Placing, //������������� �����
		Selling, //������ �����
		Upgrading //�������� �����
	}
}