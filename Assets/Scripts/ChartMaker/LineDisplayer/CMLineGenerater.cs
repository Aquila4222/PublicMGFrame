using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMLineGenerater : SingletonHungry<CMLineGenerater>
{
    
    public Vector3 NumberPosition = new Vector3(-4.81f, 0.44f, -9f);
    
    /// <summary>
      /// 生成四边形模式的横线Mesh（支持线宽控制）
      /// </summary>
      /// <param name="bpm">bpm</param>
      /// <param name="beatCount">线个数</param>
      /// <param name="dividingCount"></param>
      /// <param name="lineLength"></param>
      /// <param name="lineWidth">线宽</param>
      /// <param name="position"></param>
      /// <param name="color">线颜色</param>
      /// <returns>生成的Mesh</returns>
    private Mesh CreateLineMesh(int bpm, int beatCount,int dividingCount,float lineLength, float lineWidth,Vector3 position, Color color)
    {
        Vector3 firstLineStart = new Vector3(-lineLength/2,0,0) + position;  
        // 计算线宽的一半
        float halfWidth = lineWidth * 0.5f;
        
        int totalcount = beatCount * dividingCount;
        
        // 四边形模式：每条线4个顶点，6个三角形索引（2个三角形）
        Vector3[] vertices = new Vector3[totalcount * 4];
        int[] triangles = new int[totalcount * 6];
        Color[] colors = new Color[totalcount * 4];
        
        for (int i = 0; i < totalcount; i++)
        {
            // 计算当前线的Y轴中心位置
            float centerY = firstLineStart.y + (i * 60f * CMConstDatas.DistancePerSecond / bpm / dividingCount);
            
            // 当前线的顶点起始索引
            int vertexIndex = i * 4;
            int triangleIndex = i * 6;
            
            // 四个顶点（形成四边形）
            // 顶点0：左下角
            vertices[vertexIndex] = new Vector3(
                firstLineStart.x,          // X：起始X
                centerY - halfWidth,       // Y：中心Y向下偏移半宽度
                firstLineStart.z           // Z：起始Z
            );
            
            // 顶点1：左上角
            vertices[vertexIndex + 1] = new Vector3(
                firstLineStart.x,          // X：起始X
                centerY + halfWidth,       // Y：中心Y向上偏移半宽度
                firstLineStart.z           // Z：起始Z
            );
            
            // 顶点2：右上角
            vertices[vertexIndex + 2] = new Vector3(
                firstLineStart.x + lineLength,  // X：起始X + 线长
                centerY + halfWidth,                 // Y：中心Y向上偏移半宽度
                firstLineStart.z                     // Z：起始Z
            );
            
            // 顶点3：右下角
            vertices[vertexIndex + 3] = new Vector3(
                firstLineStart.x + lineLength,  // X：起始X + 线长
                centerY - halfWidth,                 // Y：中心Y向下偏移半宽度
                firstLineStart.z                     // Z：起始Z
            );
            
            // 设置三角形（两个三角形组成四边形）
            // 三角形1：左下 → 左上 → 右上
            triangles[triangleIndex] = vertexIndex;        // 左下
            triangles[triangleIndex + 1] = vertexIndex + 1; // 左上
            triangles[triangleIndex + 2] = vertexIndex + 2; // 右上
            
            // 三角形2：左下 → 右上 → 右下
            triangles[triangleIndex + 3] = vertexIndex;    // 左下
            triangles[triangleIndex + 4] = vertexIndex + 2; // 右上
            triangles[triangleIndex + 5] = vertexIndex + 3; // 右下
            
            // 设置顶点颜色
            colors[vertexIndex] = color;      // 左下
            colors[vertexIndex + 1] = color;  // 左上
            colors[vertexIndex + 2] = color;  // 右上
            colors[vertexIndex + 3] = color;  // 右下
        }
        
        // 创建Mesh并设置数据
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        
        // 计算法线和边界（3D渲染需要）
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        // 可选：优化Mesh性能
        mesh.Optimize();
        
        return mesh;
    }
    
    /// <summary>
    /// 创建节拍线
    /// </summary>
    /// <param name="bpm">bpm</param>
    /// <param name="count">线数量</param>
    public GameObject CreateBeatLine(int bpm, int count)
    {
        float lineLength = 20f;
        float lineWidth = 0.03f;
        Vector3 position = new Vector3(0f, 0f, 5f);
        Color color = new Color(0.25f, 0.25f, 0.25f);
        
        //创建物体并且挂载组件
        GameObject beatlineObj = new GameObject("Beatline");
        MeshFilter meshFilter = beatlineObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = beatlineObj.AddComponent<MeshRenderer>();
        
        // 生成Mesh
        Mesh beatlineMesh = CreateLineMesh(bpm, count, 1,lineLength,lineWidth, position, color);
        
        //加载mesh
        meshFilter.mesh = beatlineMesh;
        
        // 加载材质
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        
        return beatlineObj;
    }
    
    /// <summary>
    /// 创建分割线
    /// </summary>
    /// <param name="bpm">bpm</param>
    /// <param name="count">节拍数</param>
    /// <returns>switcher脚本</returns>
    public CMDividingLineSwitcher CreateDividingLine(int bpm, int count)
    {
        float lineLength = 20f;
        float lineWidth = 0.02f;
        Vector3 position = new Vector3(0f, 0f, 6f);
        Color color = new Color(0.1107547f, 0.0547585f, 0.0547585f);
        
        //创建物体并且挂载组件
        GameObject dividinglineObj = new GameObject("Dividingline");
        MeshFilter meshFilter = dividinglineObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = dividinglineObj.AddComponent<MeshRenderer>();
        CMDividingLineSwitcher switcher = dividinglineObj.AddComponent<CMDividingLineSwitcher>();
        //生成mesh组
        Mesh[] dividingMeshes = new Mesh[5];
        for (int i = 0; i < 5; i++)
        {
            dividingMeshes[i] = CreateLineMesh(bpm, count, CMConstDatas.DividingCounts[i],lineLength,lineWidth, position, color);
        }
        
        switcher.meshFilter = meshFilter;
        switcher.meshRenderer = meshRenderer;
        switcher.dividingMeshes = dividingMeshes;
        
        switcher.SwitchLine(2);
        
        
        // 加载材质
        meshRenderer.material = new Material(Shader.Find("Sprites/Default"));
        
        // meshRenderer.enabled = false;
        
        return switcher;
    }

    /// <summary>
    /// 创建节拍数字
    /// </summary>
    /// <param name="bpm">bpm</param>
    /// <param name="count">节拍数</param>
    public void CreateNumber(int bpm, int count)
    {
        GameObject numberprefab = Resources.Load<GameObject>("CMPrefabs/NunberPrefab");
        for (int i = 0; i < count; i++)
        {
            GameObject number = Object.Instantiate(numberprefab);
            CMVisualNumber numberScript = number.GetComponent<CMVisualNumber>();
            numberScript.index = i;
            numberScript.Initialize();
        }
    }
}
