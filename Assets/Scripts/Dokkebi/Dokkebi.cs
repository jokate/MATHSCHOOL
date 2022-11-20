using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
public class Dokkebi : Plyables
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

    public DokkebiActivator activator;
    private SaveBeacon Beacon;

    [Header("Spawn UI")]
    public TextMeshProUGUI SpawnUItext;

    public bool isHit;

    public List<Vector3> SpawnPoint;

    public AudioSource stunned;

    private Vector3 catchedPosition;

    private int SaveCount = 0;

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
        SpawnPoint = new List<Vector3>();
    }
    private void Update()
    {
        if(state == PlayerState.Catched)
        {
            state = PlayerState.InJail;
            StartCoroutine(TelePort());
        }
        if(isHit)
        {
            StartCoroutine(Stunned());
            isHit = false;
        }
    }
   

    public void Spawn()
    {
        StartCoroutine(ReadyToSpawn());
    }
    #endregion

    #region Player Call
    protected override void Activate()
    {
        if (activationState == ActivateState.Item || activationState == ActivateState.Beacon)
        {
            gameObject.GetComponent<MoveAble>().stop = true;
            activationAudio.Play();
            photonView.RPC(nameof(ActRPC), RpcTarget.AllBuffered);
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
    public IEnumerator TelePort()
    {
        gameObject.GetComponent<MoveAble>().stop = false;
        UIActivate.SetActive(false);
        gameObject.transform.position = catchedPosition;
        yield return null;
    }

    public void SetCatchedPoint(Vector3 position)
    {
        catchedPosition = position;
    }
    public void CatchedSet()
    {
        photonView.RPC("Catched", RpcTarget.AllBuffered);
    }
    private IEnumerator ReadyToSpawn()
    {
        float Spawntime = 5.0f;
        SpawnUItext.enabled = true;
        while (Spawntime > 0.0f)
        {
            Spawntime -= Time.deltaTime;
            SpawnUItext.text = ((int)Spawntime).ToString() + "초 후에 부활합니다!";
            yield return null;
        }
        int random = Random.Range(0, SpawnPoint.Count + 1);
        gameObject.transform.position = GameObject.FindGameObjectWithTag("Manager").GetComponent<InstantiateManager>().SpawnPoints[random].transform.position;
        SpawnUItext.enabled = false;
        state = PlayerState.Idle;
    }
    public void ResultProcess(string strYN)
    {
        if (strYN == "N")
        {
            failedAudio.Play();
            photonView.RPC("Failed", RpcTarget.AllBuffered);
            UIActivate.SetActive(false);
            SaveCount = 0;
            gameObject.GetComponent<MoveAble>().stop = false;
        }
        else if(strYN == "Y")
        {

            if (activationState == ActivateState.Item)
            {
                successAudio.Play();
                Item item = activator.returnItem();
                gameObject.GetComponent<ItemContainer>().ItemFill(item);
                photonView.RPC("Success", RpcTarget.AllBuffered);
                UIActivate.SetActive(false);
                gameObject.GetComponent<MoveAble>().stop = false;

            }
            else if (activationState == ActivateState.Beacon)
            {
                SaveCount++;
                if (SaveCount == 3)
                {
                    successAudio.Play();
                    SaveCount = 0;
                    UIActivate.SetActive(false);
                    gameObject.GetComponent<MoveAble>().stop = false;
                    photonView.RPC(nameof(Saving), RpcTarget.AllBuffered);
                } else
                {
                    Use.bRequest = true;

                }
            }        
            
        }
       
    }
    public void SetSpawnList()
    {
        foreach(var pos in GameObject.FindGameObjectsWithTag("SpawnPoints"))
        {
            SpawnPoint.Add(pos.transform.position);
        }
    }
    public IEnumerator SmallCookie()
    {
        state = PlayerState.Invincble;
        gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
        float time = 10.0f;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        state = PlayerState.Idle;
    }

    public IEnumerator Stunned()
    {
        stunned.Play();
        float time = 3.0f;
        while (time > 0.0f)
        {
            gameObject.GetComponent<MoveAble>().stop = true;
            time -= Time.deltaTime;
            yield return null;
        }
        gameObject.GetComponent<MoveAble>().stop = false;
    }
    #endregion

    #region Collision Detected
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DokkebiActivator")
        {
            activator = collision.GetComponent<DokkebiActivator>();
            if (activator.GetState() == global::Activator.BOXState.Normal)
            {
                activationState = ActivateState.Item;
                SettingButton.onClick.AddListener(Activate);
            }
        } else if(collision.tag == "RespawnBeacon")
        {
            Beacon = collision.GetComponent<SaveBeacon>();
            if(Beacon.GetState() == global::SaveBeacon.BOXState.Normal)
            {
                activationState = ActivateState.Beacon;
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
        else if(collision.tag == "RespawnBeacon")
        {
            if(Beacon.GetState() == SaveBeacon.BOXState.Normal)
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
        if (collision.tag == "DokkebiActivator")
        {
            if (activator.GetState() == global::Activator.BOXState.Normal)
            {
                activator = null;
                SettingButton.onClick.RemoveAllListeners();
                SettingButton.image.color = Color.white;
            }
        }
        else if (collision.tag == "RespawnBeacon")
        {
            Beacon = collision.GetComponent<SaveBeacon>();
            if (Beacon.GetState() == global::SaveBeacon.BOXState.Normal)
            {
                Beacon = null;
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
        if (activationState == ActivateState.Item)
        {
            if (activator != null)
            {
                activator.Activated();
            }

        } else if (activationState == ActivateState.Beacon) {
            if (Beacon != null)
            {
                Beacon.Activated();
            }
        }
                
    }

    [PunRPC]
    public void Success()
    {
        if (activationState == ActivateState.Item)
        {
            activator.Success();
        } else if(activationState == ActivateState.Beacon)
        {
            Beacon.Success();
        }
    }

    [PunRPC]
    public void Failed()
    {
        if (activationState == ActivateState.Item)
        {
            activator.Failed();
        } else if(activationState == ActivateState.Beacon)
        {
            Beacon.Failed();
        }
    }
    [PunRPC]
    public void Catched()
    {
        state = PlayerState.Catched;
    }
    [PunRPC]
    public void Saving()
    {
        if (Beacon != null)
        {
            Beacon.SaveALL();
        }
    }

    [PunRPC]
    public void EnterToi(Vector3 pos)
    {
        gameObject.transform.position = pos;
        state = PlayerState.Invincble;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "floor";
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }
    [PunRPC]
    public void ExitToi(Vector3 pos)
    {
        gameObject.transform.position = pos;
        state = PlayerState.Idle;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "floor";
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
    }
    [PunRPC]
    public void SetTransform(Vector2 pos)
    {
        gameObject.GetComponent<MoveAble>().enabled = false;
        gameObject.GetComponent<MoveAble>().movement = Vector2.zero;
        gameObject.transform.position = pos;
    }
    [PunRPC]
    public void ReturnNormal()
    {
        gameObject.GetComponent<MoveAble>().enabled = true;
    }

    [PunRPC]
    public void ChangePos(Vector3 pos)
    {
        gameObject.transform.position = pos;
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
    #endregion
}

