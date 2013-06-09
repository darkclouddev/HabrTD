using UnityEngine;

public static class myGizmos
{
	public static void DrawCircle(Vector3 pos, Vector3 normal, int segs, float radius, bool showNormal)
	{
		float stepAng = 360.0f / Mathf.Max(segs, 1.0f);
		float curAng = stepAng;
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, normal);

		while (curAng <= 360.0f)
		{
			Vector3 pPrev = rot * (Quaternion.Euler(Vector3.up * (curAng - stepAng)) * (Vector3.right * radius));
			Vector3 p = rot * (Quaternion.Euler(Vector3.up * curAng) * (Vector3.right * radius));

			Gizmos.DrawLine(pos + pPrev, pos + p);

			curAng += stepAng;
		}

		Gizmos.DrawWireSphere(pos, radius * 0.1f);

		if (showNormal) Gizmos.DrawLine(pos, pos + normal * radius);
	}

	public static void DrawArrow(Vector3 pos, Vector3 dir, Vector2 size)
	{
		Gizmos.DrawWireSphere(pos, size.x);

		Vector3 endPos = pos + dir * size.y;

		Gizmos.DrawLine(pos, endPos);

		Vector3 ax1 = Vector3.Cross(dir, Vector3.right);
		Vector3 ax2 = Vector3.Cross(dir, Vector3.forward);

		Vector3 hpB = endPos - (dir * size.y * 0.2f);
		Vector3 hp1 = hpB + ax1 * size.x;
		Vector3 hp2 = hpB - ax1 * size.x;
		Vector3 hp3 = hpB + ax2 * size.x;
		Vector3 hp4 = hpB - ax2 * size.x;

		Gizmos.DrawLine(endPos, hp1);
		Gizmos.DrawLine(endPos, hp2);
		Gizmos.DrawLine(endPos, hp3);
		Gizmos.DrawLine(endPos, hp4);

		Gizmos.DrawLine(hp1, hp4);
		Gizmos.DrawLine(hp1, hp3);
		Gizmos.DrawLine(hp2, hp4);
		Gizmos.DrawLine(hp2, hp3);
	}
}