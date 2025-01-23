using System.Text;
using System.Text.RegularExpressions;

namespace UC_ARCV.Decode
{
	public class DecodeARCV
	{
		BinaryReader br;
		public DecodeARCV(string inputpath)
		{
			try
			{
				br = new BinaryReader(new FileStream(inputpath, FileMode.Open));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		ARCV ReadFileHeader()
		{
			ARCV av = new ARCV();
			av.ID = new char[4];
			av.ID = br.ReadChars(4);
			if (new string(av.ID) != "ARCV")
			{
				//Console.WriteLine("This is not an arcv file");
				throw new Exception("Wrong file, it's not the appropriate file type for me");
			}
			av.Count = br.ReadInt32();
			av.Size = br.ReadInt32();
			av.Offest = new int[av.Count];
			av.Offest_size = new int[av.Count];
			av.Offest_crc = new int[av.Count];
			for (int i = 0; i < av.Count; i++)
			{
				av.Offest[i] = br.ReadInt32();
				av.Offest_size[i] = br.ReadInt32();
				av.Offest_crc[i] = br.ReadInt32();
			}
			av.EndOffest = br.BaseStream.Position;
			return av;
		}
		bool ContainsSpecialCharacters(string input)
		{
			return Regex.IsMatch(input, "[^a-zA-Z0-9]");
		}

		public void OutInFile(string outfolderpath)
		{
			ARCV file = ReadFileHeader();
			for (int i = 0; i < file.Count; i++)
			{
				br.BaseStream.Position = file.Offest[i];
				byte[] data = new byte[file.Offest_size[i]];
				data = br.ReadBytes((int)file.Offest_size[i]);
				string name = new string(Encoding.ASCII.GetChars(new byte[]{data[0], data[1], data[2], data[3]}).Reverse().ToArray());
				if (name == null || name == "" || ContainsSpecialCharacters(name))
				{
					name = "data";
				}
#if DEBUG
				Console.WriteLine("file {1}.{0} has finish. offest: {3} size: {4} crc: {2}", name, i,file.Offest_crc[i], file.Offest[i], file.Offest_size[i]);
#endif
				BinaryWriter bw = new(new FileStream(outfolderpath + "//" + Convert.ToString(i) + @"." + name, FileMode.Create));
				bw.Write(data);
				bw.Close();
			}
		}
		~DecodeARCV()
		{
			br.Close();
		}
	}
}
