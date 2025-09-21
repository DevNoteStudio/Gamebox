using DevNote;
using Gamebox;
using UnityEngine;

public class MainSceneContext : SceneContext
{
    [SerializeField] private RectTransform _uiContainer;
    [SerializeField] private RectTransform _fadeContainer;


    private readonly Holder<ILeaderboards> leaderboards = new();

    public override void RegisterContext()
    {
        new UI(_uiContainer, _fadeContainer);

        var menu = Register(new MenuController());
        var level = Register(new LevelController(menu, leaderboards.Item));
        var test = Register(new TestController(level));
        var start = Register(new StartController(menu, level));

    }


}
