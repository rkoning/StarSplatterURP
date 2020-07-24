using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    public static Dialog instance;
    public GameObject dialogBox;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;

    public AnimationCurve moveCurve;
    public float animateTime;
    private float animateRemaining;

    private float displayEnd;

    private bool displaying;

    private RectTransform dialogBoxRectTransform;
    public Vector3 displayPosition;
    public Vector3 hidePosition;

    private bool messageDisplayed;
    private bool down;

    public float typewriterDuration = 2f;
    private Coroutine typewriter;
    private float now;

    public delegate bool ShowUntil();
    private ShowUntil currentCondition = null;

    private Queue<DialogMessage> messageQueue = new Queue<DialogMessage>();
    private DialogMessage currentMessage;

    private void Awake() {
        Dialog.instance = this;
        dialogBoxRectTransform = dialogBox.GetComponent<RectTransform>();
    }

    private IEnumerator Typewriter(string text, float duration) {
        messageDisplayed = false;
        float timePerCharacter = duration / (float) text.Length;
        float end = now + duration;
        int currentCharacter = 0;

        float last = 0f;
        while(now < end || currentCharacter < text.Length) {
            if (now > last + timePerCharacter) {
                last = now;
                dialogText.text += text[currentCharacter];
                currentCharacter++;
            }
            yield return null;
        }
        messageDisplayed = true;
        typewriter = null;
    }

    public void QueueDialog(DialogMessage msg) {
        messageQueue.Enqueue(msg);
    }

    public void SendDialog(DialogMessage msg) {
        currentMessage = msg;
        nameText.text = msg.name;
        dialogText.text = "";
        nameText.color = msg.color;
        dialogText.color = msg.color;
        if (typewriter != null) {
            StopCoroutine(typewriter);
        }
        typewriter = StartCoroutine(Typewriter(msg.text, typewriterDuration));
        if (msg.condition != null) {
            ShowDialogUntil(() => { return msg.condition(); });
        } else {
            ShowDialog(msg.duration);
        }
    }

    public void HideDialog() {
        animateRemaining = now + animateTime;
        currentCondition = null;
        down = true;
        displaying = false;
        if (currentMessage.OnMessageEnd != null)
            currentMessage.OnMessageEnd.Invoke();
        currentMessage = null;
    }

    public void ShowDialog(float duration) {
        animateRemaining = now + animateTime;
        displayEnd = now + duration;
        down = false;
        displaying = true;
    }

    public void ShowDialogUntil(ShowUntil condition) {
        animateRemaining = now + animateTime;
        displayEnd = now + 3f;
        currentCondition = condition;
        down = false;
        displaying = true;
    }

    private void Update() {
        now = Time.fixedTime;
        if (animateRemaining > now) {
            float current = (animateRemaining - now) / animateTime;
            if (down) {
                dialogBoxRectTransform.anchoredPosition3D = Vector3.Lerp(hidePosition, displayPosition, moveCurve.Evaluate(1 - current));
            } else {
                dialogBoxRectTransform.anchoredPosition3D = Vector3.Lerp(hidePosition, displayPosition, moveCurve.Evaluate(current));
            }
        } else if (messageDisplayed && (displaying && displayEnd < now) && (currentCondition == null || currentCondition())) {
            HideDialog();
        }

        if (!displaying && messageQueue.Count > 0) {
            var next = messageQueue.Dequeue();
            SendDialog(next);
        }
    }
}

[System.Serializable]
public class DialogMessage {
    public string name;
    [TextArea]
    public string text;
    public float duration;
    public Func<bool> condition;

    public Color color = Color.white;

    public UnityEvent OnMessageEnd;
    public DialogMessage(string name, string text, float duration) {
        this.name = name;
        this.text = text;
        this.duration = duration;
    }

    public DialogMessage(string name, string text, Func<bool> condition) {
        this.name = name;
        this.text = text;
        this.condition = condition;
    }
}
