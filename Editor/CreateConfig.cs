using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CreateConfig
{
    [MenuItem("Tools/CreateSkillConfig")]
    private static void CreateS()
    {
        CreateSkillConfig();
    }

    private static void CreateSkillConfig()
    {
        SkillConfig skillConfig = ScriptableObject.CreateInstance<SkillConfig>();

        //填充数据, 可以从外部有策划配置好的配置表（如CSV、XML、JSON甚至是二进制文件）中通过通用代码读取所有数据来进行填充
        //这里只是测试就直接手写了(⊙﹏⊙)b


        //填充好数据后就可以打包到 AssetBundle 中了
        //第一步必须先创建一个保存了配置数据的 Asset 文件, 后缀必须为 asset
        AssetDatabase.CreateAsset(skillConfig, "Assets/SkillConfig.asset");

        //第二步就可以使用 BuildPipeline 打包了
        /*BuildAssetBundle(null, new[]
            {
                AssetDatabase.LoadAssetAtPath("Assets/ShopConfig.asset", typeof(SkillConfig))
            },
            Application.streamingAssetsPath + "/Config.assetbundle",
            BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.UncompressedAssetBundle,
            BuildTarget.StandaloneWindows64);*/
    }




    [MenuItem("Tools/CreateCreatureConfig")]
    private static void CreateC()
    {
        CreateCreatureConfig();
    }

    private static void CreateCreatureConfig()
    {
        CreatureConfig creatureConfig = ScriptableObject.CreateInstance<CreatureConfig>();
        AssetDatabase.CreateAsset(creatureConfig, "Assets/CreatureConfig.asset");
    }
}
