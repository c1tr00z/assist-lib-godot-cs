using System;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.Common;
using Godot;
using Godot.Collections;
namespace c1tr00z.AssistLib.Modules;

[GlobalClass]
public partial class RootModulesLoaded : Node {
    
    #region Events
    
    public static event Action ModulesLoaded;
    [Signal] public delegate void modulesLoadedEventHandler();
    
    #endregion
    
    #region Private Fields
    
    private List<Type> _moduleTypes = new();

    private bool _triggered;
    
    #endregion
    
    #region Export Fields
    
    [Export] private Array<string> _expectedModuleTypeNames = new();
    
    #endregion
    
    #region Node Implementation

    public override void _EnterTree() {
        base._EnterTree();
        Modules.RootModuleAdded += ModulesOnRootModuleAdded;
    }

    public override void _ExitTree() {
        base._ExitTree();
        Modules.RootModuleAdded -= ModulesOnRootModuleAdded;
    }

    public override void _Ready() {
        base._Ready();
        CheckLoadedModules();
    }

    #endregion
    
    #region Class Implementation

    void ModulesOnRootModuleAdded(Module rootModule) {
        CheckLoadedModules();
    }

    private void CheckLoadedModules() {

        if (_triggered) {
            return;
        }
        
        if (_moduleTypes.Count == 0) {
            _moduleTypes = _expectedModuleTypeNames.Select(ReflectionUtils.GetTypeByName).ToList();
        }

        GD.Print(_moduleTypes.Count);

        if (_moduleTypes.Any(t => Modules.Get(t) == null)) {
            return;
        }

        GD.PushError("All modules loaded");
        EmitSignal(SignalName.modulesLoaded);
        ModulesLoaded?.Invoke();
        _triggered = true;
    }
    
    #endregion
}