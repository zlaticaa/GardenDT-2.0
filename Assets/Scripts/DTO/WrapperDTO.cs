
    using System.Collections.Generic;
    using UnityEditor;
    /// <summary>
    /// an object meant to contain all the data we will convert to a json
    /// </summary>
    public class WrapperDTO
    {
        //lists of both floors and buildings
        public List<ObjectDTO> objects = new();
        public List<ObjectDTO> tiles = new();
        //all the settings for the environment pillar
        public bool Spiders;
        public bool Birds;
        public bool Insects;
        public bool Others;
        //the size of the grid used
        public int GridWidth;
        public int GridHeight;
        //the amount of plant types used in the plant diversity pillar
        public int AmountOfPlants;
        //the cleanup type and fertilizer type used for the soil health pillar
        public fertilizerCleanupType Fertilizer;
        public fertilizerCleanupType Cleanup;
    }
