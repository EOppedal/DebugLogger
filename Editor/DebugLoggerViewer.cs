#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DebugLogger.Editor {
    [InitializeOnLoad] public class DebugLoggerViewer : EditorWindow {
        [SerializeField] private VisualTreeAsset visualTreeAsset;
        [SerializeField] private Texture titleTexture;
        
        [SerializeField] private VisualTreeAsset debugLoggerElementTemplate;

        private VisualElement _debugLoggerContainer;

        private const string DebugLoggersPath = "DebugLoggers";
        private const string ResourcesPath = "Assets/Resources";
        private const string IconPath = "Packages/com.erlend-eiken-oppedal.debuglogger/Editor/DebugLoggerViewerIcon.png";

        static DebugLoggerViewer() {
            if (!Directory.Exists(ResourcesPath)) Directory.CreateDirectory(ResourcesPath);
            const string targetPath = ResourcesPath + "/DebugLoggers";
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);
        }

        [MenuItem("Window/DebugLogger")]
        public static void ShowWindow() {
            var window = GetWindow<DebugLoggerViewer>("Debug Logger");
            window.titleContent.image = AssetDatabase.LoadAssetAtPath<Texture>(IconPath);
        }

        private void CreateGUI() {
            rootVisualElement.Clear();
            visualTreeAsset.CloneTree(rootVisualElement);
            _debugLoggerContainer = rootVisualElement.Q<VisualElement>("DebugLoggerContainer");
            Populate();
            var updateButton = rootVisualElement.Q<Button>("UpdateButton");
            updateButton.RegisterCallback<ClickEvent>(_ => CreateGUI());
        }

        private void Populate() {
            var debugLoggers = ScrubUtils.GetAllScrubsInResourceFolder<DebugLogger>(DebugLoggersPath);
            foreach (var debugLogger in debugLoggers) {
                CreateDebugLoggerElement(debugLogger);
            }
        }

        private void CreateDebugLoggerElement(Object debugLogger) {
            var element = new VisualElement();
            debugLoggerElementTemplate.CloneTree(element);
            element.dataSource = debugLogger;

            _debugLoggerContainer.Add(element);
        }
    }
}
#endif