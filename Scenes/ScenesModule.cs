using System.Collections.Generic;
using System.Threading.Tasks;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Modules;
using Godot;

namespace projectwitch.addons.AssistLib.Scenes;

[GlobalClass]
public partial class ScenesModule : Module {

    #region Private Fields

    private Queue<SceneDBEntry> _requests = new();

    private DBLoadResult<PackedScene> _currentRequest = null;

    private bool _isLoading = false;

    #endregion

    #region Accessors

    public Node currentScene { get; private set; }

    #endregion

    #region Class Implementation

    public void LoadScene(SceneDBEntry dbEntry) {
        if (_currentRequest != null && _currentRequest.result == LoadResult.Loading) {
            return;
        }
        _requests.Enqueue(dbEntry);

        ProcessLoad();
    }

    public async Task ProcessLoad() {
        if (_isLoading) {
            return;
        }

        _isLoading = true;

        while (_requests.Count > 0) {
            var dbEntry = _requests.Dequeue();

            var request = await dbEntry.LoadSceneAsync();

            GetTree().ChangeSceneToPacked(request.resource);

            var scene = request.resource.Instantiate();

            currentScene = scene;
            
            // if (GetTree().CurrentScene)
            GetTree().CurrentScene.QueueFree();

            GetTree().CurrentScene = currentScene;
        }

        _isLoading = false;
    }

    #endregion
}