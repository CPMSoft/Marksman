#region

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;

#endregion

namespace Marksman.Utils
{
    using Marksman.Champions;

    internal class Utils
    {
        public const string Tab = "    ";

        public static Font Text, TextBig, SmallText;

        static Utils()
        {
            TextBig = new Font(
              Drawing.Direct3DDevice,
              new FontDescription
              {
                  FaceName = "Segoe UI",
                  Height = 25,
                  OutputPrecision = FontPrecision.Default,
                  Quality = FontQuality.ClearTypeNatural
              });            
            Text = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                    {
                        FaceName = "Segoe UI", Height = 15, OutputPrecision = FontPrecision.Default,
                        Quality = FontQuality.ClearTypeNatural
                    });
            
            SmallText = new Font(
                Drawing.Direct3DDevice,
                new FontDescription
                {
                    FaceName = "Tahoma",
                    Height = 13,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearTypeNatural
                });
        }

        public class MPing
        {
            private static Vector2 PingLocation;

            private static int LastPingT = 0;

            public static void Ping(Vector2 position)
            {
                if (LeagueSharp.Common.Utils.TickCount - LastPingT < 30 * 1000)
                {
                    return;
                }

                LastPingT = LeagueSharp.Common.Utils.TickCount;
                PingLocation = position;
                SimplePing();

                Utility.DelayAction.Add(150, SimplePing);
                Utility.DelayAction.Add(300, SimplePing);
                Utility.DelayAction.Add(400, SimplePing);
                Utility.DelayAction.Add(800, SimplePing);
            }

            private static void SimplePing()
            {
                Game.ShowPing(PingCategory.Fallback, PingLocation, true);
            }

        }

        public enum MobTypes
        {
            All,

            BigBoys
        }

        public static bool In<T>(T source, params T[] list)
        {
            return list.Equals(source);
        }

        public static bool IsFollowing(Obj_AI_Base t)
        {
            if (!t.IsFacing(ObjectManager.Player)
                && ObjectManager.Player.Position.Distance(t.Path[0])
                < ObjectManager.Player.Position.Distance(t.Position))
            {
                return true;
            }
            return false;
        }

        public static bool IsRunning(Obj_AI_Base t)
        {
            if (!t.IsFacing(ObjectManager.Player)
                && (t.Path.Count() >= 1
                    && ObjectManager.Player.Position.Distance(t.Path[0])
                    > ObjectManager.Player.Position.Distance(t.Position)))
            {
                return true;
            }
            return false;
        }

        private static readonly string[] BetterWithEvade =
            {
                "Corki", "Ezreal", "Graves", "Lucian", "Sivir", "Tristana",
                "Caitlyn", "Vayne"
            };

        public static Obj_AI_Base GetMobs(float spellRange, MobTypes mobTypes = MobTypes.All, int minMobCount = 1)
        {
            List<Obj_AI_Base> mobs = MinionManager.GetMinions(
                spellRange + 200,
                MinionTypes.All,
                MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth);

            if (mobs == null) return null;

            if (mobTypes == MobTypes.BigBoys)
            {
                Obj_AI_Base oMob = (from fMobs in mobs
                                    from fBigBoys in
                                        new[]
                                            {
                                                "SRU_Blue", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak", "SRU_Red",
                                                "SRU_Krug", "SRU_Dragon", "SRU_Baron", "Sru_Crab"
                                            }
                                    where fBigBoys == fMobs.SkinName
                                    select fMobs).FirstOrDefault();

                if (oMob != null)
                {
                    if (oMob.IsValidTarget(spellRange))
                    {
                        return oMob;
                    }
                }
            }
            else if (mobs.Count >= minMobCount)
            {
                return mobs[0];
            }

            return null;
        }


        public static void PrintMessage(string message)
        {
            Game.PrintChat(
                "<font color='#ff3232'>Marksman: </font><font color='#d4d4d4'><font color='#FFFFFF'>" + message
                + "</font>");
            //Notifications.AddNotification("Marksman: " + message, 4000);
        }
                public static void DrawText(Font vFont, string vText, float vPosX, float vPosY, ColorBGRA vColor)
        {
            vFont.DrawText(null, vText, (int) vPosX, (int) vPosY, vColor);
        }

        public static void DrawText(Font vFont, String vText, int vPosX, int vPosY, Color vColor)
        {
            vFont.DrawText(null, vText, vPosX + 2, vPosY + 2, vColor != Color.Black ? Color.Black : Color.White);
            vFont.DrawText(null, vText, vPosX, vPosY, vColor);
        }

           internal static class Jungle
        {
            public enum DrawOption
            {
                Off = 0,
                CloseToMobs = 1,
                CloseToMobsAndJungleClearActive = 2
            }

