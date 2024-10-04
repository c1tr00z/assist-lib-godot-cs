using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.Common;

namespace c1tr00z.AssistLib.Modules.Submodules;

public partial class AssistLibSubmodulesModule : Module {
    #region Private Fields

    private Dictionary<Type, Submodule> _submodules = new();

    #endregion

    #region Node Implementation

    public override void _Ready() {
        base._Ready();
        var allSubmoduleTypes = ReflectionUtils.GetSubclassesOf(typeof(Submodule), false);
        foreach (var submoduleType in allSubmoduleTypes) {
            var submodule = Activator.CreateInstance(submoduleType) as Submodule;
            AddChild(submodule);
            _submodules.Add(submoduleType, submodule);
        }
        AssistLibSubmodules.Init(this);
    }

    #endregion

    #region Class Implementation

    public T GetSubmodule<T>() where T : Submodule {
        if (!_submodules.TryGetValue(typeof(T), out Submodule submodule)) {
            return null;
        }

        if (submodule is T required) {
            return required;
        }

        return null;
    }

    #endregion
}