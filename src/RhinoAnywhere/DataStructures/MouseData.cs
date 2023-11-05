namespace RhinoAnywhere
{

    /// <summary>Stores all of the potential mouse data</summary>
    public struct MouseData
    {

        /// <summary>The type of data action</summary>
        public string method { get; set; }
        
        /// <summary>The type of action</summary>
        public string action { get; set; }

        /// <summary>The x coordinate</summary>
        public double x { get; set; }

        /// <summary>The y coordinate</summary>
        public double y { get; set; }

        /// <summary>The delta x</summary>
        public double deltax { get; set; }

        /// <summary>The delta y</summary>
        public double deltay { get; set; }

        /// <summary>The mouse movement type</summary>
        public string value { get; set; }
    }
}
