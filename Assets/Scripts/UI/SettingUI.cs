using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Dropdown dropdown;
    public Slider volumeSlider;
    public Slider BGMSlider;
    public Button closeButton;

    void Start()
    {
        StartCoroutine(init());
    }

    void OnDropdownValueChanged(int index)
    {
        string bgmName = dropdown.options[index].text;
        Game.instance.sceneManager.ChangerBGM(bgmName);
    }

    void InitializeDropdown()
    {
        dropdown.ClearOptions();
        foreach (var bgmName in Game.instance.sceneManager.audioManager.BGMNames)
        {
            dropdown.options.Add(new Dropdown.OptionData(bgmName));
        }
        dropdown.RefreshShownValue();
    }

    IEnumerator init()
    {
        yield return null;
        dropdown = transform.Find("Dropdown").GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        volumeSlider = transform.Find("VolumeSlider").GetComponent<Slider>();
        volumeSlider.onValueChanged.AddListener(Game.instance.sceneManager.SetSoundEffectVolume);

        BGMSlider = transform.Find("BGMSlider").GetComponent<Slider>();
        BGMSlider.onValueChanged.AddListener(Game.instance.sceneManager.SetBGMVolume);

        closeButton = transform.Find("ExitButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnExitButtonClicked);
        InitializeDropdown();
        gameObject.SetActive(false);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
