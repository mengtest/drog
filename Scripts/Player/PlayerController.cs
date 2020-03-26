
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 /*
public class PlayerController : MonoBehaviour {
    //获取动画控制器
    private Animator ani;
    //获取遥感脚本
    public EasyTouchMove touch;
    private float speed = 3f;

    public PlayerInf Player;
    public static int PlayerState = 1;  //

    public GameObject HP_UI;

    private GameObject buttonSkill1;
    private GameObject buttonSkill2;
    private GameObject buttonSkill3;
    private GameObject buttonSkill4;

	public GameObject SkillPoint;
    public GameObject HP_UI_Point;

	public GameObject SE_Skill2_clip0a; //技能2特效片段a
	public GameObject SE_Skill2_clip0b; //技能2特效片段b

    public GameObject  buttonPlayerInfPanel;


    public GameObject PlayerInfPanel;

    private PlayerFSM CurPlayerFSM;

    private SkillConfig CurSkillConfig;


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

 
	void Start () {
        CurSkillConfig = Resources.Load("SkillConfig") as SkillConfig;



        PlayerInfPanel = Instantiate (PlayerInfPanel, new Vector3(512,384,0), Quaternion.identity);
        PlayerInfPanel.SetActive(false);

        Player = new PlayerInf();
        Player.attack = 10;Player.name = "Alex";Player.curHP = 50;Player.maxHP = 100;
        HP_UI = Instantiate (HP_UI, HP_UI_Point.transform.position, Quaternion.identity);

        ani = GetComponent<Animator>();
        buttonSkill1 = GameObject.Find("Skill1");
        buttonSkill1.GetComponent<Button>().onClick.AddListener(PlaySkill1);
		buttonSkill2 = GameObject.Find("Skill2");
		buttonSkill2.GetComponent<Button>().onClick.AddListener(PlaySkill2);
        buttonPlayerInfPanel = GameObject.Find("buttonPlayerInfPanel");
        buttonPlayerInfPanel.GetComponent<Button>().onClick.AddListener(PlayerInfShow);

        //SKILL2
		//SE_Skill2_clip0a = Instantiate (SE_Skill2_clip0a, SkillPoint.transform.position, SkillPoint.transform.rotation);
        //SE_Skill2_clip0a.transform.parent = SkillPoint.transform;
		//SE_Skill2_clip0a.SetActive(false);
		//SE_Skill2_clip0b = Instantiate (SE_Skill2_clip0b, gameObject.transform.position, gameObject.transform.rotation);
        //SE_Skill2_clip0b.transform.parent = gameObject.transform;
		//SE_Skill2_clip0b.SetActive(false);
        Skill2ResourseTest();

	}

    void Skill2ResourseTest() {
        
        Debug.Log(CurSkillConfig.SkillDataList[0].SkillName);
        Debug.Log(CurSkillConfig.SkillDataList[0].SkillSEDates[0].SkillSEName);
        //SKILL2
        GameObject temp0 = null, temp1 = null;
        temp0 = Resources.Load(CurSkillConfig.SkillDataList[0].SkillSEDates[0].SkillSEName) as GameObject;
        temp1 = GameObject.Find(CurSkillConfig.SkillDataList[0].SkillSEDates[0].PosName) as GameObject;
        if(temp0==null) Debug.Log("资源a载入失败");
        if(temp1==null) Debug.Log("资源b载入失败");
		SE_Skill2_clip0a = Instantiate (temp0, temp1.transform.position, temp1.transform.rotation);
        SE_Skill2_clip0a.transform.parent = temp1.transform;
		SE_Skill2_clip0a.SetActive(false);

        temp0 = Resources.Load(CurSkillConfig.SkillDataList[0].SkillSEDates[1].SkillSEName) as GameObject;
        temp1 = GameObject.Find(CurSkillConfig.SkillDataList[0].SkillSEDates[1].PosName) as GameObject;
        if(temp0==null) Debug.Log("资源aa载入失败");
        if(temp1==null) Debug.Log("资源bb载入失败");
		SE_Skill2_clip0b = Instantiate (temp0, temp1.transform.position, temp1.transform.rotation);
        SE_Skill2_clip0b.transform.parent = temp1.transform;
		SE_Skill2_clip0b.SetActive(false);

		//SE_Skill2_clip0b = Instantiate (SE_Skill2_clip0b, gameObject.transform.position, gameObject.transform.rotation);
        //SE_Skill2_clip0b.transform.parent = gameObject.transform;
		//SE_Skill2_clip0b.SetActive(false);
    }
	
 
	// Update is called once per frame
	void Update () {
        PlayerInfControl();
        YaoganControl();
        //KeydownControl();
    }

    void PlayerInfShow() {
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
    void YaoganControl() {
        if(0==PlayerState) return ;
        //hor = 遥感脚本中的localPosition.x
        float hor = touch.Horizontal;
        //hor = 遥感脚本中的localPosition.y
        float ver = touch.Vertical;
 
        Vector3 direction = new Vector3(hor, 0, ver);
 
        if (direction != Vector3.zero) {
            ani.SetBool("IsRun",true);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            
            
        }else {
            //停止移动
            ani.SetBool("IsRun",false);
        }
    }
    void PlaySkill1() {
        if(PlayerState==0) return ;
        StartCoroutine ( SKILL1() );
        //PlayerState = 0;
        //ani.SetTrigger("Skill1");
    } IEnumerator SKILL1() {
        PlayerState = 0;
        ani.SetBool("Skill1",true);
        yield return new WaitForSeconds (2.767f);
        PlayerState = 1;
    }
	void PlaySkill2() {
        if(PlayerState==0) return ;
        StartCoroutine ( SKILL2() );
	} IEnumerator SKILL2() {
        PlayerState = 0;
        ani.SetTrigger("Skill2");
		SE_Skill2_clip0a.SetActive(true);
        yield return new WaitForSeconds (1.50f);
        SE_Skill2_clip0b.SetActive(true);
        yield return new WaitForSeconds (1.65f);
        PlayerState = 1;
		SE_Skill2_clip0a.SetActive(false);
        SE_Skill2_clip0b.SetActive(false);
    }

    void PlayerInfControl() {
        HP_UI.transform.position = HP_UI_Point.transform.position;
        HP_UI.transform.Find("Slider").GetComponent<Slider>().value = Player.curHP/Player.maxHP;
    }
}*/