using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Teacher : Plyables
{
    #region Variable Setting
    public TeacherActivator activator;
    private int Level;
    [Header("Teacher Exp Maxima")]
    private Dictionary<int, int> Expr;
    [Header("Teacher Experience")]
    public int experience;
    [Header("Ability Panel")]
    public GameObject AbilityPanel;

    [Header("Player Finder")]
    public CatchFactor catchFactor;
    [Header("Player Catcher")]
    public Button CatchButton;

    private Vector3 catchedPosition;
    public AudioSource LevelUpSound;
    public AudioSource StunAs;
    float KillRange = 5f;

    bool CanCatch { get { return catchFactor.LDokkebi.Count > 0; } }
    public bool isStunned;
    #endregion

    #region Unity Fucntion
    // Start is called before the first frame update
    void Start()
    {
        isStunned = false;
        Level = 1;
        experience = 0;
        catchFactor.SetKillRange(KillRange);
    }
    private void Update()
    {
        if(isStunned)
        {
            StartCoroutine(Stunned());
        }
        Catch();
    }
    public override void Awake()
    {
        Expr = new Dictionary<int, int>();
        InitializeEXP();
    }

    #endregion

    #region Play Function
    protected void InitializeEXP() {
        Expr.Add(1, 4);
        Expr.Add(2, 4);
        Expr.Add(3, 4);
        Expr.Add(4, 6);
        Expr.Add(5, 6);
        Expr.Add(6, 6);
        Expr.Add(7, 8);
        Expr.Add(8, 8);
        Expr.Add(9, 8);

    }

    protected override void Activate()
    {
        if(activator != null)
        {
            GetComponent<MoveAble>().stop = true;
            activationAudio.Play();
            photonView.RPC("ActRPC", RpcTarget.AllBuffered);
            UIActivate.SetActive(true);
            Use.bRequest = true;
            SettingButton.image.color = Color.white;
            SettingButton.onClick.RemoveAllListeners();
            
        }
    }

    protected override void SolveProblem()
    {
        throw new System.NotImplementedException();
    }
    public void EXPUP(int i)
    {
        int count = 0;
        while (count < i)
        {
            if (++experience == Expr[Level])
            {
                experience = 0;
                Level++;
                AbilityPanel.SetActive(true);
                LevelUpSound.Play();
            }
            count++;
        }
    }


    public void Catch()
    {
        if (CanCatch) { CatchButton.enabled = true; CatchButton.image.color = Color.red; }
        else { CatchButton.enabled = false; CatchButton.image.color = Color.white; };
    }

    public void ResultProcess(string strYN)
    {
        if (strYN == "N")
        {
            failedAudio.Play();
            photonView.RPC("Failed", RpcTarget.AllBuffered);
            UIActivate.SetActive(false);
        }
        else
        {
            successAudio.Play();
            photonView.RPC("Success", RpcTarget.AllBuffered);
            EXPUP(1);
            UIActivate.SetActive(false);
            
        }
        GetComponent<MoveAble>().stop = false;
    }

    IEnumerator Stunned()
    {
        StunAs.Play();
        isStunned = false;
        float time = 5.0f;
        while (time > 0.0f)
        {
            GetComponent<MoveAble>().stop = true;
            time -= Time.deltaTime;
            yield return null;
        }
        GetComponent<MoveAble>().stop = false;
    }

    #endregion 

    #region Collision Detector
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TeacherActivator") {
            activator = collision.GetComponent<TeacherActivator>();
            if (activator.GetState() == global::Activator.BOXState.Normal)
            {
            
                SettingButton.onClick.AddListener(Activate);
            }
        } 

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "TeacherActivator")
        {
            
            if (activator.GetState() == global::Activator.BOXState.Normal)
            {
                SettingButton.image.color = Color.red;
            } else
            {
                SettingButton.image.color = Color.white;
                SettingButton.onClick.RemoveAllListeners();
            }
            

        }   
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TeacherActivator")
        {
            if (activator.GetState() == global::Activator.BOXState.Normal)
            {
                activator = null;
                SettingButton.onClick.RemoveAllListeners();
                SettingButton.image.color = Color.white;
            } 
        }

    }
    #endregion

    #region RPC Calls
    [PunRPC]
    public void ActRPC()
    {
        if (activator != null)
        {
            activator.Activated();
        }
    }
    [PunRPC]
    public void Success()
    {
        activator.Success();
    }

    [PunRPC]
    public void Failed()
    {
        activator.Failed();
    }

    [PunRPC]
    public void EnterToi(Vector3 pos)
    {
        gameObject.transform.position = pos;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "floor";
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }
    [PunRPC]
    public void ExitToi(Vector3 pos)
    {
        gameObject.transform.position = pos;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "floor";
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
    }
    [PunRPC]
    public IEnumerator Dash(float speed)
    {
        float time = 0.1f;
        while (time > 0.0f)
        {
            gameObject.transform.Translate(GetComponent<RayComponent>().ray.direction * speed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
    }
    [PunRPC]
    public void ChangePos(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }
    
    #endregion 
}
