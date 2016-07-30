using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PboTools.Domain
{
	public class PboHeaderNode
	{
		private static readonly char[] PathSeparators = {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};
		private readonly IDictionary<string, PboHeaderNode> children = new Dictionary<string, PboHeaderNode>();

		public ICollection<PboHeaderNode> Children
		{
			get { return this.children.Values; }
		}

		public bool IsFolder
		{
			get { return this.Entry == null; }
		}

		public string Name { get; set; }

		public PboHeaderNode Parent { get; internal set; }

		public PboHeaderEntry Entry { get; internal set; }

		internal void AddEntry(PboHeaderEntry entry)
		{
			string[] segments = entry.FileName.Split(PathSeparators, StringSplitOptions.RemoveEmptyEntries);

			PboHeaderNode current = this;
			for (var i = 0; i < segments.Length - 1; i++)
			{
				var segment = segments[i];
				PboHeaderNode node;
				if (!current.children.TryGetValue(segment, out node))
				{
					node = new PboHeaderNode {Name = segment, Parent = current};
					current.children.Add(segment, node);
				}
				current = node;
			}

			var fileName = segments.Last();
			var leaf = new PboHeaderNode {Name = fileName, Parent = current, Entry = entry};
			current.children.Add(fileName, leaf);
		}

		public override string ToString()
		{
			var builder = new StringBuilder();

			builder.Append("Name: \"");
			builder.Append(this.Name);
			builder.Append("\"; Children: ");
			builder.Append(this.children.Count);

			return builder.ToString();
		}
	}
}