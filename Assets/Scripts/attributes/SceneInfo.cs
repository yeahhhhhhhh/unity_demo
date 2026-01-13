using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo
{
    public Int32 scene_id_ = 0;
    public Int32 scene_gid_ = 0;
    public Vector3 pos_ = new();
}

public class SceneConfigInfo
{
    public Int32 scene_id_;
    public Int32 width_;
    public Int32 height_;
    public Int32[,] grids_;

    //public SceneConfigInfo(Int32 scene_id, Int32 width, Int32 height, Int32[,] grids)
    //{
    //    scene_id_ = scene_id;
    //    width_ = width;
    //    height_ = height;
    //    grids_ = grids;
    //}
}