
using System;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointManager: MonoBehaviour, ICheckpoint
{
   [SerializeField] private RectTransform content;
   
   private HashSet<CheckpointData> checkpoints;
   private List<float> positions;
   private string currentCheckpoint;
   private Dictionary<string, float> dictCheckpoints = new Dictionary<string, float>();
   public static CheckpointManager instance;

   private void Awake()
   {
      instance = this;
   }

   public void setup()
   {
      checkpoints = Checkpoint.getAll();
      currentCheckpoint = SaveManager.getCheckpoint();
      Debug.Log(checkpoints.Count);
      foreach (CheckpointData checkpoint in checkpoints)
      {
         Debug.Log(checkpoint.key);
         dictCheckpoints[checkpoint.key] = checkpoint.position;
      }
      Debug.Log($"Starting from checkpoint '{currentCheckpoint}'");
      if (currentCheckpoint != null)
      {
         Debug.Log(dictCheckpoints[currentCheckpoint]);
         content.anchoredPosition = (dictCheckpoints[currentCheckpoint]) * Vector2.left;
      }
      EventManagers.checkpoint.registerListener(this);
   }

   private void OnDestroy()
   {
      EventManagers.checkpoint.unregisterListener(this);
   }

   public void onCheckpointDeclaration(CheckpointData checkpointData)
   {
      
   }

   public void onCheckpointReached(CheckpointData checkpointData)
   {
      if (checkpointData.key == currentCheckpoint) return;
      currentCheckpoint = checkpointData.key;
      Debug.Log($"Checkpoint reached : {checkpointData.key}");
      SaveManager.updateCheckpoint(checkpointData.key);
   }
}
