using System;
using Spectre.Console;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static System.Net.Mime.MediaTypeNames;

namespace Team_T_RPG
{
    public static class Art
    {
        // 너비 기준 출력: Art.MakeImage("Image/이름", width: 40);
        // 높이 기준 출력: Art.MakeImage("Image/이름", height: 20);
        // 강제로 비율 무시하고 출력: Art.MakeImage("Image/이름", width: 40, height: 20);
        public static void MakeImage(string imagePath, int? width = null, int? height = null)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                // 이미지 로드
                using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(imagePath);

                // 원본 가로세로 비율 계산
                float aspectRatio = (float)image.Height / image.Width;

                // 너비와 높이 계산: 하나만 주어지면 비율로 나머지 계산
                int w = width ?? (height.HasValue ? (int)(height.Value / aspectRatio) : 20);
                int h = height ?? (int)(w * aspectRatio);

                // 이미지 리사이즈
                image.Mutate(x => x.Resize(w, h));

                List<string> lines = new List<string>(); // 출력결과 List에 넣어짐
                for (int y = 0; y < image.Height - 1; y += 2)
                {
                    string line = "";

                    for (int x = 0; x < image.Width; x++)
                    {
                        var top = image[x, y];
                        var bottom = image[x, y + 1];

                        string fg = $"rgb({top.R},{top.G},{top.B})";
                        string bg = $"rgb({bottom.R},{bottom.G},{bottom.B})";

                        line += $"[{fg} on {bg}]▀[/]";
                    }

                    AnsiConsole.MarkupLine(line);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]이미지 출력 오류: {ex.Message}[/]");
            }
        }

        // -------------------------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------------
        public static class ImageBorder
        {
            /// <summary>
            /// 이미지 출력 문자열 리스트에 Spectre.Console 패널 테두리를 입혀주는 유틸
            /// </summary>
            /// <param name="lines">컬러 ASCII 라인 리스트</param>
            /// <param name="title">헤더 텍스트 (옵션)</param>
            /// <param name="border">테두리 스타일 (기본: Rounded)</param>
            /// <param name="color">헤더 색상 (기본: yellow)</param>
            /// ImageBorder.PrintWithBorder(lines, title: "파일 이름"
            public static void PrintWithBorder(List<string> lines, string? title = null, BoxBorder? border = null, string color = "yellow")
            {
                var content = string.Join("\n", lines);

                var panel = new Panel(content)
                {
                    Border = border ?? BoxBorder.Rounded,
                    Padding = new Padding(1, 0, 1, 0),
                };

                if (!string.IsNullOrEmpty(title))
                {
                    panel.Header = new PanelHeader($"[bold {color}]{title}[/]", Justify.Center);
                }

                AnsiConsole.Write(panel);
            }
        }



        public static void StartSceneImage()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("________                     ____                          \r\n\\______ \\   __ __   ____    / ___\\   ____   ____    ____   \r\n |    |  \\ |  |  \\ /    \\  / /_/  >_/ __ \\ /  _ \\  /    \\  \r\n |    `   \\|  |  /|   |  \\ \\___  / \\  ___/(  <_> )|   |  \\ \r\n/_______  /|____/ |___|  //_____/   \\___  >\\____/ |___|  / \r\n        \\/             \\/               \\/             \\/  \r\n                                                           \r\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("                          ____                             \r\n                         /  _ \\                            \r\n                         >  _ </\\                          \r\n                        /  <_\\ \\/                          \r\n                        \\_____\\ \\                          \r\n                               \\/                          \r\n");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("                                                           \r\n      _________                        __                  \r\n     /   _____/______ _____  _______ _/  |_ _____          \r\n     \\_____  \\ \\____ \\\\__  \\ \\_  __ \\\\   __\\\\__  \\         \r\n     /        \\|  |_> >/ __ \\_|  | \\/ |  |   / __ \\_       \r\n    /_______  /|   __/(____  /|__|    |__|  (____  /       \r\n            \\/ |__|        \\/                    \\/        \r\n                                                           ");
            Console.ResetColor();
        }

        // 마을 아스키 아트
        public static void TownImaget()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"


            ");
            Console.ResetColor();
        }

        // 던전 입장 시 출력할 아트
        public static void DungeonEntranceImage()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(@"
            ");
            Console.ResetColor();
        }

        public static void QuestBoardImage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
          ┌─────────────────────┐
          │   📜 퀘스트 게시판 📜   │
          └─────────────────────┘
            ");
            Console.ResetColor();
        }

        public static class HeaderUi
        {
            public static void Show(string text, string color = "yellow")
            {
                var headerText = $"[bold {color}]{text}[/]";

                var panel = new Panel("")
                {
                    Header = new PanelHeader(headerText, Justify.Center),
                    Border = BoxBorder.Double,
                    Padding = new Padding(0, 0, 0, 0)
                };

                AnsiConsole.Write(panel);
            }
        }
    }
}
