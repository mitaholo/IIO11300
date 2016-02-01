using DamienG.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava3
{
    class FileHandler
    {
        // Yrittää ylikirjoittaa tiedoston
        public bool saveFile(Image image, List<byte> binaryFile)
        {
            try
            {
                using (BinaryWriter binWriter = new BinaryWriter(File.Open(image.Path, FileMode.Truncate)))
                {
                    foreach (byte B in binaryFile) binWriter.Write(B);
                }
            }
            catch { return false; }
            return true;
        }

        // Yrittää lukea tiedoston muistiin ja korvaa/lisää siihen Image-olioon tallennetut DPI-arvot
        public List<byte> readFile(Image image)
        {
            List<byte> binaryFile = new List<byte>();
            try
            {
                using (BinaryReader binReader = new BinaryReader(File.Open(image.Path, FileMode.Open)))
                {
                    long pos = 0;
                    long length = binReader.BaseStream.Length;
                    List<byte> byteBuffer = new List<byte>();
                    List<byte> pHYs = Encoding.ASCII.GetBytes("pHYs").ToList<byte>();
                    List<byte> IDAT = Encoding.ASCII.GetBytes("IDAT").ToList<byte>();
                    bool dpiReplaced = false;
                    while (pos < length)
                    {
                        byte B = binReader.ReadByte();
                        binaryFile.Add(B);

                        if (!dpiReplaced)
                        {
                            byteBuffer.Add(B);
                            while (byteBuffer.Count > 4) byteBuffer.RemoveAt(0);

                            if (byteBuffer.SequenceEqual(pHYs))
                            {
                                binReader.ReadBytes(9 + 4);
                                pos += sizeof(byte) * (9 + 4);

                                List<byte> data = new List<byte>();
                                data.AddRange(image.GetDpmByteList("x"));
                                data.AddRange(image.GetDpmByteList("y"));
                                data.Add(image.GetUnitAsByte());

                                binaryFile.AddRange(data);

                                Crc32 crc32 = new Crc32();
                                binaryFile.AddRange(crc32.ComputeHash(new MemoryStream(data.ToArray())));

                                dpiReplaced = true;
                            }
                            else if (byteBuffer.SequenceEqual(IDAT))
                            {
                                List<byte> data = new List<byte>();
                                data.AddRange(image.GetDpmByteList("x"));
                                data.AddRange(image.GetDpmByteList("y"));
                                data.Add(image.GetUnitAsByte());

                                List<byte> physChunk = new List<byte>();

                                byte[] size = BitConverter.GetBytes(9);
                                if (BitConverter.IsLittleEndian) Array.Reverse(size);

                                physChunk.AddRange(size);
                                physChunk.AddRange(pHYs);
                                physChunk.AddRange(data);

                                Crc32 crc32 = new Crc32();
                                physChunk.AddRange(crc32.ComputeHash(new MemoryStream(data.ToArray())));

                                binaryFile.InsertRange(binaryFile.Count - 8, physChunk);
                                dpiReplaced = true;
                            }
                        }

                        pos += sizeof(byte);
                    }
                }
            }
            catch { return null; }

            return binaryFile;
        }

        // Yrittää lukea PNG-tiedostosta DPI-arvot
        public void readDpi(Image image)
        {
            try
            {
                using (BinaryReader binReader = new BinaryReader(File.Open(image.Path, FileMode.Open)))
                {
                    List<byte> byteBuffer = new List<byte>();
                    List<byte> pHYs = Encoding.ASCII.GetBytes("pHYs").ToList<byte>();
                    List<byte> IDAT = Encoding.ASCII.GetBytes("IDAT").ToList<byte>();
                    long pos = 0;
                    long length = binReader.BaseStream.Length;
                    while (pos < length)
                    {
                        byte B = binReader.ReadByte();
                        byteBuffer.Add(B);
                        while (byteBuffer.Count > 4) byteBuffer.RemoveAt(0);

                        if (byteBuffer.SequenceEqual(pHYs))
                        {
                            if (pos + sizeof(byte) * 9 < length)
                            {
                                image.SetDpiFromByteArray("x", binReader.ReadBytes(4));
                                image.SetDpiFromByteArray("y", binReader.ReadBytes(4));
                                image.Unit = binReader.ReadByte() == 1;
                            }
                            break;
                        }
                        else if(byteBuffer.SequenceEqual(IDAT))
                        {
                            image.SetNoDpi();
                            return;
                        }

                        pos += sizeof(byte);
                    }
                }
            }
            catch { }
        }
    }
}
