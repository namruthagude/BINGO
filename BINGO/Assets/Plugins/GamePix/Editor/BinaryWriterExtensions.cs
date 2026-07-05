using System;
using System.IO;
using System.Text;

namespace GamePix.Editor
{
    public static class BinaryWriterExtensions
    {
        private static void WriteInt(this BinaryWriter writer, int number)
        {
            var bytes = BitConverter.GetBytes(number);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            writer.Write(bytes, 0, bytes.Length);
        }

        public static void WriteLong(this BinaryWriter writer, long number)
        {
            var bytes = BitConverter.GetBytes(number);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            writer.Write(bytes, 0, bytes.Length);
        }


        public static void WriteString(this BinaryWriter writer, string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            writer.WriteInt(bytes.Length);
            writer.Write(bytes, 0, bytes.Length);
        }

        public static void WriteBytes(this BinaryWriter writer, byte[] bytes)
        {
            writer.WriteInt(bytes.Length);
            writer.Write(bytes, 0, bytes.Length);
        }
    }
}