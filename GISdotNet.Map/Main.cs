using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using GISdotNet.Core.Channels;
using GISdotNet.Core.Packets;
using GISdotNet.Core.Net;



namespace GISdotNet.Map
{
 
    public partial class frmMain : Form
    {
        double GeoX;
        double GeoY;
        double PlaneX;
        double PlaneY;
        double TrainerCPXCoord;
        double TrainerCPYCoord;

        string ObjKeyName     = "";
        string VectorKeyCode  = "";
        string RlsKeyCode     = "";
        string RlsZoneKeyCode = "";

        const int IdSemCode = 10;
        const int ColorSemCode = 31002;


        int FObjIncode;  // внутренний код объекта карты которым отображаем наш движущийся
        int RlsObjIncode;
        int ZoneLineObjIncode;
        public const string RSCPath = "main.rsc";
        public axGisToolKit.TxCreateSite CreateSite;

        Channel<byte[]>  DataChannel;     
        MapWork<byte[]>  work;
        Producer<byte[]> DataProducer; 
        Consumer<byte[]> DataConsumer;

        public enum RlsColor: uint
        {
            green = 2528277,
            blue = 16413722,
            red = 255,
            yellow = 1685754,
        }

        MapApi.DOUBLEPOINT center = new MapApi.DOUBLEPOINT();
        MapApi.DOUBLEPOINT point1 = new MapApi.DOUBLEPOINT();
        
        public frmMain()
        {
            InitializeComponent();
           
            mvRsc.cMapView = axMapScreen.C_CONTAINER;
            MapPoint.cMapView = axMapScreen.C_CONTAINER;
            MobilObj.cMapView = axMapScreen.C_CONTAINER;
            VectorObj.cMapView = axMapScreen.C_CONTAINER;
            pntConvert.cMapView = axMapScreen.C_CONTAINER;
            axMapScreen.PlaceOut = axGisToolKit.TxPPLACE.PP_PLANE;
            RlsObj.cMapView = axMapScreen.C_CONTAINER;
            RlsZoneObj.cMapView = axMapScreen.C_CONTAINER;
            SearchableObj.cMapView = axMapScreen.C_CONTAINER;
            MapFind.cMapView = axMapScreen.C_CONTAINER;
            MapFind.cMapObj = SearchableObj.C_CONTAINER;

            axMapScreen.MapContrast = -4;

            //GeoX = 0.534796009;
            //GeoY = 0.480257839;
            GeoX = 0.504796009;
            GeoY = 0.490257839;
            ObjKeyName = "P0000000024";
            VectorKeyCode = "L0000000025";
            RlsKeyCode = "P0000000016";
            RlsZoneKeyCode = "L0000000018";
            TrainerCPXCoord = -1;
            TrainerCPYCoord = -1;

            axMapScreen.MapOpen("egypt.sitz", true);  //Открыть основную карту масштаб 1:1000000 
            CreateSite.MapName = "test.sit";     // Имя карты обстановки
            CreateSite.Scale = 2000000;          // Масштаб карты обстановки 
            axMapScreen.CreateAndAppendSite("SiteMap.sit", RSCPath, ref CreateSite);

            mvRsc.OpenRsc(RSCPath);

            FObjIncode = mvRsc.ObjectIncodeByKey_get(ObjKeyName);
            RlsObjIncode = mvRsc.ObjectIncodeByKey_get(RlsKeyCode);
            ZoneLineObjIncode = mvRsc.ObjectIncodeByKey_get(RlsZoneKeyCode);


            MobilObj.CreateObjectByInCode(1, (int)axGisToolKit.TxMetricType.IDDOUBLE2, FObjIncode);
            MobilObj.Metric.Append(0, MapPoint.C_CONTAINER); // первую точку создаем любую           
            //MobilObj.Metric.Append(0, MapPoint.C_CONTAINER);
            FObjIncode = mvRsc.ObjectIncodeByKey_get(VectorKeyCode);
            VectorObj.CreateObjectByInCode(1, (int)axGisToolKit.TxMetricType.IDDOUBLE2, FObjIncode);
            VectorObj.Metric.Append(0, MapPoint.C_CONTAINER); // создать первую точку
            VectorObj.Metric.Append(0, MapPoint.C_CONTAINER); // создать вторую точку 

            axMapScreen.BufferObjectNeed = true;

            MapFind.cMapSelect.SiteNumber = 1;

            pntConvert.SetPoint(GeoX, GeoY);
            pntConvert.GetPoint(ref PlaneX, ref PlaneY);

            DataChannel = Channel.CreateUnbounded<byte[]>();
            work = UpdateWindowAsync;
            DataProducer = new Producer<byte[]>(DataChannel.Writer);
            DataConsumer = new Consumer<byte[]>(DataChannel.Reader, work);

            Task.Run(async () =>
            {
                UdpClient client = UdpSocket.CreateUdpClient(27681);
                var from = new IPEndPoint(0, 0);
                while (true)
                {
                    byte[] recvBuffer = client.Receive(ref from);
                    Header header = PacketReader.ReadHeader(recvBuffer);
                    byte packetId = header.PacketType;
                    byte[] PacketBody = PacketReader.GetPacketBody(recvBuffer);
                    byte[] Packet = PacketReader.AppendPacketIdByte(PacketBody, packetId);
                    if (Constant.AcceptablePacketsId.Contains(packetId))
                    {
                        await DataProducer.BeginProducing(Packet);
                    }
                }

            });

            Task.Run(async () =>
            {
                await DataConsumer.ConsumeData();
            });
        }

