using UnityEngine;
using UnityEngine.UI;
 
public class PlayerControllerV2 : MonoBehaviour {
    /*public EasyTouchMove touch; //遥感控件
    public GameObject ObjectPool;
    private float speed = 3f; //当前人物速度
    private float BackSpeed = 0;

    public PlayerInf Player; //角色信息
    public HeadHPUIManager HeadHPUI; //头顶血条管理器

    /// <summary>
    /// 技能按钮
    /// </summary>
    private GameObject buttonSkill1; 
    private GameObject buttonSkill2;
    private GameObject buttonSkill3;
    private GameObject buttonSkillA;
    private GameObject buttonSkillD;


    public GameObject HPUIPoint; //人物血条加载点

    public GameObject  buttonPlayerInfPanel; //人物信息面板显示按钮

    public GameObject PlayerInfPanel; //人物信息面板控件

    private PlayerFSM CurPlayerFSM;  //当前人物状态机

    private SkillConfig CurSkillConfig; //当前技能配置表


    /// <summary>
    /// 状态对象
    /// </summary>
    IdleS idle;
    RunS run;
    Skill_A_S skillA;
    Skill_1_S skill1;
    Skill_2_S skill2;
    Skill_3_S skill3;
    Skill_dodge_S skillD;

    /// <summary>
    /// 当前玩家使用技能
    /// </summary>
    SkillData Skill1;
    SkillData Skill2;
    SkillData Skill3;
    SkillData SkillA;
    SkillData SkillD;


	void Start () {
        InitOthers();
        InitCuSkillConfig(); //初始化技能配置表
        InitPlayer();        //初始化玩家信息
        InitUI();            //初始化UI信息
        InitButton();        //初始化按钮
        InitCurFSM();        //初始化玩家状态机
	}
    void InitOthers() {
        MyTimer.Init();
        ObjectPool = GameObject.Find("ObjectPool");
    }
    void InitCuSkillConfig() {
        CurSkillConfig = Resources.Load("SkillConfig") as SkillConfig;
        Debug.Log(CurSkillConfig.SkillDataList[0].SkillName);
    }
    void InitPlayer() {
        Player = new PlayerInf();
        Player.attack = 10;Player.name = "Alex";Player.curHP = 50;Player.maxHP = 100;
    }
    void InitUI() {
        PlayerInfPanel = Instantiate (PlayerInfPanel, new Vector3(512,384,0), Quaternion.identity);
        PlayerInfPanel.SetActive(false);
        HeadHPUI = new HeadHPUIManager();
        HeadHPUI.Init(transform.Find("HPUIPoint"));
    }
    void InitButton() {
        buttonSkill1 = GameObject.Find("Skill1");
        buttonSkill1.GetComponent<Button>().onClick.AddListener(PlaySkill1);
		buttonSkill2 = GameObject.Find("Skill2");
		buttonSkill2.GetComponent<Button>().onClick.AddListener(PlaySkill2);
        buttonSkill3 = GameObject.Find("Skill3");
        buttonSkill3.GetComponent<Button>().onClick.AddListener(PlaySkill3);
		buttonSkillA = GameObject.Find("SkillA");
		buttonSkillA.GetComponent<Button>().onClick.AddListener(PlaySkillA);
        buttonSkillD = GameObject.Find("SkillD");
		buttonSkillD.GetComponent<Button>().onClick.AddListener(PlaySkillD);
        buttonPlayerInfPanel = GameObject.Find("buttonPlayerInfPanel");
    }

	void InitCurFSM() {
        CurPlayerFSM = new PlayerFSM();
        SkillData temp = null;

        Skill1 = CurSkillConfig.SkillDataList[0];
        Skill2 = CurSkillConfig.SkillDataList[1];
        Skill3 = CurSkillConfig.SkillDataList[2];
        SkillA = CurSkillConfig.SkillDataList[3];
        SkillD = CurSkillConfig.SkillDataList[4];

        idle = new IdleS();
        idle.Init(gameObject,temp);
        run = new RunS();
        run.Init(gameObject,temp);
        skill1 = new Skill_1_S();
        skill1.Init(gameObject,CurSkillConfig.SkillDataList[0]);
        skill2 = new Skill_2_S();
        skill2.Init(gameObject,CurSkillConfig.SkillDataList[1]);
        skill3 = new Skill_3_S();
        skill3.Init(gameObject,CurSkillConfig.SkillDataList[2]);
        skillA = new Skill_A_S();
        skillA.Init(gameObject,CurSkillConfig.SkillDataList[3]);
        skillD = new Skill_dodge_S();
        skillD.Init(gameObject,CurSkillConfig.SkillDataList[4]);

        CurPlayerFSM.AddState(idle);
        CurPlayerFSM.AddState(run);
        CurPlayerFSM.AddState(skill1);
        CurPlayerFSM.AddState(skill2);
        CurPlayerFSM.AddState(skill3);
        CurPlayerFSM.AddState(skillA);
        CurPlayerFSM.AddState(skillD);

        CurPlayerFSM.CurState = idle;
        CurPlayerFSM.DefaultState = idle;
    }
	// Update is called once per frame
	void Update () {
        Debug.Log(gameObject.transform.position.x+"," + gameObject.transform.position.y+","+gameObject.transform.position.x);

        PlayerUIUpdate();
        YaoganControl();
        CurPlayerFSM.Update();
        OtherMove(); //TODO
        SkillCoolingTimeUpdate();


        //test
        if(Input.GetKeyDown("q")) {
            this.GetComponent<Animator>().Play("Player_warrior_skillA",0,0f);
        }
        else if(Input.GetKeyDown("w")) {
            this.GetComponent<Animator>().Play("Player_warrior_run_normal 1",0,0f);
        }
        else if(Input.GetKeyDown("e")) {
            this.GetComponent<Animator>().Play("Player_warrior_attack_feixingchongji 1",0,0f);
        }
        else if(Input.GetKeyDown("r")) {
            this.GetComponent<Animator>().Play("Player_warrior_buff",0,0f);
        }
        if(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95) {
            //this.GetComponent<Animator>().Play(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).fullPathHash,0,0f);
        }
    }

    void SkillCoolingTimeUpdate() {
        Skill1.Usetime += Time.deltaTime;
        buttonSkill1.transform.Find("mask").GetComponent<Image>().fillAmount = 1-(Skill1.Usetime>=Skill1.CoolingTime?1:Skill1.Usetime/Skill1.CoolingTime);
        Skill2.Usetime += Time.deltaTime;
        buttonSkill2.transform.Find("mask").GetComponent<Image>().fillAmount = 1-(Skill2.Usetime>=Skill2.CoolingTime?1:Skill2.Usetime/Skill2.CoolingTime);
        Skill3.Usetime += Time.deltaTime;
        buttonSkill3.transform.Find("mask").GetComponent<Image>().fillAmount = 1-(Skill3.Usetime>=Skill3.CoolingTime?1:Skill3.Usetime/Skill3.CoolingTime);
        SkillA.Usetime += Time.deltaTime;
        buttonSkillA.transform.Find("mask").GetComponent<Image>().fillAmount = 1-(SkillA.Usetime>=SkillA.CoolingTime?1:SkillA.Usetime/SkillA.CoolingTime);
        SkillD.Usetime += Time.deltaTime;
        buttonSkillD.transform.Find("mask").GetComponent<Image>().fillAmount = 1-(SkillD.Usetime>=SkillD.CoolingTime?1:SkillD.Usetime/SkillD.CoolingTime);
    }

    void OtherMove() {
        transform.Translate(Vector3.back * Time.deltaTime * BackSpeed);
    }
    void PlayerUIUpdate() {
        //Debug.Log("HPUI" + HPUI.name);
        //HPUI.transform.position = HPUIPoint.transform.position;
        //HPUI.transform.Find("Slider").GetComponent<Slider>().value = Player.curHP/Player.maxHP;
        HeadHPUI.Update(Player.curHP/Player.maxHP);
    }
    void YaoganControl() {
        //hor = 遥感脚本中的localPosition.x
        float hor = touch.Horizontal;
        //hor = 遥感脚本中的localPosition.y
        float ver = touch.Vertical;
 
        Vector3 direction = new Vector3(hor, 0, ver);
 
        if (direction != Vector3.zero) {
            //奔跑状态
            CurPlayerFSM.ChangeState(run.GetStateName());
            if(CurPlayerFSM.CurState.GetStateName() != run.GetStateName()) return ; //状态非移动态 不移动
            Move(Vector3.forward*Time.deltaTime*speed,
                Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime*10));

        }else {
            //站立状态
            CurPlayerFSM.ChangeState(idle.GetStateName());
        }
    }
    void PlaySkill1() { 
        if(Skill1.Usetime < Skill1.CoolingTime) return ;
        if(CurPlayerFSM.CurState.GetStateName() != idle.GetStateName() && 
            CurPlayerFSM.CurState.GetStateName() != run.GetStateName()) return ;
        Skill1.Usetime = 0;
        CurPlayerFSM.ChangeState(skill1.GetStateName()); UseSkill(Skill1); 
    }
    void PlaySkill2() { 
        if(Skill2.Usetime < Skill2.CoolingTime) return ;
        if(CurPlayerFSM.CurState.GetStateName() != idle.GetStateName() && 
            CurPlayerFSM.CurState.GetStateName() != run.GetStateName()) return ;
        Skill2.Usetime = 0;
        CurPlayerFSM.ChangeState(skill2.GetStateName()); UseSkill(Skill2); 
    }
    void PlaySkill3() { 
        if(Skill3.Usetime < Skill3.CoolingTime) return ;
        if(CurPlayerFSM.CurState.GetStateName() != idle.GetStateName() && 
            CurPlayerFSM.CurState.GetStateName() != run.GetStateName()) return ;
        Skill3.Usetime = 0;
        CurPlayerFSM.ChangeState(skill3.GetStateName()); 
        UseSkill(Skill3); 
    }
    void PlaySkillA() { 
        if(SkillA.Usetime < SkillA.CoolingTime) return ;
        if(CurPlayerFSM.CurState.GetStateName() != idle.GetStateName() && 
            CurPlayerFSM.CurState.GetStateName() != run.GetStateName()) return ;
        SkillA.Usetime = 0;
        CurPlayerFSM.ChangeState(skillA.GetStateName()); UseSkill(SkillA); 
    }
    void PlaySkillD() { 
        if(SkillD.Usetime < SkillD.CoolingTime) return ;
        if(CurPlayerFSM.CurState.GetStateName() != idle.GetStateName() && 
            CurPlayerFSM.CurState.GetStateName() != run.GetStateName()) return ;
        SkillD.Usetime = 0;
        CurPlayerFSM.ChangeState(skillD.GetStateName()); UseSkill(SkillD); 
    }
    void PlayAttribute() {
        if(!PlayerInfPanel.activeInHierarchy) {
            PlayerInfPanel.SetActive(true);
            PlayerInfPanel.transform.Find("Background").transform.
             Find("PlayerInfPanel").GetComponent<Text>().text = 
                "昵称   :"+Player.name+"\n"+
                "HP     :"+Player.curHP+"\n"+
                "MAXHP  :"+Player.maxHP+"\n"+
                "Attack :"+Player.attack+"\n";
        }
        else PlayerInfPanel.SetActive(false); 
    }

    void UseSkill(SkillData curSkill) {
        if(curSkill.IsBUFF) {
            if(curSkill.OnAttribute==0) {
                speed += 3f;
                MyTimer.DelayToInvokeDo(curSkill.SkillBeginTime,curSkill.EachCastTime,curSkill.CastTimes,
                    delegate() {
                        Player.curHP += (int)(curSkill.InfluenceRatio*Player.attack);
                    });
                MyTimer.DelayToInvokeDo(curSkill.SkillBeginTime+curSkill.EachCastTime*curSkill.CastTimes,0,1,
                    delegate() {
                        speed -= 3f;
                    });
            }
        }
        else if(curSkill.IsFly) {
            GameObject bullet = Resources.Load("LeiDianQiu") as GameObject;
            GameObject pos = GameObject.Find("BulletPoint") as GameObject;
            MyTimer.DelayToInvokeDo(curSkill.SkillBeginTime,0,1,
                    delegate() {
                        GameObject.Instantiate (bullet, pos.transform.position, pos.transform.rotation);
                    });
        }
        else if(curSkill.DistanceMovePlayer > 0) {
            BackSpeed = 3;
            MyTimer.DelayToInvokeDo(curSkill.SkillDuringTime,0,1,
                    delegate() {
                        BackSpeed = 0;
                    });
        }
        else {
            Transform trans = transform;
            Vector3 a = new Vector3(0,0,0);
            Vector3 b = new Vector3(0,0,0);
            a = trans.position;
            trans.Translate(Vector3.forward * curSkill.DistanceToPlayer);
            b = trans.position;
            trans.Translate(Vector3.forward * curSkill.DistanceToPlayer * -1f);

            Debug.Log("pos " + a.x + "," + a.y + "," + a.z);
            Debug.Log("pos " + b.x + "," + b.y + "," + b.z);

            ObjectPool.GetComponent<ePoolObjectManger>()
                .Attack(gameObject.transform.position, curSkill.DistanceToPlayer, Player.attack*curSkill.InfluenceRatio);
        }
    }



    /// <summary>
    /// 控制对象移动函数
    /// </summary>
    /// <param name="Forward"></param>
    /// <param name="Direction"></param>
    public void Move(Vector3 Forward, Quaternion Direction) {
        transform.Translate(Forward);
        transform.transform.rotation = Direction;
    }*/

}