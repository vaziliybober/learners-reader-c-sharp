using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Util.Zip;

namespace Learners_Reader.Utilities
{
	public class Decompress
	{
		String _zipFile;
		String _location;

		public Decompress(String zipFile, String location)
		{
			_zipFile = zipFile;
			_location = location;
			DirChecker("");
		}

		void DirChecker(String dir)
		{
			var file = new File(_location + dir);

			if (!file.IsDirectory)
			{
				file.Mkdirs();
			}
		}

		public void UnZip()
		{
			try
			{
				var fileInputStream = new System.IO.FileStream(_zipFile, System.IO.FileMode.Open);
				var zipInputStream = new ZipInputStream(fileInputStream);

				ZipEntry zipEntry = null;

				while ((zipEntry = zipInputStream.NextEntry) != null)
				{

					if (zipEntry.IsDirectory)
					{
						DirChecker(zipEntry.Name);
					}
					else
					{
						var fileOutputStream = new FileOutputStream(_location + zipEntry.Name);

						for (int i = zipInputStream.Read(); i != -1; i = zipInputStream.Read())
						{
							fileOutputStream.Write(i);
						}

						zipInputStream.CloseEntry();
						fileOutputStream.Close();
					}
				}
				zipInputStream.Close();
			}
			catch (Exception ex)
			{
			}
		}

	}
}