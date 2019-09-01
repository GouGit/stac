using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChooseWindowUI : WindowUI
{
    private Button acceptButton;
    private Text acceptButtonText;
    private Button declineButton;
    private Text declineButtonText;

    private Button.ButtonClickedEvent OnAcceptButtonClicked = null;
    private Button.ButtonClickedEvent OnDeclineButtonClicked = null;

    public override void Awake()
    {
        base.Awake();
        acceptButton = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>();
        acceptButtonText = acceptButton.transform.GetComponentInChildren<Text>();
        declineButton = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Button>();
        declineButtonText = declineButton.transform.GetComponentInChildren<Text>();
        OnAcceptButtonClicked = new Button.ButtonClickedEvent();
        OnDeclineButtonClicked = new Button.ButtonClickedEvent();
        acceptButton.onClick = OnAcceptButtonClicked;
        declineButton.onClick = OnDeclineButtonClicked;
    }

    public void SetWindow(string bodyText = "", string acceptText = "확인", string declineText = "취소", UnityAction OnAcceptAction = null, UnityAction OnDeclineAction = null)
    {
        base.SetWindow(bodyText);
        acceptButtonText.text = acceptText;
        declineButtonText.text = declineText;

        if (OnAcceptAction != null)
            OnAcceptButtonClicked.AddListener(OnAcceptAction);
        if (OnDeclineAction != null)
            OnDeclineButtonClicked.AddListener(OnDeclineAction);
    }
}
