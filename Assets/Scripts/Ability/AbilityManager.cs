using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
public class AbilityManager : MonoBehaviourPunCallbacks
{
    #region Variable Set
    public List<Button> ButtonList, AbilityButtons;
    public List<Image> ImageList;
    public GameObject AbilityPanel;
    public delegate void AbilityActive();
    public List<Image> AbilityImage, CounterImage;
    public List<AudioSource> listAS;


    public float Timer = 10.0f;

    Dictionary<int, AbilityActive> ActivationData;
    public List<Ability> Abilities;
    List<Ability> ActiveAbility, SelectionAbility;
    const int MaxAbilCount = 2;
    int AbilityCount;
    #endregion

    #region UnityFunction
    // Start is called before the first frame update
    private void Awake() {
        ActiveAbility = new List<Ability>();
        SelectionAbility = new List<Ability>();
        ActivationData = new Dictionary<int, AbilityActive>();
        foreach(var bt in AbilityButtons)
        {
            bt.gameObject.SetActive(false);
        }
        GeneratePool();
    }
    private void Update()
    {
    }
    #endregion

    #region Setting Function;
    public void Initialize() {
        int i = 0;
        while (i < 3)
        {
            int Number = Random.Range(0, Abilities.Count);
            if (ActiveAbility.Contains(Abilities[Number]) || AbilityCount == MaxAbilCount)
            {
                continue;
            }
            ButtonList[i].onClick.AddListener(() =>
            {
                OnSelection(Abilities[Number]);
            });
            Debug.Log("ItemNumber : " + Number);
            ImageList[i].sprite = Abilities[Number].ShowImage;
            i++;

        }
    }

    public void GeneratePool() {
        ActivationData.Add(1, (() => photonView.RPC(nameof(Speed1), RpcTarget.AllBuffered)));
        ActivationData.Add(2, (() => photonView.RPC(nameof(Rad1), RpcTarget.AllBuffered)));
        ActivationData.Add(3, (() => {
            listAS[0].Play();
            photonView.RPC(nameof(DashI), RpcTarget.AllBuffered); }));
        ActivationData.Add(4, (() => {
            listAS[1].Play();
            photonView.RPC(nameof(Chalk), RpcTarget.AllBuffered, 3); }));
    }
    public void OnSelection(Ability ability)
    {
        if (ability.AbilityType == Ability.Type.PassiveStatUP)
        {
            AbilityActive activate = ActivationData[ability.AbilityCode];
            activate();
        } else if(ability.AbilityType == Ability.Type.ActiveAbil && AbilityCount < MaxAbilCount)
        {
            AbilityButtons[AbilityCount].gameObject.SetActive(true);
            AbilityImage[AbilityCount].sprite = ability.ActiveSpr;
            int Index = AbilityCount;
            AbilityButtons[AbilityCount].onClick.AddListener(() =>
            {
                AbilityActive activate = ActivationData[ability.AbilityCode];
                activate();
                StartCoroutine(ActiveCount(AbilityButtons[Index], CounterImage[Index]));
                Debug.Log("Activation");
            });
            ActiveAbility.Add(ability);
            AbilityCount++;
            Debug.Log("Activate Ability" + 1 + "is Activated");
        }
        else if(ability.AbilityType == Ability.Type.ActiveAbil && AbilityCount == 2)
        {
            ChangeAbility();
        }
        foreach(Button bt in ButtonList) {
            bt.onClick.RemoveAllListeners();
        }
        AbilityPanel.SetActive(false);
        
    }

    public void ChangeAbility()
    {
        
    }
    #endregion

    #region Passive
    [PunRPC]
    public void Speed1() {
        gameObject.GetComponent<MoveAble>().OriginalSpeed += 1f;
        gameObject.GetComponent<MoveAble>().speed = gameObject.GetComponent<MoveAble>().OriginalSpeed;
    }
    [PunRPC]
    public void Rad1()
    {
        gameObject.GetComponentInChildren<CircleCollider2D>().radius += 1.0f;
    }

    #endregion

    #region Active

    [PunRPC]
    public void DashI() {
        gameObject.GetComponent<PhotonView>().RPC("Dash", RpcTarget.AllBuffered, 10.0f);
    }

    [PunRPC]
    public void Chalk(int AbilNumber)
    {
        var stalk = GameObject.Instantiate(Abilities[AbilNumber].Pref, gameObject.transform.position , Quaternion.identity);
        stalk.GetComponent<Stalk>().Initialize(gameObject.GetComponent<RayComponent>().ray.direction);
    }
    IEnumerator ActiveCount(Button activateButton, Image counter)
    {
        activateButton.enabled = false;
        counter.gameObject.SetActive(true);
        float time = 10.0f;
        while(time > 0.0f)
        {
            time -= Time.deltaTime;
            counter.fillAmount = 1 - (time / Timer);
            yield return null;
        }
        counter.gameObject.SetActive(false);
        activateButton.enabled = true;
        
    }

    #endregion

}
