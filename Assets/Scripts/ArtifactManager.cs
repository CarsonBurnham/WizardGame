using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ArtifactManager))]
public class ArtifactManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ArtifactManager artifactManager = (ArtifactManager)target;
        if (GUILayout.Button("Prev"))
        {
            artifactManager.PrevArt();
        }

        if (GUILayout.Button("Next"))
        {
            artifactManager.NextArt();
        }

        if (GUILayout.Button("Use"))
        {
            artifactManager.Use();
        }
    }
}

public class ArtifactManager : MonoBehaviour
{
    public List<Artifact> artifacts;
    public Artifact artifact;
    private int artifactIndex = 0;

    public void Use()
    {
        if (artifact)
            artifact.Use();
    }

    public void PrevArt()
    {
        if (artifacts.Count == 0)
            return;

        if (artifactIndex == 0)
            artifactIndex = artifacts.Count;

        artifactIndex--;
        artifact = artifacts[artifactIndex];
    }

    public void NextArt()
    {
        if (artifacts.Count == 0)
            return;

        artifactIndex++;

        if (artifactIndex == artifacts.Count)
            artifactIndex = 0;

        artifact = artifacts[artifactIndex];
    }
}