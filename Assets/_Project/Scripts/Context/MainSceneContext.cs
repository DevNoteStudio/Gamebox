namespace DevNote.LevelUp
{
    public class MainSceneContext : SceneContext
    {
        public override void RegisterContext()
        {
            var level = Register(new LevelController());



        }
    }

}

