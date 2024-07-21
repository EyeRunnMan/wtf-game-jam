using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WTF.common.AudioSystem;
using WTF.Common;

public class SoundButtonComponent : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private TextMeshProUGUI text;

    private IAudioSystem audioSystem;

    // Start is called before the first frame update
    void Start()
    {
        DependencySolver.TryGetInstance(out audioSystem);
        button.onClick.AddListener(OnButtonClick);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
    private void OnButtonClick()
    {
        audioSystem.ToggleSound(!audioSystem.isActive);
        text.text = audioSystem.isActive ? "Sound : On" : "Sound : Off";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
