using System;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

[EditorTool("Test tool")]
public class EditorToolTest : AssistLibEditorTool<EditorToolTestData>, IEditorToolRuntimeUI {
    #region AssistLibEditorTool Implementation

    protected override EditorToolTestData GetMySaveData() {
        return new EditorToolTestData();
    }
    protected override void LoadFromSaveData(EditorToolTestData saveData) {
    }

    #endregion

    #region IEditorToolRuntimeUI

    public Type panelType => typeof(TestToolPanel);

    #endregion
}