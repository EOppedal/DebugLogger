#if UNITY_EDITOR
using System.IO;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace Editor {
    [InitializeOnLoad] public class DebugLoggerViewer : EditorWindow {
        [SerializeField] private VisualTreeAsset visualTreeAsset;
        [SerializeField] private Texture titleTexture;

        private VisualElement _debugLoggerContainer;

        private const string DebugLoggersPath = "DebugLoggers";
        private const string DebugLoggerIconName = "DebugLoggerViewerIcon";
        private const string ResourcesPath = "Assets/Resources";

        static DebugLoggerViewer() {
            if (!Directory.Exists(ResourcesPath)) Directory.CreateDirectory(ResourcesPath);
            
            const string targetPath = ResourcesPath + "/DebugLoggers";
            
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);
        }

        [MenuItem("Window/MyWindows/DebugLogger")]
        public static void ShowWindow() {
            var window = GetWindow<DebugLoggerViewer>("Debug Logger");
            window.titleContent.image = Resources.Load<Texture>(DebugLoggersPath + "/" + DebugLoggerIconName);
        }

        private void CreateGUI() {
            rootVisualElement.Clear();
            visualTreeAsset.CloneTree(rootVisualElement);
            _debugLoggerContainer = rootVisualElement.Q<VisualElement>("DebugLoggerContainer");
            Populate();
            var updateButton = rootVisualElement.Q<Button>("UpdateButton");
            updateButton.RegisterCallback<ClickEvent>(_ => { CreateGUI(); });
        }

        private void Populate() {
            var debugLoggers = ScrubUtils.GetAllScrubsInResourceFolder<DebugLogger>(DebugLoggersPath);
            foreach (var debugLogger in debugLoggers) {
                CreateDebugLoggerElement(debugLogger);
            }
        }

        private void CreateDebugLoggerElement(Object debugLogger) {
            var element = new VisualElement();
            element.AddToClassList("DebugLoggers");

            var label = new Label(debugLogger.name);
            label.AddToClassList("DebugLoggersLabel");
            label.AddToClassList("DebugLoggersField");

            var logPrefixTextField = new TextField("Log Prefix");
            var logPrefixDataBinding = new UnityEngine.UIElements.DataBinding {
                bindingMode = BindingMode.TwoWay,
                dataSource = debugLogger,
                dataSourcePath = new PropertyPath("logPrefix")
            };
            logPrefixTextField.SetBinding("value", logPrefixDataBinding);
            logPrefixTextField.AddToClassList("DebugLoggersField");

            var enabledToggle = new Toggle("Enabled");
            var enabledDataBinding = new UnityEngine.UIElements.DataBinding {
                bindingMode = BindingMode.TwoWay,
                dataSource = debugLogger,
                dataSourcePath = new PropertyPath("enabled")
            };
            enabledToggle.SetBinding("value", enabledDataBinding);
            enabledToggle.AddToClassList("DebugLoggersField");

            element.Add(label);
            element.Add(logPrefixTextField);
            element.Add(enabledToggle);
            _debugLoggerContainer.Add(element);
        }
    }
}
#endif