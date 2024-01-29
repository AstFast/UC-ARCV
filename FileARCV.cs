namespace UC_ARCV
{
	internal class FileARCV
	{
		static void UnFile(string path, string folderpath)
		{
			new DecodeFile().ReadFile(path, folderpath);
		}
		static void InFile(string outpath, string folderpath)
		{
			new EncodeFile().EnFile(outpath,folderpath);
		}
		static void Main(string[] args)
		{
			string content = @"
			UC-ARCV.exe {mode} {file path} {folder path} 
notice:
	mode:
		-decode:
			file path:The file path that needs to be unpacked(*.bin/*.arcv)
			folder path:A folder for storing extracted files
		-encode:
			file path:Files used for output (generating arcv files)
			folder path:The folder where the required files exist
	other:
		There is currently no inspection function available
		This program currently does not have the ability to recognize files
		Program author:冬日-春上(@AstFast)
		&2024-1-28
				";
			if (args.Length < 2)
			{
				Console.WriteLine(content);
				return;
			}
			switch(args[0])
			{
				case "-decode":
					UnFile(args[1], args[2]);
					break;
				case "-encode":
					InFile(args[1], args[2]);
					break;
				default:
					Console.WriteLine(content);
					break;
			}
		}
	}
}
