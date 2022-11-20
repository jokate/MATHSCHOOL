using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SinglePlayer : Plyables
{
    #region Variable Setting
    public enum PlayerState
    {
        Idle,
        Catched,
        InJail,
        Invincble
    }
    public PlayerState state;
    public enum ActivateState
    {
        Item,
        Beacon,
    };
    public ActivateState activationState;

    public SingleActivator activator;
    public Image DashCountDown, CookieCountDown;
    public Button DashButton, CookieButton;
    public bool isHit;
    public AudioSource dash, Cookie;
    float Timer = 10.0f;
    #endregion

    #region Unity Function
    // Start is called before the first frame update
    void Start()
    {
    }
    public override void Awake()
    {
        isHit = false;
        state = PlayerState.Idle;
    }
    private void Update()
    {
        if (isHit)
        {
            StartCoroutine(Stunned());
            isHit = false;
        }
    }
    #endregion

    #region Player Call
    protected override void Activate()
    {
        if (activationState == ActivateState.Item || activationState == ActivateState.Beacon)
        {
            gameObject.GetComponent<LocalMove>().isstop = true;
            activationAudio.Play();
            ActRPC();
            UIActivate.SetActive(true);
            Use.bRequest = true;
            SettingButton.image.color = Color.white;
            SettingButton.onClick.RemoveAllListeners();
            Debug.Log("Activated");

        }
    }

    protected override void SolveProblem()
    {
        throw new System.NotImplementedException();
    }

    public void ResultProcess(string strYN)
    {
        if (strYN == "N")
        {
            failedAudio.Play();
            Failed();
            UIActivate.SetActive(false);
        }
        else if (strYN == "Y")
        {

            if (activationState == ActivateState.Item)
            {
                Debug.Log("Item");
                Success();
                UIActivate.SetActive(false);
            }

        }
        gameObject.GetComponent<LocalMove>().isstop = false;

    }
    public IEnumerator SmallCookie()
    {
        Cookie.Play();
        state = PlayerState.Invincble;
        gameObject.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
        gameObject.GetComponent<LocalMove>().speed = 20.0f;
        float time = 10.0f;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        gameObject.GetComponent<LocalMove>().speed = gameObject.GetComponent<LocalMove>().OriginalSpeed;
        state = PlayerState.Idle;
    }

    public IEnumerator Stunned()
    {
       
        float time = 3.0f;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            GetComponent<LocalMove>().isstop = true;
            yield return null;
        }
        gameObject.GetComponent<LocalMove>().isstop = false;
    }
    #endregion

    #region Collision Detected
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DokkebiActivator")
        {
            activator = collision.GetComponent<SingleActivator>();
            if (activator.GetState() == global::Activator.BOXState.Normal)
            {
                activationState = ActivateState.Item;
                Debug.Log("Entered");

                SettingButton.onClick.AddListener(Activate);
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "DokkebiActivator")
        {

            if (activator.GetState() == global::Activator.BOXState.Normal)
            {
                SettingButton.image.color = Color.red;
            }
            else
            {
                SettingButton.image.color = Color.white;
                SettingButton.onClick.RemoveAllListeners();
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "DokkebiActivator")
        {
            if (activator.GetState() == global::Activator.BOXState.Normal)
            {
                Debug.Log("Exit");
                activator = null;
                SettingButton.onClick.RemoveAllListeners();
                SettingButton.image.color = Color.white;
            }
        }
    }
    #endregion

    #region RPC Calls
    public void ActRPC()
    {
        if (activationState == ActivateState.Item)
        {
            if (activator != null)
            {
                activator.Activated();
            }

        }

    }

    public void Success()
    {
        if (activationState == ActivateState.Item)
        {
            activator.Success();
        }
    }


    public void Failed()
    {
        if (activationState == ActivateState.Item)
        {
            activator.Failed();
        }
    }



    public void EnterToi(Vector3 pos)
    {
        gameObject.transform.position = pos;
        state = PlayerState.Invincble;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "floor";
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }

    public void ExitToi(Vector3 pos)
    {
        gameObject.transform.position = pos;
        state = PlayerState.Idle;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "floor";
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
    }

    public void SetTransform(Vector2 pos)
    {
        gameObject.GetComponent<LocalMove>().movement = Vector2.zero;
        gameObject.GetComponent<LocalMove>().enabled = false;

        gameObject.transform.position = pos;
    }

    public void ReturnNormal()
    {
        gameObject.GetComponent<LocalMove>().enabled = true;
    }


    public void ChangePos(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }


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

    public void DashActivate()
    {
        dash.Play();
        StartCoroutine(Dash(30.0f));
        StartCoroutine(ActiveCount(5.0f, DashButton, DashCountDown));
    }
    public void CookieActivate()
    {
        StartCoroutine(SmallCookie());
        StartCoroutine(ActiveCount(30.0f, CookieButton, CookieCountDown));
    }

    IEnumerator ActiveCount(float time, Button activateButton, Image counter)
    {
        activateButton.enabled = false;
        counter.gameObject.SetActive(true);
        float timer = time;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            counter.fillAmount = 1 - (timer/ time);
            yield return null;
        }
        counter.gameObject.SetActive(false);
        activateButton.enabled = true;

    }
    #endregion

}
