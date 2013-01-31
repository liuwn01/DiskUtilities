using System;

namespace Defrag
{
    public class DefragAccessDeniedException: Exception
    {
        #region Constructor

        public DefragAccessDeniedException(string message): base(message) { }

        #endregion
    }
}
