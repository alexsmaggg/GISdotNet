using System;
using System.Runtime.InteropServices;

namespace GISdotNet.Core.Packets
{
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct  Header                         //Заголовок пакета в UDP модуле обозначен как TZagolovok размер: 12 byte.
    {
        public byte   DestinationComplexNumber;   // Номер комплекса - пока всегда 1
        public byte   DestinationProductNumber;   // Номер изделия - по Касьяну
        public byte   DestinationVehicleNumber;   // Номер машины (напр., N ПУ)
        public byte   DestinationWorkplaceNumber; // Номер раб. места
        public byte   DestinationModelNumber;     // N идентификатора модели 0-сам(не модель), 1-на РТ, 2-на ИнтстКО, 3-на ИнтстСО, 7-на ИнтстМСНР, 6-на ИнтстПУ
        public byte   SourceComplexNumber;
        public byte   SourceProductNumber;        // Номер изделия - по Касьяну
        public byte   SourceVehicleNumber;        // Номер машины (напр., N ПУ)
        public byte   SourceWorkplaceNumber;      // Номер раб. места
        public byte   SourceModelNumber;          // N идентификатора модели 0-сам(не модель), 1-на РТ,
        public byte   PacketType;                 // Тип пакета (tcp_ по Касьяну)
        public byte   PacketCount;                // Количество пакетов в этом пакете
    }

      
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Target                     // Пакет с целью 45 byte. 
    {
        public ushort TargetNumber;          // Номер цели
        public int    X;                     // Координаты цели
        public int    Y;
        public int    H;
        public short  Vx;
        public short  Vy;
        public short  Vh;                    // Вектора цели
        public short  Ax;
        public short  Ay;
        public short  Ah;                    // Ускорение
        public ushort TNext;                 // Оригинальное имя переменной. ?
        public byte   TargetType;
        public byte   APType;                // Тип чего-то
        public byte   AlienSign;             // 0 - чужой 1 - свой
        public byte   TargetKind;            // Еще одна разновидность цели?
        public byte   Oto_cel;               // Характеристики цели. Jригинальное имя переменной
        public ushort Upr;                   // Оригинальное имя переменной. ?
        public byte   Har_Ob;                // Оригинальное имя переменной. ?
        public byte   Tip_Ob;                // Оригинальное имя переменной. ?
        public byte   SignShum;              // Оригинальное имя переменной. ?
        public byte   Gr_Cel;                // Оригинальное имя переменной. ?

        public short  PassiveInterferenceStartTime;  // Время старта пассивной помехи

        public short  MetricCode;             // Код марки цели   02.07.2015
        public short  Overload;               // Перегрузка на звене трассы в ед.g (для самолета)
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Targets
    {
        public byte   TargetsCount;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 225)]
        public byte[] targetsArray;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StandingPoint          // Точка стояния комплекса 24 byte
    {
        public byte   VehicleNumber;     // Номер машины 0-МСНР / N ПУ
        public byte   Byte1;             // Резерв

        public int    X;                 // Координаты точки стояния: X
        public int    Y;                 // Y
        public int    H;                 // Высота
        public short  Azimuth;           // Азимут биссектрисы град

        public ushort ZoneNumber;        // Номер зоны
        public ushort HemisphereSign;    // Признак полушария: 1=С, 2=Ю

        public byte   IsExist;           // 0=нет этой машины, 1-есть
        public byte   Byte2;             // Резерв
        public short  MsnrAltitude;      // Высота МСНР
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Azimuth         // Пакет с азимутом 11 byte
    {
        public byte   VeichleNumber;     // 1=КО, 2=СО, 7..10= ЗРК1..4
        public byte   BRSign;            // БРА-АУ=1 БРБ=2
        public Single  Normal;            // 29,5 или 45 Номаль?
        public Single  AzimuthValue;           // Азимут Задачи
        public byte   RotationType;      // 0-первичный разворот по азимуту
                                         // 1-доворот по азимуту
    }

}
