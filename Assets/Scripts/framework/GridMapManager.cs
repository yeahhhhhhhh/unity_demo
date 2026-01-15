using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;

// 格子类型枚举
public enum TileType
{
    Grass = 0,    // 草地 - 可通行
    Water = 1,    // 湖泊 - 不可通行
    Forest = 2,   // 森林 - 减速
    Mountain = 3, // 山地 - 障碍
    Path = 4,    // 路径
    Desert = 5   // 沙漠
}

// 单个格子数据类
[System.Serializable]
public class MapTile
{
    public TileType tileType;
    public int x;
    public int y;
    public bool isWalkable;
    public float movementCost;
    public GameObject visualObject;

    public MapTile(int x, int y, TileType type)
    {
        this.x = x;
        this.y = y;
        this.tileType = type;
        UpdateTileProperties();
    }

    // 根据格子类型更新属性
    public void UpdateTileProperties()
    {
        switch (tileType)
        {
            case TileType.Grass:
                isWalkable = true;
                movementCost = 1.0f;
                break;
            case TileType.Water:
                isWalkable = false;
                movementCost = Mathf.Infinity;
                break;
            case TileType.Forest:
                isWalkable = true;
                movementCost = 1.5f;
                break;
            case TileType.Mountain:
                isWalkable = false;
                movementCost = Mathf.Infinity;
                break;
            case TileType.Path:
                isWalkable = true;
                movementCost = 0.8f;
                break;
        }
    }

    // 改变格子类型
    public void ChangeType(TileType newType)
    {
        tileType = newType;
        UpdateTileProperties();
        UpdateVisual();
    }

    // 更新视觉表现
    private void UpdateVisual()
    {
        if (visualObject != null)
        {
            // 根据类型更新材质或颜色
            Renderer renderer = visualObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = GetTileColor(tileType);
            }
        }
    }

    // 获取格子颜色
    public Color GetTileColor(TileType type)
    {
        switch (type)
        {
            case TileType.Grass: return Color.green;
            case TileType.Water: return Color.blue;
            case TileType.Forest: return new Color(0, 0.5f, 0);
            case TileType.Mountain: return Color.gray;
            case TileType.Path: return Color.yellow;
            case TileType.Desert: return new Color(1, 0.8f, 0.6f);
            default: return Color.white;
        }
    }
}

public class GridMapManager : MonoBehaviour
{
    [Header("地图设置")]
    public int mapWidth = 10;
    public int mapHeight = 10;
    public float tileSize = 1.0f;
    public Vector2 startPosition = Vector2.zero;
    public Button saveBtn;
    public Button loadBtn;
    public TMP_InputField saveID;
    public TMP_InputField loadID;
    public bool isCreator = false;
    public Int32 id = 1001;

    [Header("预制体")]
    public GameObject tilePrefab;
    public Material[] tileMaterials; // 对应不同格子类型的材质

    // 地图数据
    private MapTile[,] mapGrid;
    private Dictionary<TileType, Material> tileMaterialDict = new Dictionary<TileType, Material>();

    // 格子类型颜色映射
    private Dictionary<TileType, Color> tileColorMap = new Dictionary<TileType, Color>()
    {
        {TileType.Grass, Color.green},
        {TileType.Water, Color.blue},
        {TileType.Forest, new Color(0, 0.5f, 0)},
        {TileType.Mountain, Color.gray},
        {TileType.Path, Color.yellow},
        {TileType.Desert, new Color(1, 0.8f, 0.6f)}
    };

    void Start()
    {
        InitializeTileMaterials();
        if (isCreator)
        {
            saveBtn.onClick.AddListener(OnClickSaveMap);
            loadBtn.onClick.AddListener(OnClickLoadMap);
            LoadMapConfiguration(id);
            saveID.text = id.ToString();
            //GenerateMapFromArray(CreateSampleMapArray());
        }
        else
        {
            LoadMapConfiguration(id);
        }
    }

