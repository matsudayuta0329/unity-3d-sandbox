using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshGanerator))]
public class MashGaneratorEditor: Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        // 通常通りインスペクタ描画する
        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            // インスペクタで値が変更された
            if (target is MeshGanerator meshGen)
            {
                meshGen.GanerateMesh();
            }
        }
    }
}