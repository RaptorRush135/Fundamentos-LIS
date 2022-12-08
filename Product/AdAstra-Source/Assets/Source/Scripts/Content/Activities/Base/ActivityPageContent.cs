using UnityEngine;
using System;
using RaptorUtils.Unity.Editor;

[Serializable]
public class ActivityPageContent {
    [field: SerializeField] public InspectorSetOnlyArray<ElementInfo> Elements { get; private set; }
}
[Serializable]
public class ElementInfo {
    [field: SerializeField] public int ExtraSpacing { get; private set; }
    [field: SerializeField] public ActivityElement Element { get; private set; }
}