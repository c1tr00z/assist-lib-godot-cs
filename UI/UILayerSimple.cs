using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using c1tr00z.AssistLib.Common;
using Godot;

namespace c1tr00z.AssistLib.UI;

[GlobalClass]
public partial class UILayerSimple : UILayerBase {

    #region Private Fields

    private Dictionary<UIFrameDBEntry, Queue<UIFrame>> _cachedFrames = new();

    #endregion

    #region Accessors

    public UIFrame currentFrame { get; private set; }

    #endregion
    
    #region UILayerBase Implementation
    
    protected override bool TryGetFromActive(UIFrameDBEntry frameDbEntry, out UIFrame activeFrame) {
        if (currentFrame != null && currentFrame.dbEntry == frameDbEntry) {
            activeFrame = currentFrame;
            
            activeFrame.EnableNode();

            return true;
        }

        activeFrame = null;
        return false;
    }

    protected override bool TryGetCached(UIFrameDBEntry frameDbEntry, out UIFrame cachedFrame) {
        if (_cachedFrames.TryGetValue(frameDbEntry, out Queue<UIFrame> cachedQueue) && cachedQueue.Count > 0) {
            cachedFrame = cachedQueue.Dequeue();
            
            cachedFrame.EnableNode();

            return true;
        }

        cachedFrame = null;
        return false;
    }

    protected override void AddToCurrentFrames(UIFrame frame) {
        if (currentFrame != null) {
            AddToCached(currentFrame);
        }

        currentFrame = frame;
        canvasLayer.AddChild(currentFrame);
    }

    protected override void AddToCached(UIFrame frame) {
        frame.DisableNode();
    }

    #endregion
}