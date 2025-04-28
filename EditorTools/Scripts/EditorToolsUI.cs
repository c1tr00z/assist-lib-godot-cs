using System;
using System.Collections.Generic;
using Godot;

namespace projectwitch.addons.AssistLib.EditorTools.Scripts;

public static class EditorToolsUI {
    #region Class Implementation

    public static Label MakeLabel(string text, bool expand = false) {
        var label = new Label();
        label.Text = text;

        if (expand) {
            label.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        }
        
        return label;
    }

    public static Button MakeButton(string text, Action onPressed, bool expand = false) {
        var button = new Button();
        button.Text = text;
        button.Pressed += onPressed;
        
        if (expand) {
            button.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        }

        return button;
    }

    public static OptionButton MakeOptionsButton<T>(IEnumerable<T> options, Func<T, string> textGetter,
        bool expand = false) {
        
        var optionsButton = new OptionButton();
        
        foreach (var o in options) {
            optionsButton.AddItem(textGetter(o));
        }
        
        if (expand) {
            optionsButton.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        }

        return optionsButton;
    }

    public static LineEdit MakLineEdit(string text, string placeholderText = null, bool expand = false) {
        var lineEdit = new LineEdit();
        lineEdit.Text = text;
        lineEdit.PlaceholderText = placeholderText;
        
        if (expand) {
            lineEdit.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
        }
        
        return lineEdit;
    }

    public static HBoxContainer MakeHBoxContainer(params Node[] nodes) {
        var hBoxContainer = new HBoxContainer();

        foreach (var node2D in nodes) {
            hBoxContainer.AddChild(node2D);
        }
        
        return hBoxContainer;
    }

    #endregion
}