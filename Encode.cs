using ICSharpCode.SharpZipLib.Checksum;

namespace UC_ARCV
{
	internal class EncodeFile
	{
		static uint CalculateCrc32(byte[] data)
		{
			var crc32 = new Crc32();
			crc32.Update(data);
			return (uint)crc32.Value;
		}
		public void EnFile(string outpath, string folderpath)
		{

			string[] files = Directory.GetFiles(folderpath);
			if (files.Length == 0)
			{
				Console.WriteLine("I didn't find any files in the folder");
				throw new Exception("I didn't find any files in the folder");
			}
			BinaryWriter bw = new(new FileStream(outpath, FileMode.Create));
			bw.Write(new char[]{'A', 'R', 'C', 'V'});
			bw.Write(files.Length);
			bw.Write(0);
			uint offest = 12;
			uint offest_file = (uint)(12 + files.Length * 12);
			foreach (string filepath in files)
			{
				BinaryReader br = new(new FileStream(filepath, FileMode.Open));
				byte[] data = new byte[(int)br.BaseStream.Length];
				data = br.ReadBytes((int)br.BaseStream.Length);
				var info = new FileInfo(filepath);
				uint crc = CalculateCrc32(data);
				bw.BaseStream.Position = offest;
				bw.Write(offest_file);
				bw.Write((uint)info.Length);
				bw.Write(crc);
				bw.BaseStream.Position = offest_file;
				bw.Write(data);
				offest += 12;
				offest_file += (uint)info.Length;
				br.Close();
				Console.WriteLine("{0} has finish in file offest: {1} size: {2} crc: {3}",info.Name, offest_file, info.Length,crc);
			}
			uint das = (uint)bw.BaseStream.Position;
			bw.BaseStream.Position = 8;
			bw.Write(das);
			bw.Close();
		}
	}
}
