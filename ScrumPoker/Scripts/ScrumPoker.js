var colors = [ "Red", "Green", "Cyan", "Chartreuse", "Coral" ];
var logonUserProfile = {
    UserName: "",
    UserId: 0
};

var newUserEstimate = {
    Name: "",
    Estimate: ""
}

var PokerGame = new function () {
    this.Votes = $.extend({}, []);
    this.UserProfile = $.extend({}, logonUserProfile);
    this.UserEstimate = $.extend({}, newUserEstimate);
    this.ProjectId = 0;
}

function GetVotes(callback) {
    CallServer(getVotesAjaxUrl, callback); //getVotesAjaxUrl is in _UserVotes @script section
}

var isRefreshed = false;
function CallServer(url, callback) {
    isRefreshed = false;
    $.ajax(url, {
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify(PokerGame),
        contentType: "application/json; charset=utf-8",
        success: function (pokerGame, t, x) {
            var res = [];
            PokerGame = $.extend({}, pokerGame);
            isRefreshed = true;
            callback();
        },
        error: function (pokerGame, t, x) {
            alert(JSON.stringify(pokerGame));
        }
    });
}

function WriteVotes() {
    $("#tblVotes > thead").find("td").remove();
    $("#tblVotes > tbody > tr").find("td").remove();

    var colorCounter = 0;

    for (var i = 0; i < PokerGame.Votes.length; i++) {       
        $("#tblVotes > thead").append('<td>' + PokerGame.Votes[i].Name + '</td>');

        var colorIndex = colorCounter % 5;
        $("#tblVotes > tbody > tr").append("<td style='height:40px;font-size:xx-large;text-align:center;background-color:" + colors[colorIndex] + "'>" + PokerGame.Votes[i].Estimate + "</td>");
        colorCounter += 1;
    }
}

var keepRefreshing = false;
function RefreshVotes() {
    GetVotes(WriteVotes);
    if (keepRefreshing == true) {
        setTimeout('RefreshVotes()', 3000);
    }
}

$(document).ready(function () {
    $("#SubmitVote").click(function () {
        if ($("#firstName").val().length != 0 && $("#estimate").val().length != 0) {
            PokerGame.UserEstimate.Name = $("#firstName").val();
            PokerGame.UserEstimate.Estimate = $("#estimate").val();
            $("#estimate").val("");
            PokerGame.ProjectId = $("#projectid").val();
            RefreshVotes();
        }
    });
    keepRefreshing = true;
    RefreshVotes();
});

