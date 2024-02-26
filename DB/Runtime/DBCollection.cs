using Godot;
using Array = System.Array;

namespace AssistLib.DB.Runtime;

[GlobalClass]
[Tool]
public partial class DBCollection : Resource {
    
    [Export] public DBEntryData[] dbEntryDatas = Array.Empty<DBEntryData>();
}