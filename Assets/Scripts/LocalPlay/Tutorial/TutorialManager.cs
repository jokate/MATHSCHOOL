using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;
using Photon.Pun;
public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Player Movement")]
    public GameObject MovementUI;
    [Header("Tutorial Player Activation Button")]
    public GameObject ActivationBt;
    [Header("Tutorial Player Item Setting Button")]
    public GameObject itemBt, itemBt2;
    [Header("Tutorial Player")]
    public GameObject player;

    [Header("Direction Object")]
    public GameObject arrkkebi;

    [Header("Global Text")]
    public TextMeshProUGUI ExplainText;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI JobText;

    [Header("Tutorial Object")]
    public GameObject tutorialGuide;

    EventGiver giver;
    public static int collisionDetected = 0;
    [Header("Collision And Trigger")]
    public List<GameObject> collisionlist, triggerObject;

    [Header("CutScene Director")]
    public List<PlayableDirector> directors;

    [Header("Respawn Beacon Position")]
    public List<GameObject> positions;

    [Header("Target Camera")]
    public CinemachineVirtualCamera virtualCam;

    public GameObject RespawnBeacon, winning, resp;
    public AudioSource winningsound;

    bool moveActive, activeActive, itemActive, item2Active;

    #region Basic Method
    private void Start()
    {
        giver = new EventGiver();
        EventGiver.testEvent += TSEvent;
        directors[0].gameObject.SetActive(true);
        directors[0].Play();
    }

    public void EventPlay()
    {
        giver.RunEvent();
    }

    private void Update()
    {

    }
    public void TSEvent(int i)
    {
        switch (i)
        {
            case 1:
                StartCoroutine(nameof(Event1));
                break;
            case 2:
                StartCoroutine(Event2());
                break;
            case 3:
                StartCoroutine(Event3());
                break;
            case 4:
                StartCoroutine(Event4());
                break;
            case 5:
                StartCoroutine(Event5());
                break;
            case 6:
                StartCoroutine(Event6());
                break;
            case 7:
                StartCoroutine(Event7());
                break;
            case 8:
                StartCoroutine(Event8());
                break;
            case 9:
                StartCoroutine(Event9());
                break;
            case 10:
                StartCoroutine(Event10());
                break;
            case 11:
                StartCoroutine(Event11());
                break;
            case 12:
                StartCoroutine(Event12());
                break;
            case 13:
                StartCoroutine(Event13());
                break;
            case 14:
                StartCoroutine(Event14());
                break;
            case 15:
                StartCoroutine(Event15());
                break;
            case 16:
                StartCoroutine(Event16());
                break;
            case 17:
                StartCoroutine(Event17());
                break;
            case 18:
                StartCoroutine(Event18());
                break;
            case 19:
                StartCoroutine(Event19());
                break;
            case 20:
                StartCoroutine(Event20());
                break;
            case 21:
                StartCoroutine(Event21());
                break;
            case 22:
                StartCoroutine(Event22());
                break;
            case 23:
                StartCoroutine(Event23());
                break;
            case 24:
                StartCoroutine(Event24());
                break;
            case 25:
                StartCoroutine(Event25());
                break;
            case 26:
                StartCoroutine(Event26());
                break;
        }
    }
    public void AllSetFalse()
    {
        LocalMove.stop = true;
        moveActive = MovementUI.activeInHierarchy;
        activeActive = ActivationBt.activeInHierarchy;
        itemActive = itemBt.activeInHierarchy;
        item2Active = itemBt2.activeInHierarchy;


        MovementUI.SetActive(false);
        ActivationBt.SetActive(false);
        itemBt.SetActive(false);
        itemBt2.SetActive(false);
    }

    public void ReturnAll()
    {
        LocalMove.stop = false;
        MovementUI.SetActive(moveActive);
        ActivationBt.SetActive(activeActive);
        itemBt.SetActive(itemActive);
        itemBt2.SetActive(item2Active);
    }

    #endregion

    #region Event Coroutine
    IEnumerator Event1()
    {
        directors[0].gameObject.SetActive(false);
        NameText.text = "�����";
        JobText.text = "�л�ȸ��";

        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[0], TypingSender.tutorialTextContainer.textObj);

        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);

        MovementUI.SetActive(true);

        ExplainText.enabled = true;
        ExplainText.text = "�Ʒ� ���̽�ƽ���� ĳ���͸� ������ ������!";
        yield return new WaitUntil(() => collisionDetected == 4);


        ExplainText.enabled = false;
        collisionDetected = 0;
        collisionlist[0].SetActive(false);
        collisionlist[1].SetActive(true);
        giver.RunEvent();
    }

    IEnumerator Event2()
    {
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[1], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.instance.dialogNumber == 3);


        Arrkkebi.Target = triggerObject[0];
        arrkkebi.SetActive(true);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);


        directors[1].gameObject.SetActive(true);
        directors[1].Play();
        ExplainText.enabled = true;
        ExplainText.text = "���� ������ ���� ���� ������ ��������!";
    }

    IEnumerator Event3()
    {
        LocalMove.stop = true;
        AllSetFalse();
        directors[1].gameObject.SetActive(false);
        ExplainText.enabled = false;
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[2], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ReturnAll();
        ActivationBt.SetActive(true);
        ExplainText.enabled = true;
        ExplainText.text = "���� ��� ���� ������ ������!";
    }

    IEnumerator Event4()
    {
        LocalMove.stop = true;
        directors[2].gameObject.SetActive(true);
        ExplainText.enabled = false;
        directors[2].Play();
        yield return null;
    }

    IEnumerator Event5()
    {
        LocalMove.stop = true;
        //������ ��.
        collisionlist[2].SetActive(true);
        directors[2].gameObject.SetActive(false);
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[3], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ReturnAll();


        Arrkkebi.Target = triggerObject[1];
        arrkkebi.SetActive(true);

        ExplainText.enabled = true;
        ExplainText.text = "������ Ǯ�� �繰���� �������!";
        LocalMove.stop = false;
        yield return new WaitUntil(() => player.GetComponent<ItemContainer>().Itemcontainer[0] != null);
        ExplainText.enabled = false;
        triggerObject[1].GetComponent<CircleCollider2D>().enabled = false;
        arrkkebi.SetActive(false);

        giver.RunEvent();
    }

    IEnumerator Event6()
    {
        LocalMove.stop = true;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[4], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ReturnAll();
        itemBt.SetActive(true);
        itemBt.GetComponent<Button>().onClick.AddListener(() => giver.RunEvent());
        ExplainText.enabled = true;
        ExplainText.text = "������ ��ư�� ���� �����͸� �ҷ�������!";
        LocalMove.stop = false;
    }

    IEnumerator Event7()
    {
        LocalMove.stop = true;
        ExplainText.enabled = false;
        itemBt.SetActive(false);
        collisionlist[1].SetActive(false);
        itemBt.GetComponent<Button>().onClick.RemoveAllListeners();
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[5], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        directors[3].gameObject.SetActive(true);
        directors[3].Play();

        Arrkkebi.Target = triggerObject[2];
        arrkkebi.SetActive(true);
        ExplainText.enabled = true;
        ExplainText.text = "���нǷ� �̵��� ������!";
        LocalMove.stop = false;
    }

    IEnumerator Event8()
    {
        directors[3].gameObject.SetActive(false);

        arrkkebi.SetActive(false);
        ExplainText.enabled = false;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[6], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ExplainText.enabled = true;
        ExplainText.text = "���н��� �ö�ũ�� �̿��Ͽ� ������ �غ�����!";
        ReturnAll();
        LocalMove.stop = false;
        yield return new WaitUntil(() => isFull() == true);


        giver.RunEvent(); 

    }

    IEnumerator Event9()
    {
        ExplainText.enabled = false;
        AllSetFalse();
        NameText.text = "�÷��̾�";
        JobText.text = "���л�";
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[7], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ReturnAll();
        triggerObject[3].gameObject.SetActive(true);
        ExplainText.enabled = true;
        ExplainText.text = "�л�ȸ�忡�� ���ư�����!";
        LocalMove.stop = false;
        Arrkkebi.Target = triggerObject[4];
        arrkkebi.SetActive(true);
    }

    IEnumerator Event10() {
        arrkkebi.SetActive(false);

        triggerObject[3].gameObject.SetActive(false);
        AllSetFalse();
        NameText.text = "�����";
        JobText.text = "�л�ȸ��";
        ExplainText.enabled = false;
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[8], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ReturnAll();
        ExplainText.enabled = true;
        ExplainText.text = "�������� ��� ����غ�����!";
        itemBt.SetActive(true);
        itemBt2.SetActive(true);
        LocalMove.stop = false;
        yield return new WaitUntil(() => player.GetComponent<ItemContainer>().Itemcontainer[0] == null && player.GetComponent<ItemContainer>().Itemcontainer[1] == null);
        giver.RunEvent();
    }

    IEnumerator Event11()
    {
        LocalMove.stop = true;
        AllSetFalse();
        ExplainText.enabled = false;
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[9], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);


        collisionlist[2].SetActive(false);
        collisionlist[3].SetActive(true);
        directors[4].gameObject.SetActive(true);
        ExplainText.enabled = true;
        ExplainText.text = "����Ƿ� ��������!";
        directors[4].Play();
        Arrkkebi.Target = triggerObject[5];
        arrkkebi.SetActive(true);

    }

    IEnumerator Event12()
    {
        LocalMove.stop = true;
        arrkkebi.SetActive(false);
        ExplainText.enabled = false;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[10], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ExplainText.enabled = true;
        ExplainText.text = "���� ��Ʈ�� �����ϼ���!";
        Arrkkebi.Target = triggerObject[6];
        arrkkebi.SetActive(true);
        ReturnAll();
        yield return new WaitUntil(() => triggerObject[6].GetComponent<LocalActivator>().GetState() == Activator.BOXState.Wait);
        arrkkebi.SetActive(false);
        giver.RunEvent();
    }

    IEnumerator Event13()
    {
        LocalMove.stop = true;
        ExplainText.enabled = false;
        triggerObject[7].GetComponent<CircleCollider2D>().enabled = true;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[11], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);

        directors[4].gameObject.SetActive(false);
        directors[5].gameObject.SetActive(true);
        directors[5].Play();
        Arrkkebi.Target = triggerObject[7];
        arrkkebi.SetActive(true);

        ExplainText.enabled = true;
        ExplainText.text = "�����θ� �̿��Ͽ� �������� �ö󰡺�����!";
    }

    IEnumerator Event14()
    {
        LocalMove.stop = true;
        arrkkebi.SetActive(false);
        ExplainText.enabled = false;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[12], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ReturnAll();
        ExplainText.enabled = true;
        ExplainText.text = "����ǿ��� ���ͼ� ���� �߾����� ��������!";
        Arrkkebi.Target = triggerObject[8];
        arrkkebi.SetActive(true);
    }

    IEnumerator Event15()
    {
        LocalMove.stop = true;
        arrkkebi.SetActive(false);
        ExplainText.enabled = false;
        AllSetFalse();
        directors[5].gameObject.SetActive(false);
        directors[6].gameObject.SetActive(true);
        directors[6].Play();
        yield return null;
    }

    IEnumerator Event16()
    {
        LocalMove.stop = true;
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[13], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ExplainText.enabled = true;
        ExplainText.text = "�л�ȸ�����׼� ������ ��������!";
        Arrkkebi.Target = tutorialGuide;
        arrkkebi.SetActive(true);

        tutorialGuide.tag = "DokkebiActivator";
        tutorialGuide.GetComponent<CircleCollider2D>().enabled = true;
        tutorialGuide.GetComponent<LocalActivator>().enabled = true;
        ReturnAll();
        yield return new WaitUntil(() => player.GetComponent<ItemContainer>().Itemcontainer[0] != null);
        giver.RunEvent();
    } 

    IEnumerator Event17() 
    {
        LocalMove.stop = true;
        tutorialGuide.tag = "Untagged";
        tutorialGuide.GetComponent<CircleCollider2D>().enabled = false;
        arrkkebi.SetActive(false);
        ExplainText.enabled = false;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[14], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        collisionlist[4].SetActive(false);
        ExplainText.enabled = true;
        ExplainText.text = "�Ʒ��� ������ 2������ ��������!";
        Arrkkebi.Target = triggerObject[9];
        arrkkebi.SetActive(true);
        ReturnAll();
    }

    IEnumerator Event18()
    {
        LocalMove.stop = true;
        arrkkebi.SetActive(false);
        ExplainText.enabled = false;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[15], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        Arrkkebi.Target = triggerObject[10];
        arrkkebi.SetActive(true);
        ExplainText.enabled = true;
        ExplainText.text = "ȭ��Ƿ� ��������!";
        ReturnAll();
    }

    IEnumerator Event19()
    {
        LocalMove.stop = true;
        arrkkebi.SetActive(false);
        ExplainText.enabled = false;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[16], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ExplainText.enabled = true;
        ExplainText.text = "ȭ��� ĭ ������ ��������!!";
        ReturnAll();
    }

    IEnumerator Event20()
    {
        LocalMove.stop = true;
        ExplainText.enabled = false;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[17], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        ExplainText.enabled = true;
        ExplainText.text = "ȭ��ǿ��� ��������.";
        collisionlist[5].SetActive(true);
        ReturnAll();
    }
    
    IEnumerator Event21()
    {
        LocalMove.stop = true;
        ExplainText.enabled = false;
        AllSetFalse();
        directors[6].gameObject.SetActive(false);
        directors[7].gameObject.SetActive(true);
        directors[7].Play();
        yield return null;
    }
    IEnumerator Event22()
    {
        LocalMove.stop = true;
        directors[7].Pause();
        NameText.text = "����";
        JobText.text = "������";
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[18], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.instance.dialogNumber == 3);
        NameText.text = "�����";
        JobText.text = "�л�ȸ��";
        yield return new WaitUntil(() => TypingManager.instance.dialogNumber > 3);
        NameText.text = "����";
        JobText.text = "������";
        yield return new WaitUntil(() => TypingManager.instance.dialogNumber == 6);
        NameText.text = "�����";
        JobText.text = "�л�ȸ��";
        yield return new WaitUntil(() => TypingManager.instance.dialogNumber == 7);
        NameText.text = "����";
        JobText.text = "������";
        yield return new WaitUntil(() => TypingManager.instance.dialogNumber == 8);
        NameText.text = "�����";
        JobText.text = "�л�ȸ��";
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        directors[7].Resume();

    }
    IEnumerator Event23()
    {
        LocalMove.stop = true;
        directors[7].gameObject.SetActive(false);
        ExplainText.enabled = true;
        ExplainText.text = "ȭ��ǿ��� ��������.";
        ReturnAll();
        yield return null;

    }
    IEnumerator Event24()
    {
        LocalMove.stop = true;
        ExplainText.enabled = false;
        AllSetFalse();
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[19], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        foreach (var col in collisionlist)
            col.SetActive(false);
        ReturnAll();
        ExplainText.enabled = true;
        ExplainText.text = "��� �������� ã������!";
        RespawnBeacon.transform.position = positions[Random.Range(0, positions.Count)].transform.position;
        resp.SetActive(true);
        RespawnBeacon.SetActive(true);
        yield return new WaitUntil(() => RespawnBeacon.activeInHierarchy == false);
        giver.RunEvent();
    }
    IEnumerator Event25()
    {
        LocalMove.stop = true;
        ExplainText.enabled = false;
        AllSetFalse();
        tutorialGuide.SetActive(false);
        directors[8].gameObject.SetActive(true);
        directors[8].Play();
        NameText.text = "����";
        JobText.text = "������";
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[20], TypingSender.tutorialTextContainer.textObj);
        virtualCam.Priority = 11;
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        giver.RunEvent();
    }
    IEnumerator Event26()
    {
        LocalMove.stop = true;
        NameText.text = "�����";
        JobText.text = "�л�ȸ��";
        virtualCam.Priority = 5;
        TypingManager.instance.Typing(TypingSender.tutorialTextContainer.dialogStrings[21], TypingSender.tutorialTextContainer.textObj);
        yield return new WaitUntil(() => TypingManager.isDialogEnd == true);
        winning.SetActive(true);
        winningsound.Play();

    }

    #endregion

    #region Additional Function call
    bool isFull()
    {
        if (player.GetComponent<ItemContainer>().Itemcontainer[0] != null && player.GetComponent<ItemContainer>().Itemcontainer[1] != null)
        {
            return true;
        }
        return false;
    }
    #endregion

    public void SceneMove()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("TutorialFinish", 1);
        SceneManager.LoadSceneAsync("LobbyScene");
    }
}


delegate void TutorialEventHandler(int i);

class EventGiver {
    public static int tutorialScriptnumber = 1;

    public static event TutorialEventHandler testEvent;
    public void RunEvent()
    {
        if(testEvent != null)
        {
            testEvent(tutorialScriptnumber);
            tutorialScriptnumber++;
        }
    }
}
