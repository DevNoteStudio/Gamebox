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

        private readonly Holder<IEnvironment> environment = new();


        private void OnEnable() => Display();


        private void Start()
        {
            _switchMusicButton.onClick.AddListener(OnSwitchMusicButtonClick);
            _switchSoundButton.onClick.AddListener(OnSwitchSoundButtonClick);
        }

        private void Display()
        {
            _disabledSoundObject.SetActive(environment.Item.ChannelIsMuted(Sound.Channel.SFX));
            _enabledSoundObject.SetActive(!environment.Item.ChannelIsMuted(Sound.Channel.SFX));
            _disabledMusicObject.SetActive(environment.Item.ChannelIsMuted(Sound.Channel.Music));
            _enabledMusicObject.SetActive(!environment.Item.ChannelIsMuted(Sound.Channel.Music));
        }


        private void OnSwitchSoundButtonClick()
        {
            bool isMuted = !environment.Item.ChannelIsMuted(Sound.Channel.SFX);

            environment.Item.SetChannelMute(Sound.Channel.SFX, isMuted);

            _disabledSoundObject.SetActive(isMuted);
            _enabledSoundObject.SetActive(!isMuted);

            IConfigs.AudioHub.OpenClick?.Play();
        }

        private void OnSwitchMusicButtonClick()
        {
            bool isMuted = !environment.Item.ChannelIsMuted(Sound.Channel.Music);

            environment.Item.SetChannelMute(Sound.Channel.Music, isMuted);

            _disabledMusicObject.SetActive(isMuted);
            _enabledMusicObject.SetActive(!isMuted);

            IConfigs.AudioHub.OpenClick?.Play();
        }

    }
}



