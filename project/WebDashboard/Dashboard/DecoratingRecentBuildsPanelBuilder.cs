using System;
using System.Web.UI.HtmlControls;
using ThoughtWorks.CruiseControl.WebDashboard.MVC.Cruise;
using ThoughtWorks.CruiseControl.WebDashboard.MVC.View;

namespace ThoughtWorks.CruiseControl.WebDashboard.Dashboard
{
	public class DecoratingRecentBuildsPanelBuilder : HtmlBuilderViewBuilder, IRecentBuildsViewBuilder
	{
		private readonly IUrlBuilder urlBuilder;
		private readonly IRecentBuildsViewBuilder builderToDecorate;

		public DecoratingRecentBuildsPanelBuilder(IHtmlBuilder htmlBuilder, IUrlBuilder urlBuilder, IRecentBuildsViewBuilder builderToDecorate) : base(htmlBuilder)
		{
			this.builderToDecorate = builderToDecorate;
			this.urlBuilder = urlBuilder;
		}

		public HtmlTable BuildRecentBuildsTable(string serverName, string projectName)
		{
			HtmlTable subTable = builderToDecorate.BuildRecentBuildsTable(serverName, projectName);
			subTable.Rows.Insert(0, TR(TD("Recent Builds")));
			subTable.Rows.Add(TR(
				TD(A("Show All", 
				urlBuilder.BuildProjectUrl("Controller.aspx", new ActionSpecifierWithName(CruiseActionFactory.VIEW_ALL_BUILDS_ACTION_NAME), serverName, projectName)))));
			return subTable;
		}
	}
}