    // 初始化材质字典
    void InitializeTileMaterials()
    {
        if (tileMaterials != null && tileMaterials.Length >= System.Enum.GetValues(typeof(TileType)).Length)
        {
            for (int i = 0; i < System.Enum.GetValues(typeof(TileType)).Length; i++)
            {
                tileMaterialDict[(TileType)i] = tileMaterials[i];
            }
        }
    }

    // 从二维数组生成地图[4](@ref)
    public void GenerateMapFromArray(MapData mapData)
    {
        mapWidth = mapData.width;
        mapHeight = mapData.height;
        mapGrid = new MapTile[mapWidth, mapHeight];

        // 创建父物体用于组织格子
        GameObject mapParent = GameObject.Find("MapGrid");
        if (mapParent == null)
        {
            mapParent = new GameObject("MapGrid");
        }

        for (int i = mapParent.transform.childCount - 1; i >= 0; --i)
        {
            Transform child = mapParent.transform.GetChild(i);
            Destroy(child.gameObject);
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // 创建格子物体
                Vector3 tilePosition = new Vector3(
                    startPosition.x + x * tileSize,
                    0,
                    startPosition.y + y * tileSize
                );

                GameObject tileObj = Instantiate(tilePrefab, tilePosition, Quaternion.identity, mapParent.transform);
                tileObj.transform.localPosition = tilePosition;
                tileObj.name = $"Tile_{x}_{y}";

                // 创建格子数据
                TileType tileType = (TileType)mapData.GetTileType(x, y);
                MapTile tile = new MapTile(x, y, tileType);
                tile.visualObject = tileObj;

                mapGrid[x, y] = tile;

                // 设置视觉表现
                SetupTileVisual(tileObj, tileType);

                // 添加点击交互
                SetupTileInteraction(tileObj, x, y);
            }
        }
    }

    // 设置格子视觉表现[6](@ref)
    void SetupTileVisual(GameObject tileObj, TileType tileType)
    {
        Renderer renderer = tileObj.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (tileMaterialDict.ContainsKey(tileType))
            {
                renderer.material = tileMaterialDict[tileType];
            }

            renderer.material.color = tileColorMap[tileType];
        }
    }

    // 设置格子交互
    void SetupTileInteraction(GameObject tileObj, int x, int y)
    {
        // 添加碰撞体
        if (tileObj.GetComponent<BoxCollider>() == null)
        {
            tileObj.AddComponent<BoxCollider>();
        }

        // 添加点击处理
        TileInteraction interaction = tileObj.GetComponent<TileInteraction>();
        if (interaction == null)
        {
            interaction = tileObj.AddComponent<TileInteraction>();
        }
        interaction.Initialize(this, x, y);
    }

    // 创建示例地图数组
    MapData CreateSampleMapArray()
    {
        MapData mapData = new(mapWidth, mapHeight);
        //int[,] sampleMap = new int[mapWidth, mapHeight];

        // 随机生成示例地图[6](@ref)
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (x == 0 || y == 0 || x == mapWidth - 1 || y == mapHeight - 1)
                {

                    mapData.tileTypesFlat[mapData.GetFlatIndex(x, y)] = (int)TileType.Mountain; // 边界为山地
                }
                else
                {
                    // 随机生成内部格子
                    int randomValue = UnityEngine.Random.Range(0, 100);
                    if (randomValue < 60) mapData.tileTypesFlat[mapData.GetFlatIndex(x, y)] = (int)TileType.Grass;      // 60% 草地
                    else if (randomValue < 80) mapData.tileTypesFlat[mapData.GetFlatIndex(x, y)] = (int)TileType.Forest; // 20% 森林
                    else if (randomValue < 90) mapData.tileTypesFlat[mapData.GetFlatIndex(x, y)] = (int)TileType.Water; // 10% 水
                    else mapData.tileTypesFlat[mapData.GetFlatIndex(x, y)] = (int)TileType.Mountain;                     // 10% 山地
                }
            }
        }

        // 创建一条路径
        CreatePath(mapData, 1, 1, mapWidth - 2, mapHeight - 2);

        return mapData;
    }

    // 创建路径（A*算法简化版）
    void CreatePath(MapData mapData, int startX, int startY, int endX, int endY)
    {
        int currentX = startX;
        int currentY = startY;

        while (currentX != endX || currentY != endY)
        {
            mapData.tileTypesFlat[mapData.GetFlatIndex(currentX, currentY)] = (int)TileType.Path;

            if (currentX < endX) currentX++;
            else if (currentX > endX) currentX--;

            if (currentY < endY) currentY++;
            else if (currentY > endY) currentY--;
        }
        mapData.tileTypesFlat[mapData.GetFlatIndex(endX, endY)] = (int)TileType.Path;
    }

    // 获取指定位置格子
    public MapTile GetTile(int x, int y)
    {
        if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
        {
            return mapGrid[x, y];
        }
        return null;
    }

    // 改变格子类型
    public void ChangeTileType(int x, int y, TileType newType)
    {
        MapTile tile = GetTile(x, y);
        if (tile != null)
        {
            tile.ChangeType(newType);
        }
    }

    public void OnClickSaveMap()
    {
        if (saveID.text == "")
        {
            Debug.Log("saveID.text == null");
            return;
        }
        int id = System.Convert.ToInt32(saveID.text);
        SaveMapConfiguration(id);
    }

    public void OnClickLoadMap()
    {
        if (loadID.text == "")
        {
            Debug.Log("loadID.text == null");
            return;
        }
        int id = System.Convert.ToInt32(loadID.text);
        LoadMapConfiguration(id);
    }

    // 保存地图配置[3](@ref)
    public void SaveMapConfiguration(int id)
    {
        string fileName = "map_" + id.ToString();
        MapData mapData = new MapData(mapWidth, mapHeight);
        mapData.id = id;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                int type = (int)mapGrid[x, y].tileType;
                mapData.tileTypesFlat[mapData.GetFlatIndex(x, y)] = type;
                if (type == (int)TileType.Path)
                {
                    mapData.bornX = x;
                    mapData.bornY = y;
                }
            }
        }

        string jsonData = JsonUtility.ToJson(mapData);
        System.IO.File.WriteAllText(Directory.GetParent(Application.dataPath).FullName + "/GameConfig/json/map/" + fileName + ".json", jsonData);
        Debug.Log("地图配置已保存: " + fileName + " data:" + jsonData);
    }

    // 加载地图配置
    public void LoadMapConfiguration(int id)
    {
        string fileName = "map_" + id.ToString();
        string filePath = Directory.GetParent(Application.dataPath).FullName + "/GameConfig/json/map/" + fileName + ".json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            MapData mapData = JsonUtility.FromJson<MapData>(jsonData);

            // 重新生成地图
            GenerateMapFromArray(mapData);
        }
    }
}

// 地图数据存储类
[System.Serializable]
public class MapData
{
    public int id;
    public int width;
    public int height;
    public int[] tileTypesFlat; // 改为一维数组
    public int version = 1; // 添加版本控制，便于后续格式升级
    public int bornX;
    public int bornY;

    public MapData(int w, int h)
    {
        width = w;
        height = h;
        tileTypesFlat = new int[w * h]; // 初始化一维数组
    }

    // 辅助方法：将二维坐标转换为一维索引
    public int GetFlatIndex(int x, int y)
    {
        return y * width + x;
    }

    // 辅助方法：通过二维坐标访问数据
    public int GetTileType(int x, int y)
    {
        return tileTypesFlat[GetFlatIndex(x, y)];
    }

    public void SetTileType(int x, int y, int tileType)
    {
        tileTypesFlat[GetFlatIndex(x, y)] = tileType;
    }
}
