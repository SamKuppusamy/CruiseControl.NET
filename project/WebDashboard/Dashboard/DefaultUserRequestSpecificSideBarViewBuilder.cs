using System.Web.UI;
using System.Web.UI.HtmlControls;
using ThoughtWorks.CruiseControl.WebDashboard.MVC.Cruise;
using ThoughtWorks.CruiseControl.WebDashboard.MVC.View;

namespace ThoughtWorks.CruiseControl.WebDashboard.Dashboard
{
	public class DefaultUserRequestSpecificSideBarViewBuilder : HtmlBuilderViewBuilder, IUserRequestSpecificSideBarViewBuilder
	{
		private readonly IRecentBuildsViewBuilder recentBuildsViewBuilder;
		private readonly IBuildNameRetriever buildNameRetriever;
		private readonly IUrlBuilder urlBuilder;

		public DefaultUserRequestSpecificSideBarViewBuilder(IHtmlBuilder htmlBuilder, IUrlBuilder urlBuilder, IBuildNameRetriever buildNameRetriever, IRecentBuildsViewBuilder recentBuildsViewBuilder) 
			: base (htmlBuilder)
		{
			this.urlBuilder = urlBuilder;
			this.buildNameRetriever = buildNameRetriever;
			this.recentBuildsViewBuilder = recentBuildsViewBuilder;
		}

		public HtmlTable GetFarmSideBar()
		{
			return Table(
				TR( TD( A("Add Project", urlBuilder.BuildUrl("controller.aspx", new ActionSpecifierWithName(CruiseActionFactory.ADD_PROJECT_DISPLAY_ACTION_NAME))))));
		}

		public HtmlTable GetServerSideBar(string serverName)
		{
			return Table(
				TR( TD( A("View Server Log", urlBuilder.BuildServerUrl("ViewServerLog.aspx", new ActionSpecifierWithName(CruiseActionFactory.ADD_PROJECT_DISPLAY_ACTION_NAME), serverName)))),
				TR( TD( A("Add Project", urlBuilder.BuildServerUrl("controller.aspx", serverName)))));
		}

		public HtmlTable GetProjectSideBar(string serverName, string projectName)
		{
			return Table(
				TR( TD( A("Latest", urlBuilder.BuildBuildUrl("BuildReport.aspx", serverName, projectName, buildNameRetriever.GetLatestBuildName(serverName, projectName)))))
				);
		}

		public HtmlTable GetBuildSideBar(string serverName, string projectName, string buildName)
		{
			return Table(
				TR( TD( A("Latest", urlBuilder.BuildBuildUrl("BuildReport.aspx", serverName, projectName, buildNameRetriever.GetLatestBuildName(serverName, projectName))))),
				TR( TD( A("Next", urlBuilder.BuildBuildUrl("BuildReport.aspx", serverName, projectName, buildNameRetriever.GetNextBuildName(serverName, projectName, buildName))))),
				TR( TD( A("Previous", urlBuilder.BuildBuildUrl("BuildReport.aspx", serverName, projectName, buildNameRetriever.GetPreviousBuildName(serverName, projectName, buildName))))),
				TR( TD( A("View Build Log", urlBuilder.BuildBuildUrl("ViewLog.aspx", serverName, projectName, buildName)))),
				TR( TD( recentBuildsViewBuilder.BuildRecentBuildsTable(serverName, projectName)))
					);
		}
	}
}
