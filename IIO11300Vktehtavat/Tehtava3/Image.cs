using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava3
{
    public class Image
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int DpiX { get; set; }
        public int DpiY { get; set; }
        public bool Unit { get; set; }

        // Palauttaa DPI-arvot nätissä merkkijonossa
        public string DpiString
        {
            get
            {
                if (DpiX < 0 && DpiY < 0 && !Unit) return "-";

                string dpiString = "";
                if (DpiX >= 0) dpiString += DpiX.ToString();
                else dpiString += "?";
                dpiString += " x ";
                if (DpiY >= 0) dpiString += DpiY.ToString();
                else dpiString += "?";
                if(Unit) dpiString += " (1)";
                else dpiString += " (0)";
                return dpiString;
            }
        }

        // Palauttaa DPI-yksikön arvon nätissä merkkijonossa
        public string UnitString
        {
            get
            {
                if (Unit) return "1 (määrätty)";
                else return "0 (määrittelemätön)";
            }
        }


        public Image(string name, string path)
        {
            Name = name;
            Path = path;
            DpiX = -1;
            DpiY = -1;
            Unit = false;
        }

        // Merkitsee muuttujiin, että kuvalla ei ole DPI-tietoja
        public void SetNoDpi()
        {
            DpiX = -1;
            DpiY = -1;
            Unit = false;
        }

        // Tallentaa DPI:n tavutaulukosta
        public void SetDpiFromByteArray(string axis, byte[] dpmBytes)
        {
            if (BitConverter.IsLittleEndian) Array.Reverse(dpmBytes);
            int dpi = Decimal.ToInt32(Math.Round(BitConverter.ToInt32(dpmBytes, 0) * 0.0254M));

            if (axis == "x") DpiX = dpi;
            else DpiY = dpi;
        }

        // Palauttaa DPI:n tavulistassa
        public List<byte> GetDpmByteList(string axis)
        {
            int dpi = 0;
            if (axis == "x") dpi = DpiX;
            else dpi = DpiY;

            byte[] bytes = BitConverter.GetBytes(Decimal.ToInt32(Math.Round(dpi / 0.0254M)));
            if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
            return bytes.ToList();
        }

        // Palauttaa DPI-yksikön tavuna
        public byte GetUnitAsByte()
        {
            return Convert.ToByte(Unit);
        }
    }
}