            private static readonly Dictionary<Vector3, System.Drawing.Color> JunglePositions =
                new Dictionary<Vector3, System.Drawing.Color>();
            public static void DrawJunglePosition(int drawOption)
            {
                if (drawOption == (int)DrawOption.Off)
                {
                    return;
                }

                if (Game.MapId == (GameMapId)11)
                {
                    const float CircleRange = 115f;

                    JunglePositions.Add(new Vector3(7461.018f, 3253.575f, 52.57141f), System.Drawing.Color.Blue);
                        // blue team :red;
                    JunglePositions.Add(new Vector3(3511.601f, 8745.617f, 52.57141f), System.Drawing.Color.Blue);
                        // blue team :blue
                    JunglePositions.Add(new Vector3(7462.053f, 2489.813f, 52.57141f), System.Drawing.Color.Blue);
                        // blue team :golems
                    JunglePositions.Add(new Vector3(3144.897f, 7106.449f, 51.89026f), System.Drawing.Color.Blue);
                        // blue team :wolfs
                    JunglePositions.Add(new Vector3(7770.341f, 5061.238f, 49.26587f), System.Drawing.Color.Blue);
                        // blue team :wariaths

                    JunglePositions.Add(new Vector3(10930.93f, 5405.83f, -68.72192f), System.Drawing.Color.Yellow);
                        // Dragon

                    JunglePositions.Add(new Vector3(7326.056f, 11643.01f, 50.21985f), System.Drawing.Color.Red);
                        // red team :red
                    JunglePositions.Add(new Vector3(11417.6f, 6216.028f, 51.00244f), System.Drawing.Color.Red);
                        // red team :blue
                    JunglePositions.Add(new Vector3(7368.408f, 12488.37f, 56.47668f), System.Drawing.Color.Red);
                        // red team :golems
                    JunglePositions.Add(new Vector3(10342.77f, 8896.083f, 51.72742f), System.Drawing.Color.Red);
                        // red team :wolfs
                    JunglePositions.Add(new Vector3(7001.741f, 9915.717f, 54.02466f), System.Drawing.Color.Red);
                        // red team :wariaths                    

                    foreach (var junglePosition in JunglePositions)
                    {
                        switch (drawOption)
                        {
                            case (int)DrawOption.CloseToMobs:
                                if (ObjectManager.Player.Distance(junglePosition.Key)
                                    <= Orbwalking.GetRealAutoAttackRange(null) + 65)
                                {
                                    Render.Circle.DrawCircle(junglePosition.Key, CircleRange, junglePosition.Value);
                                }
                                break;
                            case (int)DrawOption.CloseToMobsAndJungleClearActive:
                                if (ObjectManager.Player.Distance(junglePosition.Key)
                                    <= Orbwalking.GetRealAutoAttackRange(null) + 65 && Program.CClass.JungleClearActive)
                                {
                                    Render.Circle.DrawCircle(junglePosition.Key, CircleRange, junglePosition.Value);
                                }
                                break;
                        }
                    }
                }
            }
        }

        public static int GetEnemyPriority(string championName)
        {
            string[] lowPriority =
                {
                    "Alistar", "Amumu", "Bard", "Blitzcrank", "Braum", "Cho'Gath", "Dr. Mundo", "Garen",
                    "Gnar", "Hecarim", "Janna", "Jarvan IV", "Leona", "Lulu", "Malphite", "Nami",
                    "Nasus", "Nautilus", "Nunu", "Olaf", "Rammus", "Renekton", "Sejuani", "Shen",
                    "Shyvana", "Singed", "Sion", "Skarner", "Sona", "Soraka", "Tahm", "Taric", "Thresh",
                    "Volibear", "Warwick", "MonkeyKing", "Yorick", "Zac", "Zyra"
                };

            string[] mediumPriority =
                {
                    "Aatrox", "Akali", "Darius", "Diana", "Ekko", "Elise", "Evelynn", "Fiddlesticks",
                    "Fiora", "Fizz", "Galio", "Gangplank", "Gragas", "Heimerdinger", "Irelia", "Jax",
                    "Jayce", "Kassadin", "Kayle", "Kha'Zix", "Lee Sin", "Lissandra", "Maokai",
                    "Mordekaiser", "Morgana", "Nocturne", "Nidalee", "Pantheon", "Poppy", "RekSai",
                    "Rengar", "Riven", "Rumble", "Ryze", "Shaco", "Swain", "Trundle", "Tryndamere",
                    "Udyr", "Urgot", "Vladimir", "Vi", "XinZhao", "Yasuo", "Zilean"
                };

            string[] highPriority =
                {
                    "Ahri", "Anivia", "Annie", "Ashe", "Azir", "Brand", "Caitlyn", "Cassiopeia",
                    "Corki", "Draven", "Ezreal", "Graves", "Jinx", "Kalista", "Karma", "Karthus",
                    "Katarina", "Kennen", "Kindred", "KogMaw", "Leblanc", "Lucian", "Lux", "Malzahar",
                    "MasterYi", "MissFortune", "Orianna", "Quinn", "Sivir", "Syndra", "Talon", "Teemo",
                    "Tristana", "TwistedFate", "Twitch", "Varus", "Vayne", "Veigar", "VelKoz",
                    "Viktor", "Xerath", "Zed", "Ziggs"
                };

            if (lowPriority.Contains(championName))
            {
                return 0;
            }
            if (mediumPriority.Contains(championName))
            {
                return 1;
            }
            if (highPriority.Contains(championName))
            {
                return 2;
            }
            return 1;
        }
        
        public static Vector3 CenterOfVectors(Vector3[] vectors)
        {
            var sum = Vector3.Zero;
            if (vectors == null || vectors.Length == 0)
                return sum;

            sum = vectors.Aggregate(sum, (current, vec) => current + vec);
            return sum / vectors.Length;
        }
    }
}
