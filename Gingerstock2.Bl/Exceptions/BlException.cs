using System;

namespace Gingerstock2.Bl
{
    /// <summary>
    /// ��������� �� ������ ������-��������
    /// </summary>
    public class BlException : Exception
    {
        public BlException(string msg, Exception inner = null): base(msg, inner)
        {
        }
        
    }
}