﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Handball.IO.Contracts;

namespace Handball.IO
{
    internal class TextWriter: IWriter
    {
        public TextWriter()
        {
            File.Delete(path);

        }

        private string path = "../../../output.txt";

        public void Write(string text)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.Write(text);
            }
        }

        public void WriteLine(string text)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(text);
            }
        }
    }
}