    var originalActorsArray = [];

    $(document).ready(function () {
        boxvalue = document.getElementById('originalActorsArr').value;
        elementsArr = boxvalue.split(',');
        originalActorsArray = elementsArr.map(Number);
    });

    function removeActorFromMovieLoaded(event) {  
    
        var actorOld = event.target.children[0].innerHTML;
        var actorIdOld = Number(actorOld.split(' ')[0]); 

        for (var m = originalActorsArray.length - 1; m >= 0; m--)
        {
            if (originalActorsArray[m] == actorIdOld)
            {
                originalActorsArray.splice(m, 1);
                break;
            }
        }

        $(document).on("click", ".deleteActor-btn", function () {
            var clickedBtnOld = $(this);  
            clickedBtnOld.parent().remove();
            clickedBtnOld.remove();            
        });
      
    }    