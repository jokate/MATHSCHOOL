using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TexDrawLib;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class MathPidAPIUse : MonoBehaviour
{
    [Header("Woongin Caller")]
    public WJ_Conn wJ_Conn;

    [Header("UI Factor")]
    public TEXDraw txQuestion;
    public GameObject MathPIDUI;
    public List<TEXDraw> txtAnswer;

    protected WJ_Conn.Learning_Data cLearning;
    protected int nLearning_Idx;    
    protected int nDigonstic_Idx;
    protected string[] strQstCransr = new string[8];       
    protected long[] nQstDelayTime = new long[8];          

    protected enum STATE
    {
        DN_SET = 0,       
        DN_PROG,        
        LEARNING,      
    }
    protected STATE eState;
    public bool bRequest;
    public int Level = 0;
    public bool isDiagnosed = false;

    public void Start()
    {
        LevelSet();
        if ((STATE)PlayerPrefs.GetInt("LearningState") == (int)STATE.DN_SET)
        {
            wJ_Conn.OnRequest_DN_Setting(Level);
            eState = (STATE)PlayerPrefs.GetInt("LearningState");
        }
        else if(PlayerPrefs.GetInt("LearningState") == (int)STATE.LEARNING)
        {
            eState = (STATE)PlayerPrefs.GetInt("LearningState");
            DoLearning(string.Empty);
            isDiagnosed = true;
        }
        nDigonstic_Idx = 0;
        nLearning_Idx = 0;
    }
    public void Update()
    {
        try
        {
            if (!string.IsNullOrEmpty(wJ_Conn.cDiagnotics.data.prgsCd))
            {
                if (wJ_Conn.cDiagnotics.data.prgsCd == "E" && isDiagnosed == false)
                {
                    nDigonstic_Idx = 0;
                    eState = STATE.LEARNING;
                    PlayerPrefs.SetInt("LearningState", (int)STATE.LEARNING);
                    isDiagnosed = true;
                    DoLearning(string.Empty);
                    Debug.Log(wJ_Conn.CheckState_Request());
                }
            }
        } catch
        {

        }
        if (bRequest == true &&
          wJ_Conn.CheckState_Request() == 1)
        {

            switch (eState)
            {
                case STATE.DN_SET:
                    {
                        MakeQuestion(wJ_Conn.cDiagnotics.data.qstCn, wJ_Conn.cDiagnotics.data.qstCransr, wJ_Conn.cDiagnotics.data.qstWransr);

                        ++nDigonstic_Idx;

                        eState = STATE.DN_PROG;
                    }
                    break;
                case STATE.DN_PROG:
                    {
                        MakeQuestion(wJ_Conn.cDiagnotics.data.qstCn, wJ_Conn.cDiagnotics.data.qstCransr, wJ_Conn.cDiagnotics.data.qstWransr);
                         ++nDigonstic_Idx;
                    }
                    break;
                case STATE.LEARNING:
                    {
                        cLearning = wJ_Conn.cLearning_Info.data;
                        MakeQuestion(cLearning.qsts[nLearning_Idx].qstCn, cLearning.qsts[nLearning_Idx].qstCransr, cLearning.qsts[nLearning_Idx].qstWransr);
                        ++nLearning_Idx;
                    }
                    break;
            }
            bRequest = false;
        }

    }


    private string Ansr;

    private void OnEnable()
    {

        
    }

    public void MakeQuestion(string _qstCn, string _qstCransr, string _qstWransr)
    {
        char[] SEP = { ',' };
        string[] tmWrAnswer;
        txQuestion.text = wJ_Conn.GetLatexCode(_qstCn);
        string strAnswer = _qstCransr;
        tmWrAnswer = _qstWransr.Split(SEP, System.StringSplitOptions.None);   // 오답 리스트
        for (int i = 0; i < tmWrAnswer.Length; ++i)
            tmWrAnswer[i] = wJ_Conn.GetLatexCode(tmWrAnswer[i]);

        int nWrCount = tmWrAnswer.Length;
        if (nWrCount >= 4)       // 5지선다형 이상은 강제로 4지선다로 변경함
            nWrCount = 3;
        int nAnsrCount = nWrCount + 1;
        int nAnsridx = UnityEngine.Random.Range(0, nAnsrCount);
        for (int i = 0, q = 0; i < nAnsrCount; ++i, ++q)
        {
            if (i == nAnsridx)
            {
                txtAnswer[i].text = strAnswer;
                --q;
            }
            else
                txtAnswer[i].text = tmWrAnswer[q];
        }
    }
    



    public void DoDN_Prog(string _qstCransr)
    {
        string strYN = "N";
        if (wJ_Conn.cDiagnotics.data.qstCransr.CompareTo(_qstCransr) == 0)
            strYN = "Y";

        Debug.Log(strYN);
        ResultProcess(strYN);
             

        wJ_Conn.OnRequest_DN_Progress("W",
                                         wJ_Conn.cDiagnotics.data.qstCd,         
                                         _qstCransr,                             
                                         strYN,                                    
                                         wJ_Conn.cDiagnotics.data.sid,       
                                         5000);                                   
    }
    public void DoLearning(string _qstCransr)
    {
        Debug.Log("Doing Learning");
        if (cLearning == null)
        {
            nLearning_Idx = 0;

            wJ_Conn.OnRequest_Learning();
        }
        else
        {
            string strYN = "N";
            if (cLearning.qsts[nLearning_Idx - 1].qstCransr.CompareTo(_qstCransr) == 0)
                strYN = "Y";
            ResultProcess(strYN);

            if (nLearning_Idx >= wJ_Conn.cLearning_Info.data.qsts.Count)
            {
                //wJ_Conn.OnLearningResult(cLearning, strQstCransr, nQstDelayTime);
                cLearning = null;
                DoLearning(string.Empty);
            }



        }
    }

    public void OnClick_Ansr(int _nindex)
    {
        switch (eState)
        {
            case STATE.DN_SET:
            case STATE.DN_PROG:
                {

                    DoDN_Prog(txtAnswer[_nindex].text);
                }
                break;
            case STATE.LEARNING:
                {

                    strQstCransr[nLearning_Idx - 1] = txtAnswer[_nindex].text;
                    nQstDelayTime[nLearning_Idx - 1] = 5000;   

                    DoLearning(txtAnswer[_nindex].text);
                }
                break;
        }
    }

    private void LevelSet()
    {
        int Age = 6;
        if (Age == 6 || Age == 7)
        {
            Level = 0;
        }
        else if (Age == 8 || Age == 9)
        {
            Level = 1;
        }
        else if (Age == 10 || Age == 11)
        {
            Level = 2;
        }
        Debug.Log("Player Level = " + Level);
    }
    private void ResultProcess(string strYN)
    {
        if(gameObject.tag == "Dokkebi")
        {
            PhotonView pb;
            if (gameObject.TryGetComponent<PhotonView>(out pb))
            {
                gameObject.GetComponent<Dokkebi>().ResultProcess(strYN);
            }
            else
            {
                LocalPlayer lc;
                if (gameObject.TryGetComponent<LocalPlayer>(out lc))
                {
                    gameObject.GetComponent<LocalPlayer>().ResultProcess(strYN);
                } else
                {
                    gameObject.GetComponent<SinglePlayer>().ResultProcess(strYN);
                }
            }
            
        }
        else if(gameObject.tag == "Teacher")
        {
            gameObject.GetComponent<Teacher>().ResultProcess(strYN);
        }
    }
}
