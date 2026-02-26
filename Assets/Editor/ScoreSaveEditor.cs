using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScoreSave))]
public class ScoreSaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 绘制默认 Inspector
        DrawDefaultInspector();

        // 添加一个按钮
        if (GUILayout.Button("Reset All Instances"))
        {
            ResetAllInstances();
        }
    }

    private void ResetAllInstances()
    {
        // 查找所有 MySettings 类型的资源 GUID
        string[] guids = AssetDatabase.FindAssets("t:ScoreSave");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ScoreSave instance = AssetDatabase.LoadAssetAtPath<ScoreSave>(path);
            if (instance  != null)
            {
                // 调用自定义重置方法
                instance.ResetData();
                EditorUtility.SetDirty(instance); // 标记为已修改
            }
        }

        // 保存所有修改
        AssetDatabase.SaveAssets();
        Debug.Log($"已重置 {guids.Length} 个实例");
    }
}