var actorArray = [];
var ignoredActors = [];

$(function searchForActors() {
    $("#ActorSearchInput").keyup(function (e) {
        if (event.which >= 65 && event.which <= 90) {
            var firstName = $("#firstName").val();
                ignoredActors = actorArray.join(',');
                $.post("/Actor/FindActors?firstName=" + firstName, "ignoredActors=" + ignoredActors, function (r) {
                    //update ui with results
                    $("#resultsActorsTable").html(r);
                });
                       
        }
    });
});

function addActor(event) {
    var actor = event.target.innerHTML;
    var actorId = actor.split(' ')[0];

    actorArray.push(actorId);

    d = document.createElement('div');
    $(d).addClass("actorsAdded")
        .html(actor)
        .appendTo($("#placedActorResults"))
        .click(function removeActor() {
            var currActor = $(this).html();
            var currActorId = currActor.split(' ')[0];
            removeA(actorArray, currActorId);
            removeA(ignoredActors, currActorId);
            $(this).remove();
        });

    $(this).remove();

    $(document).on("click", ".btn-actor", function () {
        var clickedBtn = $(this);
        clickedBtn.remove();
    });
}

function addAllActors(event) {
    $(".actors-form-submit").val(actorArray.join(','));
}

  