        public async Task UpdateWindowAsync(byte[] packet)
        {
            byte packetId = packet[0];
            switch (packetId)
            {
                case 246:
                    byte[] targets = packet.Skip(1).ToArray();
                    var targetsData = PacketReader.ReadTargets(targets);
                    UpdateTargets(targetsData);
                    break;
                case 241:
                    byte[] azmth = packet.Skip(1).ToArray();
                    var azmthData = PacketReader.ReadAzimuth(azmth);
                    UpdateRlsAzimuth(azmthData);
                    break;
                case 243:
                    byte[] standingPoint = packet.Skip(1).ToArray();
                    var standingPointData = PacketReader.ReadStandingPoints(standingPoint);
                    try
                    {
                        PaintNewRlsOnMap(standingPointData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    break;
            }
          
        }

        public void UpdateTargets(List<Target> targets)
        {
            double coordsX;
            double coordsY;
            double angle;
            axMapScreen.ClearObjects(0);
            MapPoint.PlaceInp = axGisToolKit.TxPPLACE.PP_PLANE;
            foreach (Target target in targets)
            {
                if (target.AlienSign == 1)
                {
                    MobilObj.Semantic.AddWithValueAsInteger(ColorSemCode, (int)RlsColor.red);
                    VectorObj.Semantic.AddWithValueAsInteger(ColorSemCode, (int)RlsColor.red);
                }
                else
                {
                    //Black
                    MobilObj.Semantic.AddWithValueAsInteger(ColorSemCode,  0);
                    VectorObj.Semantic.AddWithValueAsInteger(ColorSemCode, 0);
                }                
                
                coordsX = PlaneX +  ((double)target.X * 10);
                coordsY = PlaneY +  ((double)target.Y * 10); 
                MapPoint.SetPoint(coordsX, coordsY);
                MobilObj.Metric.Update(0, 1, MapPoint.C_CONTAINER);               
                MobilObj.PaintObjectUp();  // нарисуем объект в буфер

                VectorObj.Metric.Update(0, 1, MapPoint.C_CONTAINER);
                MapPoint.SetPoint(coordsX + Utils.GetVectorLength(target.Vx, target.Vy) * 100, coordsY);
                VectorObj.Metric.Update(0, 2, MapPoint.C_CONTAINER);
                angle = - Utils.CalculateVectorAngle(target.Vx, target.Vy);
                VectorObj.RotateObject_EP(ref coordsX, ref coordsY, ref angle);              
                VectorObj.PaintObjectUp(); // нарисуем объект в буфер
                                          
                
            }
            axMapScreen.RepaintWindow();
        }

        public void PaintNewRlsOnMap(StandingPoint sp)
        {
            // Взять координаты в системе тренаженра 0 - КП и не рисовать на карте
            if (sp.VehicleNumber == 0)
            {
                TrainerCPXCoord = sp.X;
                TrainerCPYCoord = sp.Y;
                return;
            }

            double deltaX = (double)sp.X - TrainerCPXCoord;
            double deltaY = (double)sp.Y - TrainerCPYCoord;
            

            CreateRls(sp);
            PaintRlsZoneLine(sp.VehicleNumber, PlaneX + deltaX, PlaneY + deltaY, sp.Azimuth , -0.53);
            PaintRlsZoneLine(sp.VehicleNumber, PlaneX + deltaX, PlaneY + deltaY, sp.Azimuth ,  0.53);

            axMapScreen.Repaint();
        }

        public void CreateRls(StandingPoint standingPoint)
        {
            
            double deltaX = standingPoint.X - TrainerCPXCoord;
            double deltaY = standingPoint.Y - TrainerCPYCoord;

            MapPoint.PlaceInp = axGisToolKit.TxPPLACE.PP_PLANE;

            FObjIncode = mvRsc.ObjectIncodeByKey_get(RlsKeyCode);
            RlsObj.CreateObjectByInCode(1, (int)axGisToolKit.TxMetricType.IDDOUBLE2, FObjIncode);
            int hobj = (int)RlsObj.ObjHandle;

            // Добавление семантики
            RlsObj.Semantic.AddWithValueAsInteger(IdSemCode, (int)standingPoint.VehicleNumber);
            if (standingPoint.VehicleNumber <= 2)
            {
                RlsObj.Semantic.AddWithValueAsInteger(ColorSemCode, (int)RlsColor.green);
            }
            else
            {
                RlsObj.Semantic.AddWithValueAsInteger(ColorSemCode, (int)RlsColor.blue);
            }
            
          
            MapPoint.SetPoint(PlaneX + deltaX, PlaneY + deltaY);
            RlsObj.Metric.Append(0, MapPoint.C_CONTAINER); 
            RlsObj.Commit();
            RlsObj.Repaint();                    
        }

        //MAPAPI функции 
        public void PaintRlsZoneLine(int VeichleId, double X, double Y, double azimuth, double angle)
        {
            FObjIncode = mvRsc.ObjectIncodeByKey_get(RlsZoneKeyCode);
            RlsZoneObj.CreateObjectByInCode(1, (int)axGisToolKit.TxMetricType.IDDOUBLE2, FObjIncode);  //создать линейный объект
            long hobj = RlsZoneObj.ObjHandle;
            RlsZoneObj.Semantic.AddWithValueAsInteger(10, VeichleId);
            if (VeichleId <= 2)
            {
                RlsZoneObj.Semantic.AddWithValueAsInteger(ColorSemCode, (int)RlsColor.green);
            }
            else
            {
                RlsZoneObj.Semantic.AddWithValueAsInteger(ColorSemCode, (int)RlsColor.blue);
            }
            
            center.x = X;
            center.y = Y;
            double delta = 3000; //TODO
            double length = 200000;
            double azimuthRad = -(azimuth * (Math.PI / 180));
            try
            {
                MapApi.mapAppendPointPlane(hobj, center.x, center.y, 0);
                MapApi.mapAppendPointPlane(hobj, center.x + length, center.y, 0);

                MapApi.mapRotateObject(hobj, ref center, ref azimuthRad); // направление

                MapApi.mapRotateObject(hobj, ref center, ref angle);
                MapApi.mapGetPlanePoint(hobj, ref point1, 2, 0);
                MapApi.mathSetLineLength(ref center.x, ref center.y, ref point1.x, ref point1.y, length - delta, 1);
                MapApi.mapUpdatePointPlane(hobj, center.x, center.y, 1, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            RlsZoneObj.Commit();
            RlsZoneObj.Repaint();
        }

        public void UpdateRlsAzimuth(Azimuth Azmth)
        {
            double X=0, Y=0;
            SetSearchContextForRlsObj(Azmth.VeichleNumber);
            MapFind.Active = true;            
            SearchableObj.Metric.GetPoint(0, 1, MapPoint.C_CONTAINER);
            SetSearchContextForZoneLineObj(Azmth.VeichleNumber);
            MapFind.Active = true;
            SearchableObj.Delete();
            SearchableObj.Commit();
            MapFind.Next();
            SearchableObj.Delete();
            SearchableObj.Commit();
            MapPoint.GetPoint(ref X, ref Y);
            PaintRlsZoneLine(Azmth.VeichleNumber, X, Y, Azmth.AzimuthValue,   0.53);
            PaintRlsZoneLine(Azmth.VeichleNumber, X, Y, Azmth.AzimuthValue, - 0.53);


            axMapScreen.Repaint();
        }


        public void SetSearchContextForRlsObj(int id)
        {
            string stringId = id.ToString();
            MapFind.Active = false;
            MapFind.cMapSelect.ClearSemantic();
            MapFind.cMapSelect.AddSemantic(10, stringId, axGisToolKit.TxSemanticCondition.SC_EQUAL);
            MapFind.cMapSelect.Layers[-1] = false;
            MapFind.cMapSelect.Layers[1] = true;  //РЛС слой           
            MapFind.cMapSelect.InCode[-1] = false;
            MapFind.cMapSelect.InCode[RlsObjIncode] = true;
            MapFind.cMapSelect.MapSites[0] = false;
            MapFind.cMapSelect.MapSites[1] = true;         
        }

        public void SetSearchContextForZoneLineObj(int id)
        {
            string stringId = id.ToString();
            MapFind.Active = false;
            MapFind.cMapSelect.ClearSemantic();
            MapFind.cMapSelect.AddSemantic(10, stringId, axGisToolKit.TxSemanticCondition.SC_EQUAL);
            MapFind.cMapSelect.Layers[-1] = false;
            MapFind.cMapSelect.Layers[1] = true;  //РЛС слой           
            MapFind.cMapSelect.InCode[-1] = false;
            MapFind.cMapSelect.InCode[ZoneLineObjIncode] = true;
            MapFind.cMapSelect.MapSites[0] = false;
            MapFind.cMapSelect.MapSites[1] = true;           
        }

        private void btnFileOpen_Click(object sender, EventArgs e)
        {
             OpenMapDialog.FileName = "";
            if (OpenMapDialog.ShowDialog() == DialogResult.OK && OpenMapDialog.FileName != "")
            {
                axMapScreen.MapFileName = OpenMapDialog.FileName;
                axMapScreen.MapOpen(OpenMapDialog.FileName, true);
              
            }        
        }
      
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
              axMapScreen.MapClose();
        }
     
        // масштабирование
        private void onMouseWheel(object sender, MouseEventArgs e)
        {
            if (!axMapScreen.Active) return;
            //если курсор над MapScreen
            if ((e.X >= axMapScreen.Left) && (e.X <= axMapScreen.Left + axMapScreen.Width) &&
                (e.Y >= axMapScreen.Top) && (e.Y <= axMapScreen.Top + axMapScreen.Height))
            {
                int scale = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
                // вычислим нужный центр карты
                MapPoint.PlaceInp = axGisToolKit.TxPPLACE.PP_PICTURE;
                MapPoint.SetPoint(axMapScreen.MapLeft + e.X, axMapScreen.MapTop + e.Y);
                // вычислим новый масштаб
                int iScale = axMapScreen.ViewScale;
                if (scale < 0)
                    iScale = iScale * 2;
                else
                if (scale > 0)
                    iScale = iScale / 2;
                axGisToolKit.axMapPoint IPnt = MapPoint.C_CONTAINER;
                axMapScreen.ScaleInPoint(iScale, IPnt);

            }
        }
       
        private void axMapScreen_OnMapScreenUpdate(object sender, AxaxGisToolKit.IaxMapScreenEvents_OnMapScreenUpdateEvent e)
        {
    
        }

        private void axMapScreen_OnMapMouseMove(object sender, AxaxGisToolKit.IaxMapScreenEvents_OnMapMouseMoveEvent e)
        {
            slMap1.Text = "Масштаб: " + axMapScreen.ViewScale.ToString();
            slMap2.Text = "  X=  " + e.x.ToString();
            slMap3.Text = "  Y= " + e.y.ToString();

        }

        /*private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FObjIncode = mvRsc.ObjectIncodeByKey_get(RlsKeyCode);
            RlsObj.ObjHandle = 0;          
            MapFind.Active = false;
            MapFind.cMapSelect.AddSemantic(10, "9", axGisToolKit.TxSemanticCondition.SC_EQUAL);
            MapFind.cMapSelect.Layers[-1] = false;
            MapFind.cMapSelect.Layers[1] = true;  //P0000000016           
            MapFind.cMapSelect.InCode[-1] = false;
            MapFind.cMapSelect.InCode[FObjIncode] = true;
            MapFind.cMapSelect.MapSites[0] = false;
            MapFind.cMapSelect.MapSites[1] = true;
            MapFind.Active = true;
            RlsObj.Style = axGisToolKit.TxObjectStyle.OS_SELECT;
            Console.WriteLine(MapFind.Eof);
            Console.WriteLine(RlsObj.ObjHandle);
        }*/
    }
}
