using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using NAudio.Wave;

namespace Team_T_RPG
{
    // 참고사항
    // 플레이 화면이 마을 > 던전으로 이동해도 BGM은 유지됨
    // 마을에서 던전으로 이동한 후 새로운 BGM을 넣고 싶으면 Sound.PlayBgm("이름");만 넣어도 그 전 BGM은 알아서 멈춤
    // BGM과 효과음 서로 영향을 주지 않아 효과음을 재생해도 BGM은 끊기지 않음


    // 사용법
    // Sound.PlayBgm("이름"); > 배경음(MP3)을 틀고 싶으면 
    // Sound.PlaySound("이름"); > 효과음(WAV)을 틀고 싶으면 
    // Sound.PlayBgm("이름", false); > 루프 없이 1회만 진행하고 싶은 경우
    // Sound.PlayBgm("이름", 0.3f); > 볼륨을 조절하고 싶으면 (볼륨 설정: 0.0f ~ 1.0f 범위 내에서만 허용)
    // 배경음 정지 Sound.StopBgm();

    public static class Sound
    {
        // 사운드 등록하는 곳
        private static Dictionary<string, string> soundPaths = new Dictionary<string, string>
    {
        // { 이름(자유), 실제 경로 (Sound 폴더 기준) }
        // 예시: Sound.PlayBgm("mainBgm")로 나중에 사용하기 편한 이름으로 지을 것
        { "mainBgm", "Sound/Nameless Tavern.mp3" },     // 마을 BGM
        { "dungeonBgm", "Sound/Dungeon Opening.mp3" },  // 던전 입장 BGM
        { "select", "Sound/select.wav" },               // 메뉴 선택 효과음
        { "win", "Sound/win.wav" }                      // 전투 승리 효과음



        // 필요한 만큼 추가 가능
        // 반드시!! 음악을 파일에 추가한 후 속성에서 "빌드 액션"을 "내용"으로 설정하고 "출력 디렉토리에 복사"를 "새로 고칠 때만 복사"로 설정해야 함
    };

        private static WaveOutEvent bgmPlayer;
        private static AudioFileReader bgmReader;


        // 효과음을 재생하는 함수 (WAV 전용)
        public static void PlaySound(string name)
        {
            // 이름이 없거나, 경로에 파일이 없으면 알려줌. 파일이 있다면 재생
            if (soundPaths.TryGetValue(name, out string path) && File.Exists(path))
            {
                new SoundPlayer(path).Play();
            }
            else
            {
                Console.WriteLine($"[Sound] '{name}' 효과음 재생 실패. 경로 확인: {path}");
            }
        }


        // 배경음(BGM)을 재생하는 함수 (MP3 전용)
        // PlayBgm("이름", 볼륨, 루프 여부)
        // 볼륨은 0.0f ~ 1.0f 사이의 값으로 설정 가능 (기본값 1.0f)
        // 루프 여부는 기본값 true로 설정 (루프 재생)
        public static void PlayBgm(string name, float volume = 1.0f, bool loop = true)
        {
            // 이름이 없거나, 경로에 파일이 없으면 알려줌
            if (!soundPaths.TryGetValue(name, out string path) || !File.Exists(path))
            {
                Console.WriteLine($"[Sound] '{name}' BGM 재생 실패. 경로 확인: {path}");
                return;
            }

            // 기존에 재생 중이던 BGM이 있다면 정리 (중복 재생 방지)
            bgmPlayer?.Stop();
            bgmPlayer?.Dispose();
            bgmReader?.Dispose();

            // 새로 재생할 MP3 파일을 읽고 출력 장치에 연결
            bgmReader = new AudioFileReader(path);
            bgmReader.Volume = Math.Clamp(volume, 0.0f, 1.0f);
            bgmPlayer = new WaveOutEvent();        
            bgmPlayer.Init(bgmReader);             

            // loop가 true이면 음악이 끝났을 때 자동으로 다시 재생
            if (loop)
            {
                bgmPlayer.PlaybackStopped += (s, e) =>
                {
                    bgmReader.Position = 0;
                    bgmPlayer.Play();       
                };
            }

            bgmPlayer.Play();
        }

        // 현재 재생 중인 배경음을 정지하는 함수
        public static void StopBgm()
        {
            bgmPlayer?.Stop();
        }
    }
}
