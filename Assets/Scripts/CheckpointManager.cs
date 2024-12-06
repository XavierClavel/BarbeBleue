
using System;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointManager: MonoBehaviour, ICheckpoint
{
   [SerializeField] private RectTransform content;
   private string currentCheckpoint;
   private Dictionary<string, float> dictCheckpoints = new Dictionary<string, float>();
   public static CheckpointManager instance;
   private List<Checkpoint> list = new List<Checkpoint>();

   private void Awake()
   {
      instance = this;
      EventManagers.checkpoint.registerListener(this);
   }

   public void setup()
   {
      list.ForEach(it => it.setup());
      currentCheckpoint = SaveManager.getCheckpoint();
      list.ForEach(it => dictCheckpoints[it.data.key] = it.data.position);
      if (currentCheckpoint != null && dictCheckpoints.TryGetValue(currentCheckpoint, out var checkpoint))
      {
         Debug.Log($"Starting game from checkpoint '{currentCheckpoint}'");
         content.anchoredPosition = (checkpoint + ParallaxManager.offset) * Vector2.left;
      }
      list.ForEach(it => it.setInitialPosition());
   }

   private void OnDestroy()
   {
      EventManagers.checkpoint.unregisterListener(this);
   }

   public void onCheckpointDeclaration(Checkpoint checkpoint)
   {
      list.Add(checkpoint);
   }

   public void onCheckpointReached(CheckpointData checkpointData)
   {
      if (checkpointData.key == currentCheckpoint) return;
      currentCheckpoint = checkpointData.key;
      Debug.Log($"Checkpoint reached : {checkpointData.key}");
      SaveManager.updateCheckpoint(checkpointData.key);
   }
}
