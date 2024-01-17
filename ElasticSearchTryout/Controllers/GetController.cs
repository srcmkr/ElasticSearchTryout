using ElasticSearchTryout.Models;
using ElasticSearchTryout.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchTryout.Controllers;

[Route("[controller]")]
public class GetController(ElasticService elasticService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Init(int page = 1, string skills="", string languages="", double latitude = 0, double longitude = 0, double radius = 0)
    {
        const int pageSize = 5;
        var skip = (page - 1) * pageSize;

        var skillIds = skills.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var languageIds = languages.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

        var mustQueries = new List<Func<QueryContainerDescriptor<DbUserModel>, QueryContainer>>();
        mustQueries.AddRange(skillIds.Select(s => (Func<QueryContainerDescriptor<DbUserModel>, QueryContainer>)
            (q => q.Term(t => t.Field(f => f.SkillIds).Value(s)))
        ));

        var shouldQueries = languageIds.Select(languageId => 
            (Func<QueryContainerDescriptor<DbUserModel>, QueryContainer>)
            (q => q.Term(t => t.Field(f => f.Languages).Value(languageId))));

        if (radius > 0)
        {
            mustQueries.Add(q => q
                .GeoDistance(g => g
                    .Field(f => f.Location)
                    .DistanceType(GeoDistanceType.Arc)
                    .Location(latitude, longitude)
                    .Distance($"{radius}km")
                )
            );
        }

        var users = await elasticService.GetActiveUsers(new SearchDescriptor<DbUserModel>()
            .Query(q => q
                .Bool(b => b
                    .Must(mustQueries)
                    .Should(shouldQueries)
                    .MinimumShouldMatch(1)
                )
            )
            .Sort(s => s.Ascending(f => f.Id))
            .From(skip)
            .Size(pageSize)
        );

        return Ok(users);
    }

    
}