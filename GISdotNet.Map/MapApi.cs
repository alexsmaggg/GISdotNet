using System;
using System.Runtime.InteropServices;

namespace GISdotNet.Map
{
    class MapApi
    {
        public const string GisLibrary = "gis64acces.dll";
        public const string GisMathLibrary = "gis64math.dll";

        [DllImport(GisLibrary, CharSet = CharSet.Ansi)]
        public static extern int mapAppendPointPlane(long info, double x, double y, int subject);

        [DllImport(GisLibrary, CharSet = CharSet.Ansi)]
        public static extern int mapUpdatePointPlane(long info, double x, double y, int number, int subject);

        [DllImport(GisLibrary, CharSet = CharSet.Ansi)]
        public static extern int mapRotateObject(long info, ref DOUBLEPOINT center, ref double angle);

        [DllImport(GisLibrary, CharSet = CharSet.Ansi)]
        public static extern int mapGetPlanePoint(long info, ref DOUBLEPOINT point, int number, int subject);

        [DllImport(GisLibrary, CharSet = CharSet.Ansi)]
        public static extern int mapGetObjectNumber(long info);

        [DllImport(GisLibrary, CharSet = CharSet.Ansi)]
        public static extern int mapCreateArc(long hmap, long info, ref DOUBLEPOINT point1, ref DOUBLEPOINT point2, ref DOUBLEPOINT point3, double radius);

        [DllImport(GisMathLibrary, CharSet = CharSet.Ansi)]
        public static extern int mathSetLineLength(ref double x1, ref double y1, ref double x2, ref double y2, double delta, int number);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DOUBLEPOINT
        {
            public double x;
            public double y;

            public DOUBLEPOINT(double _x, double _y)
            {
                x = _x;
                y = _y;
            }
        }
    }
}
