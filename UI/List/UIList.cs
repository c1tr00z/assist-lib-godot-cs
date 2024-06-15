using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssistLib.DB.Runtime;
using c1tr00z.AssistLib.Common;
using Godot;
using Godot.Collections;

namespace c1tr00z.AssistLib.UI.List;

[GlobalClass]
public partial class UIList : Control {

    #region Nested Classes

    private struct ItemsRequest {
        public List<object> items;
    }

    #endregion

    #region Private Fields

    private Container _container;

    private List<UIListItem> _listItems = new();
    
    private System.Collections.Generic.Dictionary<String, Type> _itemTypesByNames = new();

    private System.Collections.Generic.Dictionary<Type, Queue<UIListItem>> _cached = new();

    private bool _inProcess = false;

    private Queue<ItemsRequest> _requests = new();

    #endregion
    
    #region Export Fields

    [Export] private NodePath _containerPath;

    [Export] private Array<UIListItemDBEntry> _items = new();

    [Export] private bool _useOnlyLastRequest;

    #endregion

    #region Accessors

    private Container container => this.GetCached(ref _container, _containerPath);

    #endregion

    #region Class Implementation

    public void SetItems(List<object> newItems) {
        _requests.Enqueue(new ItemsRequest {
            items = newItems,
        });

        if (_inProcess) {
            return;
        }

        ProcessRequests();
    }

    private async Task ProcessRequests() {
        if (_inProcess) {
            return;
        }

        _inProcess = true;

        if (_requests.Count > 0) {
            if (_useOnlyLastRequest) {
                var request = _requests.Last();
                _requests.Clear();
                await ProcessRequest(request);
            } else {
                while (_requests.Count > 0) {
                    var request = _requests.Dequeue();
                    await ProcessRequest(request);
                }
            }
        }
        
        _inProcess = false;
    }

    private async Task ProcessRequest(ItemsRequest request) {
        if (_listItems.Count > request.items.Count) {
            while (_listItems.Count > request.items.Count) {
                ReturnToPool(_listItems.Last());
            }
        }

        for (int i = 0; i < request.items.Count; i++) {
            var item = request.items[i];
            var itemType = item.GetType();

            if (i >= _listItems.Count) {
                var newListItem = await GetListItem(itemType);
                PlaceListItem(newListItem);
                newListItem.SetItem(item);
                _listItems.Add(newListItem);
            } else {
                var current = _listItems[i];

                if (current.itemType != itemType) {
                    ReturnToPool(current);
                    current = await GetListItem(itemType);
                    PlaceListItem(current);
                    container.MoveChild(current, i);
                    _listItems.Add(current);
                }
                
                current.SetItem(item);
            }
        }
    }

    private async Task<UIListItem> GetListItem(Type type) {
        if (_cached.TryGetValue(type, out Queue<UIListItem> items) && items.Count > 0) {
            return items.Dequeue();
        }

        if (_itemTypesByNames.Count == 0) {
            _itemTypesByNames =
                _items.ToDictionary(i => i.fullTypeName, i => ReflectionUtils.GetTypeByName(i.fullTypeName));
        }

        var uiListItemDBEntry = _items.FirstOrDefault(i => _itemTypesByNames[i.fullTypeName] == type);

        if (uiListItemDBEntry == null) {
            uiListItemDBEntry = _items.FirstOrDefault(i => _itemTypesByNames[i.fullTypeName].IsAssignableFrom(type));
        }

        if (uiListItemDBEntry == null) {
            throw new Exception($"No item for type {type.FullName}");
        }

        var loadResult = await uiListItemDBEntry.LoadSceneAsync();

        if (loadResult.result == LoadResult.Failed) {
            throw new Exception($"Error on load item for type {type.FullName}");
        }

        var uiListItem = loadResult.resource.Instantiate<UIListItem>();

        return uiListItem;
    }

    private void PlaceListItem(UIListItem uiListItem) {
        if (uiListItem.GetParent() != container) {
            container.CallDeferred(Node.MethodName.AddChild, uiListItem);
        } else {
            uiListItem.EnableNode();
        }
    }

    private void ReturnToPool(UIListItem listItem) {
        if (_listItems.Contains(listItem)) {
            _listItems.Remove(listItem);
        }

        if (!_cached.ContainsKey(listItem.itemType)) {
            _cached[listItem.itemType] = new Queue<UIListItem>();
        }
        
        listItem.DisableNode();
        _cached[listItem.itemType].Enqueue(listItem);
    }

    #endregion
}