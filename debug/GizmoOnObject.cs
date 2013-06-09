using UnityEngine;

public class GizmoOnObject : MonoBehaviour
{
	public Color GizmoColor = Color.yellow;
	public float GizmoRadius = 40.0f;
	public int GizmoSegments = 36;
	public bool ShowNormal;

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = GizmoColor;
		myGizmos.DrawCircle(transform.position, Vector3.up, GizmoSegments, GizmoRadius, ShowNormal);
	}
}