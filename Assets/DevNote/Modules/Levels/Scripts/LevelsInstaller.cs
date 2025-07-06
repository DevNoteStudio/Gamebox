using UnityEngine;
using Zenject;


namespace DevNote.Modules.Levels
{
    public class LevelsInstaller : MonoInstaller
    {
        [SerializeField] private RectTransform _uiContainer;


        public override void InstallBindings()
        {
            new SceneInjector(Container);

            var level = Bind(new LevelController());
            var screen = Bind(new ScreenController(_uiContainer));
        }


        private T Bind<T>(T controller) where T : class
        {
            ProjectInstaller.ProjectContainer.Inject(controller);
            Container.BindInterfacesAndSelfTo<T>().FromInstance(controller).AsSingle();
            return controller;
        }

    }
}

