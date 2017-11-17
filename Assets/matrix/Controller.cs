using UnityEngine;
using System.Collections;
using Powerup;
using MatrixRenderer;

namespace Matrix {
    public delegate void TileDigged (int row, int column, Matrix.Controller matrix, Qeqe.Digger digger);
    public delegate void BoneEaten (int row, int column, Matrix.Controller matrix, Consumer consumer);
    public delegate void LittleChangeDelegate (int row, int column, Matrix.Controller matrix, LittleChange.Type change);
    
    [ExecuteInEditMode]
    public class Controller : MonoBehaviour{
        public TextAsset testLvl;
        [HideInInspector]
        public Status status;
        public bool update = false;
        public MapRenderer _renderer;

        public int Width { get { return status.W.GetLength(0); } }
        public int Height { get { return status.W.GetLength(1); } }

        public event TileDigged OnTileDigged;
        public event BoneEaten OnBoneEaten;
        public event LittleChangeDelegate OnLittleChange;

        void Awake () {
            GameObject.FindWithTag("world controller").
                GetComponent<World.Controller>().Subscribe(this);
        }

        void Start () {
            status = Parser.Digest(testLvl);
            _renderer.Initialize(this);
        }

        void Update () {
            if (update) {
                Start();
            }
        }

        public void TriggerTileDigged (int i, int j, Qeqe.Digger digger) {
            if (OnTileDigged != null) {
                OnTileDigged(i, j, this, digger);
            }
        }

        public bool StartGettingDigged (int i, int j, Qeqe.Digger digger) {
            return GetComponent<Matrix.Digger>().StartGettingDigged(i, j, digger);
        }

        public bool StopGettingDigged (int i, int j, Qeqe.Digger digger) {
            return GetComponent<Matrix.Digger>().StopGettingDigged(i, j, digger);
        }

        public void TriggerBoneEaten (int row, int col, Consumer eater) {
            GetComponent<Matrix.Powerup>().RemoveBone(row, col);
            if (OnBoneEaten != null) {
                OnBoneEaten(row, col, this, eater);
            }
        }

        public void TriggerLittleChange (int row, int col, LittleChange.Type change) {
            switch (change) {
                case LittleChange.Type.bone:
                    GetComponent<Matrix.Powerup>().AddBone(row, col);
                    break;
                case LittleChange.Type.tile:
                    GetComponent<Matrix.Digger>().Undig(row, col);
                    break;
            }

            if (OnLittleChange != null) {
                OnLittleChange(row, col, this, change);
            }
        }

        public bool CanDig (Qeqe.Digger digger, Tile tile) {
            Qeqe.Digger tileDigger = GetComponent<Matrix.Digger>().GetDigger(tile.row, tile.column);

            if (tileDigger == null || tileDigger == digger)
                return true;
            return false;
        }
    }
}
