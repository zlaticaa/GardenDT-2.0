
    using System.Collections.Generic;
    using UnityEditor;

    public class WrapperDTO
    {
        public List<ObjectDTO> objects = new();
        public List<ObjectDTO> tiles = new();
        public bool Spiders;
        public bool Birds;
        public bool Insects;
        public bool Others;
        public int GridWidth;
        public int GridHeight;
        public int AmountOfPlants;
        public fertilizerCleanupType Fertilizer;
        public fertilizerCleanupType Cleanup;
    }
