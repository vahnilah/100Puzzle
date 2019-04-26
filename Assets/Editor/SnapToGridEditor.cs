using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
[CustomEditor(typeof(SnapToGrid), true)]
[CanEditMultipleObjects]
public class SnapToGridEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SnapToGrid actor = target as SnapToGrid;
        if (actor.snapToGrid)
            actor.transform.position = RoundTransform(actor.transform.position, actor.snapValue);

        if (actor.sizeToGrid)
            actor.transform.localScale = RoundTransform(actor.transform.localScale, actor.sizeValue);
    }

    // The snapping code
    private Vector3 RoundTransform(Vector3 v, float snapValue)
    {
        return new Vector3
        (
            snapValue * Mathf.Round(v.x / snapValue),
            snapValue * Mathf.Round(v.y / snapValue),
            v.z
        );
    }
}