using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Common;
using Godot;

namespace c1tr00z.AssistLib.UI.Utils;

[GlobalClass]
public partial class UIImageByDBEntry : Control {

    #region Nested Classes

    private struct ImageRequest {
        public DBEntry dbEntry;
        public String key;
    }

    #endregion

    #region Private Fields

    private TextureRect _textureRect;

    private TextureRect _placeholderObject;

    private Texture2D _texture;

    private ImageRequest _lastRequest;

    private Queue<ImageRequest> _requests = new();

    private bool _isLoading;

    #endregion
    
    #region Export Fields

    [Export] private NodePath _textureRectPath;

    [Export] private string _key = "icon.png";

    [Export] private NodePath _placeholderObjectPath;

    #endregion

    #region Node Implementation

    public override void _EnterTree() {
        base._EnterTree();

        if (_texture == null && this.TryGetCached(ref _placeholderObject, _placeholderObjectPath)) {
            _placeholderObject.Visible = true;
        }
    }

    #endregion
    
    #region Class Implementation

    public void Load(DBEntry dbEntry) {
        _requests.Enqueue(new ImageRequest {
            dbEntry = dbEntry,
            key = _key,
        });

        AsyncLoad();
    }

    private async Task AsyncLoad() {
        if (_isLoading) {
            return;
        }

        _isLoading = true;

        while (_requests.Count > 0) {
            var request = _requests.Dequeue();

            if (request.dbEntry == _lastRequest.dbEntry && request.key == _lastRequest.key) {
                continue;
            }
            
            if (this.TryGetCached(ref _placeholderObject, _placeholderObjectPath)) {
                _placeholderObject.Visible = false;
            }

            _lastRequest = request;

            if (request.dbEntry == null) {
                continue;
            }
            
            var result = await request.dbEntry.LoadAsync<Texture2D>(_key);
            
            if (result.result == LoadResult.Failed) {
                throw new Exception($"[UIImageByDBEntry] Error on load icon for {request.dbEntry}");
            }

            _texture = result.resource;

            if (this.TryGetCached(ref _textureRect, _textureRectPath)) {
                _textureRect.Texture = _texture;
            }
            
            GD.PushError($"Load {request.dbEntry.GetName()}_{_key}::: 4");
            
            if (this.TryGetCached(ref _placeholderObject, _placeholderObjectPath)) {
                _placeholderObject.Visible = false;
            }
        }
        
        _isLoading = false;
    }

    #endregion
}