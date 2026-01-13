// 格子交互组件
using UnityEngine;

public class TileInteraction : MonoBehaviour
{
    private GridMapManager mapManager;
    private int tileX, tileY;

    public void Initialize(GridMapManager manager, int x, int y)
    {
        mapManager = manager;
        tileX = x;
        tileY = y;
    }

    void OnMouseDown()
    {
        if (mapManager != null)
        {
            // 循环切换格子类型
            MapTile tile = mapManager.GetTile(tileX, tileY);
            if (tile != null)
            {
                TileType currentType = tile.tileType;
                TileType nextType = (TileType)(((int)currentType + 1) % System.Enum.GetValues(typeof(TileType)).Length);
                mapManager.ChangeTileType(tileX, tileY, nextType);
            }
        }
    }

    void OnMouseEnter()
    {
        // 鼠标悬停高亮
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
    }

    void OnMouseExit()
    {
        // 恢复原色
        MapTile tile = mapManager.GetTile(tileX, tileY);
        if (tile != null && GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().material.color = tile.GetTileColor(tile.tileType);
        }
    }
}