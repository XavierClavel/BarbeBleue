using System;

public interface ICheckpoint
{
    public void onCheckpointDeclaration(Checkpoint checkpoint);
    public void onCheckpointReached(CheckpointData checkpointData);
}