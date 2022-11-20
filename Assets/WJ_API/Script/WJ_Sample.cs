using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WJ_Sample : MonoBehaviour
{
    public WJ_Conn scWJ_Conn;
    public GameObject goPopup_Level_Choice;
    public Text txQuestion;
    public Button []btAnsr = new Button[4];
    public Text txState;

    public Button btStart;

    protected Text []txAnsr;



    protected enum STATE
    {
        DN_SET,         // 진단평가 진행해야 하는 단계
        DN_PROG,        // 진단평가 진행중
        LEARNING,       // 학습 진행중
    }

    protected STATE eState;
    protected bool bRequest;

    protected int nDigonstic_Idx;   // 진단평가 인덱스

    protected WJ_Conn.Learning_Data cLearning;
    protected int nLearning_Idx;     // Learning 문제 인덱스
    protected string[] strQstCransr = new string[8];        // 사용자가 보기에서 선택한 답 내용
    protected long[] nQstDelayTime = new long[8];           // 풀이에 소요된 시간




    // Start is called before the first frame update
    void Awake()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;

        eState = STATE.DN_SET;
        goPopup_Level_Choice.active = false;

        cLearning = null;
        nLearning_Idx = 0;

        txState.text = "상태 : 진단 평가 미수행";

        txAnsr = new Text[btAnsr.Length];
        for (int i = 0; i < btAnsr.Length; ++i)
            txAnsr[i] = btAnsr[i].GetComponentInChildren<Text>();

        SetActive_Question(false);
        btStart.gameObject.active = true;

        bRequest = false;
    }



    // 문제 출제 버튼 클릭시 호출
    public void OnClick_MakeQuestion()
    {        
        switch (eState)
        {
            case STATE.DN_SET: DoDN_Start(); break;
            //호출 안됨. case STATE.DN_PROG: DoDN_Prog(); break;
            case STATE.LEARNING: DoLearning(); break;
        }        
    }




    // 학습 수준 선택 팝업에서 사용자가 수준에 맞는 학습을 선택시 호출
    public void OnClick_Level(int _nLevel)
    {
        nDigonstic_Idx = 0;
        SetActive_Question(true);
        btStart.gameObject.active = false;

        // 문제 요청
        scWJ_Conn.OnRequest_DN_Setting(_nLevel);

        // 수준 선택 팝업 닫기
        goPopup_Level_Choice.active = false;

        bRequest = true;
    }


    // 보기 선택
    public void OnClick_Ansr(int _nIndex)
    {
        switch (eState)
        {
            case STATE.DN_SET:
            case STATE.DN_PROG:
                {
                    // 다음문제 출제
                    DoDN_Prog(txAnsr[_nIndex].text);
                }
                break;
            case STATE.LEARNING:
                {
                    // 선택한 정답을 저장함
                    strQstCransr[nLearning_Idx - 1] = txAnsr[_nIndex].text;
                    nQstDelayTime[nLearning_Idx - 1] = 5000;        // 임시값
                    // 다음문제 출제
                    DoLearning();
                }
                break;
        }
    }





    protected void DoDN_Start()
    {
        goPopup_Level_Choice.active = true;
    }


    protected void DoDN_Prog(string _qstCransr)
    {
        string strYN = "N";
        if (scWJ_Conn.cDiagnotics.data.qstCransr.CompareTo(_qstCransr) == 0)
            strYN = "Y";

        scWJ_Conn.OnRequest_DN_Progress("W",
                                         scWJ_Conn.cDiagnotics.data.qstCd,          // 문제 코드
                                         _qstCransr,                                // 선택한 답내용 -> 사용자가 선택한 문항 데이터 입력
                                         strYN,                                     // 정답여부("Y"/"N")
                                         scWJ_Conn.cDiagnotics.data.sid,            // 문제 SID
                                         5000);                                     // 임시값 - 문제 풀이에 소요된 시간

        bRequest = true;
    }


    protected void DoLearning()
    {
        if (cLearning == null)
        {
            nLearning_Idx = 0;
            SetActive_Question(true);
            btStart.gameObject.active = false;

            scWJ_Conn.OnRequest_Learning();            

            bRequest = true;
        }
        else
        {            
            if (nLearning_Idx >= scWJ_Conn.cLearning_Info.data.qsts.Count)
            {
                txState.text = "상태 : 학습 완료";
                scWJ_Conn.OnLearningResult(cLearning, strQstCransr, nQstDelayTime);      // 학습 결과 처리
                cLearning = null;

                SetActive_Question(false);
                btStart.gameObject.active = true;
                return;
            }

            MakeQuestion(cLearning.qsts[nLearning_Idx].qstCn, cLearning.qsts[nLearning_Idx].qstCransr, cLearning.qsts[nLearning_Idx].qstWransr);

            txState.text = "상태 : 문제 학습 " + (nLearning_Idx + 1).ToString();


            ++nLearning_Idx;

            bRequest = false;
        }
    }





    protected void MakeQuestion(string _qstCn, string _qstCransr, string _qstWransr)
    {
        char[] SEP = { ',' };
        string[] tmWrAnswer;
        
        txQuestion.text = scWJ_Conn.GetLatexCode(_qstCn);// 문제 출력

        string strAnswer = _qstCransr;  // 문제 정답을 메모리에 넣어둠                
        tmWrAnswer = _qstWransr.Split(SEP, System.StringSplitOptions.None);   // 오답 리스트
        for(int i = 0; i < tmWrAnswer.Length; ++i)
            tmWrAnswer[i] = scWJ_Conn.GetLatexCode(tmWrAnswer[i]);



        int nWrCount = tmWrAnswer.Length;
        if (nWrCount >= 4)       // 5지선다형 이상은 강제로 4지선다로 변경함
            nWrCount = 3;


        int nAnsrCount = nWrCount + 1;       // 보기 갯수
        for (int i = 0; i < btAnsr.Length; ++i)
        {
            if (i < nAnsrCount)
                btAnsr[i].gameObject.active = true;
            else
                btAnsr[i].gameObject.active = false;
        }


        // 보기 리스트에 정답을 넣음.
        int nAnsridx = UnityEngine.Random.Range(0, nAnsrCount);        // 정답 인덱스! 랜덤으로 배치
        for (int i = 0, q = 0; i < nAnsrCount; ++i, ++q)
        {
            if (i == nAnsridx)
            {
                txAnsr[i].text = strAnswer;
                --q;
            }
            else
                txAnsr[i].text = tmWrAnswer[q];
        }


    }




    protected void SetActive_Question(bool _bActive)
    {
        txQuestion.text = "";
        for (int i = 0; i < btAnsr.Length; ++i)
            btAnsr[i].gameObject.active = _bActive;
    }


    // Update is called once per frame
    void Update()
    {
        if(bRequest == true && 
           scWJ_Conn.CheckState_Request() == 1)
        {
            switch(eState)
            {
                case STATE.DN_SET:
                    {
                        MakeQuestion(scWJ_Conn.cDiagnotics.data.qstCn, scWJ_Conn.cDiagnotics.data.qstCransr, scWJ_Conn.cDiagnotics.data.qstWransr);

                        txState.text = "상태 : 진단평가 " + (nDigonstic_Idx + 1).ToString();
                        ++nDigonstic_Idx;

                        eState = STATE.DN_PROG;
                    }
                    break;
                case STATE.DN_PROG:
                    {
                        if (scWJ_Conn.cDiagnotics.data.prgsCd == "E")
                        {
                            SetActive_Question(false);

                            nDigonstic_Idx = 0;
                            txState.text = "상태 : 진단평가 완료";
                            btStart.gameObject.active = true;
                            
                            eState = STATE.LEARNING;            // 진단 학습 완료
                        }
                        else
                        {
                            MakeQuestion(scWJ_Conn.cDiagnotics.data.qstCn, scWJ_Conn.cDiagnotics.data.qstCransr, scWJ_Conn.cDiagnotics.data.qstWransr);

                            txState.text = "상태 : 진단평가 " + (nDigonstic_Idx + 1).ToString();
                            ++nDigonstic_Idx;
                        }
                    }
                    break;
                case STATE.LEARNING:
                    {
                        cLearning = scWJ_Conn.cLearning_Info.data;
                        MakeQuestion(cLearning.qsts[nLearning_Idx].qstCn, cLearning.qsts[nLearning_Idx].qstCransr, cLearning.qsts[nLearning_Idx].qstWransr);
                        txState.text = "상태 : 문제 학습 " + (nLearning_Idx + 1).ToString();

                        ++nLearning_Idx;                        
                    }
                    break;
            }
            bRequest = false;
        }
        
    }
}
