﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using static System.Console;

namespace Seotta
{
    public class Game
    {
        Form1 form;
        TextBox pae1;
        TextBox pae2;
        TextBox pae3;
        TextBox pae4;
        TextBox gameProgress;
        TextBox jokboHelper;
        Panel jokboPanel;

        Timer timer1;
        Timer timer2;

        // ASCII ART가 담겨있는 텍스트 파일명 리스트
        private List<string> asciiText = new List<string>();

        // ASCII ART 출력에 사용할 인덱스 변수
        private int[] cpuIndex = new int[2];
        private int[] playerIndex = new int[2];

        // 패 객체 20개가 담길 리스트
        private List<Pae> pae = new List<Pae>();

        // cpu, player 패2장씩 담을 리스트
        private Pae[] cpuPae;
        private Pae[] playerPae;

        private bool endBetting = false;

        public Game(Form1 form, TextBox pae1, TextBox pae2, TextBox pae3, TextBox pae4, TextBox gameProgress, TextBox jokboHelper, Panel jokboPanel)
        {
            this.form = form;
            this.pae1 = pae1;
            this.pae2 = pae2;
            this.pae3 = pae3;
            this.pae4 = pae4;
            this.gameProgress = gameProgress;
            this.jokboHelper = jokboHelper;
            this.jokboPanel = jokboPanel;

            // 타이머1 설정
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1; // 1밀리초마다 변경
            timer1.Tick += Timer1_Tick;

            // 타이머2 설정
            timer2 = new System.Windows.Forms.Timer();
            timer2.Interval = 1; // 1밀리초마다 변경
            timer2.Tick += Timer2_Tick;
        }

        public void StartGame()
        {
            // 게임 안내 문구 출력
            DisplayTextFromFile("game_start.txt", gameProgress);

            // 패의 이름(1광, 1띠, etc..)가 담겨 있는 파일을 매개변수로 Pae 클래스 객체 20개 생성(Pae 클래스 객체를 사용하여 족보 계산)
            ReadPaeFromFile("Pae_Name.txt");

            cpuPae = new Pae[2];
            playerPae = new Pae[2];

            ResetPae();
            SelectPae();
            PrintPae();
        }

        public Timer GetTimer(int i)
        {
            if (i == 1)
            {
                return timer1;
            }
            else
            {
                return timer2;
            }
        }

        public void CheckEndBetting()
        {
            endBetting = true;
        }

        public void InitIndex()
        {
            for (int i = 0; i < 2; i++)
            {
                cpuIndex[i] = 0;
                playerIndex[i] = 0;
            }
        }

        public Pae[] GetCpuPae()
        {
            return cpuPae;
        }

        public Pae[] GetPlayerPae()
        {
            return playerPae;
        }

        // Pae 객체 20개 생성(1광 ~ 10띠)
        public void ReadPaeFromFile(string filePath)
        {
            List<string> paeName = new List<string>();

            // ASCII ART가 담겨진 텍스트 파일명 불러오기
            asciiText.Clear();
            for (int i = 1; i <= 20; i++) asciiText.Add($"{i}.txt");

            foreach (string str in File.ReadLines(filePath))
            {
                paeName.Add(str); // 리스트에 문자열 추가
            }

            // PaeMonth, PaeName을 필드로 가지는 객체 생성
            // pae[0] => ("1", "1광", "아스키 아트 텍스트 파일 이름" , "아스키 아트 전체 텍스트")
            for (int i = 0; i < asciiText.Count; i++)
            {
                if (i == 0)
                {
                    pae.Add(new Pae((i + 1).ToString(), paeName[i], asciiText[i]));
                }
                else
                {
                    pae.Add(new Pae((i / 2 + 1).ToString(), paeName[i], asciiText[i]));
                }
            }
        }

