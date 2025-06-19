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

    public static CheckBox MakeCheckBox(string text, bool defaultValue, BaseButton.ToggledEventHandler onToggled, bool expand = false) {
        var checkBox = new CheckBox();
        checkBox.Text = text;
        checkBox.ToggleMode = true;
        checkBox.Toggled += onToggled;
        checkBox.ButtonPressed = defaultValue;

        return checkBox;
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

    public static ScrollContainer MakeScrollContainer(bool isVertical = true, bool expand = false, Node child = null) {
        var scrollContainer = new ScrollContainer();

        if (expand) {
            if (isVertical) {
                scrollContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            } else {
                scrollContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            }
        }

        if (child is not null) {
            scrollContainer.AddChild(child);
        }
        
        return scrollContainer;
    }

    #endregion
}