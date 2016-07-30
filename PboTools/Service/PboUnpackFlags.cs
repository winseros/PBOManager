using System;

namespace PboTools.Service
{
	[Flags]
	public enum PboUnpackFlags
	{
		None,
		CreateFolder,
		OverwriteFiles,
		WithFullPath
	}
}
