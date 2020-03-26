using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{
    public static EasyTouchMove touch; //遥感控件
    public static EasyTouchMove touchR; //右摇杆
    private static GameObject buttonSkill1;
    private static GameObject buttonSkill2;
    private static GameObject buttonSkill3;
    private static GameObject buttonSkill4;
    private static GameObject buttonSkillA;
    private static GameObject buttonSkillD;
    private static GameObject buttonSkillB;
    private static GameObject buttonPlayerInfPanel;
    private static GameObject PlayerInfPanel;
    private static FollowPlayer follow;
    public static void Init()
    {
        follow = GameObject.Find("Main Camera").GetComponent<FollowPlayer>();
        //----------初始化遥感-----------------------------------------------------------------------
        touch  = GameObject.Find("touch" ).GetComponent<EasyTouchMove>();

        //----------初始化生物信息-------------------------------------------------------------------
        //InitCreature(0);
        //-----------加载人物面板控件--------------------------------------------------------------------
        PlayerInfPanel = GameObject.Instantiate(RSDB.DB["PlayerInfPanel"], new Vector3(512,384,0), Quaternion.identity);
        PlayerInfPanel.SetActive(false);
        //----------加载按钮-----------------------------------------------------------------------
        buttonSkillA = GameObject.Find("SkillA");
		buttonSkillA.GetComponent<Button>().onClick.AddListener(PlaySkillA);
        buttonSkillA.GetComponent<Image>().sprite = Logic.obinfmp[Client.name].curSkillDataList[5].SkillIcon;
        buttonSkillD = GameObject.Find("SkillD");
		buttonSkillD.GetComponent<Button>().onClick.AddListener(PlaySkillD);
        buttonSkillD.GetComponent<Image>().sprite = Logic.obinfmp[Client.name].curSkillDataList[6].SkillIcon;
        buttonSkillB = GameObject.Find("SkillB");
        buttonSkillB.GetComponent<Button>().onClick.AddListener(PlaySkillB);
        buttonSkillB.GetComponent<Image>().sprite = Logic.obinfmp[Client.name].curSkillDataList[7].SkillIcon;
        buttonSkill1 = GameObject.Find("Skill1");
        buttonSkill1.GetComponent<Button>().onClick.AddListener(PlaySkill1);
        buttonSkill1.GetComponent<Image>().sprite = Logic.obinfmp[Client.name].curSkillDataList[8].SkillIcon;
		buttonSkill2 = GameObject.Find("Skill2");
		buttonSkill2.GetComponent<Button>().onClick.AddListener(PlaySkill2);
        buttonSkill2.GetComponent<Image>().sprite = Logic.obinfmp[Client.name].curSkillDataList[9].SkillIcon;
        buttonSkill3 = GameObject.Find("Skill3");
        buttonSkill3.GetComponent<Button>().onClick.AddListener(PlaySkill3);
        buttonSkill3.GetComponent<Image>().sprite = Logic.obinfmp[Client.name].curSkillDataList[10].SkillIcon;
        buttonSkill4 = GameObject.Find("Skill4");
        buttonSkill4.GetComponent<Button>().onClick.AddListener(PlaySkill4);
        buttonSkill4.GetComponent<Image>().sprite = Logic.obinfmp[Client.name].curSkillDataList[11].SkillIcon;

        buttonPlayerInfPanel = GameObject.Find("buttonPlayerInfPanel");
        buttonPlayerInfPanel.GetComponent<Button>().onClick.AddListener(ShowPlayerInfPanel);
    }

    public static void Update()
    {
        if(Client.name=="" || !Logic.obinfmp.ContainsKey(Client.name)) return ;
        //Debug.Log("&*(*(^*(^*(^*(^ "+Client.name);
        UpdateYaogan();
        UpdateButton();
        UpdateInfPanel();
    }
    static void UpdateButton() {
        var p = Logic.obinfmp[Client.name];
        buttonSkillA.transform.Find("mask").GetComponent<Image>().fillAmount = p.GetSkillRemainingCoolingTime(5)/p.curSkillDataList[5].CoolingTime;
        buttonSkillD.transform.Find("mask").GetComponent<Image>().fillAmount = p.GetSkillRemainingCoolingTime(6)/p.curSkillDataList[6].CoolingTime;
        buttonSkillB.transform.Find("mask").GetComponent<Image>().fillAmount = p.GetSkillRemainingCoolingTime(7)/p.curSkillDataList[7].CoolingTime;
        buttonSkill1.transform.Find("mask").GetComponent<Image>().fillAmount = p.GetSkillRemainingCoolingTime(8)/p.curSkillDataList[8].CoolingTime;
        buttonSkill2.transform.Find("mask").GetComponent<Image>().fillAmount = p.GetSkillRemainingCoolingTime(9)/p.curSkillDataList[9].CoolingTime;
        buttonSkill3.transform.Find("mask").GetComponent<Image>().fillAmount = p.GetSkillRemainingCoolingTime(10)/p.curSkillDataList[10].CoolingTime;
        buttonSkill4.transform.Find("mask").GetComponent<Image>().fillAmount = p.GetSkillRemainingCoolingTime(11)/p.curSkillDataList[11].CoolingTime;
    }
    
    static float pret = 0;
    static void UpdateYaogan() {
        //hor = 遥感脚本中的localPosition.x//
        float hor = touch.Horizontal;
        //hor = 遥感脚本中的localPosition.y
        float ver = touch.Vertical;
 
        Vector3 direction = new Vector3(hor, 0, ver);

        FramOpt opt = new FramOpt();
        opt.Doplayername = Client.name;
        if (direction != Vector3.zero) {
            opt.Optype = Client.MOVETYPE;
            var Direction = Quaternion.LookRotation(direction);
            opt.Dir = ((int)((Direction.eulerAngles.y+7.5f)/15.0f) + follow.GetCameraDir())%24;
            if(Time.time-pret>=0.01f) {
                Client.Send(Client.FRAMOPTYPE,opt);
                pret = Time.time;
            }
            //Debug.Log("direction :::: " + Direction);
            //Debug.Log("direction y : " + Direction.eulerAngles.y + " dir " + opt.Dir);
        } else {
            //站立状态
            opt.Optype = Client.SKILLTYPE;
            opt.Skillid = 0;
            if(Time.time - pret>=0.1f) {
                Client.Send(Client.FRAMOPTYPE,opt);
                pret = Time.time;
            }
        }

        if(Input.GetKeyDown("x")||Input.GetKey("x")) {
            PlaySkillA();
        } else if(Input.GetKeyDown("c")||Input.GetKey("c")) {
            /*opt.Optype = Client.SKILLTYPE;
            opt.Skillid = 5;
            Client.Send(Client.FRAMOPTYPE,opt);
            pret = Time.time;*/
            PlaySkillD();
        } else if(Input.GetKeyDown("a")||Input.GetKey("a")) {
            PlaySkill1();
        } else if(Input.GetKeyDown("s")||Input.GetKey("s")) {
            PlaySkill2();
        } else if(Input.GetKeyDown("d")||Input.GetKey("d")) {
            PlaySkill3();
        } else if(Input.GetKeyDown("p")) {
            PlaySkillBeAttacked();
        } else if(Input.GetKeyDown(KeyCode.Space)||Input.GetKey(KeyCode.Space)) {
            PlaySkillB();
        } else if(Input.GetKeyDown("o")) {
            var optt = new FramOpt();
            optt.Optype = Client.SKILLTYPE;
            optt.Doplayername = "shabao";
            optt.Skillid = 0;
            optt.Id = 2;
            Client.Send(Client.FRAMOPTYPE,optt);
            pret = Time.time;
        } else if(Input.GetKeyDown("f")||Input.GetKey("f")) {
            PlaySkill4();
        }

    }
    static void PlaySkillA() {
        SendSkillOpt(5);
    }
    static void PlaySkillD() {
        SendSkillOpt(6);
    }
    static void PlaySkillB() {
        SendSkillOpt(7);
    }
    static void PlaySkill1() {
        SendSkillOpt(8);
    }
    static void PlaySkill2() {
        SendSkillOpt(9);
    }
    static void PlaySkill3() {
        SendSkillOpt(10);
    }
    static void PlaySkill4() {
        SendSkillOpt(11);
    }
    static void PlaySkillBeAttacked() {
        Logic.obinfmp[Client.name].ToBeAttacked(3,Client.name,new AttackAdditionalFeatures());
    }

    static void SendSkillOpt(int id) {
        var opt = new FramOpt();
        opt.Optype = Client.SKILLTYPE;
        opt.Doplayername = Client.name;
        opt.Skillid = id;
        Client.Send(Client.FRAMOPTYPE,opt);
        pret = Time.time;
    }

    static void ShowPlayerInfPanel() {
        //Debug.Log("SHOW INFPANEL " + PlayerInfPanel.activeInHierarchy);
        if(!PlayerInfPanel.activeInHierarchy) {
            PlayerInfPanel.SetActive(true);
        }
        else PlayerInfPanel.SetActive(false); 
    }
    static void UpdateInfPanel() {
        var curLivingCreatureData = Logic.obinfmp[Client.name].curCreatureData;
        PlayerInfPanel.transform.Find("Background").transform.
            Find("PlayerInfPanel").GetComponent<Text>().text = 
                "昵称   :"+curLivingCreatureData.Name+"\n"+
                "HP     :"+curLivingCreatureData.curHP+"/"+curLivingCreatureData.maxHP+"\n"+
                "MP     :"+curLivingCreatureData.curMP+"/"+curLivingCreatureData.maxMP+"\n"+
                "Attack :"+curLivingCreatureData.Attack+"\n"+
                "Defence:"+curLivingCreatureData.Defence+"\n"+
                "Speed  :"+curLivingCreatureData.Speed;
    }
        //Debug.Log("DEBUG NOWSTATE  " + curFSM.CurState.GetStateName());
}
