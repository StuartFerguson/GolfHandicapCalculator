var processScorePublished = function(s,e)
{
    var scoreDetails = {
        ScoreId: e.data.EventId,
        TournamentId: e.data.AggregateId,
        GolfClubId: e.data.GolfClubId,
        MeasuredCourseId: e.data.MeasuredCourseId,
        GrossScore: e.data.GrossScore,
        NetScore: e.data.NetScore,
        CSS: e.data.CSS,
        HoleScores: e.data.HoleScores,
        DateTime: e.data.EventCreatedDateTime
    };

    s.ScoresToProcess.push(scoreDetails);
};

var processHandicapAdjustment =function(s,e)
{
    // Get the score that has been processed
    var scoreIndex = findScoreIndexByScoreId(s.ScoresToProcess, e.data.ScoreId);

    if (scoreIndex != -1)
    {
        // We have found the score, so delete it as has been processed
        s.ScoresToProcess.splice(scoreIndex, 1);
    }
};

var findScoreIndexByScoreId = function(scoreList, scoreId)
{
    var index = scoreList.findIndex(function (e) { return e.ScoreId == scoreId; });

    return index;
};

var produceResult = function(state)
{
    return state.ScoresToProcess.length;
}

fromStreams("$et-ManagementAPI.Tournament.DomainEvents.PlayerScorePublishedEvent",
            "$et-ManagementAPI.Player.DomainEvents.HandicapAdjustmentMade")
.partitionBy(function (e)
{
    if (e.eventType == "ManagementAPI.Tournament.DomainEvents.PlayerScorePublishedEvent")
    {
        return "PlayerScoresStream-" + e.data.PlayerId.replace(/-/gi, "");
    }
    else if (e.eventType == "ManagementAPI.Player.DomainEvents.HandicapAdjustmentMade")
    {
        return "PlayerScoresStream-" + e.data.AggregateId.replace(/-/gi, "");
    }
})
.when(
    {
        $init: function (s, e) {
            return {
                ScoresToProcess: []
            };
        },

        'ManagementAPI.Tournament.DomainEvents.PlayerScorePublishedEvent': processScorePublished//,
        //'ManagementAPI.Player.DomainEvents.HandicapAdjustmentMade': processHandicapAdjustment
    })
.transformBy(produceResult);
