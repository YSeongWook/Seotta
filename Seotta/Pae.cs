﻿using System.Windows.Forms;
using System;
using static System.Console;
using System.IO;
using System.Collections.Generic;

namespace Seotta
{
    public class Pae
    {
        public string PaeMonth { get; set; }
        public string PaeName { get; set; }
        public int PaeLevel { get; set; }

        public string AsciiArtTextFileName { get; set; }
        public string specialPae;

        public Pae(string paeMonth, string paeName, string asciiArtTextFileName)
        {
            this.PaeMonth = paeMonth;
            this.PaeName = paeName;
            this.AsciiArtTextFileName = asciiArtTextFileName;
        }

        // Pae 클래스에 ASCII ART를 입력 받아서 넣으면 될듯

        // 출력은 PaeNum으로 "{pae.PaeNum}.txt"을 이용해 출력
        // 족보 계산은 PaeNum, PaeName으로 계산
    }
}