var actorArray = [];

function addActor(event) {
    var actor = event.target.innerHTML;
    var actorId = actor.split(' ')[0];

    if (actorArray.includes(actorId)) {
        actorArray.splice(actorId, 1);
    }
    else {
        actorArray.push(actorId);

    }
    $(".actors-form-submit").val(actorArray.join(','));
    console.log(event.target.innerHTML);
}        