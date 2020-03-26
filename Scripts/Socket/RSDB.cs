using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//预加载资源管理类
public class RSDB
{
    /// <summary>
    /// 存放各个预制体的字典，用来实例化
    /// </summary>
    /// <typeparam name="string">预制体名称</typeparam>
    /// <typeparam name="GameObject">预制体</typeparam>
    /// <returns></returns>
    public static Dictionary<string,GameObject> DB = new Dictionary<string, GameObject>();
    /// <summary>
    /// 技能配置表
    /// </summary>
    /// <typeparam name="SkillConfig">技能配置</typeparam>
    /// <returns></returns>
    public static SkillConfig skillConfig = ScriptableObject.CreateInstance<SkillConfig>();
    /// <summary>
    /// 人物配置表
    /// </summary>
    /// <typeparam name="CreatureConfig">人物配置</typeparam>
    /// <returns></returns>
    public static CreatureConfig creatureConfig = ScriptableObject.CreateInstance<CreatureConfig>();
    /// <summary>
    /// 人物皮肤数组
    /// </summary>
    /// <typeparam name="Material">材质</typeparam>
    /// <returns></returns>
    public static List<Material> mat = new List<Material>();
    public static List<string> SEName = new List<string>();




    /// <summary>
    /// 预加载资源管理类初始化->加载资源
    /// </summary>
    public static void Init() { 
        //SEName.Add("W_A1");




        //-----------------小球测试用-------------------------------
        DB["blackS"] = Resources.Load("blackS") as GameObject;
        DB["blueS"] = Resources.Load("blueS") as GameObject;
        DB["yellowS"] = Resources.Load("yellowS") as GameObject;
        DB["redS"] = Resources.Load("redS") as GameObject;

        //战士人物
        DB["PlayerW"] = Resources.Load("PlayerW") as GameObject;
        DB["PlayerA"] = Resources.Load("Archer/PlayerA") as GameObject;

        //人物信息面板
        DB["PlayerInfPanel"] = Resources.Load("PlayerInfPanel") as GameObject;
        

        //特效
        DB["BUFF_HP"] = Resources.Load("BUFF_HP") as GameObject;
        DB["BUFF_SPEED"] = Resources.Load("BUFF_SPEED") as GameObject;
        DB["A_Block"] = Resources.Load("A_Block") as GameObject;
        DB["W_Block"] = Resources.Load("W_Block") as GameObject;
        DB["W_Block"] = Resources.Load("W_Block") as GameObject;
        //DB["W_Daoguangjianying"] = Resources.Load("W_Daoguangjianying") as GameObject;

        SEName.Clear();
        string path = "Warrior/";
        SEName.Add("W_A1");
        SEName.Add("W_A2");
        SEName.Add("W_A11");
        SEName.Add("W_A22");
        SEName.Add("W_Ah");
        SEName.Add("W_Daoguangjianying");
        SEName.Add("W_Shanguang");
        SEName.Add("W_Tiaozhanluodi");
        SEName.Add("W_Xuanfeng3");
        SEName.Add("W_Xuanfenghong");
        SEName.Add("W_Xuanfengzhan");
        SEName.Add("W_Xuanfengzhan2");
        SEName.Add("W_Xuli");
        foreach(var e in SEName) {
            if(!DB.ContainsKey(e)) {
                DB[e] = Resources.Load(path+e) as GameObject;
            }
        }


        SEName.Clear();
        path = "Archer/";
        SEName.Add("A_A");
        SEName.Add("A_AA");
        SEName.Add("A_Houtiao");
        SEName.Add("A_Huoyan");
        SEName.Add("A_Huoyanjian");
        SEName.Add("A_Lianshejian");
        SEName.Add("A_Lianshezhen");
        SEName.Add("A_Qiantiao");
        SEName.Add("A_Tifei");
        SEName.Add("A_Tifei2");
        SEName.Add("A_Hudun");
        SEName.Add("A_Suoding");
        SEName.Add("A_Mofajian");
        SEName.Add("A_Suoding2");
        SEName.Add("A_Kongduidi");
        SEName.Add("A_Kongduidi2");
        foreach(var e in SEName) {
            if(!DB.ContainsKey(e)) {
                DB[e] = Resources.Load(path+e) as GameObject;
            }
        }

        

        //子弹
        DB["LeiDianQiu"] = Resources.Load("LeiDianQiu") as GameObject;

        //皮肤
        mat.Add(Resources.Load("c") as Material);
        mat.Add(Resources.Load("a") as Material);
        mat.Add(Resources.Load("b") as Material);
        mat.Add(Resources.Load("d") as Material);
        mat.Add(Resources.Load("e") as Material);


       /* mat.Add(new Material(Shader.Find("Custom/Skin/Cube")));
        mat.Add(new Material(Shader.Find("Custom/Effect/DiffuseCapGlass")));
        mat.Add(new Material(Shader.Find("Custom/Skin/CubeNoMask")));
        mat.Add(new Material(Shader.Find("Custom/Skin/Effect")));
        mat.Add(new Material(Shader.Find("Custom/Skin/MainPlayer")));
        mat.Add(new Material(Shader.Find("Custom/Effect/DiffuseCapGlass")));*/


        //skillConfig = Resources.Load("SkillConfig") as SkillConfig;


        skillConfig.SkillDataList = new List<SkillData>();
        /*                                    //持续时间   前摇        后摇        冷却时间   伤害时间  位移速度   伤害
        skillConfig.SkillDataList.Add(new SkillData(0,"zhanli","Idle","W_Idle"                 ,999999f    ,999999f    ,999999f    ,0         ,0                  ));
        skillConfig.SkillDataList.Add(new SkillData(1,"bengpao","Run","W_Run"                  ,999999f    ,999999f    ,999999f    ,0         ,0                  ));
        skillConfig.SkillDataList.Add(new SkillData(2,"shouji","BeAttacked","W_BeAttacked"     ,0.333f     ,0.333f     ,0.333f     ,0         ,0                  ));
        skillConfig.SkillDataList.Add(new SkillData(3,"siwang","Dead","W_Dead"                 ,999999f    ,999999f    ,999999f    ,0         ,0                  ));
        skillConfig.SkillDataList.Add(new SkillData(4,"putonggongji","SkillA","W_SkillA"       ,1.333f     ,0.2f       ,0.7f       ,1.4f      ,0.23f    ,0         ,5f));
        skillConfig.SkillDataList.Add(new SkillData(5,"shanbi","SkillD","W_SkillD"             ,0.666f     ,0f         ,0.66f      ,2f        ,0        ,2f        ));*/
        skillConfig = Resources.Load("SkillConfig") as SkillConfig;

        creatureConfig.CreatureDataList = new List<CreatureData>();
        creatureConfig = Resources.Load("CreatureConfig") as CreatureConfig;

    }
}
