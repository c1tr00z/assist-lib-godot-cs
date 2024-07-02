using System.IO;
using System.Linq;
using System.Threading.Tasks;
using c1tr00z.AssistLib.Common;
using Godot;
using Godot.Collections;

namespace AssistLib.DB.Runtime;

public static class DBEntryExt {
    
    #region Class Implementation

    public static string GetName(this DBEntry dbEntry) {
        return DB.GetDBEntryName(dbEntry);
    }

    public static string GetPath(this DBEntry dbEntry) {
        return DB.GetDBEntryPath(dbEntry);
    }

    public static T Load<T>(this DBEntry dbEntry, string key) where T : Resource {
        return GD.Load<T>($"{dbEntry.GetPath()}_{key}");
    }

    public static PackedScene LoadScene(this DBEntry dbEntry) {
        return Load<PackedScene>(dbEntry, "scene.tscn");
    }
    
    public static void LoadThreadedRequest(this DBEntry dbEntry, string key) {
        ResourceLoader.LoadThreadedRequest($"{dbEntry.GetPath()}_{key}");
    }
    
    public static T LoadThreadedGet<T>(this DBEntry dbEntry, string key) where T : Resource {
        return ResourceLoader.LoadThreadedGet($"{dbEntry.GetPath()}_{key}") as T;
    }

    public static ResourceLoader.ThreadLoadStatus LoadThreadedStatus(this DBEntry dbEntry, string key, out double progress) {
        var progressArray = new Array();
        var status = ResourceLoader.LoadThreadedGetStatus($"{dbEntry.GetPath()}_{key}", progressArray);
        var progressObject = progressArray.FirstOrDefault();
        if (progressObject.VariantType == Variant.Type.Float) {
            progress = progressObject.AsDouble();
        } else {
            progress = 0;
        }

        return status;
    }

    public static void LoadThreadedRequestScene(this DBEntry dbEntry) {
        LoadThreadedRequest(dbEntry, "scene.tscn");
    }
    
    public static PackedScene LoadThreadedGetScene(this DBEntry dbEntry) {
        return LoadThreadedGet<PackedScene>(dbEntry, "scene.tscn");
    }

    public static ResourceLoader.ThreadLoadStatus LoadThreadedStatusScene(this DBEntry dbEntry) {
        return LoadThreadedStatus(dbEntry, "scene.tscn", out double progress);
    }

    public static async Task<DBLoadResult<T>> LoadAsync<T>(this DBEntry dbEntry, string key) where T : Resource {
        var result = new DBLoadResult<T>(dbEntry, key);

        await ProcessRequestAsync(result);

        return result;
    }

    public static async Task<DBLoadResult<PackedScene>> LoadSceneAsync(this DBEntry dbEntry) {
        return await dbEntry.LoadAsync<PackedScene>("scene.tscn");
    }

    private async static Task ProcessRequestAsync<T>(DBLoadResult<T> request) where T : Resource {
        var dbEntry = request.dbEntry;
        var key = request.key;
        
        var status = LoadThreadedStatus(dbEntry, key, out double progress);
        if (status == ResourceLoader.ThreadLoadStatus.InvalidResource) {
            dbEntry.LoadThreadedRequest(key);
        }
        
        while (status == ResourceLoader.ThreadLoadStatus.InProgress) {
            status = LoadThreadedStatus(dbEntry, key, out progress);
            request.progress = progress;
            await Task.Delay(100);
        }
        
        if (status == ResourceLoader.ThreadLoadStatus.Failed) {
            request.result = LoadResult.Failed;

            return;
        }
        
        request.progress = 1;
        
        request.resource = dbEntry.LoadThreadedGet<T>(key);
        request.result = LoadResult.Success;
    }

    public static DBLoadResult<T> LoadRequestAsync<T>(this DBEntry dbEntry, string key) where T : Resource {
        var result = new DBLoadResult<T>(dbEntry, key);

        ProcessRequestAsync(result);
        
        return result;
    }

    public static T GetCachedNodeDBEntry<T>(this Node node, ref T cached) where T : DBEntry {
        if (cached == null) {
            cached = node.FindInChildrenByType<DBEntryResource>().dbEntry as T;
        }

        return cached;
    }

    #endregion
}