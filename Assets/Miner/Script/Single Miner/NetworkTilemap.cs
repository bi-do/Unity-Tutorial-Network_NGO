using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetworkTilemap : NetworkBehaviour
{
    [SerializeField] private GameObject[] minerals;

    private Tilemap tilemap;

    private NetworkList<Vector3Int> destroyed_tiles = new NetworkList<Vector3Int>();

    void Awake()
    {
        this.tilemap = GetComponent<Tilemap>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        destroyed_tiles.OnListChanged += OnTileDestroyed;

        foreach (Vector3Int element in destroyed_tiles)
        {
            tilemap.SetTile(element, null);
        }
    }

    private void OnTileDestroyed(NetworkListEvent<Vector3Int> change_event)
    {
        if (change_event.Type == NetworkListEvent<Vector3Int>.EventType.Add)
        {
            tilemap.SetTile(change_event.Value, null);
        }
    }
    
    public void RemoveTile(Vector3 hit_pos)
    {
        if (!IsServer)
            return;

        Vector3Int cell_pos = tilemap.WorldToCell(hit_pos);

        int drop_probability = Random.Range(0, 101);
        if (drop_probability >= 70)
        {
            int random_idx = Random.Range(0, minerals.Length);

            // NetworkObject.Instantiate() 한줄 스폰

            GameObject temp_mineral = Instantiate(minerals[random_idx], cell_pos, Quaternion.identity);
            temp_mineral.GetComponent<NetworkObject>().Spawn();
        }

        if (tilemap.GetTile(cell_pos) != null)
        {
            destroyed_tiles.Add(cell_pos);
        }
    }
}
