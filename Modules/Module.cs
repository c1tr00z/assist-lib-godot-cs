using Godot;

namespace c1tr00z.AssistLib.Modules;

public partial class Module : Node {
    #region Node Implementation

    public override void _Ready() {
        base._Ready();
        Modules.AddRootModule(this);
    }

    #endregion
}