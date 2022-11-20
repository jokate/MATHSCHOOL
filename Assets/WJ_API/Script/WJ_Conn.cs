using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WJ_Conn : MonoBehaviour
{
    // 요청 : 게임 진단 문항요청     
    public class Request_DN_Setting
    {
        public string mbrId;
        //public string deviceScnCd;
        public string deviceNm;
        public string gameVer;
        public string osScnCd;
        public string bgnLvl;
        public string gameCd;
        public string langCd;
        //public string nationCd;
        public int timeZone;

        public Request_DN_Setting()
        {

        }
    }




    // 요청 : 게임 진단 결과전송
    public class Request_DN_Resut
    {
        public string gameCd;
        public string mbrId;
        public string prgsCd;
        public long sid;
        public string qstCd;
        public string qstCransr;
        public string ansrCwYn;
        public long slvTime;

        public Request_DN_Resut()
        {

        }
    }



    public class Request_Learning
    {
        public string gameCd;
        public string mbrId;
        public string gameVer;
        public string osScnCd;
        public string deviceNm;
        public string deviceScnCd;
        public string langCd;
        public int timeZone;
        public string nationCd;
        public string mathpidId;        // v-Famr0.011 추가(API에서는 이전부터 추가되어 있었음. 내가 적용안한것임)

        public Request_Learning()
        {

        }
    }

    public class Request_Learning_Result
    {
        public string gameCd;
        public string mbrId;
        public string prgsCd;
        public long sid;
        public string bgnDt;
        public List<RequsetLearning_Result_Data> data;

        public Request_Learning_Result()
        {
            data = new List<RequsetLearning_Result_Data>();
        }
    }

    [System.Serializable]
    public class RequsetLearning_Result_Data
    {
        public string qstCd;
        public string qstCransr;
        public string ansrCwYn;
        public int slvTime;
    }








    [System.Serializable]
    public class Diagnotics_Data
    {
        public long sid;
        public string prgsCd;
        public string qstCd;
        public string qstCn;
        public string textCn;
        public string qstCransr;
        public string qstWransr;
        public int accuracy;
        public int estQstNowNo;
        public string estPreStgCd;
    }

    public class DN_Response 
    {
        public bool result;
        public string msg;
        public Diagnotics_Data data;
        public DN_Response()
        {
            data = new Diagnotics_Data();
        }


    }
    public DN_Response cDiagnotics = null;



    #region Learning
    [System.Serializable]
    public class Learning_Quest
    {
        public string qstCd;
        public string qstCn;
        public string textCn;
        public string qstCransr;
        public string qstWransr;
    }

    [System.Serializable]
    public class Learning_Data
    {
        public long sid;
        public string bgnDt;
        public List<Learning_Quest> qsts;
    }



    public class Learning_Info
    {
        public bool result;
        public string msg;
        public Learning_Data data;

        public Learning_Info()
        {
        }
    }

    public Learning_Info cLearning_Info = null;



    [System.Serializable]
    public class Recv_Learning_ResultData
    {
        public string explSpedCd;
        public string explSped;
        public string lrnPrgsStsCd;
        public string acrcyCd;
        public string explAcrcyRt;
    }


    public class Recv_Learning_Result
    {
        public string result;
        public string msg;
        public Recv_Learning_ResultData data;

        public Recv_Learning_Result()
        {
        }
    }

    public Recv_Learning_Result cRecv_Learning_Result = null;
    #endregion // Learning


    public string strGameCD;
    public string strGameKey;

    protected string strMBR_ID;
    protected int nState_Request;          // 서버 요청 상태(0: 전송   1: 전송 성공   -1: 전송 실패   -2: 결과 파씽 에러)   


    protected string strDeviceScnCd;    // 디바이스 장치
    protected string strDeviceNm;
    protected string strGameVer;
    protected string strCountryCD;      // 국가 코드

    protected string strOS;     // OS 

    protected string strAuthorization;



    protected string strError;

    void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            strDeviceScnCd = "NORMAL";
            strDeviceNm = "PC";
        }
        else
        {
            strDeviceScnCd = "MOBILE";
            strDeviceNm = SystemInfo.deviceModel;;
        }
               

        strGameVer = "Test_1.0";

        // 15자리 제한
        strOS = SystemInfo.operatingSystem;
        if (strOS.Length >= 15)
            strOS = strOS.Substring(0, 14);

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Authorization")))
        {
            strAuthorization = PlayerPrefs.GetString("Authorization");
        } else
        {
            strAuthorization = "";
        }

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("MemberId")))
        {
            strMBR_ID = PlayerPrefs.GetString("MemberId");
        }

        strError = "";
    }




    #region Interface
    // 진단평가 요청 사용자가 선택한 학습 수준 전송 및 문제 요청
    public void OnRequest_DN_Setting(int _nLevel)
    {        
        Make_MBR_ID();

        StartCoroutine(Send_Diagnosis(_nLevel));

    }



    public void On_Account_authGet(int _nLevel)
    {
        StartCoroutine(Send_Diagnosis(_nLevel));
    }



    // 진단평가 진행 중! 사용자 답안 전송 및 다음 문O제 요청
    public void OnRequest_DN_Progress(string _prgsCd, string _qstCd, string _qstCransr, string _ansrCwYn, long _sid, long _nQstDelayTime)
    {
        StartCoroutine(SendProgress_Diagnosis(_prgsCd, _qstCd, _qstCransr, _ansrCwYn, _sid, _nQstDelayTime));
    }




    // 학습 문제 요청(진단평가 이후)
    public void OnRequest_Learning()
    {
        StartCoroutine(GetLearning());
    }




    // 학습 결과 전송
    public void OnLearningResult(Learning_Data _data, string[] _strQstCransr, long[] _nQstDelayTime)
    {
        StartCoroutine(SendLearning_Result(_data, _strQstCransr, _nQstDelayTime));
    }



    // 요청 상태 체크
    public int CheckState_Request()
    {
        return nState_Request;
    }
    #endregion // Interface


    #region Call Data 
    public Learning_Data GetLearningData()
    {
        if (cLearning_Info == null)
            return null;

        return cLearning_Info.data;
    }


    public Diagnotics_Data GetDiagnosticData()
    {
        if (cDiagnotics.data == null)
            return null;
        
        return cDiagnotics.data;
    }
    #endregion // Call Data


    #region Fuction Progress
    // 진단평가 유저 선택 정보 전송
    protected IEnumerator Send_Diagnosis(int _nSelectLevel)
    {
        Request_DN_Setting request = new Request_DN_Setting();
        request.mbrId = strMBR_ID;
        //request.deviceScnCd = strDeviceScnCd;//"MOBILE";
        request.deviceNm = strDeviceNm;
        request.gameVer = strGameVer;
        request.osScnCd = strOS;
        switch (_nSelectLevel)
        {
            case 0: request.bgnLvl = "A"; break;
            case 1: request.bgnLvl = "B"; break;
            case 2: request.bgnLvl = "C"; break;
            case 3: request.bgnLvl = "D"; break;
            default: request.bgnLvl = "A"; break;
        }
        request.gameCd = strGameCD;
        request.langCd = "KO";
        //request.nationCd = "";
        request.timeZone = TimeZoneInfo.Local.BaseUtcOffset.Hours;//-2;

        string strBody = JsonUtility.ToJson(request);

        string url = "https://prd-brs-relay-model.mathpid.com/api/v1/contest/diagnosis/setting";
        using (UnityWebRequest uwr = UnityWebRequest.Post(url, string.Empty))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(strBody);
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("x-api-key", strGameKey);
            
            nState_Request = 0;
            yield return uwr.SendWebRequest();
                        
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                nState_Request = -1;
                strError = uwr.downloadHandler.text;
            }
            else
            {

                try
                {
                    cDiagnotics = JsonUtility.FromJson<DN_Response>(uwr.downloadHandler.text);
                    nState_Request = 1;
                }
                catch (Exception _e)
                {
                    nState_Request = -2;
                    Debug.Log(_e.Message);
                }


                strAuthorization = uwr.GetResponseHeader("authorization");
                PlayerPrefs.SetString("Authorization", strAuthorization);
            }

           
            uwr.Dispose();
        }
    }



    // 진단 학습 진행중! 유저 답안 제출 및 다음 문제 호출
    protected IEnumerator SendProgress_Diagnosis(string _prgsCd, string _qstCd, string _qstCransr, string _ansrCwYn, long _sid, long _nQstDelayTime)
    {
        Request_DN_Resut request = new Request_DN_Resut();
        request.mbrId = strMBR_ID;
        request.gameCd = strGameCD;
        request.prgsCd = _prgsCd;// "W";    // W: 진단 진행    E: 진단 완료    X: 기타 취소?
        request.qstCd = _qstCd;             // 문항 코드
        request.qstCransr = _qstCransr;     // 입력한 답내용
        request.ansrCwYn = _ansrCwYn;//"Y"; // 정답 여부
        request.sid = _sid;                 // 진단 ID
        request.slvTime = _nQstDelayTime;//5000;



        string strBody = JsonUtility.ToJson(request);

        string url = "https://prd-brs-relay-model.mathpid.com/api/v1/contest/diagnosis/progress";

        using (UnityWebRequest uwr = UnityWebRequest.Post(url, string.Empty))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(strBody);
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("x-api-key", strGameKey);
            uwr.SetRequestHeader("Authorization", strAuthorization);

            nState_Request = 0;
            yield return uwr.SendWebRequest();
                        
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                nState_Request = -1;
                strError = uwr.downloadHandler.text;
            }
            else
            {

                try
                {
                    cDiagnotics = JsonUtility.FromJson<DN_Response>(uwr.downloadHandler.text);
                    nState_Request = 1;
                }
                catch (Exception _e)
                {
                    nState_Request = -2;
                    Debug.Log(_e.Message);
                }
            }

           
            uwr.Dispose();
        }


    }






    // 게임 학습 문항 요청
    protected IEnumerator GetLearning()
    {
        Request_Learning request = new Request_Learning();
        request.gameCd = strGameCD;
        request.mbrId = strMBR_ID;
        request.deviceScnCd = strDeviceScnCd;
        request.gameVer = strGameVer;
        request.osScnCd = strOS;
        request.deviceNm = strDeviceNm;
        request.langCd = "KO";

        request.timeZone = TimeZoneInfo.Local.BaseUtcOffset.Hours; //-2;
        request.nationCd = "";
        request.mathpidId = "";
        

        string strBody = JsonUtility.ToJson(request);
        string url = "https://prd-brs-relay-model.mathpid.com/api/v1/contest/learning/setting";

        using (UnityWebRequest uwr = UnityWebRequest.Post(url, string.Empty))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(strBody);
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("x-api-key", strGameKey);
            uwr.SetRequestHeader("authorization", strAuthorization);

            nState_Request = 0;
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                strError = uwr.downloadHandler.text;
                Debug.Log(strError);
                nState_Request = -1;
            }
            else
            {

                try
                {
                    cLearning_Info = JsonUtility.FromJson<Learning_Info>(uwr.downloadHandler.text);
                    Debug.Log(cLearning_Info.data);
                    nState_Request = 1;
                }
                catch (Exception _e)
                {
                    Debug.Log(_e.Message);
                    nState_Request = -2;
                }


                //txRecvMsg.text = uwr.downloadHandler.text;
            }

            uwr.uploadHandler.Dispose();
            uwr.Dispose();
        }
    }



    // 학습 결과 전송
    protected IEnumerator SendLearning_Result(Learning_Data _data, string[] _strQstCransr, long[] _nQstDelayTime)
    {
        Request_Learning_Result request = new Request_Learning_Result();
        try
        {
            request.gameCd = strGameCD;
            request.mbrId = strMBR_ID;
            request.prgsCd = "E";
            request.sid = _data.sid;
            request.bgnDt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            for (int i = 0; i < _data.qsts.Count; ++i)
            {
                RequsetLearning_Result_Data data = new RequsetLearning_Result_Data();
                data.qstCd = _data.qsts[i].qstCd;
                data.qstCransr = _strQstCransr[i];
                if (_data.qsts[i].qstCransr.CompareTo(_strQstCransr[i]) == 0)
                    data.ansrCwYn = "Y";
                else
                    data.ansrCwYn = "N";
                data.slvTime = (int)_nQstDelayTime[i];
                request.data.Add(data);
            }
        }
        catch
        {

        }

        string strBody = JsonUtility.ToJson(request);

        string url = "https://prd-brs-relay-model.mathpid.com/api/v1/contest/learning/progress";

        using (UnityWebRequest uwr = UnityWebRequest.Post(url, string.Empty))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(strBody);
            uwr.uploadHandler = new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("x-api-key", strGameKey);
            uwr.SetRequestHeader("Authorization", strAuthorization);

            nState_Request = 0;
            yield return uwr.SendWebRequest();
                        
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                strError = uwr.downloadHandler.text;
                nState_Request = -1;
            }
            else
            {

                try
                {
                    cRecv_Learning_Result = JsonUtility.FromJson<Recv_Learning_Result>(uwr.downloadHandler.text);
                    
                    nState_Request = 1;
                }
                catch (Exception _e)
                {
                    Debug.Log(_e.Message);
                    nState_Request = -2;
                }


                //txRecvMsg.text = uwr.downloadHandler.text;



            }
            uwr.uploadHandler.Dispose();
            uwr.Dispose();
        }
    }

    #endregion //  Fuction Progress




    // MBR ID 생성 ->   MBR ID가 등록되어 있지 않으면 신규 생성.
    protected void Make_MBR_ID()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("MemberId")))
        {
            DateTime dt = DateTime.Now;
            strMBR_ID = string.Format("{0}{1:0000}{2:00}{3:00}{4:00}{5:00}{6:00}{7:000}", strGameCD, dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);    // 매쓰팜용 리얼 MBR_ID
            PlayerPrefs.SetString("MemberId",strMBR_ID);
            PlayerPrefs.SetInt("LearningState", 0);
        } else
        {
            strMBR_ID = PlayerPrefs.GetString("MemberId");
            PlayerPrefs.SetInt("LearningState", 0);
        }
    }





    // WJ API를 통해 수신 받은 Latex 코드를 TexDraw에 출력되는 형태로 일부 변환
    public string GetLatexCode(string _strLatex)
    {
        string strRT = _strLatex;
        int nSIdx = _strLatex.IndexOf("\\boxed{\\phantom{");
        if (nSIdx != -1)
        {
            int nCount = 0;
            int nEIdx = 0;
            string strChange = "";
            for (int i = nSIdx + 16; i < _strLatex.Length; ++i)
            {
                if (_strLatex[i] == '0')
                    strChange += "\\square";
                else if (_strLatex[i] == '}')
                {
                    ++nCount;
                    if (nCount >= 2)
                    {
                        nEIdx = i;
                        break;
                    }
                }
            }

            if (nEIdx != 0)
            {
                string strTarget = _strLatex.Substring(nSIdx, nEIdx - nSIdx + 1);
                strRT = _strLatex.Replace(strTarget, strChange);
            }
        }
        else
        {
            strRT = _strLatex.Replace("\\boxed", "\\square");
        }

        return strRT;
    }



    // Update is called once per frame
    void Update()
    {
    }
}
