using DevNote;
using UnityEngine;
using UnityEngine.UI;


namespace Gamebox
{
    public class SettingsWidgetView : MonoBehaviour
    {
        [SerializeField] private Button _switchMusicButton;
        [SerializeField] private GameObject _enabledMusicObject;
        [SerializeField] private GameObject _disabledMusicObject;
        [SerializeField] private Button _switchSoundButton;
        [SerializeField] private GameObject _enabledSoundObject;
        [SerializeField] private GameObject _disabledSoundObject;


        private void OnEnable() => Display();


        private void Start()
        {
            _switchMusicButton.onClick.AddListener(OnSwitchMusicButtonClick);
            _switchSoundButton.onClick.AddListener(OnSwitchSoundButtonClick);
        }

        private void Display()
        {
            _disabledSoundObject.SetActive(!Sound.Settings.SfxEnabled);
            _enabledSoundObject.SetActive(Sound.Settings.SfxEnabled);
            _disabledMusicObject.SetActive(!Sound.Settings.MusicEnabled);
            _enabledMusicObject.SetActive(Sound.Settings.MusicEnabled);
        }


        private void OnSwitchSoundButtonClick()
        {
            Sound.Settings.SfxEnabled = !Sound.Settings.SfxEnabled;
            _disabledSoundObject.SetActive(!Sound.Settings.SfxEnabled);
            _enabledSoundObject.SetActive(Sound.Settings.SfxEnabled);
            IConfigs.Gamebox.OpenClickSound.Play();
        }

        private void OnSwitchMusicButtonClick()
        {
            Sound.Settings.MusicEnabled = !Sound.Settings.MusicEnabled;
            _disabledMusicObject.SetActive(!Sound.Settings.MusicEnabled);
            _enabledMusicObject.SetActive(Sound.Settings.MusicEnabled);
            IConfigs.Gamebox.OpenClickSound.Play();
        }

    }
}



