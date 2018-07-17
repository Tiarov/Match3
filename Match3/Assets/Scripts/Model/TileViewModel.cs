using Assets.Scripts.Match3Engine;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class TileViewModel
    {
        public TileViewModel(Match3TileModel model, Transform transform, PoolObject po)
        {
            Model = model;
            Transform = transform;
            PoolOComponent = po;
        }

        public Match3TileModel Model { get; private set; }
        public Transform Transform { get; private set; }
        public PoolObject PoolOComponent { get; private set; }
    }
}
