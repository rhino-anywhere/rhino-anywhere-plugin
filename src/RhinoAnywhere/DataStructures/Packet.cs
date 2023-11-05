namespace RhinoAnywhere.DataStructures
{

    /// <summary>Wraps a Server Data Packet</summary>
    /// <typeparam name="T">The payload</typeparam>
    public struct Packet<T>
    {
        /// <summary> The type of Packet</summary>
        public string type { get; set; }

        /// <summary>The Data</summary>
        public T data { get; set; }
    }
}
