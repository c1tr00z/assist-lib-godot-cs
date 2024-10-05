using System;
using System.Collections.Generic;
using System.Linq;
using AssistLib.DB.Runtime;
using Godot;

namespace c1tr00z.AssistLib.Modules;

[GlobalClass]
public partial class SceneModules : Node {

	#region Private Fields

	private List<SceneModule> _sceneModules = new();

	#endregion

	#region Export Fields

	[Export] private SceneModulesCollection _modulesCollection;

	#endregion

	#region Accessors

	private SceneModulesCollection modulesCollection => _modulesCollection ?? DB.Get<SceneModulesCollection>();

	#endregion
	
	#region Node Implementation

	public override void _EnterTree() {
		base._EnterTree();
		Modules.AddSceneModules(this);
		InitModules();
	}

	public override void _ExitTree() {
		Modules.RemoveSceneModules(this);
		_sceneModules.ForEach(m => m.QueueFree());
		_sceneModules.Clear();
		base._ExitTree();
	}

	#endregion

	#region Class Implementation

	private async void InitModules() {
		foreach (var modulesDbEntry in modulesCollection.modulesDbEntries) {
			var module = modulesDbEntry.LoadScene().Instantiate<SceneModule>();

			if (module == null) {
				GD.PushError($"[SceneModules] No module for {modulesDbEntry.GetDBEntryPath()}");
				continue;
			}

			module.Name = modulesDbEntry.GetName() + GD.Randi();
			
			AddChild(module);
			
			_sceneModules.Add(module);

			await module.Init();
			
			_sceneModules.Add(module);
		}
	}

	public bool TryGet<T>(out T module) where T : Module {
		module = _sceneModules.OfType<T>().FirstOrDefault();

		return module != null;
	}
	
	public bool TryGet(Type type, out Module module) {
		module = _sceneModules .FirstOrDefault(m => m.GetType() == type);

		return module != null;
	}

	#endregion
}
