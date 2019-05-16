var originalActorsArray = [];

    $(document).ready(function () {
        boxvalue = document.getElementById('originalActorsArr').value;
        elementsArr = boxvalue.split(',');
        originalActorsArray.push(elementsArr);
        console.log(originalActorsArray);
    });

function removeActor(event) {
    var actorOld = event.target.innerHTML;
    var actorIdOld = actorOld.split(' ')[0];
	console.log(actorIdOld);

    removeA(originalActorsArray, actorIdOld);    
	
    $(document).on("click", ".deleteActor-btn", function () {
        var clickedBtnOld = $(this);
        clickedBtnOld.remove();
    });
}

function addAllActors(event) {
	var joinedNewActorsString = actorArray.join(',');
	var joinedOriginalActorsString = originalActorsArray.join(',');
	var finalActorsArrToAdd = joinedNewActorsString + joinedOriginalActorsString;
    $(".actors-form-submit").val(finalActorsArrToAdd);	
}

  