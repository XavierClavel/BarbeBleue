using System;

public interface ICheckpoint
{
    public void onCheckpointDeclaration(CheckpointData checkpointData);
    public void onCheckpointReached(CheckpointData checkpointData);
}