namespace GUI.Model
{
    public class DefragModel
    {
        public void DefragFile(string filePath)
        {
            Defrag.Defrag defrag = new Defrag.Defrag();
            defrag.DefragmentFile(filePath);
        }
    }
}
