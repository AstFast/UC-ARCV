
using System.Text;
using System.Text.RegularExpressions;

namespace UC_ARCV
{
	internal class DecodeFile
	{
		
		struct ARCV
		{
			public char[] ID;
			public int Count;
			public int Size;
			public uint[] Offest;
			public uint[] Offest_size;
			public uint[] Offest_crc;
			public long EndOffest;
		}
		ARCV ReadFileHeader(string path)
		{
			BinaryReader br = new(new FileStream(path, FileMode.Open));
			ARCV av = new ARCV();
			av.ID = new char[4];
			av.ID = br.ReadChars(4);
			if (new string(av.ID) != "ARCV")
			{
				Console.WriteLine("This is not an arcv file");
				throw new Exception("Wrong file, it's not the appropriate file type for me");
			}
			av.Count = br.ReadInt32();
			av.Size = br.ReadInt32();
			av.Offest = new uint[av.Count];
			av.Offest_size = new uint[av.Count];
			av.Offest_crc = new uint[av.Count];
            for (int i = 0; i < av.Count; i++)
            {
				av.Offest[i] = br.ReadUInt32();
				av.Offest_size[i] = br.ReadUInt32();
				av.Offest_crc[i] = br.ReadUInt32();
			}
			av.EndOffest = br.BaseStream.Position;
			br.Close();
            return av;
		}
		bool ContainsSpecialCharacters(string input)
		{
			return Regex.IsMatch(input, "[^a-zA-Z0-9]");
		}
		public void ReadFile(string path,string fiderpath)
		{
			var file = ReadFileHeader(path);
			BinaryReader br = new(new FileStream(path, FileMode.Open));
			
            for (int i = 0; i < file.Count; i++)
            {
				br.BaseStream.Position = file.Offest[i];
				byte[] data = new byte[file.Offest_size[i]];
				data = br.ReadBytes((int)file.Offest_size[i]);
				string name = new string(Encoding.ASCII.GetChars([data[0], data[1], data[2], data[3]]).Reverse().ToArray());
				if (name==null || name=="" || ContainsSpecialCharacters(name))
				{
					name = "data";
				}
				Console.WriteLine("file {1}.{0} has finish",name,i);
				BinaryWriter bw = new(new FileStream(fiderpath + "//" + Convert.ToString(i) + @"." + name, FileMode.Create));
				bw.Write(data);
				bw.Close();
            }
        }
	}
}
