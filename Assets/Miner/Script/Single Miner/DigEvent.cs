using UnityEngine;
using UnityEngine.Tilemaps;

public class DigEvent : MonoBehaviour
{
    private NetworkTilemap network_tilemap;
    [SerializeField] private LayerMask tile_layer;
    [SerializeField] private Transform[] hit_points;

    void Awake()
    {
        this.tile_layer = 1 << 6;

        this.network_tilemap = GameObject.FindAnyObjectByType<NetworkTilemap>();

    }

    void OnDig()
    {
        foreach (Transform element in hit_points)
        {
            Collider2D coll = Physics2D.OverlapCircle(element.position, 0.1f, tile_layer);

            if (coll != null)
            {
                network_tilemap.RemoveTile(element.position);
                break;
            }
        }
    }
}

