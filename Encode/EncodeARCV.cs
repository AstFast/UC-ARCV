using CRC;
using System.Reflection;

namespace UC_ARCV.Encode
{
	public class EncodeARCV
	{
		string[] files;
		static uint Crc32(byte[] buffer)
		{
			uint crc = 0xFFFFFFFF;
			for (int i = 0; i < buffer.Length; i++)
			{
				crc ^= (uint)(buffer[i] << 24);
				for (int j = 0; j < 8; j++)
				{
					if ((crc & 0x80000000) > 0)
						crc = (crc << 1) ^ 0x04C11DB7;
					else
						crc = crc << 1;
				}
			}
			byte[] ret = BitConverter.GetBytes(crc);
			Array.Reverse(ret);
			return BitConverter.ToUInt32(ret,0);
		}
		public EncodeARCV(string inputfolderpath)
		{
			files = Directory.GetFiles(inputfolderpath);
			if (files.Length == 0)
			{
				throw new Exception("I didn't find any files in the folder");
			}
		}
		public void OutEncodeFile(string outfilepath)
		{
			BinaryWriter bw = new(new FileStream(outfilepath, FileMode.Create));
			bw.Write(new char[] { 'A', 'R', 'C', 'V' });
			bw.Write(files.Length);
			bw.Write(0);
			long data_offest = 12 + files.Length * 12;
			long local;
			foreach (string file in files)
			{
				BinaryReader br = new(new FileStream(file, FileMode.Open));
				byte[] data = br.ReadBytes((int)br.BaseStream.Length);
				bw.Write((uint)data_offest);
				bw.Write(data.Length);
				bw.Write(Crc32(data));
				local = bw.BaseStream.Position;
				bw.BaseStream.Position = data_offest;
				bw.Write(data);
				data_offest += data.Length;
				while (true)
				{
					if (bw.BaseStream.Position % 4 == 0)
					{
						break;
					}
					else
					{
						bw.Write((byte)0xAC);
						data_offest++;
					}
				}
				bw.BaseStream.Position = local;
				br.Close();
			}
			long position = bw.BaseStream.Position;
			bw.Seek(8, SeekOrigin.Begin);
			bw.Write((int)position);
			bw.Flush();
			bw.Close();
		}
	}
}
