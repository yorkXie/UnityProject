using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text loadPercent;

    private void Awake()
    {
        Messenger<int, int>.AddListener(StartupEvent.MANAGERS_PROGRESS, OnManagersProgress);
        Messenger.AddListener(StartupEvent.MANAGERS_STARTED, OnManagersStarted);
    }

    private void OnDestroy()
    {
        Messenger<int, int>.RemoveListener(StartupEvent.MANAGERS_PROGRESS, OnManagersProgress);
        Messenger.RemoveListener(StartupEvent.MANAGERS_STARTED, OnManagersStarted);
    }

    private void OnManagersProgress(int numReady, int numModules)
    {
        float progress = (float)numReady / numModules;
        progressBar.value = progress;
        loadPercent.text = Mathf.Ceil(progress * 100f) + "%";
    }

    private void OnManagersStarted()
    {
        Managers.Mission.GoToNext();
    }
}
