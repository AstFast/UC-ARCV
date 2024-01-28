namespace UC_ARCV
{
	internal class FileARCV
	{
		static void UnFile(string path, string fiderpath)
		{
			new DecodeFile().ReadFile(path, fiderpath);
		}
		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine(@"
			UC-ARCV.exe {file path} {folder path} 
notice:
	file path:The file path that needs to be unpacked(*.bin/*.arcv)
	folder path:A folder for storing extracted files
	other:
		There is currently no compression function available
		There is currently no inspection function available
		This program currently does not have the ability to recognize files
		Program author:冬日-春上(@AstFast)
		&2024-1-28
				");
				return;
			}
			UnFile(args[0], args[1]);
		}
	}
}
