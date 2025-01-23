using System.IO;
using UC_ARCV.Decode;
using UC_ARCV.Encode;

namespace UC_ARCV
{
	internal class FileARCV
	{
		static void Main(string[] args)
		{
			string content = @"
			UC-ARCV.exe {mode} {file path} {folder path} 
notice:
	mode:
		-decode:
			file path:The file path that needs to be unpacked(*.bin/*.arcv)
			*v1.0.3 allow filepath is folder(Batch processing),
			 But still unable to recognize the file, will exit when encountering an error file
			folder path:A folder for storing extracted files
		-encode:
			file path:Files used for output (generating arcv files)
			folder path:The folder where the required files exist
	other:
		There is currently no inspection function available
		This program currently does not have the ability to recognize files
		Program author:冬日-春上(@AstFast)
		&2025-1-23
				";
			if (args.Length != 3)
			{
				Console.WriteLine(content);
				return;
			}
			switch(args[0])
			{
				case "-decode":
					if (Directory.Exists(args[1]))
					{
						string[] files = Directory.GetFiles(args[1], "*.*", SearchOption.AllDirectories);
                        foreach (var item in files)
                        {
							string apath = args[2] +"\\"+Path.GetFileNameWithoutExtension(item);
							_ = Directory.CreateDirectory(apath);
							var file3 = new DecodeARCV(item);
							file3.OutInFile(apath);
						}
                        break;
					}
					var file1 = new DecodeARCV(args[1]);
					file1.OutInFile(args[2]);
					break;
				case "-encode":
					var file2 = new EncodeARCV(args[2]);
					file2.OutEncodeFile(args[1]);
					break;
				default:
					Console.WriteLine(content);
					break;
			}
		}
	}
}
