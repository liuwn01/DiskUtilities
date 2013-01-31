using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace GUI.Model
{
    public class MainModel
    {
        #region Const
        
        private const int MaxCells = 2054;

        #endregion

        #region Fields

        public bool CanDefrag { get; set; }
        public string FilePath { get; set; }
        public ObservableCollection<SolidColorBrush> VolumeMap { get; set; }

        #endregion

        #region Private methods

        private void SetUpVolumeMap(string filePath)
        {
            VolumeMap.Clear();

            Defrag.Defrag defrag = new Defrag.Defrag();
            BitArray volMap = defrag.GetVolumeMap(filePath.Substring(0, 2));
            int clustersPerCell = volMap.Length / MaxCells;

            for (int i = 0; i < MaxCells - 1; i++)
            {
                VolumeMap.Add(IsEngaged(volMap, i * clustersPerCell, i * clustersPerCell + clustersPerCell)
                                  ? Brushes.DarkGray
                                  : Brushes.Snow);
            }

            VolumeMap.Add(IsEngaged(volMap, (MaxCells - 1) * clustersPerCell, volMap.Length) 
                            ? Brushes.DarkGray
                            : Brushes.Snow);
        }

        private bool IsEngaged(BitArray volMap, int first, int last)
        {
            for (int i = first; i < last; i++)
            {
                if (volMap[i])
                    return true;
            }

            return false;
        }

        private void MarkFile(string filePath)
        {
            Defrag.Defrag defrag = new Defrag.Defrag();

            BitArray volMap = defrag.GetVolumeMap(filePath.Substring(0, 2));
            int clustersPerCell = volMap.Length / MaxCells;
            long[,] fileMap = defrag.GetFileMap(filePath);
            SolidColorBrush brush = fileMap.Length > 2 ? Brushes.Red : Brushes.Blue;

            int startCell = (int)fileMap[0, 1] / clustersPerCell;
            int endCell = (int)(fileMap[0, 1] + fileMap[0, 0]) / clustersPerCell;
            for (int j = startCell; j <= endCell; j++)
            {
                VolumeMap[j] = brush;
            }
            for (int i = 1; i < fileMap.Length / 2; i++)
            {
                startCell = (int)fileMap[i, 1] / clustersPerCell;
                endCell = (int)(fileMap[i, 0] - fileMap[i - 1, 0] + fileMap[i, 1]) / clustersPerCell;
                if (endCell == MaxCells)
                    --endCell;

                for (int j = startCell; j <= endCell; j++)
                {
                    VolumeMap[j] = brush;
                }
            }
        }

        #endregion

        #region Constructor

        public MainModel(string filePath)
        {
            VolumeMap = new ObservableCollection<SolidColorBrush>();
            FilePath = filePath;
            SetUpVolumeMap(filePath);
            MarkFile(filePath);
            CanDefrag = true;
        }

        #endregion
    }
}
