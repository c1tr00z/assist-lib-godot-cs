using System.Collections.Generic;
using System.Threading.Tasks;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Modules;
using Godot;

namespace c1tr00z.AssistLib.UI;

public partial class GUI : Module {

    #region Nested Classes

    private struct GUIRequest {
        public UIFrameDBEntry frameDbEntry;
        public object[] args;
    }

    #endregion

    #region Private Fields

    private Dictionary<UILayerDBEntry, UILayerBase> _layers = new();

    private Queue<GUIRequest> _requests = new();

    private bool _isShowingProcess;

    #endregion

    #region Node Implementation

    public override void _Ready() {
        var allLayersDbEntries = DB.GetAll<UILayerDBEntry>();
        allLayersDbEntries.Sort((l1, l2) => Mathf.Sign(l1.index - l2.index));
        allLayersDbEntries.ForEach(layerDbEntry => {
            var layer = layerDbEntry.LoadScene().Instantiate<UILayerBase>();
            layer.Name = layerDbEntry.GetName();
            AddChild(layer);
            _layers.Add(layer.dbEntry, layer);
            layer.canvasLayer.Layer = layerDbEntry.index;
        });
        base._Ready();
    }

    #endregion
    
    #region Class Implementation

    public void ShowFrame(UIFrameDBEntry frameDbEntry, object[] args) {
        _requests.Enqueue(new GUIRequest {
            frameDbEntry = frameDbEntry,
            args = args,
        });

        if (_isShowingProcess) {
            return;
        }

        ShowProcess();
    }

    public void HideFrame(UIFrameDBEntry frameDbEntry) {
        if (!_layers.TryGetValue(frameDbEntry.layer, out UILayerBase layer)) {
            GD.PushError($"No layer with DB Entry {frameDbEntry.layer?.GetName()}");
            return;
        }
        
        layer.HideFrame(frameDbEntry);
    }

    public void HideFrame(UIFrame uiFrame) {
        if (!_layers.TryGetValue(uiFrame.dbEntry.layer, out UILayerBase layer)) {
            GD.PushError($"No layer with DB Entry {uiFrame.dbEntry.layer?.GetName()}");
            return;
        }
        
        layer.HideFrame(uiFrame);
    }

    private async Task ShowProcess() {
        _isShowingProcess = true;

        while (_requests.Count > 0) {
            var request = _requests.Dequeue();

            if (!_layers.TryGetValue(request.frameDbEntry.layer, out UILayerBase layer)) {
                GD.PushError($"No layer with DB Entry {request.frameDbEntry.layer?.GetName()}");
                continue;
            }

            await layer.ShowFrame(request.frameDbEntry, request.args);
        }
        
        _isShowingProcess = false;
    }

    #endregion
    
}