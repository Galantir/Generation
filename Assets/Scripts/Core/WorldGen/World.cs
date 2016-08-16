using UnityEngine;
using System.Collections;
using SimplexNoise;
using System.Collections.Generic;
using TerrainGenerator;

public class World : MonoBehaviour {

	public static World Current { get; set;}

	public int Radius = 4;
	private Vector2i PreviousPlayerChunkPosition;
	public Transform Player;
	public TerrainChunkGenerator Generator;
	public int Seed = 0;

	internal Vector3 Grain0Offset;

	void Awake() {
		Current = this;

		if (Seed == 0)
			Seed = Random.Range(0, int.MaxValue);

		Grain0Offset = new Vector3(UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000, UnityEngine.Random.value * 10000);
	}

	// Use this for initialization
	void Start () {
		StartCoroutine(InitializeCoroutine());
	}

	private IEnumerator InitializeCoroutine()
	{
		var canActivateCharacter = false;
		Generator.UpdateTerrain(Player.position, Radius);
		do
		{
			var exists = Generator.IsTerrainAvailable(Player.position);
			//var exists = Generator.IsTerrainAvailable(new Vector3());
			if (exists)
				canActivateCharacter = true;
			yield return null;
		} while (!canActivateCharacter);

		//PreviousPlayerChunkPosition = Generator.GetChunkPosition(new Vector3());
		PreviousPlayerChunkPosition = Generator.GetChunkPosition(Player.position);
		Player.position = new Vector3(Player.position.x, Generator.GetTerrainHeight(Player.position) + 0.5f, Player.position.z);
		Player.gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update () {
		if (Player.gameObject.activeSelf)
		{
			var playerChunkPosition = Generator.GetChunkPosition(Player.position);
			if (!playerChunkPosition.Equals(PreviousPlayerChunkPosition))
			{
				Generator.UpdateTerrain(Player.position, Radius);
				PreviousPlayerChunkPosition = playerChunkPosition;
			}
		}
	}

}
