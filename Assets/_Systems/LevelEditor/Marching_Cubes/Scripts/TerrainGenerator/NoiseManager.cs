using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : Singleton<NoiseManager>
{

	[Header("World configuration")]
	public WorldConfig worldConfig;//Current world configuration of the noiseManager
	public bool regenerateWorld;
	[System.Serializable]
	public class WorldConfig
	{
		[Tooltip("Surface desired level")]
		[Range(-(Constants.MAX_HEIGHT/2), (Constants.MAX_HEIGHT/2))]
		public int surfaceLevel = Constants.MAX_HEIGHT / 8;
	}

	public void CreateTerrain()
	{
        if (regenerateWorld)//Generate random seed when use 0 and is scene testing (no WorldManager exists)
		{
			string selectedWorld = WorldManager.GetSelectedWorldName();
			WorldManager.DeleteWorld(selectedWorld);//Remove previous data
			WorldManager.CreateWorld(selectedWorld, worldConfig);//Create a new world folder for correct working
			regenerateWorld = false;
		}
		else if((Constants.AUTO_CLEAR_WHEN_NOISE_CHANGE) && !WorldManager.IsCreated())//If AUTO_CLEAR_WHEN_NOISE_CHANGE true and world manager not exist, we clear old world data (we assume we are using a debug scene)
		{
			string selectedWorld = WorldManager.GetSelectedWorldName();
			WorldConfig loadedWorldConfig = WorldManager.GetSelectedWorldConfig();
			//If worldConfig loaded is different to the current one, remove old data and save the new config
			if(loadedWorldConfig.surfaceLevel != worldConfig.surfaceLevel)
			{
				WorldManager.DeleteWorld(selectedWorld);//Remove old world
				WorldManager.CreateWorld(selectedWorld, worldConfig);//Create new world with the new worldConfig
			}

		}
		else if(WorldManager.IsCreated())//Load config of the world
		{
			worldConfig = WorldManager.GetSelectedWorldConfig();
		}
        ChunkManager.Instance.Initialize();
    }

	public byte[] GenerateChunkData(Vector2Int vecPos)
	{
		byte[] chunkData = new byte[Constants.CHUNK_BYTES];

		for (int x= 0;  x< Constants.CHUNK_VERTEX_SIZE; x++)
		{
			for(int z=0; z<Constants.CHUNK_VERTEX_SIZE; z++)
			{
				int index = x + z * Constants.CHUNK_VERTEX_SIZE;

				for (int y = 0; y < Constants.CHUNK_VERTEX_HEIGHT; y++)
				{
					if (y <= worldConfig.surfaceLevel + (Constants.MAX_HEIGHT / 2))
                    {
                        int chunkByteIndex = (index + y * Constants.CHUNK_VERTEX_AREA) * Constants.CHUNK_POINT_BYTE;
                        chunkData[chunkByteIndex] = 255;
                        chunkData[chunkByteIndex + 1] = 255;
                    }
                }
			}
		}

		return chunkData;

	}
}
