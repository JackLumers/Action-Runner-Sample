namespace Covers
{
    // Interface for all objects that can take covers
    public interface ICoverTaker
    {
        public Cover Cover { get; set; }

        public void OnCoverTaken();

        public void OnCoverFreed();
    }
}