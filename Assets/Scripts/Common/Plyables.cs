using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Plyables : MonoBehaviourPunCallbacks
{
    [Header("Button Setting")]
    public Button SettingButton;
    [Header("QuestionAndAnswerUI")]
    public GameObject UIActivate;
    [Header("MathPID API Factor")]
    public MathPidAPIUse Use;
    [Header("Audio Effect")]
    public AudioSource successAudio, failedAudio, activationAudio;
    public abstract void Awake();

    protected abstract void SolveProblem();

    protected abstract void Activate();
}
