using UnityEngine;


public class Utils : MonoBehaviour
{
    public static string yipeeString = "<size=20><color=red>Y</color><color=orange>I</color><color=yellow>P</color><color=lime>E</color><color=cyan>E</color> <color=purple>!</color></size>";
    public static string warningString = "<size=20><color=red>WARNING!</color></size>";
    public static void BigText(string str, string col = "white", int size = 20) => Debug.Log($"<size={size}><color={col}>{str}</color></size>");

    public static void DrawRect(Vector2 center, Vector2 size) =>
        Gizmos.DrawWireCube(new Vector3(center.x, center.y, 0f), new Vector3(size.x, size.y, 0f));
    public static void DrawCross(Vector2 pos, float size = 0.5f, float duration = 4f)
    {
        Debug.DrawLine(pos + (Vector2.up + Vector2.left) * size, pos + (Vector2.down + Vector2.right) * size, Gizmos.color, duration);
        Debug.DrawLine(pos + (Vector2.left + Vector2.down) * size, pos + (Vector2.right + Vector2.up) * size, Gizmos.color, duration);
    }
}
