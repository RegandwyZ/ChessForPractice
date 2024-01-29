using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(UIInputReceiver))]
public class UIButton : Button
{
    private InputReceiver _receiver;

    protected override void Awake()
    {
        base.Awake();
        _receiver = GetComponent<UIInputReceiver>();
        onClick.AddListener(() => _receiver.OnInputReceived());
    }
}
