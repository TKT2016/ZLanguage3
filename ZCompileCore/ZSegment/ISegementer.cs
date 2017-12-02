using ZNLP.Segment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNLP
{
    public interface ISegementer
    {
        //void AddWord(string word);
        //void DeleteWord(string word);
        string[] Cut(string src); 
    }
}
