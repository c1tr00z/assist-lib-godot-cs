using System.Threading.Tasks;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Common;
using Godot;

namespace c1tr00z.AssistLib.UI;

public abstract partial class UILayerBase : Node2D {

    #region Private Fields

    private UILayerDBEntry _dbEntry;

    private CanvasLayer _canvasLayer;

    #endregion

    #region Accessors

    public UILayerDBEntry dbEntry => CommonExt.GetCached(ref _dbEntry,
        () => this.FindInChildrenByType<DBEntryResource>().dbEntry as UILayerDBEntry);

    public CanvasLayer canvasLayer => this.GetCachedInChildren(ref _canvasLayer);

    #endregion
    
    #region Class Implementation
    
    public async Task ShowFrame(UIFrameDBEntry frameDBEntry, params object[] args) {
        
        if (TryGetFromActive(frameDBEntry, out UIFrame activeFrame)) {
            activeFrame.ShowFrame(args);
            return;
        }
        
        if (TryGetCached(frameDBEntry, out UIFrame cachedFrame)) {
            AddToCurrentFrames(cachedFrame);
            cachedFrame.EnableNode();
            cachedFrame.ShowFrame(args);
            return;
        }
        
        var frameResult = await frameDBEntry.LoadSceneAsync();

        if (frameResult.result == LoadResult.Failed) {
            GD.PushError($"[GUI] Error loading ui frame scene for {frameDBEntry.GetDBEntryName()}");
            return;
        }

        var frame = frameResult.resource.Instantiate<UIFrame>();
        AddToCurrentFrames(frame);
        frame.ShowFrame(args);
    }

    public void HideFrame(UIFrameDBEntry frameDBEntry) {
        if (!TryGetFromActive(frameDBEntry, out UIFrame activeFrame)) {
            return;
        }
        
        HideFrame(activeFrame);
    }

    public void HideFrame(UIFrame uiFrame) {
        uiFrame.HideFrame();
        AddToCached(uiFrame);
    }

    protected abstract bool TryGetFromActive(UIFrameDBEntry frameDbEntry, out UIFrame activeFrame);
    
    protected abstract bool TryGetCached(UIFrameDBEntry frameDbEntry, out UIFrame cachedFrame);

    protected abstract void AddToCurrentFrames(UIFrame frame);

    protected abstract void AddToCached(UIFrame frame);

    #endregion
}