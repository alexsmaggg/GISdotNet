using System;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.Net.Sockets;
using System.Net;
using System.Data;
using System.Linq;
using GISdotNet.Core.Channels;
using GISdotNet.Core.Packets;
using GISdotNet.Core.Net;
using System.ComponentModel;


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
        string TextKeyCode    = "";

        const int IdSemCode    = 10;
        const int SignSemCode  = 9;
        const int SignSemCodeLeft = 11;
        const int ColorSemCode = 31002;

        int FObjIncode;  // внутренний код объекта карты которым отображаем наш движущийся
        int RlsObjIncode;
        int ZoneLineObjIncode;
        int TextObjIncode;
        public const string RSCPath = "main.rsc";
        public axGisToolKit.TxCreateSite CreateSite;

        Channel<byte[]>  DataChannel;     
        MapWork<byte[]>  work;
        Producer<byte[]> DataProducer; 
        Consumer<byte[]> DataConsumer;

        private BindingSource TargetInfoBindingSource;        
        DataTable DTable;
        SynchronizationContext context;

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
            TextObj.cMapView = axMapScreen.C_CONTAINER;
            MapFind.cMapView = axMapScreen.C_CONTAINER;
            MapFind.cMapObj = SearchableObj.C_CONTAINER;

            axMapScreen.MapContrast = -4;
            axMapScreen.ViewType = axGisToolKit.TxMapViewType.VT_PRINT;

            //GeoX = 0.534796009;
            //GeoY = 0.480257839;
            GeoX = 0.504796009;
            GeoY = 0.490257839;
            ObjKeyName = "P0000000024";
            VectorKeyCode = "L0000000025";
            RlsKeyCode = "P0000000016";
            RlsZoneKeyCode = "L0000000018";
            TextKeyCode = "T0000000026";
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
            TextObjIncode = mvRsc.ObjectIncodeByKey_get(TextKeyCode);

            MobilObj.CreateObjectByInCode(1, (int)axGisToolKit.TxMetricType.IDDOUBLE2, FObjIncode);
            MobilObj.Metric.Append(0, MapPoint.C_CONTAINER); // первую точку создаем любую           
            //MobilObj.Metric.Append(0, MapPoint.C_CONTAINER);
            FObjIncode = mvRsc.ObjectIncodeByKey_get(VectorKeyCode);
            VectorObj.CreateObjectByInCode(1, (int)axGisToolKit.TxMetricType.IDDOUBLE2, FObjIncode);
            VectorObj.Metric.Append(0, MapPoint.C_CONTAINER); // создать первую точку
            VectorObj.Metric.Append(0, MapPoint.C_CONTAINER); // создать вторую точку 

           TextObj.CreateObjectByInCode(1, (int)axGisToolKit.TxMetricType.IDDOUBLE2, TextObjIncode);
           TextObj.Metric.Append(0, MapPoint.C_CONTAINER); // создать первую точку
           TextObj.Metric.Append(0, MapPoint.C_CONTAINER); // создать вторую точку 

            axMapScreen.BufferObjectNeed = true;

            MapFind.cMapSelect.SiteNumber = 1;

            pntConvert.SetPoint(GeoX, GeoY);
            pntConvert.GetPoint(ref PlaneX, ref PlaneY);

            dataGridView.AllowUserToAddRows = false;

            DataChannel = Channel.CreateUnbounded<byte[]>();
            work = UpdateWindowAsync;
            DataProducer = new Producer<byte[]>(DataChannel.Writer);
            DataConsumer = new Consumer<byte[]>(DataChannel.Reader, work);


            /*targetInfoList = new BindingList<TargetInfo>();
            TargetInfoBindingSource.DataSource = targetInfoList;
            dataGridView.DataSource = TargetInfoBindingSource;*/

            context = SynchronizationContext.Current;

            DTable = InitTable();
            TargetInfoBindingSource = new BindingSource();
            TargetInfoBindingSource.DataSource = DTable;
            dataGridView.Columns.Clear();
            dataGridView.DataSource = TargetInfoBindingSource;



            //DataTableReader reader = new DataTableReader(GetTableDataTest());
            //DTable.Load(reader);


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
                    //UpdateTable(targetsData);
                    context.Post(UpdateTable, targetsData);
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


        public void UpdateTable(object state)
        {
            var targets = state as List<Target>;

            if (targets == null)
            {
                return;
            }

            if (DTable.Rows.Count == 0)
            {
                UpdateUI(targets);
                return;

            }

            DataRowCollection rows = DTable.Rows;
            List<Target> addTargetList = new List<Target>();

            foreach (Target target in targets)
            {
                DataGridViewRow row;
                //row = dataGridView.Rows[index];
                //row.DefaultCellStyle.BackColor = Color.Orange;
                var targetInfoItem = rows.Cast<DataRow>().SingleOrDefault(t => (int)t["N Target"] == target.TargetNumber);

                if (targetInfoItem == null)
                {
                    addTargetList.Add(target);
                }
                else
                {
                    int index = rows.IndexOf(targetInfoItem);
                    row = dataGridView.Rows[index];

                    if (target.AlienSign == 1)
                    {
                        row.DefaultCellStyle.BackColor = Color.Orange;
                    }

                        if ((int)DTable.Rows[index][1] != target.X)
                    {
                        DTable.Rows[index].SetField(1, target.X);                      
                    }
                    if ((int)DTable.Rows[index][2] != target.Y)
                    {
                        DTable.Rows[index].SetField(2, target.Y);
                    }
                    if ((int)DTable.Rows[index][3] != target.Vx)
                    {
                        DTable.Rows[index].SetField(3, target.Vx);
                    }
                    if ((int)DTable.Rows[index][4] != target.Vy)
                    {
                        DTable.Rows[index].SetField(4, target.Vy);
                    }
                    if ((int)DTable.Rows[index][5] != target.AlienSign)
                    {
                        DTable.Rows[index].SetField(5, target.AlienSign);
                    }

                }
            }

            if (addTargetList.Count != 0)
            {
                UpdateUI(addTargetList);
            }

            return;
        }

        private static DataTable InitTable()
        {
            // Create sample Customers table, in order
            // to demonstrate the behavior of the DataTableReader.
            DataTable table = new DataTable();

            // Create two columns, ID and Name.
            DataColumn NTargetColumn = table.Columns.Add("N Target", typeof(int));
            
            // Set the ID column as the primary key column.
            table.PrimaryKey = new DataColumn[] { NTargetColumn };

            table.Columns.Add("X", typeof(int));
            table.Columns.Add("Y", typeof(int));
            table.Columns.Add("Vx", typeof(int));
            table.Columns.Add("Vy", typeof(int));
            table.Columns.Add("Type", typeof(int));
            table.AcceptChanges();
            return table;
        }


        public void UpdateUI( List<Target> targets)
        {
            
            foreach (Target target in targets)
            {
                DTable.Rows.Add(new object[] { target.TargetNumber, target.X, target.Y, target.Vx, target.Vy, target.AlienSign });
            }

            return;
        }

        /*public void UpdateTable(object state)
        {
            var targets  = state as List<Target>;
           

            if (targetInfoList.Count == 0)
            {
                foreach (Target target in targets)
                {
                    targetInfoList.Add(new TargetInfo {Ntarget = target.TargetNumber, RA = target.X} );
                }
                
                return;

            }

            
            foreach (Target target in targets)
            {
                DataGridViewRow row;             
                var targetInfoItem = targetInfoList.SingleOrDefault(t => t.Ntarget == target.TargetNumber);
                
                if (targetInfoItem == null)
                {
                    targetInfoList.Add(new TargetInfo { Ntarget = target.TargetNumber, RA = target.X });
                } 
                else
                {
                    int index = targetInfoList.IndexOf(targetInfoItem);
                    row = dataGridView.Rows[index];
                    if (target.X > 5000)
                    {
                        row.DefaultCellStyle.BackColor = Color.Orange;
                    }               
                    targetInfoList[index].RA = target.X;
                }              
            }

        }*/

        /*  public void UpdateTable(List<Target> targets)
          {
              if (targets.Count == 0)
              {
                  return;
              }
              DataTableReader reader = new DataTableReader(GetTableData(targets));

              context.Post(UpdateUI, reader);           
          }

          public void UpdateUI(object state)
          {
              var reader = state as DataTableReader;            
              DTable.Load(reader, LoadOption.Upsert);          
          }

          public DataTable GetTableData(List<Target> targets)
          {
              DataTable table = new DataTable();

              DataColumn NTargetColumn = table.Columns.Add("N Target", typeof(int));
              table.Columns.Add("R", typeof(int));
              table.Columns.Add("A", typeof(int));
              table.Columns.Add("H", typeof(int));
              table.Columns.Add("V", typeof(int));
              table.Columns.Add("Type", typeof(int));

              table.PrimaryKey = new DataColumn[] { NTargetColumn };

              foreach (Target target in targets)
              {
                  table.Rows.Add(new object[] { target.TargetNumber,  target.X, target.Y, target.Vx, target.Vy, target.AlienSign });
              }
              table.AcceptChanges();        
              return table;

          }

          public DataTable GetTableDataTest()
          {
              DataTable table = new DataTable();

              DataColumn NTargetColumn = table.Columns.Add("N Target", typeof(int));
              table.Columns.Add("R", typeof(int));
              table.Columns.Add("A", typeof(int));
              table.Columns.Add("H", typeof(int));
              table.Columns.Add("V", typeof(int));
              table.Columns.Add("Type", typeof(int));

              table.PrimaryKey = new DataColumn[] { NTargetColumn };

              table.Rows.Add(new object[] { 0, 1, 2, 3, 4, 5});
              table.Rows.Add(new object[] { 1, 1, 2, 3, 4, 5});
              table.Rows.Add(new object[] { 2, 1, 2, 3, 4, 5});

              table.AcceptChanges();
              return table;
          }*/

        public void UpdateTargets(List<Target> targets)
        {
            double coordsX;
            double coordsY;
            double angle;
            int ratio;

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

                //Подпись
                MobilObj.Semantic.AddWithValue(SignSemCode, target.TargetNumber.ToString());
                //MobilObj.Semantic.AddWithValue(SignSemCodeLeft, "Target");


                ratio = Utils.CalculateVectorRatio(axMapScreen.ViewScale);

                coordsX = PlaneX +  ((double)target.X * 10);
                coordsY = PlaneY +  ((double)target.Y * 10); 
                MapPoint.SetPoint(coordsX, coordsY);
                MobilObj.Metric.Update(0, 1, MapPoint.C_CONTAINER);               
                MobilObj.PaintObjectUp();  // нарисуем объект в буфер

                VectorObj.Metric.Update(0, 1, MapPoint.C_CONTAINER);              
                MapPoint.SetPoint(coordsX + Utils.GetVectorLength(target.Vx, target.Vy) * ratio, coordsY);
                VectorObj.Metric.Update(0, 2, MapPoint.C_CONTAINER);
                angle = - Utils.CalculateVectorAngle(target.Vx, target.Vy);
                VectorObj.RotateObject_EP(ref coordsX, ref coordsY, ref angle);              
                VectorObj.PaintObjectUp(); // нарисуем объект в буфер
                //Console.WriteLine(Utils.GetVectorLength(target.Vx, target.Vy));                          
                
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

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            /*// If the column is the Artist column, check the
            // value.
            if (this.dataGridView.Columns[e.ColumnIndex].Name == "Type")
            {
                if (e.Value != null)
                {
                    // Check for the string "pink" in the cell.
                    int stringValue = (int)e.Value;
                    DataGridViewRow row = this.dataGridView.Rows[e.RowIndex]; 
                    if (stringValue == 1)
                    { 
                        row.DefaultCellStyle.BackColor = Color.Orange;
                    }
                    
                }
            }           */
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

        private void dataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
           
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
