using System;
using System.Globalization;
using System.IO;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Core.Test;
using Exortech.NetReflector;

namespace ThoughtWorks.CruiseControl.Core.Sourcecontrol.Test
{
	[TestFixture]
	public class SvnTest : CustomAssertion
	{
		public static string CreateSourceControlXml(string trunkUrl)
		{
			return string.Format(
				@"    <sourceControl type=""svn"">
					      <executable>..\tools\subversion-0.37.0\svn.exe</executable>
						  <trunkUrl>{0}</trunkUrl>
					  </sourceControl>"
				, trunkUrl);	
		}

		[Test]
		public void PropertyPopulationFromXml()
		{
			Svn svn = CreateSvn(CreateSourceControlXml("svn://myserver/mypath"));
			AssertEquals("..\\tools\\subversion-0.37.0\\svn.exe", svn.Executable);
			AssertEquals("svn://myserver/mypath", svn.TrunkUrl);
		}

		[Test]
		public void CreatingHistoryProcessIncludesCorrectlyFormattedArguments()
		{
			Svn svn = CreateSvn(CreateSourceControlXml("svn://someserver/"));
			DateTime from = DateTime.Parse("2001-01-21  20:00:00 'GMT'");
			DateTime to = DateTime.Parse("2001-01-21  20:30:50 'GMT'");
			Process actualProcess = svn.CreateHistoryProcess(from, to);

			AssertEquals("log -v -r \"{2001-01-21T20:00:00Z}:{2001-01-21T20:30:50Z}\" --xml svn://someserver/", actualProcess.StartInfo.Arguments);
		}

		private Svn CreateSvn(string xml)
		{
			Svn svn = new Svn();
			NetReflector.Read(xml, svn);
			return svn;
		}
	} 
}