        // 문자열 파일 읽어 출력
        public void DisplayTextFromFile(string filePath, TextBox targetTextBox)
        {
            try
            {
                // 지정된 파일의 모든 텍스트를 읽는다.
                string fileContent = File.ReadAllText(filePath);

                // 읽어온 텍스트를 대상 텍스트 박스에 표시
                targetTextBox.Text = fileContent;
            }
            catch (Exception ex)
            {
                // 오류 발생 시 처리
                MessageBox.Show($"파일을 읽는 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ASCII ART 텍스트 파일 불러오기
        public string[] ReadAllLinesFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllLines(fileName);
            }
            else
            {
                WriteLine($"{fileName} 파일을 불러올 수 없습니다.");

                return new string[0];
            }
        }

        // 각 패 텍스트 박스 초기화
        public void ResetPae()
        {
            pae1.Clear();
            pae2.Clear();
            pae3.Clear();
            pae4.Clear();
        }

        // 패 선택
        public void SelectPae()
        {
            Random rand = new Random();

            for (int i = 0; i < 4; i++)
            {
                int randIndex;
                do
                {
                    randIndex = rand.Next(0, pae.Count);
                } while (pae[randIndex] == null);

                if (i < 2)
                {
                    // cpu 패 선택
                    cpuPae[i] = pae[randIndex];
                }
                else
                {
                    // 플레이어 패 선택
                    playerPae[i - 2] = pae[randIndex];
                }

                pae.RemoveAt(randIndex); // 중복된 값 제거
            }
        }

        // SelectPae()
        // PrintPae()
        // Betting()
        // PrintPae()
        // Betting()
        // PrintResult()

        public void PrintPae()
        {
            // Timer 조정
            // 타이머2 정지
            timer2.Stop();

            // 타이머1 시작
            timer1.Start();
        }

        // PrintResult(), cpu 패도 모두 출력하고 비교하여 결과 산출
        private void Timer1_Tick(object sender, EventArgs e)
        {
            // 베팅이 끝나지 않았다면
            if (!endBetting)
            {
                DisplayLinesBeforeBetting(0, pae1, cpuPae[0], true);
                DisplayLinesBeforeBetting(0, pae3, playerPae[0], false);
            }
            else
            {
                DisplayLines(0, pae1, cpuPae[0], true);
                DisplayLines(0, pae3, playerPae[0], false);
                DisplayLines(1, pae2, cpuPae[1], true);
                DisplayLines(1, pae4, playerPae[1], false);
            }
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            // 베팅이 끝나지 않았다면
            if (!endBetting)
            {
                DisplayLinesBeforeBetting(1, pae2, cpuPae[1], true);
                DisplayLinesBeforeBetting(1, pae4, playerPae[1], false);
            }
        }

        // ASCII ART 출력
        private void DisplayLines(int index, TextBox textBox, Pae pae, bool isCpu)
        {
            if (isCpu)
            {
                if (cpuIndex[index] < pae.AsciiArt.Length)
                {
                    // ASCII ART를 한줄 씩 TextBox에 추가
                    textBox.AppendText(pae.AsciiArt[cpuIndex[index]] + Environment.NewLine);
                    cpuIndex[index]++;
                }
                else
                {
                    if (index == 0)
                    {
                        timer1.Stop();
                        timer2.Start();
                    }
                    else
                    {
                        timer2.Stop();
                    }
                }
            }
            else
            {
                if (playerIndex[index] < pae.AsciiArt.Length)
                {
                    // ASCII ART를 한줄 씩 TextBox에 추가
                    textBox.AppendText(pae.AsciiArt[playerIndex[index]] + Environment.NewLine);
                    playerIndex[index]++;
                }
                else
                {
                    if (index == 0)
                    {
                        timer1.Stop();
                        timer2.Start();
                    }
                    else
                    {
                        timer2.Stop();
                    }
                }
            }
        }

        // ASCII ART 출력(cpu는 뒷면 출력)
        private void DisplayLinesBeforeBetting(int index, TextBox textBox, Pae pae, bool isCpu)
        {
            if (isCpu)
            {
                if (cpuIndex[index] < pae.AsciiArt.Length)
                {
                    // ASCII ART를 한줄 씩 TextBox에 추가
                    textBox.AppendText(pae.BackAsciiArt[cpuIndex[index]] + Environment.NewLine);
                    cpuIndex[index]++;
                }
                else
                {
                    if (index == 0)
                    {
                        timer1.Stop();
                    }
                    else
                    {
                        timer2.Stop();
                    }
                }
            }
            else
            {
                if (playerIndex[index] < pae.AsciiArt.Length)
                {
                    // ASCII ART를 한줄 씩 TextBox에 추가
                    textBox.AppendText(pae.AsciiArt[playerIndex[index]] + Environment.NewLine);
                    playerIndex[index]++;
                }
                else
                {
                    if (index == 0)
                    {
                        timer1.Stop();
                    }
                    else
                    {
                        timer2.Stop();
                    }
                }
            }
        }

        // 족보 도우미를 출력하려면 족보 계산을 할 수 있어야함, 출력 자체는 잘된다. 이제 특수 족보의 예외처리를 해주고 계산 처리도해야함
        public void DisplayJokboHelper(TextBox textBox, Pae[] pae, string name)
        {
            string jokbo = "";

            int level = 0;

            // 첫번째 패가 두번째 패보다 낮은 월이라면
            if (string.Compare(pae[0].PaeName, pae[1].PaeName) < 0)
            {
                jokbo = pae[0].PaeName + pae[1].PaeName;
            }
            else
            {
                jokbo = pae[1].PaeName + pae[0].PaeName;
            }

            if (jokbo.Equals("3광8광"))
            {
                jokbo = "38광땡";
                level = 15;
                textBox.AppendText(name + ": " + jokbo);
            }
            else if (jokbo.Equals("4열끗7열끗"))
            {
                jokbo = "7·4암행어사";
                level = 14;
                textBox.AppendText(name + ": " + jokbo);
            }
            else if (CountOccurrences(jokbo, "광")) // jokbo가 38광땡이 아니고, 광이 2개 이상 들어있다면(광땡이라면)
            {
                if (string.Compare(pae[0].PaeMonth, pae[1].PaeMonth) < 0)
                {
                    jokbo = pae[0].PaeMonth + pae[1].PaeMonth;
                }
                else
                {
                    jokbo = pae[1].PaeMonth + pae[0].PaeMonth;
                }

                jokbo += "광땡";
                level = 13;
                textBox.AppendText(name + ": " + jokbo);
            }
            else if (CountOccurrences(jokbo, "10")) // jokbo에 10이 2개 이상 들어있다면(장땡이라면)
            {
                jokbo = "장땡";
                level = 12;
                textBox.AppendText(name + ": " + jokbo);
            }
            else if (jokbo.Equals("4열끗9열끗"))   // 멍텅구리 구사
            {
                jokbo = "멍텅구리구사";
                level = 11;
                textBox.AppendText(name + ": " + jokbo);
            }
            else if (jokbo.Equals("3광7열끗"))    // 땡잡이
            {
                jokbo = "땡잡이";
                level = 10;
                textBox.AppendText(name + ": " + jokbo);
            }
            else if (jokbo[0] == jokbo[2])    // 같은 월인 경우 ex) 1광1띠
            {
                jokbo = jokbo[0] + "땡";
                level = 9;
                textBox.AppendText(name + ": " + jokbo);
            }
            else
            {
                if (string.Compare(pae[0].PaeMonth, pae[1].PaeMonth) < 0)
                {
                    jokbo = pae[0].PaeMonth + pae[1].PaeMonth;
                } else
                {
                    jokbo = pae[1].PaeMonth + pae[0].PaeMonth;
                }

                switch (jokbo)
                {
                    case "49":
                        jokbo = "구사";
                        level = 8;
                        break;
                    case "12":
                        jokbo = "알리";
                        level = 7;
                        break;
                    case "14":
                        jokbo = "독사";
                        level = 6;
                        break;
                    case "19":
                        jokbo = "구삥";
                        level = 5;
                        break;
                    case "110":
                        jokbo = "장삥";
                        level = 4;
                        break;
                    case "104":
                        jokbo = "장사";
                        level = 3;
                        break;
                    case "46":
                        jokbo = "세륙";
                        level = 2;
                        break;
                    default:
                        int kkut = (Int32.Parse(pae[0].PaeMonth) + Int32.Parse(pae[1].PaeMonth)) % 10;
                        switch (kkut)
                        {
                            case 9:
                                jokbo = "갑오(아홉끗)";
                                break;
                            case 8:
                                jokbo = "여덟끗";
                                break;
                            case 7:
                                jokbo = "일곱끗";
                                break;
                            case 6:
                                jokbo = "여섯끗";
                                break;
                            case 5:
                                jokbo = "다섯끗";
                                break;
                            case 4:
                                jokbo = "네끗";
                                break;
                            case 3:
                                jokbo = "세끗";
                                break;
                            case 2:
                                jokbo = "두끗";
                                break;
                            case 1:
                                jokbo = "한끗";
                                break;
                            case 0:
                                jokbo = "망통(0끗)";
                                break;
                        }
                        level = 1;
                        break;
                }
                textBox.AppendText(name + ": " + jokbo);
            }

            // 이부분에서 jokbo에 패종류만 들어가게 해야함
             if(jokbo.Contains("38광땡"))
            {
                jokbo = "1.38광땡";
                form.HighlightJokboButton(jokboPanel, jokbo);
            } else if(jokbo.Contains("광땡"))
            {
                jokbo = "2.광땡";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if (jokbo.Contains("암행어사"))
            {
                jokbo = "* 암행어사";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if(jokbo.Contains("땡잡이"))
            {
                jokbo = "* 땡잡이";
                form.HighlightJokboButton(jokboPanel, jokbo);
            } else if(jokbo.Contains("땡"))
            {
                jokbo = "3.땡";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if (jokbo.Contains("구사"))
            {
                jokbo = "* 구사";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if(jokbo.Contains("알리"))
            {
                jokbo = "4.알리";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if (jokbo.Contains("독사"))
            {
                jokbo = "5.독사";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if (jokbo.Contains("구삥"))
            {
                jokbo = "6.구삥";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if (jokbo.Contains("장삥"))
            {
                jokbo = "7.장삥";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if (jokbo.Contains("장사"))
            {
                jokbo = "8.장사";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if (jokbo.Contains("세륙"))
            {
                jokbo = "9.세륙";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if (jokbo.Contains("갑오"))
            {
                jokbo = "10.갑오";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
            else if (jokbo.Contains("끗"))
            {
                jokbo = "11.끗,망통";
                form.HighlightJokboButton(jokboPanel, jokbo);
            }
        }

        // 문자열에 어떤 문자가 몇개 들어있는지 반환
        public bool CountOccurrences(string text, string pattern)
        {
            int count = 0;
            int index = 0;
            while ((index = text.IndexOf(pattern, index)) != -1)
            {
                index += pattern.Length;
                count++;
            }

            // 같은 문자가 2개이상 들어있다면
            if (count >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 게임 재시작
        public void RestartGame()
        {
            // Pae 배열 초기화
            pae.Clear();

            // cpuPae 및 playerPae 배열 초기화
            cpuPae = new Pae[2];
            playerPae = new Pae[2];

            // endBetting 변수 초기화
            endBetting = false;

            // cpuIndex 및 playerIndex 배열 초기화
            InitIndex();

            // TextBox 초기화
            gameProgress.Clear();
            jokboHelper.Clear();

            // 게임을 다시 시작
            StartGame();
        }
    }
}
