using System;

namespace Gingerstock2.Bl
{
    /// <summary>
    /// Сообщение об ошибке бизнес-операции
    /// </summary>
    public class BlException : Exception
    {
        public BlException(string msg, Exception inner = null): base(msg, inner)
        {
        }
        
    }
}