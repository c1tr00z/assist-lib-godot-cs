using AssistLib.DB.Runtime;
using Godot;
namespace c1tr00z.AssistLib.Scenes;

[GlobalClass]
public partial class LoadSceneNode : Node {
    #region Export Fields

    [Export] private SceneDBEntry _sceneToLoad;

    #endregion
    
    #region Class Implementation

    public void LoadScene() {
        _sceneToLoad.LoadThisScene();
    }
    
    #endregion
}