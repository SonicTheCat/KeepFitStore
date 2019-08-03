namespace KeepFitStore.WEB.Rules
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Rewrite;
    using Microsoft.Net.Http.Headers;

    using KeepFitStore.WEB.Common;

    public class AdminRewriteRule : IRule
    {
        private const string AdminPanelSegment = "/Administrator";

        private const string IdentityPanelSegment = "/Identity";

        public AdminRewriteRule()
        {            
        }

        public void ApplyRule(RewriteContext context)
        {
            var isInRole = context.HttpContext.User.IsInRole(WebConstants.AdministratorRoleName);

            if (isInRole)
            {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;

                if (!request.Path.StartsWithSegments(new PathString(AdminPanelSegment)) &&
                    !request.Path.StartsWithSegments(new PathString(IdentityPanelSegment)))
                {
                    response.StatusCode = StatusCodes.Status301MovedPermanently;
                    context.Result = RuleResult.EndResponse;

                    response.Headers[HeaderNames.Location] =
                        AdminPanelSegment + request.Path + request.QueryString; 
                }
            }
        }
    }
}